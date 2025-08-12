using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOProcesser
{
    public class EOCardManagerCardEffect : IEnumerable<EOCardManagerEffect>
    {
        public bool IsRogue = false;
        public List<ERACode> PrefixDescription = [];
        private readonly List<EOCardManagerEffect> effects = [];

        private const string NumString = "①②③④⑤⑥⑦⑧⑨⑩";

        public ERACodeMultiLines GetExplanationFuncContent()
        {
            ERACodeMultiLines lines = [
                new ERACodeDimLine("#DIM DYNAMIC 種類"),
                new ERACodeGenericLine("")];
            int index = -1;
            if (IsRogue)
            {
                lines.Add(@"CALL TEXT_DECORATION(""ROGUE"")");
            }
            foreach(ERACode m in PrefixDescription)
            {
                lines.Add(m);
            }
            foreach(EOCardManagerEffect effect in effects)
            {
                string? no = "";
                //不计入编号的情况
                if (effect.EffectNo != null)
                {
                    if (effect.EffectNo != "")
                    {
                        no = $"{effect.EffectNo}：";
                    }
                }
                else
                {
                    index++;
                    no = $"{NumString[index]}：";
                }
                foreach (string effectLine in effect.Descriptions)
                {
                    //其它代码，用原文追加
                    if (effectLine.StartsWith('@'))
                    {
                        lines.Add(effectLine[1..]);
                    }
                    else
                    {
                        if (no != null)
                        {
                            lines.Add(new ERACodePrintLine($"PRINTL {no}{effectLine}"));
                            no = null;
                        }
                        else
                        {
                            lines.Add(new ERACodePrintLine($"PRINTL {effectLine}"));
                        }
                    }
                }
            }
            if (IsRogue)
            {
                lines.Add(@"CALL TEXT_DECORATION(""ROGUE"")");
            }
            return lines;
        }

        public ERACodeMultiLines GetCanFuncContent()
        {
            ERACodeMultiLines lines = ERACodeAnalyzer.AnalyzeCode("""
                #DIMS DYNAMIC 決闘者
                #DIMS DYNAMIC ゾーン
                #DIM DYNAMIC 種類
                #DIM DYNAMIC 場所
                #DIMS DYNAMIC 対面者
                #DIM DYNAMIC 条件達成
                CALL 対面者判定(決闘者)
                対面者 = %RESULTS%

                CALL CARD_NEGATE(決闘者,種類,ゾーン,場所,24147)
                SIF RESULT == 1
                	RETURN 0
                
                """);
            ERACodeIfSegment segment = new("")
            {
                Condition = ""
            };
            return lines;
        }

        public void Add(EOCardManagerEffect effect)
        {
            effects.Add(effect);
        }

        public void Remove(EOCardManagerEffect effect)
        {
            effects.Remove(effect);
        }

        public void Clear()
        {
            effects.Clear();
        }

        public int Count => effects.Count;

        public EOCardManagerEffect this[int index]
        {
            get { return effects[index]; }
        }

        public IEnumerator<EOCardManagerEffect> GetEnumerator()
        {
            return effects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public class EOCardManagerEffect
    {
        public List<string> Descriptions = [];
        public string Condition = "";
        public ERACodeMultiLines CanFunc;
        public ERACodeMultiLines EffectFunc;

        //null → ①②③④⑤…自动； 非null → 强制使用该符号且不计入编号
        //"" → 不编号且不计入编号（用于一个效果多个触发条件）
        public string? EffectNo = null;

        //额外的functions
        public List<ERACodeFuncSegment> ExtraFuncs = [];
    }
}
