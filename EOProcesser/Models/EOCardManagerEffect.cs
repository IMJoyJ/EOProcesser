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

            // 1. 处理说明文本，根据CALLFORM CARD_ABLE行分割效果
            var explanationFunc = card.GetCardExplanationFunc() ??
                throw new InvalidOperationException
                ($"Card {card.Id} does not have an explanation function");
            
            List<EOCardManagerEffect> effects = [];
            List<ERACode> currentDescriptionLines = [];
            string? currentEffectNo = null;
            bool hasFoundFirstCallform = false;
            int consecutiveNumberedLines = 0;
            
            // 添加前缀描述
            foreach (var code in explanationFunc)
            {
                if (code is ERACode codeLine)
                {
                    string line = codeLine.ToString();
                    
                    // 检查是否是ROGUE标记
                    if (line.Trim() == @"CALL TEXT_DECORATION(""ROGUE"")")
                    {
                        result.IsRogue = true;
                        continue;
                    }
                    
                    // 检查是否是CALLFORM CARD_ABLE行
                    if (line.Contains("CALLFORM CARD_ABLE"))
                    {
                        // 如果已经找到第一个CALLFORM，则创建新效果
                        if (hasFoundFirstCallform)
                        {
                            // 如果有之前的描述，将其作为一个效果添加
                            if (currentDescriptionLines.Count > 0)
                            {
                                effects.Add(new EOCardManagerEffect(
                                    currentEffectNo,
                                    currentDescriptionLines));
                                
                                currentDescriptionLines = [];
                                consecutiveNumberedLines = 0;
                            }
                        }
                        else
                        {
                            // 第一次找到CALLFORM，将之前的所有内容加入前缀
                            result.DescriptionDefinition.AddRange(currentDescriptionLines);
                            currentDescriptionLines = [];
                            hasFoundFirstCallform = true;
                        }
                        
                        // 添加当前CALLFORM行到新效果的描述中
                        currentDescriptionLines.Add(code);
                        continue;
                    }
                    
                    // 检查是否是带编号的效果行（如：①：、②：等）
                    bool isNumberedLine = false;
                    for (int i = 0; i < NumString.Length; i++)
                    {
                        if (line.Contains($"{NumString[i]}："))
                        {
                            isNumberedLine = true;
                            consecutiveNumberedLines++;
                            
                            // 如果已经连续找到两个编号行但未找到CALLFORM，则在这里切断
                            if (consecutiveNumberedLines >= 2 && currentDescriptionLines.Count > 0)
                            {
                                effects.Add(new EOCardManagerEffect(
                                    currentEffectNo,
                                    currentDescriptionLines));
                                
                                currentDescriptionLines = [];
                                consecutiveNumberedLines = 1; // 重置为1，因为当前行是新的编号行
                            }
                            
                            // 设置当前效果编号
                            currentEffectNo = NumString[i].ToString();
                            
                            // 添加当前行到描述中（保留编号）
                            currentDescriptionLines.Add(code);
                            break;
                        }
                    }
                    
                    if (!isNumberedLine)
                    {
                        // 检查是否是以任意字符+冒号开头的行
                        if (Regex.IsMatch(line, @"PRINTL .+："))
                        {
                            // 如果已经连续找到两个编号行但未找到CALLFORM，则在这里切断
                            if (consecutiveNumberedLines >= 2 && currentDescriptionLines.Count > 0)
                            {
                                effects.Add(new EOCardManagerEffect(
                                    currentEffectNo,
                                    currentDescriptionLines));
                                
                                currentDescriptionLines = [];
                                consecutiveNumberedLines = 0;
                            }
                        }
                        
                        // 不是编号行，添加到当前描述
                        if (!hasFoundFirstCallform)
                        {
                            // 未找到第一个CALLFORM前，添加到前缀
                            result.DescriptionDefinition.Add(code);
                        }
                        else
                        {
                            // 已找到第一个CALLFORM，添加到当前效果
                            currentDescriptionLines.Add(code);
                        }
                    }
                }
                else
                {
                    // 非打印行
                    if (!hasFoundFirstCallform)
                    {
                        // 未找到第一个CALLFORM前，添加到前缀
                        result.DescriptionDefinition.Add(code);
                    }
                    else
                    {
                        // 已找到第一个CALLFORM，添加到当前效果
                        currentDescriptionLines.Add(code);
                    }
                }
            }
            
            // 添加最后一个效果（如果有）
            if (currentDescriptionLines.Count > 0)
            {
                effects.Add(new EOCardManagerEffect(
                    currentEffectNo,
                    currentDescriptionLines));
            }

            // 2. 处理条件和效果代码
            // 处理Can函数
            var canFunc = card.GetCardCanFunc();
            if (canFunc != null)
            {
                ProcessFunctionSegment(canFunc, effects, result.CanDefinition, true);
            }
            
            // 处理Effect函数
            var effectFunc = card.GetCardEffectFunc();
            if (effectFunc != null)
            {
                ProcessFunctionSegment(effectFunc, effects, result.EffectDefinition, false);
            }

            // 处理额外的函数
            var extraFuncs = card.GetExtraFuncs();
            foreach (var extraFunc in extraFuncs)
            {
                result.ExtraFuncs.Add(extraFunc);
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
            StringBuilder sb = new();
            sb.Append(GetExplanationFuncContent());
            sb.Append(GetCanFuncContent());
            sb.Append(GetEffectFuncContent());
            return sb.ToString();
        }

        public List<TreeNode> GetTreeNodes()
        {
            // 创建根节点
            TreeNode rootNode = new($"カード効果 [{Id}]") { Tag = this };
            
            // 効果文定義（DescriptionDefinition）
            TreeNode descDefNode = new("効果文定義") { Tag = DescriptionDefinition };
            foreach (var code in DescriptionDefinition)
            {
                descDefNode.Nodes.Add(new TreeNode(code.ToString()) { Tag = code });
            }
            rootNode.Nodes.Add(descDefNode);
            
            // 効果可用性定義（CanDefinition）
            TreeNode canDefNode = new("効果可用性定義") { Tag = CanDefinition };
            foreach (var code in CanDefinition)
            {
                canDefNode.Nodes.Add(new TreeNode(code.ToString()) { Tag = code });
            }
            rootNode.Nodes.Add(canDefNode);
            
            // 効果関数定義（EffectDefinition）
            TreeNode effectDefNode = new("効果関数定義") { Tag = EffectDefinition };
            foreach (var code in EffectDefinition)
            {
                effectDefNode.Nodes.Add(new TreeNode(code.ToString()) { Tag = code });
            }
            rootNode.Nodes.Add(effectDefNode);
            
            // 追加関数（ExtraFuncs）
            TreeNode extraFuncsNode = new("追加関数") { Tag = ExtraFuncs };
            foreach (var func in ExtraFuncs)
            {
                extraFuncsNode.Nodes.Add(new TreeNode(func.FuncName) { Tag = func });
            }
            rootNode.Nodes.Add(extraFuncsNode);
            
            // 効果一覧（遍历effects的GetTreeNodes()追加为子节点）
            TreeNode effectsListNode = new("効果一覧") { Tag = effects };
            foreach (var effect in effects)
            {
                // 为每个效果添加子节点
                effectsListNode.Nodes.AddRange(effect.GetTreeNodes());
            }
            rootNode.Nodes.Add(effectsListNode);
            
            return [rootNode];
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
                effects.Add(new EOCardManagerEffect());
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

        public bool IsRogue = false;
        List<ERACode> EffectDefinition = [];
        List<ERACode> CanDefinition = [];
        public List<ERACode> DescriptionDefinition = [];
        private readonly List<EOCardManagerEffect> effects = [];
        HashSet<ERACodeFuncSegment> ExtraFuncs = [];
        public string Id = "";

        public const string NumString = "①②③④⑤⑥⑦⑧⑨⑩⑪⑫⑬⑭⑮⑯⑰⑱⑲⑳㉑㉒㉓㉔㉕㉖㉗㉘㉙㉚㉛㉜㉝㉞㉟㊱㊲㊳㊴㊵㊶㊷㊸㊹㊺㊻㊼㊽㊾㊿";

        public ERACodeMultiLines GetExplanationFuncContent()
        {
            ERACodeMultiLines lines = [];
            int index = -1;
            if (IsRogue)
            {
                lines.Add(@"CALL TEXT_DECORATION(""ROGUE"")");
            }
            lines.AddRange([..DescriptionDefinition]);
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
                {GetExtraLines(EffectDefinition)}
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
                {GetExtraLines(EffectDefinition)}
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

    public class EOCardManagerEffect(string? effectNo = "", IEnumerable<ERACode>? desc = null, string? condition = null, IEnumerable<ERACode>? canFuncs = null, IEnumerable<ERACode>? effectFuncs = null)
    {
        //効果文
        public List<ERACode> Descriptions = [.. desc ?? []];
        public string? Condition = condition;
        
        public ERACodeMultiLines CanFuncs = [.. canFuncs ?? []];
        public ERACodeMultiLines EffectFuncs = [.. effectFuncs ?? []];

        public ERACodeMultiLines GetCardDescriptionSegment(ref int currentEffectNoIndex)
        {
            ERACodeMultiLines result = [];
            
            // 处理编号逻辑
            string effectNoPrefix = "";
            if (EffectNo == null)
            {
                // 自动编号，使用当前索引并递增
                effectNoPrefix = $"{EOCardManagerCardEffect.NumString[currentEffectNoIndex]}：";
                currentEffectNoIndex++;
            }
            else if (EffectNo != "")
            {
                // 指定编号，使用指定的编号并且不递增currentEffectNoIndex
                effectNoPrefix = $"{EffectNo}：";
                
                // 如果指定的编号是带圈数字，更新currentEffectNoIndex为对应的值+1
                int index = EOCardManagerCardEffect.NumString.IndexOf(EffectNo);
                if (index >= 0 && index + 1 > currentEffectNoIndex)
                {
                    currentEffectNoIndex = index + 1;
                }
            }
            // 如果EffectNo为空字符串，则不添加编号前缀，也不递增currentEffectNoIndex
            
            // 将编号添加到第一个PRINTL行
            bool prefixAdded = false;
            foreach (var desc in Descriptions)
            {
                if (!prefixAdded && desc is ERACodePrintLine printLine && effectNoPrefix != "")
                {
                    // 创建新的打印行，添加编号前缀
                    string originalLine = printLine.ToString();
                    string newLine;
                    
                    if (originalLine.Trim().StartsWith("PRINTL "))
                    {
                        // 如果已经有内容，在内容前添加编号
                        newLine = originalLine.Replace("PRINTL ", $"PRINTL {effectNoPrefix}：");
                    }
                    else
                    {
                        // 如果是空PRINTL，直接添加编号
                        newLine = originalLine.Replace("PRINTL", $"PRINTL {effectNoPrefix}：");
                    }
                    
                    result.Add(ERACodeLineFactory.CreateFromLine(newLine));
                    prefixAdded = true;
                }
                else
                {
                    // 直接添加原始行
                    result.Add(desc);
                }
            }
            
            return result;
        }
        
        public ERACodeMultiLines GetCardCanSegment(ref int currentEffectNoIndex)
        {
            return CanFuncs;
        }

        public ERACodeMultiLines GetCardEffectSegment(ref int currentEffectNoIndex)
        {
            return EffectFuncs;
        }

        //null → ①②③④⑤…自动； 非null → 强制使用该符号且不计入编号
        //"" → 不编号且不计入编号（用于一个效果多个触发条件）
        public string? EffectNo = effectNo;
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

            return [rootNode];
        }
    }
}