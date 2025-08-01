using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace EOProcesser
{
    public class ERAOCGCard
    {
        public string Name = "";
        public int Id = -1;
        public List<ERAOCGCardDescription> Card = [];
        public List<ERAOCGCardEffect> Effects = [];

        override public string ToString()
        {
            return $"[{Id}]{Name}";
        }
    }
    

    //对应@CARD_(id)处理。
    public class ERAOCGCardInfo
    {
        public List<ERAOCGCardInfoField> Fields = [];

        public ERACodeSelectCase ToSelectCase()
        {
            ERACodeSelectCase sel = new("参照先");
            foreach (var f in Fields)
            {
                sel.AddCase(f.ToSubCase());
            }
            return sel;
        }

        public string GetValue(string name)
        {
            foreach (var field in Fields)
            {
                if (field.Name == name)
                {
                    return field.GetValue()?.ToString() ?? "";
                }
            }
            throw new KeyNotFoundException($"Field '{name}' not found.");
        }
    }
    
    public abstract class ERAOCGCardInfoField
    {
        // 字段名
        public string Name { get; }

        protected ERAOCGCardInfoField(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 对外同一入口，返回 object，
        /// 调用方可以再做类型判断或模式匹配
        /// </summary>
        public abstract object GetValue();

        public override string ToString() => $"{Name}: {GetValue()}";

        public virtual ERACodeSelectCaseSubCase ToSubCase()
        {
            // 创建一个新的子案例
            return new(Name, GetValue()?.ToString() ?? "");
        }
    }

    public abstract class ERAOCGCardInfoField<T>(string name, T value) : ERAOCGCardInfoField(name)
    {
        public T Value { get; set; } = value;

        // 用 object 实现父类抽象方法
        public override object GetValue() => Value!;
    }

    //数字
    public class ERAOCGCardInfoNumField(string name, decimal value) : ERAOCGCardInfoField<decimal>(name, value)
    {
    }

    //是字符串的参数
    public class ERAOCGCardInfoStringField(string name, string value) : ERAOCGCardInfoField<string>(name, value)
    {
    }

    //只能从几个string选择肢里选1个的参数
    public class ERAOCGCardInfoSelectionField : ERAOCGCardInfoField<string>
    {
        public List<string> AllowedValues { get; }

        public ERAOCGCardInfoSelectionField(string name, string value, List<string> allowedValues)
            : base(name, value)
        {
            AllowedValues = allowedValues;
            if (!allowedValues.Contains(value))
            {
                throw new ArgumentException($"Value '{value}' is not in the list of allowed values for field '{name}'");
            }
        }

        public override string GetValue() => Value;
    }

    //参数父类
    public abstract class ERAOCGCardNameInfoField
    {
        public string Name = "";
        public List<string> Fields = [];

        public string? GetFirstValue() => Fields.Count > 0 ? Fields[0] : null;

        public override string ToString() => $"{Name}: {FormatFields()}";

        public ERACodeSelectCaseSubCase ToSubCase()
        {
            //DO NOT use new(a,b)
            ERACodeSelectCaseSubCase subCase = new(Name);
            AddFieldsToSubCase(subCase);
            return subCase;
        }

        protected abstract string FormatFields();
        protected abstract void AddFieldsToSubCase(ERACodeSelectCaseSubCase subCase);
    }

    public class ERAOCGCardNameInfoSingleField : ERAOCGCardNameInfoField
    {
        protected override string FormatFields() => 
            Fields.Count == 0 ? "(None)" : Fields[0];

        protected override void AddFieldsToSubCase(ERACodeSelectCaseSubCase subCase)
        {
            if (Fields.Count > 0)
            {
                subCase.Add($"RESULTS = {Fields[0]}");
            }
        }
    }

    public class ERAOCGCardNameInfoMultiField : ERAOCGCardNameInfoField
    {
        protected override string FormatFields() => 
            Fields.Count == 0 
                ? "(None)" 
                : $"[{string.Join(", ", Fields.Select((field, index) => $"{index}: {field}"))}]";

        protected override void AddFieldsToSubCase(ERACodeSelectCaseSubCase subCase)
        {
            for (int index = 0; index < Fields.Count; index++)
            {
                subCase.Add($"RESULTS:{index} = {Fields[index]}");
            }
        }
    }

    //对应CARDNAME_(id)处理
    public class ERAOCGCardNameInfo
    {
        public List<ERAOCGCardNameInfoField> Fields = [];
        
        public ERACodeSelectCase ToSelectCase()
        {
            ERACodeSelectCase sel = new("参照先");
            foreach (var f in Fields)
            {
                sel.AddCase(f.ToSubCase());
            }
            return sel;
        }
    }

    public class ERAOCGCardEffect
    {
    }

    public class ERAOCGCardDescription
    {
        //每行一句
        List<ERACode> descriptions = [];
        bool isRogueCard = false;
    }

    public class ERAOCGCardFunc
    {
    }
}
