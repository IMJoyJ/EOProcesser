using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Collections;
using System.Reflection.Emit;

namespace EOProcesser
{
    public abstract class ERACode
    {
        abstract public override string ToString();
        abstract public int Indentation { get; set; }
        abstract public List<TreeNode> GetTreeNodes();
    }

    public class ERACodeSegment : ERACode, IEnumerable<ERACode>
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
        internal ERACodeSegment? StartCode { get; set; }
        internal ERACodeSegment? EndCode { get; set; }
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

        public ERACodeSegment() { }
        public ERACodeSegment(IEnumerable<ERACode> initialCodes)
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
            sb.Append(string.Join("\r\n", codes.Select(code => code.ToString())));
            if (EndCode != null)
            {
                sb.Append(EndCode.ToString());
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
            ERACodeLine line = new(code);
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

        public IEnumerator<ERACode> GetEnumerator()
        {
            return GetEnumerator();
        }

        public override List<TreeNode> GetTreeNodes()
        {
            return [.. codes.SelectMany(code => code.GetTreeNodes())];
        }
    }

    public class ERABlockSegment : ERACodeSegment
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
        new readonly ERACodeSegment StartCode;
        public ERABlockSegment(string startCode, string? endCode = null, IEnumerable<ERACode>? initialCodes = null)
        {
            StartCode = [new ERACodeLine(startCode)];
            if (endCode != null)
            {
                EndCode = [new ERACodeLine(endCode)];
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
            StartCode = [new ERACodeLine(startCode)];
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
    }

    //Not supported lines. Same to base class ERACode.
    public class ERACodeLine(string codeLine) : ERACode
    {
        public string CodeLine = codeLine;

        public override int Indentation { get; set; } = 0;

        public override List<TreeNode> GetTreeNodes()
        {
            return [new TreeNode(CodeLine)
            {
                Tag = this
            }];
        }

        public override string ToString()
        {
            string indentation = new('\t', Indentation);
            return indentation + ToString().TrimStart();
        }
    }
    public class ERAFuncSegment : ERABlockSegment
    {
        new ERACodeSegment StartCode
        {
            get
            {
                return [new ERACodeLine($"@{FuncName}")];
            }
        }
        public string FuncName;
        public ERAFuncSegment(string funcName) : base($"@{funcName}")
        {
            FuncName = funcName;
        }

        public ERAFuncSegment(string funcName, IEnumerable<ERACode> codes)
            : base($"@{funcName}", codes)
        {
            FuncName = funcName;
        }
    }
    public class ERACodeSelectCase(string condition) : ERABlockSegment($"SELECTCASE {condition}", "ENDSELECT")
    {
        public string Condition { get; private set; } = condition;

        public void AddCase(string caseValue, string returnValue)
        {
            ERACodeSelectCaseSubCase subCase = new(caseValue, returnValue);
            subCase.Indentation = this.Indentation + 1;
            Add(subCase);
        }

        public void AddCase(string caseValue, ERACodeSegment codeSegment)
        {
            ERACodeSelectCaseSubCase subCase = new(caseValue, codeSegment);
            subCase.Indentation = this.Indentation + 1;
            Add(subCase);
        }
    }

    public class ERACodeSelectCaseSubCase : ERABlockSegment
    {
        public string CaseValue { get; private set; }

        public ERACodeSelectCaseSubCase(string caseValue)
            : base($"CASE {caseValue}")
        {
            CaseValue = caseValue;
        }

        public ERACodeSelectCaseSubCase(string caseValue, ERACodeSegment codeSegment)
            : base($"CASE {caseValue}")
        {
            CaseValue = caseValue;

            foreach (var code in codeSegment)
            {
                code.Indentation = this.Indentation + 1;
                Add(code);
            }
        }

        public ERACodeSelectCaseSubCase(string caseValue, string returnValue)
            : this($"CASE {caseValue}", [new ERACodeLine($"RETURN {returnValue}")]) { }
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
            Add(new ERACodeLine(code));
        }

        public ERACodeSIfSegment(string condition, ERACode code)
            : base($"SIF {condition}")
        {
            Condition = condition;
            Add(code);
        }
    }
    
}
