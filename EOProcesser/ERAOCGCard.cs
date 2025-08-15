using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.IO;
using System.Text.RegularExpressions;

namespace EOProcesser
{
    public class ERAOCGCardScript
    {
        //一个脚本里可能有多张卡——主卡片和衍生
        public string ScriptFile;
        public List<ERAOCGCard> Cards = [];
        public List<ERACode> Codes = [];
        public List<ERACodeFuncSegment> Funcs = [];

        public ERAOCGCardScript(int id, string name, string folder, bool isMonster = true)
        {
            string info = $"""
                @CARD_{id}(参照先)

                #DIMS DYNAMIC 参照先
                VARSET RESULT
                VARSET RESULTS

                SELECTCASE 参照先
                	;効果モン/通常モン/儀式/融合/シンクロ/エクシーズ/リンク/超次元
                	CASE "種類"
                		RETURN 通常モン
                	;闇属性/光属性/地属性/水属性/炎属性/風属性
                	CASE "属性"
                		RETURN 光属性

                	CASE "種族"
                		RETURN 幻神獣族
                	CASE "レベル"
                		RETURN 0
                	CASE "攻撃力"
                		RETURN 0
                	CASE "守備力"
                		RETURN 0
                	;牝性/雄性があるよ
                	CASE "性別"
                		RETURN 牝性
                ENDSELECT
                RETURN 0
            """;
            if (!isMonster)
            {
                info = $"""
                @CARD_{id}(参照先)

                #DIMS DYNAMIC 参照先
                VARSET RESULT
                VARSET RESULTS

                SELECTCASE 参照先
                    CASE "種類"
                        RETURN 通常魔法
                ENDSELECT

                RETURN 0
                """;
            }
            string template = $"""
                @CARDNAME_{id},参照先
                ;ココで指定カードの名前、略称を返す予定
                #DIMS DYNAMIC 参照先

                VARSET RESULT
                VARSET RESULTS

                SELECTCASE 参照先
                	CASE "名前"
                		RESULTS = 名前
                	CASE "略称"
                		RESULTS = 略称
                	CASE "カテゴリ"
                		RESULTS:0 = 仮実装
                ENDSELECT

                {info}

                @CARD_EXPLANATION_{id}(種類)
                #DIM DYNAMIC 種類

                CALL TEXT_DECORATION("DUELIST")
                PRINTL 
                
                
                @CARDSUMMON_AA_{id}
                RETURN 0

                @CARDCAN_{id}(決闘者,種類,ゾーン,場所)
                #DIMS DYNAMIC 決闘者
                #DIMS DYNAMIC ゾーン
                #DIM DYNAMIC 種類
                #DIM DYNAMIC 場所

                CALL CARD_NEGATE(決闘者,種類,ゾーン,場所,XXXX)
                SIF RESULT == 1
                	RETURN 0

                @CARDEFFECT_{id}(決闘者,種類,ゾーン,場所)
                #DIMS DYNAMIC 決闘者
                #DIMS DYNAMIC 対面者 
                #DIMS DYNAMIC ゾーン
                #DIM DYNAMIC 種類
                #DIM DYNAMIC 場所
                #DIM DYNAMIC カウンタ
                #DIM DYNAMIC 攻撃力修正

                CALL 対面者判定(決闘者)
                対面者 = %RESULTS%

                """;
            string sanitizedName = string.Join("_", name.Split(Path.GetInvalidFileNameChars()));
            ScriptFile = Path.Combine(folder, $"{id}_{sanitizedName}.ERB");
            File.WriteAllText(ScriptFile, template, new UTF8Encoding(true));
            Initialize();
        }

        public void Initialize()
        {
            string file = ScriptFile;
            ERACodeMultiLines scriptCode = ERACodeAnalyzer.AnalyzeCode(File.ReadAllLines(file));
            foreach (var code in scriptCode)
            {
                if (code is ERACodeFuncSegment func)
                {
                    Funcs.Add(func);
                    if (func.FuncName.Contains('_'))
                    {
                        string[] parts = func.FuncName.Split('_');
                        string lastPart = parts[^1].Split([',',' '])[0];
                        if (int.TryParse(lastPart, out int cardCode))
                        {
                            // Check if card with this code doesn't already exist
                            if (!Cards.Any(c => c.Id == cardCode))
                            {
                                try
                                {
                                    // Try to create a new card and add it to the list
                                    ERAOCGCard newCard = new(this, cardCode);
                                    Cards.Add(newCard);
                                }
                                catch (Exception ex)
                                {
                                    ;
                                }
                            }
                        }
                    }
                }
            }
            if (Cards.Count == 0)
            {
                throw new InvalidOperationException("Invalid card script");
            }
        }

