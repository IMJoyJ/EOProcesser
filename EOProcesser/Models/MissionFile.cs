using EOProcesser;
using EOProcesser.Models;
using System.Text.RegularExpressions;

public class MissionFile
{
    public MissionFile() { }
    public MissionFile(string file)
    {
        LoadFromFile(file);
    }
    //用于生成IRAI_EVENT_{Id}(参照先)和@IRAI_ACT_{Id}(参照)
    //如果Deck非空，则生成@IRAI_DECK_{Id}(決闘者)和@DUELDECK_{Id+20000}(決闘者)
    //（使用的Deck总是Id+20000）
    //Load时从IRAI_EVENT读取。
    public int Id { get; set; } = 0;

    //名前
    public string Name { get; set; } = "";
    //イベント種
    public string EventType { get; set; } = "イベント_通常";
    //依頼先
    public string EventPosition { get; set; } = "即時開始";
    //詳細
    public ERACodeMultiLines Description { get; set; } = [];

    //条件
    public ERACodeMultiLines Condition { get; set; } = [];

    public void LoadFromFile(string file)
    {
        // 读取文件内容
        string fileContent = File.ReadAllText(file);
        
        // 使用ERACodeAnalyzer解析代码
        var codeLines = ERACodeAnalyzer.AnalyzeCode(fileContent);
        
        // 查找所有函数段
        var funcSegments = codeLines.OfType<ERACodeFuncSegment>().ToList();
        
        // 提取ID
        var eventFuncSegment = funcSegments.FirstOrDefault(f => 
            Regex.IsMatch(f.FuncName, @"^IRAI_EVENT_\d+(\(.*\))?$"));
        
        int id = -1;
        if (eventFuncSegment != null)
        {
            // 从函数名中提取ID，使用正则表达式
            var match = Regex.Match(eventFuncSegment.FuncName, @"^IRAI_EVENT_(\d+)");
            if (match.Success && int.TryParse(match.Groups[1].Value, out id))
            {
                Id = id;
            }
            
            // 处理IRAI_EVENT函数
            // 查找第一个SelectCase
            var selectCase = eventFuncSegment.codes.OfType<ERACodeSelectCase>().FirstOrDefault();
            if (selectCase != null)
            {
                // 提取SelectCase前的内容
                int selectCaseIndex = eventFuncSegment.codes.IndexOf(selectCase);
                IraiEventFuncExtraContent = [];
                for (int i = 0; i < selectCaseIndex; i++)
                {
                    IraiEventFuncExtraContent.Add(eventFuncSegment.codes[i]);
                }
                
                // 从SelectCase中提取各个字段的值
                Name = selectCase.GetValue("\"名前\"") ?? "";
                EventType = selectCase.GetValue("\"イベント種\"") ?? "";
                EventPosition = selectCase.GetValue("\"依頼先\"") ?? "";
                
                // 提取详细信息和条件
                var detailCase = selectCase.codes.OfType<ERACodeSelectCaseSubCase>()
                    .FirstOrDefault(c => c.CaseCondition == "\"詳細\"");
                if (detailCase != null)
                {
                    Description = [.. detailCase.codes];
                }
                
                var conditionCase = selectCase.codes.OfType<ERACodeSelectCaseSubCase>()
                    .FirstOrDefault(c => c.CaseCondition == "\"条件\"");
                if (conditionCase != null)
                {
                    Condition = [.. conditionCase.codes];
                }
            }
        }
        
        // 提取IRAI_ACT函数内容
        var actFuncSegment = funcSegments.FirstOrDefault(f => 
            Regex.IsMatch(f.FuncName, @"^IRAI_ACT_\d+(\(.*\))?$"));
        
        if (actFuncSegment != null)
        {
            IraiActFuncContent = [.. actFuncSegment.codes];
        }
        
        // 特别处理DUELDECK函数，只有DUELDECK_2xxxx（其中xxxx是任务ID）才被视为主卡组
        string expectedDeckName = $"DUELDECK_{20000 + id}";
        var duelDeckFuncSegment = funcSegments.FirstOrDefault(f => 
        {
            // 提取函数的基本名称（不含参数）
            var match = Regex.Match(f.FuncName, @"^([^(]+)");
            if (match.Success)
            {
                string baseName = match.Groups[1].Value;
                return baseName == expectedDeckName;
            }
            return false;
        });
        
        if (duelDeckFuncSegment != null)
        {
            // 创建敌人卡组对象，直接使用函数段作为初始化代码
            EnemyDeck = new DeckEditorDeck(duelDeckFuncSegment);
        }
        
        // 提取其他额外函数
        foreach (var func in funcSegments)
        {
            // 提取函数的基本名称（不含参数）
            var match = Regex.Match(func.FuncName, @"^([^(]+)");
            if (!match.Success) continue;
            
            string baseName = match.Groups[1].Value;
            
            // 检查是否是我们已处理的特定函数
            bool isProcessedFunction = false;
            
            // 检查是否是IRAI_EVENT_ID函数
            if (Regex.IsMatch(baseName, @"^IRAI_EVENT_\d+$"))
            {
                var idMatch = Regex.Match(baseName, @"^IRAI_EVENT_(\d+)$");
                if (idMatch.Success && int.TryParse(idMatch.Groups[1].Value, out int funcId) && funcId == id)
                {
                    isProcessedFunction = true;
                }
            }
            
            // 检查是否是IRAI_ACT_ID函数
            if (Regex.IsMatch(baseName, @"^IRAI_ACT_\d+$"))
            {
                var idMatch = Regex.Match(baseName, @"^IRAI_ACT_(\d+)$");
                if (idMatch.Success && int.TryParse(idMatch.Groups[1].Value, out int funcId) && funcId == id)
                {
                    isProcessedFunction = true;
                }
            }
            
            // 检查是否是IRAI_DECK_ID函数
            if (Regex.IsMatch(baseName, @"^IRAI_DECK_\d+$"))
            {
                var idMatch = Regex.Match(baseName, @"^IRAI_DECK_(\d+)$");
                if (idMatch.Success && int.TryParse(idMatch.Groups[1].Value, out int funcId) && funcId == id)
                {
                    isProcessedFunction = true;
                }
            }
            
            // 检查是否是主DUELDECK函数
            if (baseName == expectedDeckName)
            {
                isProcessedFunction = true;
            }
            
            // 如果不是已处理的特定函数，则添加到额外函数列表
            if (!isProcessedFunction)
            {
                ExtraFuncs.Add(func);
            }
        }
    }

