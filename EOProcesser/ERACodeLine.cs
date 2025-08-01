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
        
        // 每个子类必须尝试解析给定的文本行
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
            return indentation + CodeLine.TrimStart();
        }
    }

    public class ERACodeGenericLine(string codeLine) : ERACodeLine(codeLine)
    {
        public override bool CanParse(string line) => true; // 通用行总是可以解析
    }

    public class ERACodeCommentLine : ERACodeLine
    {
        private static readonly Regex _pattern = new(@"^\s*;(.*)$");
        
        public string Comment { get; private set; }

        public ERACodeCommentLine(string codeLine) : base(codeLine)
        {
            var match = _pattern.Match(codeLine);
            if (!match.Success)
            {
                throw new FormatException("Not a valid comment line.");
            }
            
            Comment = match.Groups[1].Value;
        }

        public override bool CanParse(string line)
        {
            return _pattern.IsMatch(line);
        }

        public override string GetNodeText() => $"(コメント){Comment}";
    }

    public class ERACodePrintLine : ERACodeLine
    {
        private static readonly Regex _pattern = new(@"^\s*PRINT([A-Z_]+)?\s+(.*)$");
        
        public string? PrintType { get; private set; }
        public string? Content { get; private set; }

        public ERACodePrintLine(string codeLine) : base(codeLine)
        {
            var match = _pattern.Match(codeLine);
            if (!match.Success)
            {
                throw new FormatException("Not a valid PRINT line.");
            }
            
            PrintType = match.Groups[1].Success ? match.Groups[1].Value : null;
            Content = match.Groups[2].Success ? match.Groups[2].Value : null;
        }

        public override bool CanParse(string line)
        {
            return _pattern.IsMatch(line);
        }

        public override string GetNodeText() => $"(表示:{PrintType}) {Content ?? ""}";
    }

    public class ERACodeDimLine : ERACodeLine
    {
        private static readonly Regex _pattern = new(@"^\s*#DIM\s+(?:(DYNAMIC|SAVEDATA)\s+)?(\S.*)$");
        
        public string VarName { get; private set; }
        public string? VarScope { get; private set; }

        public ERACodeDimLine(string codeLine) : base(codeLine)
        {
            var match = _pattern.Match(codeLine);
            if (!match.Success)
            {
                throw new FormatException("Not a valid #DIM line.");
            }
            
            VarScope = match.Groups[1].Success ? match.Groups[1].Value : null;
            VarName = match.Groups[2].Value.Trim();
            
            if (string.IsNullOrWhiteSpace(VarName))
            {
                throw new FormatException("Invalid #DIM format. Variable name cannot be empty.");
            }
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
    }

    public class ERACodeDimsLine : ERACodeLine
    {
        private static readonly Regex _pattern = new(@"^\s*#DIMS\s+(?:(DYNAMIC|SAVEDATA)\s+)?(\S+?)(?:\s*,\s*(\d+))?\s*$");
        
        public string VarName { get; private set; }
        public string? VarScope { get; private set; }
        public int? ArrayLength { get; private set; }

        public ERACodeDimsLine(string codeLine) : base(codeLine)
        {
            var match = _pattern.Match(codeLine);
            if (!match.Success)
            {
                throw new FormatException("Not a valid #DIMS line.");
            }
            
            VarScope = match.Groups[1].Success ? match.Groups[1].Value : null;
            VarName = match.Groups[2].Value;
            ArrayLength = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : null;
            
            if (string.IsNullOrWhiteSpace(VarName))
            {
                throw new FormatException("Invalid #DIMS format. Variable name cannot be empty.");
            }
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
    }

    public class ERACodeGotoLine : ERACodeLine
    {
        private static readonly Regex _pattern = new(@"^\s*GOTO[ ]+(.+)$");
        
        public string LabelName { get; private set; }

        public ERACodeGotoLine(string codeLine) : base(codeLine)
        {
            var match = _pattern.Match(codeLine);
            if (!match.Success)
            {
                throw new FormatException("Not a valid GOTO line.");
            }
            
            LabelName = match.Groups[1].Value.Trim();
            
            if (string.IsNullOrWhiteSpace(LabelName))
            {
                throw new FormatException("Invalid GOTO format. Label name cannot be empty.");
            }
        }

        public override bool CanParse(string line)
        {
            return _pattern.IsMatch(line);
        }

        public override string GetNodeText() => $"(跳躍)→{LabelName}";
    }

    public class ERACodeLabelLine : ERACodeLine
    {
        private static readonly Regex _pattern = new(@"^\s*\$(.+)$");
        
        public string LabelName { get; private set; }

        public ERACodeLabelLine(string codeLine) : base(codeLine)
        {
            var match = _pattern.Match(codeLine);
            if (!match.Success)
            {
                throw new FormatException("Not a valid label line.");
            }
            
            LabelName = match.Groups[1].Value.Trim();
            
            if (string.IsNullOrWhiteSpace(LabelName))
            {
                throw new FormatException("Invalid label format. Label name cannot be empty.");
            }
        }

        public override bool CanParse(string line)
        {
            return _pattern.IsMatch(line);
        }

        public override string GetNodeText() => $"(識別子){LabelName}";
    }

    

    // 工厂类用于创建适当的ERACodeLine实例
    public static class ERACodeLineFactory
    {
        // 使用反射获取所有ERACodeLine的子类
        private static readonly Lazy<List<Type>> _codeLineTypesLazy = new(() =>
        {
            return [.. typeof(ERACodeLine).Assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(ERACodeLine).IsAssignableFrom(t))
                .OrderBy(t => t == typeof(ERACodeGenericLine) ? int.MaxValue : 0)];
        });

        private static List<Type> _codeLineTypes => _codeLineTypesLazy.Value;

        public static ERACodeLine CreateFromLine(string line)
        {
            // 尝试使用每个已知的代码行类型来解析
            foreach (var type in _codeLineTypes)
            {
#pragma warning disable CS8603 // 可能返回 null 引用。
#pragma warning disable CS8602 // 解引用可能出现空引用。
#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
                try
                {
                    // 创建实例来检查是否可以解析
                    var instance = (ERACodeLine)Activator.CreateInstance(type, line);

                    // 如果是通用类型，直接返回
                    if (type == typeof(ERACodeGenericLine))
                    {
                        return instance;
                    }

                    // 对于其他类型，检查是否可以解析
                    if (instance.CanParse(line))
                    {
                        return instance;
                    }
                }
#pragma warning restore CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
#pragma warning restore CS8602 // 解引用可能出现空引用。
#pragma warning restore CS8603 // 可能返回 null 引用。
                catch
                {
                    // 如果实例化失败，继续尝试下一个类型
                    continue;
                }
            }

            // 如果没有匹配的特定类型，则返回通用行
            return new ERACodeGenericLine(line);
        }
    }
}