        public ERAOCGCardScript(string file)
        {
            ScriptFile = file;
            Initialize();
        }

        public string GetFuncActualName(string funcName)
        {
            return funcName.Split(['(', ' ', ','])[0];
        }

        public ERACodeFuncSegment? GetFunc(string funcName)
        {
            foreach (var func in Funcs)
            {
                string actualFuncName = GetFuncActualName(func.FuncName);
                if (actualFuncName == funcName)
                {
                    return func;
                }
            }
            return null;
        }

        public List<TreeNode> GetTreeNode()
        {
            List<TreeNode> list = [];
            foreach (var card in Cards)
            {
                list.AddRange(card.GetTreeNode());
            }
            return [new TreeNode(new FileInfo(this.ScriptFile).Name,
                    [.. list]) { Tag = this } ];
        }
    }
    public class ERAOCGCard
    {
        public ERAOCGCardScript CardScript;
        public string Name;
        public string ShortName;
        public int Id = -1;
        public ERACodeFuncSegment? GetCardNameFunc() => CardScript.GetFunc($"CARDNAME_{Id}");
        public ERACodeFuncSegment? GetCardInfoFunc() => CardScript.GetFunc($"CARD_{Id}");
        public ERACodeFuncSegment? GetCardExplanationFunc() => CardScript.GetFunc($"CARD_EXPLANATION_{Id}");
        public ERACodeFuncSegment? GetCardCanFunc() => CardScript.GetFunc($"CARDCAN_{Id}");
        public ERACodeFuncSegment? GetCardAAFunc() => CardScript.GetFunc($"CARDSUMMON_AA_{Id}");
        public ERACodeFuncSegment? GetCardEffectFunc() => CardScript.GetFunc($"CARDEFFECT_{Id}");
        public List<ERACodeFuncSegment> GetExtraFuncs()
        {
            List<ERACodeFuncSegment> result = new();
            foreach (var func in CardScript.Funcs)
            {
                string actualFuncName = CardScript.GetFuncActualName(func.FuncName);
                
                // 使用正则表达式确保ID是全字匹配的
                // 检查函数名是否包含完整的卡片ID（作为一个完整的单词）
                string idPattern = $@"(^|[^0-9])({Id})([^0-9]|$)";
                if (Regex.IsMatch(actualFuncName, idPattern))
                {
                    // 跳过包含该卡片ID的函数，这些函数应该已经由特定的getter处理
                    continue;
                }
                
                // 检查函数名是否不包含任何数字
                bool hasDigits = actualFuncName.Any(char.IsDigit);
                
                if (!hasDigits)
                {
                    result.Add(func);
                }
            }
            return result;
        }
        public List<string> GetCardCategory()
        {
            var func = GetCardNameFunc();
            if (func == null)
            {
                return [];
            }
            List<string> result = [];
            if (func.FirstOrDefault((func) => func is ERACodeSelectCase)
                is ERACodeSelectCase selectCase)
            {
                if (selectCase.GetValueList(@"""カテゴリ""") is Dictionary<string, string> dic)
                {
                    return [.. dic.Values];
                }
            }
            return result;
        }

        public ERAOCGCard(ERAOCGCardScript script, int cardId)
        {
            CardScript = script;
            Id = cardId;
            var nameFunc = GetCardNameFunc();
            if (nameFunc == null)
            {
                throw new InvalidOperationException("Missing required information to create card");
            }

            foreach (var code in nameFunc)
            {
                if (code is ERACodeSelectCase selectCase)
                {
                    var n = selectCase.GetValue(@"""名前""");
                    if (n != null)
                    {
                        Name ??= n;
                    }
                    n = selectCase.GetValue(@"""略称""");
                    if (n != null)
                    {
                        ShortName ??= n;
                    }
                }
            }
            if (Name == null || ShortName == null)
            {
                throw new InvalidOperationException("Missing required information to create card");
            }
        }

        override public string ToString()
        {
            return $"[{Id}]{Name}";
        }
        public List<TreeNode> GetTreeNode()
        {
            return [new(ToString()) { Tag = this }];
        }
    }
}
