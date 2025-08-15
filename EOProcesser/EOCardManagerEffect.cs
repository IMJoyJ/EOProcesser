using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace EOProcesser
{
    public class EOCardManagerCardEffect : IEnumerable<EOCardManagerEffect>
    {
        public static EOCardManagerCardEffect Parse(ERAOCGCard card)
        {
            EOCardManagerCardEffect result = [];
            result.Id = card.Id.ToString();

            // 1. 处理说明文本，根据①②③④等符号分割效果
            var explanationFunc = card.GetCardExplanationFunc();
            if (explanationFunc == null)
            {
                throw new InvalidOperationException($"Card {card.Id} does not have an explanation function");
            }

            bool hasNumberedEffect = false;
            List<EOCardManagerEffect> effects = [];
            List<ERACode> currentDescriptionLines = [];
            List<ERACodeFuncSegment> ExtraFuncs = [];
            string? currentEffectNo = null;

            // 添加前缀描述
            foreach (var code in explanationFunc)
            {
                if (code is ERACodePrintLine printLine)
                {
                    string line = printLine.ToString();
                    if (line.Trim() == @"CALL TEXT_DECORATION(""ROGUE"")")
                    {
                        result.IsRogue = true;
                        continue;
                    }
                    
                    // 检查是否是带编号的效果行
                    bool isNumberedLine = false;
                    for (int i = 0; i < NumString.Length; i++)
                    {
                        if (line.Contains($"{NumString[i]}："))
                        {
                            hasNumberedEffect = true;
                            isNumberedLine = true;
                            
                            // 如果有之前的描述，将其作为一个效果添加
                            if (currentDescriptionLines.Count > 0)
                            {
                                effects.Add(new EOCardManagerEffect(
                                    currentEffectNo,
                                    currentDescriptionLines,
                                    null, // 条件会在后面填充
                                    [],[],[]
                                ));
                                
                                currentDescriptionLines = [];
                            }
                            
                            // 设置当前效果编号
                            currentEffectNo = NumString[i].ToString();
                            
                            // 移除编号，保留描述
                            string description = line.Replace($"PRINTL {NumString[i]}：", "PRINTL ");
                            currentDescriptionLines.Add(new ERACodePrintLine(description));
                            break;
                        }
                    }
                    
                    if (!isNumberedLine)
                    {
                        if (!hasNumberedEffect)
                        {
                            // 未找到编号效果前的行加入PrefixDescription
                            result.PrefixDescription.Add(code);
                        }
                        else
                        {
                            // 已有编号效果，继续添加到当前效果描述
                            currentDescriptionLines.Add(code);
                        }
                    }
                }
                else if (!hasNumberedEffect)
                {
                    // 非打印行且未找到编号效果，加入PrefixDescription
                    result.PrefixDescription.Add(code);
                }
                else
                {
                    // 非打印行且已找到编号效果，加入当前效果描述
                    currentDescriptionLines.Add(code);
                }
            }
            
            // 添加最后一个效果（如果有）
            if (currentDescriptionLines.Count > 0)
            {
                effects.Add(new EOCardManagerEffect(
                    currentEffectNo,
                    currentDescriptionLines,
                    null,
                    [],
                    [],
                    []
                ));
            }
            
            // 特殊情况：如果没有编号效果，将所有PRINTL行作为一个单独的效果
            if (!hasNumberedEffect)
            {
                var printLines = explanationFunc.Where(c => c is ERACodePrintLine).ToList();
                var nonPrintLines = result.PrefixDescription.Where(c => !(c is ERACodePrintLine)).ToList();
                
                // 前缀只保留非打印行
                result.PrefixDescription = nonPrintLines;
                
                // 所有打印行作为一个效果
                effects.Add(new EOCardManagerEffect(
                    null, // 自动编号
                    printLines,
                    null,[],[],[]
                ));
            }

            // 2. 处理条件和效果代码
            // 处理Can函数
            var canFunc = card.GetCardCanFunc();
            if (canFunc != null)
            {
                ProcessFunctionSegment(canFunc, effects, result.PrefixCan, true);
            }
            
            // 处理Effect函数
            var effectFunc = card.GetCardEffectFunc();
            if (effectFunc != null)
            {
                ProcessFunctionSegment(effectFunc, effects, result.PrefixEffect, false);
            }

            // 处理额外的函数
            var extraFuncs = card.GetExtraFuncs();
            if (extraFuncs.Count > 0)
            {
                // 暂时将所有额外函数添加到第一个效果中
                if (effects.Count > 0)
                {
                    ExtraFuncs.AddRange(extraFuncs);
                }
            }

            // 添加所有效果到结果
            foreach (var effect in effects)
            {
                result.Add(effect);
            }
            return result;
        }
        public string GetAllEffectFuncContent()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetExplanationFuncContent());
            sb.Append(GetCanFuncContent());
            sb.Append(GetEffectFuncContent());
            return sb.ToString();
        }

        private static void ProcessFunctionSegment(ERACodeFuncSegment func, List<EOCardManagerEffect> effects, List<ERACode> prefixCodes, bool isCan)
        {
            // 查找IF语句
            var ifSegments = func.Where(c => c is ERACodeIfSegment).Cast<ERACodeIfSegment>().ToList();
            
            if (ifSegments.Count > 1)
            {
                throw new InvalidOperationException($"Expected only one IF segment in function {func.FuncName}, but found {ifSegments.Count}");
            }
            
            // 如果没有IF语句，所有代码都是前缀
            if (ifSegments.Count == 0)
            {
                foreach (var code in func)
                {
                    prefixCodes.Add(code);
                }
                return;
            }
            
            // 提取IF语句前的代码作为前缀
            bool foundIf = false;
            foreach (var code in func)
            {
                if (code is ERACodeIfSegment)
                {
                    foundIf = true;
                    break;
                }
                
                if (!foundIf)
                {
                    prefixCodes.Add(code);
                }
            }
            
            // 处理IF语句
            var ifSegment = ifSegments[0];
            
            // 匹配效果数量
            int effectCount = effects.Count;
            int conditionCount = 1 + ifSegment.ElseSegments.Count; // IF + ELSEIF/ELSE
            
            // 确保条件和效果数量匹配
            while (effectCount < conditionCount)
            {
                // 需要创建新的效果条目
                effects.Add(new EOCardManagerEffect(
                    "", // 不编号且不计入编号
                    [],
                    null,
                    [],
                    [],
                    []
                ));
                effectCount++;
            }
            
            // 设置主IF条件和代码
            effects[0].Condition = ifSegment.Condition;
            if (isCan)
            {
                foreach (var code in ifSegment.codes)
                {
                    effects[0].CanFuncs.Add(code);
                }
            }
            else
            {
                foreach (var code in ifSegment.codes)
                {
                    effects[0].EffectFuncs.Add(code);
                }
            }
            
            // 设置ELSEIF/ELSE条件和代码
            for (int i = 0; i < ifSegment.ElseSegments.Count; i++)
            {
                var elseSegment = ifSegment.ElseSegments[i];
                effects[i + 1].Condition = elseSegment.Condition ?? "ELSE";
                
                if (isCan)
                {
                    foreach (var code in elseSegment.Codes)
                    {
                        effects[i + 1].CanFuncs.Add(code);
                    }
                }
                else
                {
                    foreach (var code in elseSegment.Codes)
                    {
                        effects[i + 1].EffectFuncs.Add(code);
                    }
                }
            }
        }

        public List<ERACode> PrefixEffect = [];
        public List<ERACode> PrefixCan = [];
        public bool IsRogue = false;
        public List<ERACode> PrefixDescription = [];
        private readonly List<EOCardManagerEffect> effects = [];
        public string Id = "";

        private const string NumString = "①②③④⑤⑥⑦⑧⑨⑩⑪⑫⑬⑭⑮⑯⑰⑱⑲⑳㉑㉒㉓㉔㉕㉖㉗㉘㉙㉚㉛㉜㉝㉞㉟㊱㊲㊳㊴㊵㊶㊷㊸㊹㊺㊻㊼㊽㊾㊿";

        public ERACodeMultiLines GetExplanationFuncContent()
        {
            ERACodeMultiLines lines = [];
            int index = -1;
            if (IsRogue)
            {
                lines.Add(@"CALL TEXT_DECORATION(""ROGUE"")");
            }
            lines.AddRange([..PrefixDescription]);
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
                foreach (ERACode effectLine in effect.Descriptions)
                {
                    lines.Add(effectLine.ToString());
                }
            }
            if (IsRogue)
            {
                lines.Add(@"CALL TEXT_DECORATION(""ROGUE"")");
            }
            return lines;
        }

        private string GetExtraLines(IEnumerable<ERACode> list)
        {
            StringBuilder sb = new();
            foreach(var code in list)
            {
                sb.AppendLine(code.ToString());
            }
            return sb.ToString();
        }
        private ERACodeIfSegment? GetEffectIfSegment()
        {
            if (effects.Count < 1)
            {
                return null;
            }
            ERACodeIfSegment segment = new(effects[0].Condition ?? "1 == 1", effects[0].EffectFuncs);
            for (int i = 1; i < effects.Count; i++)
            {
                segment.AddElseIf(effects[i].Condition ?? "1 == 1", effects[i].EffectFuncs);
            }
            return segment;
        }
        private ERACodeIfSegment? GetCanIfSegment()
        {
            if (effects.Count < 1)
            {
                return null;
            }
            ERACodeIfSegment segment = new(effects[0].Condition ?? "1 == 1", effects[0].CanFuncs);
            for (int i = 1; i < effects.Count; i++)
            {
                segment.AddElseIf(effects[i].Condition ?? "1 == 1", effects[i].CanFuncs);
            }
            return segment;
        }

        public ERACodeMultiLines GetCanFuncContent()
        {
            ERACodeMultiLines lines = ERACodeAnalyzer.AnalyzeCode($"""
                {GetExtraLines(PrefixEffect)}
                {GetEffectIfSegment()}
                """);
            if (Count == 0)
            {
                return lines;
            }
            ERACodeIfSegment segment = new(this[0].Condition ?? "1 == 1", this[0].CanFuncs);
            lines.Add(segment);
            return lines;
        }

        public ERACodeMultiLines GetEffectFuncContent()
        {
            ERACodeMultiLines lines = ERACodeAnalyzer.AnalyzeCode($"""
                {GetExtraLines(PrefixEffect)}
                {GetEffectIfSegment()}
                """);
            if (Count == 0)
            {
                return lines;
            }
            ERACodeIfSegment segment = new(this[0].Condition ?? "1 == 1", this[0].CanFuncs);
            lines.Add(segment);
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
        public EOCardManagerEffect(string? effectNo, IEnumerable<ERACode> desc, string? condition, IEnumerable<ERACode> canFuncs, IEnumerable<ERACode> effectFuncs, IEnumerable<ERACodeFuncSegment> extraFuncs)
        {
            EffectNo = effectNo;
            Descriptions = desc.ToList();
            Condition = condition;
            CanFuncs = new(canFuncs);
            EffectFuncs = new(effectFuncs);
            ExtraFuncs = extraFuncs.ToList();
        }
        public List<ERACode> Descriptions = [];
        public string? Condition = null;
        public ERACodeMultiLines CanFuncs;
        public ERACodeMultiLines EffectFuncs;

        //null → ①②③④⑤…自动； 非null → 强制使用该符号且不计入编号
        //"" → 不编号且不计入编号（用于一个效果多个触发条件）
        public string? EffectNo = null;

        //额外的functions
        public List<ERACodeFuncSegment> ExtraFuncs = [];

        public TreeNode[] GetTreeNodes()
        {
            // 创建父节点，使用效果编号或描述作为显示文本
            string displayText = EffectNo != null ? $"效果{EffectNo}" : "效果(自動数え)";
            TreeNode rootNode = new(displayText) { Tag = this };

            // 添加条件子节点
            TreeNode conditionNode = new($"条件: {Condition ?? "(無し)"}")
            {
                Tag = Condition
            };
            rootNode.Nodes.Add(conditionNode);

            // 添加效果描述子节点
            TreeNode descNode = new("効果文") { Tag = Descriptions };
            foreach (var desc in Descriptions)
            {
                descNode.Nodes.Add(new TreeNode(desc.ToString()) { Tag = desc });
            }
            rootNode.Nodes.Add(descNode);
            
            // 添加Can函数子节点
            TreeNode canFuncNode = new("効果可用性") { Tag = CanFuncs };
            foreach (var func in CanFuncs)
            {
                canFuncNode.Nodes.AddRange([.. func.GetTreeNodes()]);
            }
            rootNode.Nodes.Add(canFuncNode);
            
            // 添加Effect函数子节点
            TreeNode effectFuncNode = new("効果関数") { Tag = EffectFuncs };
            foreach (var func in EffectFuncs)
            {
                effectFuncNode.Nodes.AddRange([.. func.GetTreeNodes()]);
            }
            rootNode.Nodes.Add(effectFuncNode);
            
            // 添加额外函数子节点
            TreeNode extraFuncNode = new("追加関数") { Tag = ExtraFuncs };
            foreach (var func in ExtraFuncs)
            {
                extraFuncNode.Nodes.AddRange([..func.GetTreeNodes()]);
            }
            rootNode.Nodes.Add(extraFuncNode);
            
            return [rootNode];
        }
    }
}