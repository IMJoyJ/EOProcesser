using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Collections;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace EOProcesser
{
    public abstract class ERACode
    {
        abstract public override string ToString();
        abstract public int Indentation { get; set; }
        abstract public List<TreeNode> GetTreeNodes();
    }

    public class ERACodeMultiLines : ERACode, IEnumerable<ERACode>
    {
        private int _indentation;
        public override int Indentation
        {
            get => _indentation;
            set
            {
                _indentation = value;
                foreach (var code in codes)
                {
                    code.Indentation = value;
                }
                if (StartCode != null)
                {
                    StartCode.Indentation = value;
                }
                if (EndCode != null)
                {
                    EndCode.Indentation = value;
                }
            }
        }

        internal readonly List<ERACode> codes = [];
        internal ERACodeMultiLines? StartCode { get; set; }
        internal ERACodeMultiLines? EndCode { get; set; }
        protected virtual string GetNodeText()
        {
            if (StartCode != null)
            {
                return StartCode.ToString();
            }
            if (codes.Count > 0)
            {
                return codes[0].ToString();
            }
            if (EndCode != null)
            {
                return EndCode.ToString();
            }
            return "(NONE)";
        }

        public ERACodeMultiLines() { }
        public ERACodeMultiLines(IEnumerable<ERACode> initialCodes)
        {
            AddRange(initialCodes);
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            if (StartCode != null)
            {
                sb.Append(StartCode.ToString());
            }
            sb.AppendLine(string.Join("\r\n", codes.Select(code => code.ToString())));
            if (EndCode != null)
            {
                sb.AppendLine(EndCode.ToString());
            }
            return sb.ToString();
        }

        public void Add(ERACode code)
        {
            code.Indentation = this.Indentation;
            codes.Add(code);
        }

        public void Add(string code)
        {
            var line = ERACodeLineFactory.CreateFromLine(code);
            line.Indentation = this.Indentation;
            codes.Add(line);
        }

        public void AddRange(IEnumerable<ERACode> codeCollection)
        {
            foreach (var code in codeCollection)
            {
                code.Indentation = this.Indentation;
                codes.Add(code);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (StartCode != null)
            {
                yield return StartCode;
            }

            foreach (var code in codes)
            {
                yield return code;
            }

            if (EndCode != null)
            {
                yield return EndCode;
            }
        }

        virtual public IEnumerator<ERACode> GetEnumerator()
        {
            if (StartCode != null)
            {
                yield return StartCode;
            }

            foreach (var code in codes)
            {
                yield return code;
            }

            if (EndCode != null)
            {
                yield return EndCode;
            }
        }

        public override List<TreeNode> GetTreeNodes()
        {
            return [.. codes.SelectMany(code => code.GetTreeNodes())];
        }

        internal void Remove(ERACode code)
        {
            if (code == StartCode || code == EndCode)
            {
                return;
            }
            codes.Remove(code);
        }
    }

    public class ERABlockSegment : ERACodeMultiLines
    {
        private int _indentation;
        public override int Indentation
        {
            get => _indentation;
            set
            {
                _indentation = value;
                foreach (var code in this)
                {
                    if (code != StartCode && code != EndCode)
                    {
                        code.Indentation = value + 1;
                    }
                    else
                    {
                        code.Indentation = value;
                    }
                }
            }
        }
        protected override string GetNodeText()
        {
            return StartCode!.ToString();
        }
        public ERABlockSegment(string startCode, string? endCode = null, IEnumerable<ERACode>? initialCodes = null)
        {

            StartCode = [ERACodeLineFactory.CreateFromLine(startCode)];
            if (endCode != null)
            {
                EndCode = [ERACodeLineFactory.CreateFromLine(endCode)];
            }
            if (initialCodes != null)
            {
                foreach (var code in initialCodes)
                {
                    code.Indentation = this.Indentation;
                    codes.Add(code);
                }
            }
        }

        public ERABlockSegment(string startCode, IEnumerable<ERACode> initialCodes) : base(initialCodes)
        {
            StartCode = [ERACodeLineFactory.CreateFromLine(startCode)];
            Indentation = 0; // Trigger the indentation setter
        }
        public override List<TreeNode> GetTreeNodes()
        {
            TreeNode rootNode = new(this.GetNodeText())
            {
                Tag = this
            };

            // Add all child nodes except StartCode and EndCode
            foreach (var code in this)
            {
                if (code != StartCode && code != EndCode)
                {
                    foreach (var node in code.GetTreeNodes())
                    {
                        rootNode.Nodes.Add(node);
                    }
                }
            }

            // DO NOT add EndCode as the last node

            return [rootNode];
        }

        public void RefreshFromChildTreeNodes(IEnumerable<TreeNode> nodes)
        {
            codes.Clear();
            foreach (var node in nodes)
            {
                if (node.Tag is ERACode code)
                {
                    codes.Add(code);
                }
            }
        }
    }
    public class ERACodeFuncSegment : ERABlockSegment
    {
        public string FuncName;
        public ERACodeFuncSegment(string funcName) : base($"@{funcName}")
        {
            FuncName = funcName;
        }

        public ERACodeFuncSegment(string funcName, IEnumerable<ERACode> codes)
            : base($"@{funcName}", codes)
        {
            FuncName = funcName;
        }
    }
    public class ERACodeSelectCase(string condition)
        : ERABlockSegment($"SELECTCASE {condition}", "ENDSELECT"),IEnumerable<ERACodeSelectCaseSubCase>
    {
        public string? GetValue(string c)
        {
            var caseSubCodes = codes.OfType<ERACodeSelectCaseSubCase>().ToList();
            foreach (var subCase in caseSubCodes)
            {
                if (subCase.CaseCondition == c)
                {
                    return subCase.GetValue();
                }
            }
            return null;
        }
        public Dictionary<string, string>? GetValueList(string c)
        {
            var caseSubCodes = codes.OfType<ERACodeSelectCaseSubCase>().ToList();
            foreach (var subCase in caseSubCodes)
            {
                if (subCase.CaseCondition == c)
                {
                    return subCase.GetValueList();
                }
            }
            return null;
        }
        public string Condition { get; private set; } = condition;

        public void AddCase(string caseValue, string returnValue)
        {
            ERACodeSelectCaseSubCase subCase = new(caseValue, returnValue)
            {
                Indentation = this.Indentation + 1
            };
            Add(subCase);
        }

        public void AddCase(ERACodeSelectCaseSubCase subCase)
        {
            Add(subCase);
        }

        public void AddCase(string? caseValue, ERACodeMultiLines codeSegment)
        {
            ERACodeSelectCaseSubCase subCase = new(caseValue, codeSegment)
            {
                Indentation = this.Indentation + 1
            };
            Add(subCase);
        }

        IEnumerator<ERACodeSelectCaseSubCase> IEnumerable<ERACodeSelectCaseSubCase>.GetEnumerator()
        {
            foreach(var code in this.codes)
            {
                if (code is ERACodeSelectCaseSubCase c)
                {
                    yield return c;
                }
            }
        }
    }

    public partial class ERACodeSelectCaseSubCase : ERABlockSegment
    {
        public string? CaseCondition { get; private set; }

        public ERACodeSelectCaseSubCase(string? caseCond)
            : base(caseCond == null ? $"CASEELSE" : $"CASE {caseCond}")
        {
            CaseCondition = caseCond;
        }

        public string? GetValue()
        {
            var valList = GetValueList();
            if (valList.Count > 1)
            {
                return string.Join(", ", valList.Select((vk) => $"{vk.Key} = {vk.Value}"));
            }
            if (valList.Count > 0)
            {
                return valList.ElementAt(0).Value;
            }
            return null;
        }

        public Dictionary<string, string> GetValueList()
        {
            string str = "";
            Dictionary<string, string> valList = [];
            for (int i = 0; i < codes.Count; i++)
            {
                string val = codes[i].ToString().TrimStart();
                if (val.StartsWith("RETURN "))
                {
                    str += $"({val[7..]})";
                    valList.Add("Value", str);
                    break;
                }
                var m = SubCaseValueRegex().Match(val);
                if (m.Success)
                {
                    if (string.IsNullOrEmpty(m.Groups[1].Value))
                    {
                        valList.Add("Value", m.Groups[2].Value.Trim());
                    }
                    else
                    {
                        valList[m.Groups[1].Value.Trim()] = m.Groups[2].Value.Trim();
                    }
                }
            }
            return valList;
        }

        protected override string GetNodeText()
        {
            var val = GetValue();
            string result = $"{CaseCondition ?? "CASEELSE"}";
            if (val != null)
            {
                result += $" ({val})";
            }
            return result;
        }

        public ERACodeSelectCaseSubCase(string? caseValue, ERACodeMultiLines codeSegment)
            : base(caseValue == null ? $"CASEELSE" : $"CASE {caseValue}")
        {
            CaseCondition = caseValue;

            foreach (var code in codeSegment)
            {
                code.Indentation = this.Indentation + 1;
                Add(code);
            }
        }

        public ERACodeSelectCaseSubCase(string caseValue, string returnValue)
            : this(caseValue, [ERACodeLineFactory.CreateFromLine($"RETURN {returnValue}")]) { }

        [GeneratedRegex(@"^RESULTS\s*(?::\s*(?<code>\d+)\s*=\s*|\s*=\s*)(?<text>.+)$")]
        private static partial Regex SubCaseValueRegex();
    }
    public class ERACodeSIfSegment : ERABlockSegment
    {
        public string Condition { get; private set; }

        public ERACodeSIfSegment(string condition)
            : base($"SIF {condition}")
        {
            Condition = condition;
        }

        public ERACodeSIfSegment(string condition, string code)
            : base($"SIF {condition}")
        {
            Condition = condition;
            Add(ERACodeLineFactory.CreateFromLine(code));
        }

        public ERACodeSIfSegment(string condition, ERACode code)
            : base($"SIF {condition}")
        {
            Condition = condition;
            Add(code);
        }
    }
    public class ERACodeIfSegment : ERABlockSegment, IEnumerable<ERACode>
    {
        private int _indentation;
        public override int Indentation
        {
            get => _indentation;
            set
            {
                _indentation = value;
                // 为IF区块内的所有代码设置缩进
                foreach (var code in codes)
                {
                    code.Indentation = value + 1;
                }
                // 为开始和结束标记设置缩进
                if (StartCode != null)
                {
                    StartCode.Indentation = value;
                }
                if (EndCode != null)
                {
                    EndCode.Indentation = value;
                }
                // 为所有ELSEIF和ELSE设置缩进
                foreach (var elseSegment in elseSegments)
                {
                    // ELSEIF/ELSE语句本身的缩进与IF相同
                    elseSegment.Indentation = value;
                    // ELSEIF/ELSE块内的代码缩进+1
                    foreach (var code in elseSegment.Codes)
                    {
                        code.Indentation = value + 1;
                    }
                }
            }
        }
        public string Condition { get; set; }
        private List<ERACodeElseSegment> elseSegments = [];

        // 用于获取elseSegments的属性
        public IReadOnlyList<ERACodeElseSegment> ElseSegments => elseSegments;

        public ERACodeIfSegment(string condition)
            : base($"IF {condition}", "ENDIF")
        {
            Condition = condition;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (StartCode != null)
            {
                yield return StartCode;
            }

            foreach (var code in codes)
            {
                yield return code;
            }

            foreach (var segment in elseSegments)
            {
                yield return segment;
            }

            if (EndCode != null)
            {
                yield return EndCode;
            }
        }

        override public IEnumerator<ERACode> GetEnumerator()
        {
            if (StartCode != null)
            {
                yield return StartCode;
            }

            foreach (var code in codes)
            {
                yield return code;
            }

            foreach (var segment in elseSegments)
            {
                yield return segment;
            }

            if (EndCode != null)
            {
                yield return EndCode;
            }
        }

        public ERACodeIfSegment(string condition, IEnumerable<ERACode> codeBlock)
            : base($"IF {condition}", "ENDIF", codeBlock)
        {
            Condition = condition;
        }

        public void AddElseIf(string condition, IEnumerable<ERACode> codeBlock)
        {
            ERACodeElseSegment elseIfSegment = new(condition, codeBlock)
            {
                Indentation = this.Indentation
            };
            elseSegments.Add(elseIfSegment);
        }

        public void AddElse(IEnumerable<ERACode> codeBlock)
        {
            ERACodeElseSegment elseSegment = new(null, codeBlock)
            {
                Indentation = this.Indentation
            };
            elseSegments.Add(elseSegment);
        }

        public override List<TreeNode> GetTreeNodes()
        {
            // 创建IF节点作为父节点
            TreeNode rootNode = new($"IF {Condition}")
            {
                Tag = this
            };

            // 添加IF主体代码作为子节点
            foreach (var code in codes)
            {
                foreach (var node in code.GetTreeNodes())
                {
                    rootNode.Nodes.Add(node);
                }
            }

            // 添加所有ELSEIF和ELSE块作为子节点
            foreach (var segment in elseSegments)
            {
                // 将ELSEIF/ELSE节点添加到IF节点
                rootNode.Nodes.AddRange([.. segment.GetTreeNodes()]);
            }

            return [rootNode];
        }
    }

    public class ERACodeElseSegment : ERABlockSegment
    {
        public string? Condition { get; private set; }
        
        // 添加获取内部代码的属性
        public IReadOnlyList<ERACode> Codes => codes;

        public ERACodeElseSegment(string? condition, IEnumerable<ERACode> codeBlock)
            : base(condition == null ? "ELSE" : $"ELSEIF {condition}", null, codeBlock)
        {
            Condition = condition;
        }

        protected override string GetNodeText()
        {
            return Condition == null ? "ELSE" : $"ELSEIF {Condition}";
        }
        
        // 由于ELSEIF/ELSE现在是IF的子节点，不需要单独生成节点树
        public override List<TreeNode> GetTreeNodes()
        {
            // 这个方法在新的结构中不会被调用，但为了完整性保留
            TreeNode rootNode = new(GetNodeText())
            {
                Tag = this
            };
            
            foreach (var code in codes)
            {
                foreach (var node in code.GetTreeNodes())
                {
                    rootNode.Nodes.Add(node);
                }
            }
            
            return [rootNode];
        }
    }

    public class ERACodeForSegment : ERABlockSegment
    {
        public string Counter { get; private set; }
        public string StartValue { get; private set; }
        public string EndValue { get; private set; }

        public ERACodeForSegment(string counter, string startValue, string endValue)
            : base($"FOR {counter},{startValue},{endValue}", "NEXT")
        {
            Counter = counter;
            StartValue = startValue;
            EndValue = endValue;
        }

        public ERACodeForSegment(string counter, string startValue, string endValue, IEnumerable<ERACode> codeBlock)
            : base($"FOR {counter},{startValue},{endValue}", "NEXT", codeBlock)
        {
            Counter = counter;
            StartValue = startValue;
            EndValue = endValue;
        }
    }
    public class ERACodeWhileSegment : ERABlockSegment
    {
        public string Condition { get; private set; }

        public ERACodeWhileSegment(string condition)
            : base($"WHILE {condition}", "WEND")
        {
            Condition = condition;
        }

        public ERACodeWhileSegment(string condition, IEnumerable<ERACode> codeBlock)
            : base($"WHILE {condition}", "WEND", codeBlock)
        {
            Condition = condition;
        }
    }
    public class ERACodeDoSegment : ERABlockSegment
    {
        public string Condition { get; private set; }

        public ERACodeDoSegment(string condition)
            : base("DO", $"LOOP {condition}")
        {
            Condition = condition;
        }

        public ERACodeDoSegment(string condition, IEnumerable<ERACode> codeBlock)
            : base("DO", $"LOOP {condition}", codeBlock)
        {
            Condition = condition;
        }
    }

    public class ERACodeRepeatSegment : ERABlockSegment
    {
        public string Condition { get; private set; }

        public ERACodeRepeatSegment(string condition)
            : base("REPEAT {condition}", $"REND")
        {
            Condition = condition;
        }

        public ERACodeRepeatSegment(string condition, IEnumerable<ERACode> codeBlock)
            : base("REPEAT {condition}", $"REND", codeBlock)
        {
            Condition = condition;
        }
    }
    public class ERACodeSkipSegment : ERABlockSegment,IEnumerable<ERACode>
    {
        private readonly List<string> commentLines = [];

        public ERACodeSkipSegment(IEnumerable<string> comments)
            : base("[SKIPSTART]", "[SKIPEND]")
        {
            commentLines.AddRange(comments);
        }

        public ERACodeSkipSegment(params string[] comments)
            : base("[SKIPSTART]", "[SKIPEND]")
        {
            commentLines.AddRange(comments);
        }

        // 重写ToString方法来原样输出注释内容，忽略缩进
        public override string ToString()
        {
            StringBuilder sb = new();

            // 添加起始标签，应用缩进
            string indentStr = new('\t', Indentation);
            sb.AppendLine($"{indentStr}[SKIPSTART]");

            // 添加注释行，原样输出不应用缩进
            foreach (var line in commentLines)
            {
                sb.AppendLine(line);
            }

            // 添加结束标签，应用缩进
            sb.Append($"{indentStr}[SKIPEND]");

            return sb.ToString();
        }

        // 重写GetEnumerator方法，使其只返回StartCode和EndCode，跳过注释内容
        override public IEnumerator<ERACode> GetEnumerator()
        {
            if (StartCode != null)
            {
                yield return StartCode;
            }

            // 注释行不作为ERACode单独存在，它们只在ToString()时被处理
            // 所以这里不需要迭代commentLines

            if (EndCode != null)
            {
                yield return EndCode;
            }
        }

        // 同时重写非泛型的GetEnumerator
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (StartCode != null)
            {
                yield return StartCode;
            }

            if (EndCode != null)
            {
                yield return EndCode;
            }
        }

        // 可能也需要重写GetTreeNodes方法，让它处理注释内容
        public override List<TreeNode> GetTreeNodes()
        {
            TreeNode rootNode = new("[SKIPSTART]")
            {
                Tag = this
            };

            foreach (var line in commentLines)
            {
                TreeNode commentNode = new(line)
                {
                    Tag = line
                };
                rootNode.Nodes.Add(commentNode);
            }

            return [rootNode];
        }
    }
}