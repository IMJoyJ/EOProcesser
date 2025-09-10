using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;

namespace EOProcesser
{
    //Base ERA code lines.
    public abstract class ERACodeLine : ERACode
    {
        public string CodeLine { get; protected set; }

        // 受保护的构造函数，只能由子类调用
        protected ERACodeLine(string codeLine)
        {
            CodeLine = codeLine;
        }
        
        // 静态创建方法，用于尝试从字符串创建特定类型的代码行
        // 如果不匹配任何格式，则返回通用行
        public static ERACodeLine Parse(string line)
        {
            return ERACodeLineFactory.CreateFromLine(line);
        }
        
        // 修改接口：添加尝试解析方法以避免异常
        public abstract bool TryParse(string line, out ERACodeLine? result);
        
        // 保留兼容性
        public abstract bool CanParse(string line);

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
            return indentation + CodeLine.TrimStart() + "\r\n";
        }
    }

    public class ERACodeGenericLine : ERACodeLine
    {
        public ERACodeGenericLine(string codeLine) : base(codeLine) { }
        
        public override bool TryParse(string line, out ERACodeLine? result)
        {
            result = new ERACodeGenericLine(line);
            return true; // 通用行总是能解析
        }
        
        public override bool CanParse(string line) => true; // 通用行总是可以解析
    }

    public partial class ERACodeCommentLine : ERACodeLine
    {
        private static readonly Regex _pattern = CommentRegex();
        
        public string Comment { get; private set; }

        public ERACodeCommentLine(string codeLine) : base(codeLine)
        {
            var match = _pattern.Match(codeLine);
            Comment = match.Groups[1].Value;
        }
        
        public override bool TryParse(string line, out ERACodeLine? result)
        {
            result = null;
            var match = _pattern.Match(line);
            if (!match.Success) return false;
            
            result = new ERACodeCommentLine(line);
            return true;
        }

        public override bool CanParse(string line)
        {
            return _pattern.IsMatch(line);
        }

        public override string GetNodeText() => $"(コメント){Comment}";
        [GeneratedRegex(@"^\s*;(.*)$", RegexOptions.Compiled)]
        private static partial Regex CommentRegex();
    }

    public partial class ERACodePrintLine : ERACodeLine
    {
        private static readonly Regex _pattern = PrintRegex();
        private static readonly Regex _printlPattern = PrintLRegex();
        
        public string? PrintType { get; private set; }
        public string? Content { get; private set; }

        public ERACodePrintLine(string codeLine) : base(codeLine)
        {
            if (_printlPattern.IsMatch(codeLine))
            {
                PrintType = "L";
                Content = null;
                return;
            }
            
            var match = _pattern.Match(codeLine);
            PrintType = match.Groups[1].Success ? match.Groups[1].Value : null;
            Content = match.Groups[2].Success ? match.Groups[2].Value : null;
        }
        
        public override bool TryParse(string line, out ERACodeLine? result)
        {
            result = null;
            
            if (_printlPattern.IsMatch(line))
            {
                result = new ERACodePrintLine(line);
                return true;
            }
            
            var match = _pattern.Match(line);
            if (!match.Success) return false;
            
            result = new ERACodePrintLine(line);
            return true;
        }

        public override bool CanParse(string line)
        {
            return _pattern.IsMatch(line) || _printlPattern.IsMatch(line);
        }

        public override string GetNodeText() => $"(PRINT{PrintType}) {Content ?? ""}";
        [GeneratedRegex(@"^\s*PRINT([A-Z_]+)?\s+(.*)$", RegexOptions.Compiled)]
        private static partial Regex PrintRegex();
        [GeneratedRegex(@"^\s*PRINTL\s*$", RegexOptions.Compiled)]
        private static partial Regex PrintLRegex();
    }

    public partial class ERACodeDimLine : ERACodeLine
    {
        private static readonly Regex _pattern = DimRegex();
        
        public string VarName { get; private set; }
        public string? VarScope { get; private set; }

        public ERACodeDimLine(string codeLine) : base(codeLine)
        {
            var match = _pattern.Match(codeLine);
            VarScope = match.Groups[1].Success ? match.Groups[1].Value : null;
            VarName = match.Groups[2].Value.Trim();
        }
        
        public override bool TryParse(string line, out ERACodeLine? result)
        {
            result = null;
            var match = _pattern.Match(line);
            if (!match.Success) return false;
            
            var varName = match.Groups[2].Value.Trim();
            if (string.IsNullOrWhiteSpace(varName)) return false;
            
            result = new ERACodeDimLine(line);
            return true;
        }

        public override bool CanParse(string line)
        {
            return _pattern.IsMatch(line);
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

        [GeneratedRegex(@"^\s*#DIM\s+(?:(DYNAMIC|SAVEDATA)\s+)?(\S.*)$", RegexOptions.Compiled)]
        private static partial Regex DimRegex();
    }

    public partial class ERACodeDimsLine : ERACodeLine
    {
        private static readonly Regex _pattern = DimsRegex();
        
        public string VarName { get; private set; }
        public string? VarScope { get; private set; }
        public int? ArrayLength { get; private set; }

        public ERACodeDimsLine(string codeLine) : base(codeLine)
        {
            var match = _pattern.Match(codeLine);
            VarScope = match.Groups[1].Success ? match.Groups[1].Value : null;
            VarName = match.Groups[2].Value;
            ArrayLength = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : null;
        }
        
        public override bool TryParse(string line, out ERACodeLine? result)
        {
            result = null;
            var match = _pattern.Match(line);
            if (!match.Success) return false;
            
            var varName = match.Groups[2].Value;
            if (string.IsNullOrWhiteSpace(varName)) return false;
            
            result = new ERACodeDimsLine(line);
            return true;
        }

        public override bool CanParse(string line)
        {
            return _pattern.IsMatch(line);
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

        [GeneratedRegex(@"^\s*#DIMS\s+(?:(DYNAMIC|SAVEDATA)\s+)?(\S+?)(?:\s*,\s*(\d+))?\s*$", RegexOptions.Compiled)]
        private static partial Regex DimsRegex();
    }

    public partial class ERACodeGotoLine : ERACodeLine
    {
        private static readonly Regex _pattern = GotoRegex();
        
        public string LabelName { get; private set; }

        public ERACodeGotoLine(string codeLine) : base(codeLine)
        {
            var match = _pattern.Match(codeLine);
            LabelName = match.Groups[1].Value.Trim();
        }
        
        public override bool TryParse(string line, out ERACodeLine? result)
        {
            result = null;
            var match = _pattern.Match(line);
            if (!match.Success) return false;
            
            var labelName = match.Groups[1].Value.Trim();
            if (string.IsNullOrWhiteSpace(labelName)) return false;
            
            result = new ERACodeGotoLine(line);
            return true;
        }

        public override bool CanParse(string line)
        {
            return _pattern.IsMatch(line);
        }

        public override string GetNodeText() => $"(跳躍)→{LabelName}";
        [GeneratedRegex(@"^\s*GOTO[ ]+(.+)$", RegexOptions.Compiled)]
        private static partial Regex GotoRegex();
    }

    public partial class ERACodeLabelLine : ERACodeLine
    {
        private static readonly Regex _pattern = LabelRegex();
        
        public string LabelName { get; private set; }

        public ERACodeLabelLine(string codeLine) : base(codeLine)
        {
            var match = _pattern.Match(codeLine);
            LabelName = match.Groups[1].Value.Trim();
        }
        
        public override bool TryParse(string line, out ERACodeLine? result)
        {
            result = null;
            var match = _pattern.Match(line);
            if (!match.Success) return false;
            
            var labelName = match.Groups[1].Value.Trim();
            if (string.IsNullOrWhiteSpace(labelName)) return false;
            
            result = new ERACodeLabelLine(line);
            return true;
        }

        public override bool CanParse(string line)
        {
            return _pattern.IsMatch(line);
        }

        public override string GetNodeText() => $"(識別子){LabelName}";
        [GeneratedRegex(@"^\s*\$(.+)$", RegexOptions.Compiled)]
        private static partial Regex LabelRegex();
    }

    // 工厂类用于创建适当的ERACodeLine实例
    public static class ERACodeLineFactory
    {
        // 使用委托存储解析函数
        private delegate bool TryParseDelegate(string line, out ERACodeLine? result);
        
        // 存储所有解析函数的列表
        private static readonly Lazy<List<TryParseDelegate>> _parsersLazy = new(() =>
        {
            var parsers = new List<TryParseDelegate>
            {
                // 按特定顺序添加解析函数，确保更具体的模式先匹配
                new ERACodeCommentLine(string.Empty).TryParse,
                new ERACodePrintLine(string.Empty).TryParse,
                new ERACodeDimLine(string.Empty).TryParse,
                new ERACodeDimsLine(string.Empty).TryParse,
                new ERACodeGotoLine(string.Empty).TryParse,
                new ERACodeLabelLine(string.Empty).TryParse,

                // 通用行解析函数放在最后
                new ERACodeGenericLine(string.Empty).TryParse
            };
            
            return parsers;
        });
        
        private static List<TryParseDelegate> _parsers => _parsersLazy.Value;

        public static ERACodeLine CreateFromLine(string line)
        {
            // 尝试使用每个解析函数
            foreach (var parser in _parsers)
            {
                if (parser(line, out var result) && result != null)
                {
                    return result;
                }
            }
            
            // 如果所有解析器都失败（不应该发生，因为通用解析器总是成功），返回通用行
            return new ERACodeGenericLine(line);
        }
    }
}