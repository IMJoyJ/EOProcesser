using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EOProcesser
{
    //Base ERA code lines.
    public abstract class ERACodeLine : ERACode
    {
        public string CodeLine { get; protected set; }

        protected ERACodeLine(string codeLine)
        {
            CodeLine = codeLine;
        }

        public abstract bool TryParse(string line, out ERACodeLine? result);

        public override int Indentation { get; set; } = 0;

        public virtual string GetNodeText() => CodeLine;

        public override List<TreeNode> GetTreeNodes()
        {
            return [new TreeNode(GetNodeText())
            {
                Tag = this
            }];
        }

        public override string ToString()
        {
            string indentation = new('\t', Indentation);
            return indentation + CodeLine.TrimStart();
        }
    }

    public class ERACodeGenericLine : ERACodeLine
    {
        public ERACodeGenericLine(string codeLine) : base(codeLine) { }

        public override bool TryParse(string line, out ERACodeLine? result)
        {
            // 通用行总是能解析
            result = new ERACodeGenericLine(line);
            return true;
        }
    }

    public class ERACodeCommentLine : ERACodeLine
    {
        private static readonly Regex _pattern = new(@"^\s*;(.*)$");
        
        public string Comment { get; private set; }

        public ERACodeCommentLine(string codeLine, string comment) : base(codeLine)
        {
            Comment = comment;
        }

        public override bool TryParse(string line, out ERACodeLine? result)
        {
            result = null;
            
            var match = _pattern.Match(line);
            if (match.Success)
            {
                result = new ERACodeCommentLine(line, match.Groups[1].Value);
                return true;
            }
            
            return false;
        }

        public override string GetNodeText() => $"(コメント){Comment}";
    }

    public class ERACodePrintLineLine : ERACodeLine
    {
        private static readonly Regex _pattern = new(@"^\s*PRINTL(?:\s+(.+))?$");
        
        public string? LineContent { get; private set; }

        public ERACodePrintLineLine(string codeLine, string? lineContent) : base(codeLine)
        {
            LineContent = lineContent;
        }

        public override bool TryParse(string line, out ERACodeLine? result)
        {
            result = null;
            
            var match = _pattern.Match(line);
            if (match.Success)
            {
                string? content = match.Groups[1].Success ? match.Groups[1].Value : null;
                result = new ERACodePrintLineLine(line, content);
                return true;
            }
            
            return false;
        }

        public override string GetNodeText() => LineContent == null ? "(空行出力)" : $"(行出力){LineContent}";
    }

    public class ERACodeDimLine : ERACodeLine
    {
        private static readonly Regex _pattern = new(@"^\s*#DIM\s+(?:(DYNAMIC|SAVEDATA)\s+)?(\S.*)$");
        
        public string VarName { get; private set; }
        public string? VarScope { get; private set; }

        public ERACodeDimLine(string codeLine, string varName, string? varScope) : base(codeLine)
        {
            VarName = varName;
            VarScope = varScope;
        }

        public override bool TryParse(string line, out ERACodeLine? result)
        {
            result = null;
            
            var match = _pattern.Match(line);
            if (match.Success)
            {
                string? scope = match.Groups[1].Success ? match.Groups[1].Value : null;
                string varName = match.Groups[2].Value.Trim();
                
                if (string.IsNullOrWhiteSpace(varName))
                {
                    return false;
                }
                
                result = new ERACodeDimLine(line, varName, scope);
                return true;
            }
            
            return false;
        }

        public override string GetNodeText()
        {
            string scopeText = VarScope switch
            {
                "DYNAMIC" => "(ローカル)",
                "SAVEDATA" => "(セーブデータ)",
                _ => "(グローバル)"
            };
            return $"(変数宣言){scopeText}{VarName}";
        }
    }

    public class ERACodeDimsLine : ERACodeLine
    {
        private static readonly Regex _pattern = new(@"^\s*#DIMS\s+(?:(DYNAMIC|SAVEDATA)\s+)?(\S+?)(?:\s*,\s*(\d+))?\s*$");
        
        public string VarName { get; private set; }
        public string? VarScope { get; private set; }
        public int? ArrayLength { get; private set; }

        public ERACodeDimsLine(string codeLine, string varName, string? varScope, int? arrayLength) : base(codeLine)
        {
            VarName = varName;
            VarScope = varScope;
            ArrayLength = arrayLength;
        }

        public override bool TryParse(string line, out ERACodeLine? result)
        {
            result = null;
            
            var match = _pattern.Match(line);
            if (match.Success)
            {
                string? scope = match.Groups[1].Success ? match.Groups[1].Value : null;
                string varName = match.Groups[2].Value;
                int? length = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : null;
                
                result = new ERACodeDimsLine(line, varName, scope, length);
                return true;
            }
            
            return false;
        }

        public override string GetNodeText()
        {
            string scopeText = VarScope switch
            {
                "DYNAMIC" => "(ローカル)",
                "SAVEDATA" => "(セーブデータ)",
                _ => "(グローバル)"
            };

            string lengthText = ArrayLength.HasValue ? $"[{ArrayLength}]" : "[]";

            return $"(文字列宣言){scopeText}{VarName}{lengthText}";
        }
    }
    
    // 工厂类用于创建适当的ERACodeLine实例
    public static class ERACodeLineFactory
    {
        private static readonly List<ERACodeLine> _prototypes = new()
        {
            new ERACodeCommentLine("", ""),
            new ERACodePrintLineLine("", null),
            new ERACodeDimLine("", "", null),
            new ERACodeDimsLine("", "", null, null)
        };
        
        public static ERACodeLine CreateFromLine(string line)
        {
            foreach (var prototype in _prototypes)
            {
                if (prototype.TryParse(line, out var result) && result != null)
                {
                    return result;
                }
            }
            
            // 如果没有匹配的特定类型，则返回通用行
            return new ERACodeGenericLine(line);
        }
        
        // 注册新的代码行类型
        public static void RegisterCodeLineType(ERACodeLine prototype)
        {
            _prototypes.Add(prototype);
        }
    }
}