    public string ToFileContent()
    {
        return $"""
        {GetIraiEventFunc()}
        {GetIraiActFunc()}
        {GetIraiDeckFuncs()}
        {GetExtraFuncs()}
        """;
    }
    //IRAI_EVENT_{Id}、IRAI_ACT_{Id}、IRAI_DECK_{Id}、DUELDECK_{Id+20000}以外全都放在这里。
    //比如：DUELDECK_{Id+30000}等其他函数。
    ERACodeMultiLines ExtraFuncs = [];
    private ERACodeMultiLines GetExtraFuncs()
    {
        return ExtraFuncs;
    }

    //保留第一个SELECTCASE前的所有行
    ERACodeMultiLines IraiEventFuncExtraContent { get; set; } = [];
    public ERACodeFuncSegment GetIraiEventFunc()
    {
        ERACodeFuncSegment result = new($"IRAI_EVENT_{Id}(参照先)");
        result.AddRange(IraiEventFuncExtraContent);
        ERACodeSelectCase sc = new("参照先");
        sc.AddCase(new("\"名前\"", [new ERACodeGenericLine($"RETURNS {Name}")]));
        sc.AddCase("\"イベント種\"", EventType);
        sc.AddCase("\"依頼先\"", EventPosition);
        sc.AddCase("\"詳細\"", Description);
        sc.AddCase("\"条件\"", Condition);
        result.Add(sc);
        result.Add("");
        result.Add("RETURN 0");

        return result;
    }

    public ERACodeMultiLines IraiActFuncContent { get; set; } = [];
    public ERACodeMultiLines GetIraiActFunc()
    {
        ERACodeFuncSegment result = new($"IRAI_ACT_{Id}(参照)");
        result.AddRange(IraiActFuncContent);
        return result;
    }

    public DeckEditorDeck? EnemyDeck { get; set; } = null;
    public ERACodeMultiLines GetIraiDeckFuncs()
    {
        ERACodeMultiLines result = [];
        if (EnemyDeck == null)
        {
            return result;
        }
        ERACodeFuncSegment segment = new($"IRAI_DECK_{Id}(決闘者)")
        {
            "#DIMS DYNAMIC 決闘者",
            "",
            new ERACodeSIfSegment("決闘者 == \"相手\"", "RETURN 20456")
        };
        result.Add(segment);
        result.Add(EnemyDeck.ToFunction());
        return result;
    }

}