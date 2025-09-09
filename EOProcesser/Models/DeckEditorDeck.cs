using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOProcesser.Models
{
    internal class DeckEditorDeck
    {
        public List<int> MainDeckContent;
        public List<int> ExtraDeckContent;
        public string DeckName;
        public int DeckId;
        public List<ERACodeCommentLine> commentLines;
        public override string ToString()
        {
            return $"({DeckId}):{DeckName}";
        }
        public DeckEditorDeck(ERACodeMultiLines initCodes)
        {
            // コレクションを初期化
            MainDeckContent = [];
            ExtraDeckContent = [];
            commentLines = [];

            ERACodeFuncSegment? funcSegment = null;

            // コメント行と関数セグメントを抽出
            foreach (var code in initCodes)
            {
                if (code is ERACodeCommentLine commentLine)
                {
                    commentLines.Add(commentLine);
                }
                else if (code is ERACodeFuncSegment fs)
                {
                    if (funcSegment != null)
                    {
                        throw new Exception("複数の関数セグメントが見つかりました");
                    }
                    funcSegment = fs;
                }
                else if (code is not null && !string.IsNullOrWhiteSpace(code.ToString().Trim()))
                {
                    // コメントでも関数でもなく、空行でもない
                    throw new Exception($"予期しないコード: {code}");
                }
            }

            if (funcSegment == null)
            {
                throw new Exception("関数セグメントが見つかりません");
            }

            // 関数名からデッキIDを解析
            string funcName = funcSegment.FuncName;
            if (!funcName.StartsWith("DUELDECK_") || !int.TryParse(funcName.Substring(9), out int deckId))
            {
                throw new Exception($"無効なデッキ関数名: {funcName}");
            }
            DeckId = deckId;

            // 関数の内容を解析
            bool foundDims = false;
            bool foundResults = false;
            bool foundSif = false;

            // 関数の内容を走査
            foreach (var code in funcSegment)
            {
                if (code is ERACodeGenericLine genericLine)
                {
                    string line = genericLine.CodeLine.Trim();

                    // 空行を無視
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    // DIMS宣言をチェック
                    if (line == "#DIMS DYNAMIC 決闘者")
                    {
                        foundDims = true;
                        continue;
                    }

                    // RESULTS行をチェック
                    if (line.StartsWith("RESULTS = "))
                    {
                        DeckName = line.Substring(9).Trim();
                        foundResults = true;
                        continue;
                    }

                    // SETVAR行をチェック
                    if (line.StartsWith("SETVAR "))
                    {
                        ParseSetVarLine(line);
                        continue;
                    }
                }
                else if (code is ERACodeSIfSegment sifSegment &&
                        sifSegment.Condition == "決闘者 == \"存在確認\"")
                {
                    // RETURN 1が含まれているかチェック
                    bool hasReturnOne = false;
                    foreach (var sifCode in sifSegment)
                    {
                        if (sifCode is ERACodeGenericLine sifLine &&
                            sifLine.CodeLine.Trim() == "RETURN 1")
                        {
                            hasReturnOne = true;
                            break;
                        }
                    }

                    if (hasReturnOne)
                    {
                        foundSif = true;
                        continue;
                    }
                    throw new Exception("デッキ関数内の無効なSIFセグメント");
                }
                else if (code is not null && !string.IsNullOrWhiteSpace(code.ToString().Trim()))
                {
                    // 予期しないコード
                    throw new Exception($"デッキ関数内の予期しないコード: {code}");
                }
            }

            // 必要なコンポーネントがすべて見つかったかチェック
            if (!foundDims || !foundResults || !foundSif)
            {
                throw new Exception("デッキ関数内に必要なコンポーネントがありません");
            }
            DeckName ??= "名無しのデッキ";
        }

        private void ParseSetVarLine(string line)
        {
            // 可能な2つの形式:
            // SETVAR @"%決闘者%_デッキ:数字", カードID
            // SETVAR @"%決闘者%_デッキ:POOL_COUNT(0/1)", カードID
            // または追加デッキの場合:
            // SETVAR @"%決闘者%_EXデッキ:数字", カードID
            // SETVAR @"%決闘者%_EXデッキ:POOL_COUNT(0/1)", カードID
            
            // 変数部分と値部分を抽出
            int commaPos = line.IndexOf(',');
            if (commaPos == -1)
            {
                throw new Exception($"無効なSETVAR行: {line}");
            }
            
            string varPart = line.Substring(7, commaPos - 7).Trim();
            string valuePart = line.Substring(commaPos + 1).Trim();
            
            // カードIDを解析
            if (!int.TryParse(valuePart, out int cardId))
            {
                throw new Exception($"SETVARの無効なカードID: {valuePart}");
            }
            
            // メインデッキまたは追加デッキかを判断
            bool isMainDeck = varPart.Contains("_デッキ:");
            bool isExtraDeck = varPart.Contains("_EXデッキ:");
            
            if (!isMainDeck && !isExtraDeck)
            {
                throw new Exception($"SETVARの不明なデッキタイプ: {varPart}");
            }
            
            // 該当するデッキにカードを追加
            if (isMainDeck)
            {
                MainDeckContent.Add(cardId);
            }
            else // isExtraDeck
            {
                ExtraDeckContent.Add(cardId);
            }
        }
        public ERACodeMultiLines ToFunction()
        {
            ERACodeFuncSegment segment = new($"DUELDECK_{DeckId}")
            {
                "#DIMS DYNAMIC 決闘者",
                "",
                $"RESULTS = {DeckName}",
                new ERACodeSIfSegment(@"決闘者 == ""存在確認""", "RETURN 1"),
                ""
            };
            bool reset = false;
            foreach (var cardId in MainDeckContent)
            {
                segment.Add($"SETVAR @\"%決闘者%_デッキ:POOL_COUNT({(reset ? 0 : 1)})\", 2171");
                reset = true;
            }
            segment.Add("");
            reset = false;
            foreach (var cardId in ExtraDeckContent)
            {
                segment.Add($"SETVAR @\"%決闘者%_EXデッキ:POOL_COUNT({(reset ? 0 : 1)})\", 2171");
                reset = true;
            }
            segment.Add("");
            var result = new ERACodeMultiLines();
            result.AddRange(commentLines);
            result.Add(segment);
            return result;
        }
    }
}
