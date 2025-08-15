using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace EOProcesser
{
    public static class ERACodeAnalyzer
    {
        public static ERACodeMultiLines AnalyzeCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return [];
            }

            code = code.Replace("\r\n", "\n");

            // 以\n分割代码字符串
            string[] lines = code.Split('\n');
            
            // 调用原有的处理多行代码的方法
            return AnalyzeCode(lines);
        }
        public static ERACodeMultiLines AnalyzeCode(IEnumerable<string> codes)
        {
            var codeLines = codes.ToList();
            var result = new ERACodeMultiLines();

            // 如果没有代码，返回空结果
            if (codeLines.Count == 0)
            {
                return result;
            }

            // 预处理所有SKIP块
            PreprocessSkipBlocks(codeLines, result);

            // 处理预处理后的代码（如果有效的代码还剩余）
            if (codeLines.Count > 0)
            {
                // 查找第一个函数定义的索引
                int firstFuncIndex = codeLines.FindIndex(line => line.TrimStart().StartsWith('@'));

                // 如果有函数定义前的代码，做一次处理
                if (firstFuncIndex > 0)
                {
                    ParseCodeBlock(codeLines, 0, firstFuncIndex - 1, result);
                }
                else if (firstFuncIndex == -1)
                {
                    // 如果没有函数定义，则解析整个代码块
                    ParseCodeBlock(codeLines, 0, codeLines.Count - 1, result);
                    return result;
                }

                // 处理所有函数
                int currentIndex = firstFuncIndex;
                while (currentIndex < codeLines.Count)
                {
                    string currentLine = codeLines[currentIndex].TrimStart();

                    // 如果当前行是函数定义
                    if (currentLine.StartsWith('@'))
                    {
                        string funcName = currentLine[1..].Trim();
                        ERACodeFuncSegment funcSegment = new(funcName);

                        // 查找下一个函数定义
                        int nextFuncIndex = codeLines.FindIndex(currentIndex + 1, line => line.TrimStart().StartsWith("@"));
                        if (nextFuncIndex == -1)
                            nextFuncIndex = codeLines.Count;

                        // 解析函数体
                        ParseCodeBlock(codeLines, currentIndex + 1, nextFuncIndex - 1, funcSegment);

                        result.Add(funcSegment);
                        currentIndex = nextFuncIndex;
                    }
                    else
                    {
                        // 如果不是函数定义但在函数之间，作为普通代码行处理
                        result.Add(ERACodeLineFactory.CreateFromLine(codeLines[currentIndex]));
                        currentIndex++;
                    }
                }
            }

            return result;
        }

        private static void PreprocessSkipBlocks(List<string> codeLines, ERACodeMultiLines parent)
        {
            int currentIndex = 0;

            while (currentIndex < codeLines.Count)
            {
                string line = codeLines[currentIndex].TrimStart();

                if (line == "[SKIPSTART]")
                {
                    int startIndex = currentIndex;
                    int skipEndIndex = -1;

                    // 查找对应的[SKIPEND]
                    for (int i = startIndex + 1; i < codeLines.Count; i++)
                    {
                        if (codeLines[i].TrimStart() == "[SKIPEND]")
                        {
                            skipEndIndex = i;
                            break;
                        }
                    }

                    if (skipEndIndex == -1)
                    {
                        // 如果没有找到对应的[SKIPEND]，将剩余全部代码视为注释
                        skipEndIndex = codeLines.Count - 1;
                    }

                    // 提取SKIP块中的所有注释内容
                    var skipComments = codeLines.GetRange(startIndex + 1, skipEndIndex - startIndex - 1);

                    // 创建SKIP段并添加到父容器
                    ERACodeSkipSegment skipSegment = new(skipComments);
                    parent.Add(skipSegment);

                    // 从代码行列表中移除已处理的SKIP块
                    codeLines.RemoveRange(startIndex, skipEndIndex - startIndex + 1);

                    // currentIndex不需要递增，因为已经移除了元素，下一个元素现在位于相同位置
                }
                else
                {
                    // 如果不是SKIP块，继续检查下一行
                    currentIndex++;
                }
            }
        }
        private static void ParseCodeBlock(List<string> codeLines, int startIndex, int endIndex, ERACodeMultiLines parent)
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                string line = codeLines[i].TrimStart();

                // 跳过空行
                if (string.IsNullOrWhiteSpace(line))
                {
                    parent.Add(ERACodeLineFactory.CreateFromLine(line));
                    continue;
                }

                // 其他控制结构的处理...
                if (line.StartsWith("SIF "))
                {
                    // SIF 只影响下一行
                    string condition = line[4..].Trim();
                    ERACodeSIfSegment sifSegment = new(condition);

                    // 如果SIF后面还有行，添加到SIF段
                    if (i + 1 <= endIndex)
                    {
                        sifSegment.Add(ERACodeLineFactory.CreateFromLine(codeLines[i + 1]));
                        i++; // 跳过下一行，因为已经处理了
                    }

                    parent.Add(sifSegment);
                }
                else if (line.StartsWith("IF "))
                {
                    string condition = line[3..].Trim();
                    ERACodeIfSegment ifSegment = new(condition);

                    // 查找对应的ENDIF，考虑嵌套
                    int endIfIndex = FindMatchingEndKeyword(codeLines, i + 1, endIndex, "IF", "ENDIF");
                    if (endIfIndex == -1)
                        endIfIndex = endIndex;

                    // 解析IF块内容
                    ParseIfBlock(codeLines, i + 1, endIfIndex - 1, ifSegment);

                    parent.Add(ifSegment);
                    i = endIfIndex; // 跳到ENDIF之后
                }
                else if (line.StartsWith("FOR "))
                {
                    // 解析FOR语句: FOR counter,start,end
                    string forParams = line[4..].Trim();
                    string[] parts = forParams.Split(',');
                    if (parts.Length >= 3)
                    {
                        string counter = parts[0].Trim();
                        string startValue = parts[1].Trim();
                        string endValue = parts[2].Trim();

                        ERACodeForSegment forSegment = new(counter, startValue, endValue);

                        // 查找对应的NEXT
                        int nextIndex = FindMatchingEndKeyword(codeLines, i + 1, endIndex, "FOR", "NEXT");
                        if (nextIndex == -1)
                            nextIndex = endIndex;

                        // 解析FOR块内容
                        ParseCodeBlock(codeLines, i + 1, nextIndex - 1, forSegment);

                        parent.Add(forSegment);
                        i = nextIndex; // 跳到NEXT之后
                    }
                    else
                    {
                        // FOR语句格式不正确，作为普通代码行处理
                        parent.Add(ERACodeLineFactory.CreateFromLine(codeLines[i]));
                    }
                }
                else if (line.StartsWith("WHILE "))
                {
                    string condition = line[6..].Trim();
                    ERACodeWhileSegment whileSegment = new(condition);

                    // 查找对应的WEND
                    int wendIndex = FindMatchingEndKeyword(codeLines, i + 1, endIndex, "WHILE", "WEND");
                    if (wendIndex == -1)
                        wendIndex = endIndex;

                    // 解析WHILE块内容
                    ParseCodeBlock(codeLines, i + 1, wendIndex - 1, whileSegment);

                    parent.Add(whileSegment);
                    i = wendIndex; // 跳到WEND之后
                }
                else if (line.StartsWith("DO"))
                {
                    // 查找对应的LOOP
                    int loopIndex = FindMatchingEndKeyword(codeLines, i + 1, endIndex, "DO", "LOOP");
                    if (loopIndex == -1)
                        loopIndex = endIndex;

                    // 获取LOOP后的条件
                    string condition = "";
                    if (loopIndex < codeLines.Count)
                    {
                        string loopLine = codeLines[loopIndex].TrimStart();
                        if (loopLine.StartsWith("LOOP "))
                            condition = loopLine[5..].Trim();
                    }

                    ERACodeDoSegment doSegment = new(condition);

                    // 解析DO块内容
                    ParseCodeBlock(codeLines, i + 1, loopIndex - 1, doSegment);

                    parent.Add(doSegment);
                    i = loopIndex; // 跳到LOOP之后
                }
                else if (line.StartsWith("REPEAT "))
                {
                    string condition = line[7..].Trim();
                    ERACodeRepeatSegment repeatSegment = new(condition);

                    // 查找对应的REND
                    int rendIndex = FindMatchingEndKeyword(codeLines, i + 1, endIndex, "REPEAT", "REND");
                    if (rendIndex == -1)
                        rendIndex = endIndex;

                    // 解析REPEAT块内容
                    ParseCodeBlock(codeLines, i + 1, rendIndex - 1, repeatSegment);

                    parent.Add(repeatSegment);
                    i = rendIndex; // 跳到REND之后
                }
                else if (line.StartsWith("SELECTCASE "))
                {
                    string condition = line[11..].Trim();
                    ERACodeSelectCase selectCaseSegment = new(condition);

                    // 查找对应的ENDSELECT
                    int endSelectIndex = FindMatchingEndKeyword(codeLines, i + 1, endIndex, "SELECTCASE", "ENDSELECT");
                    if (endSelectIndex == -1)
                        endSelectIndex = endIndex;

                    // 解析SELECTCASE块内容
                    ParseSelectCaseBlock(codeLines, i + 1, endSelectIndex - 1, selectCaseSegment);

                    parent.Add(selectCaseSegment);
                    i = endSelectIndex; // 跳到ENDSELECT之后
                }
                else
                {
                    // 普通代码行
                    parent.Add(ERACodeLineFactory.CreateFromLine(codeLines[i]));
                }
            }
        }

        private static void ParseIfBlock(List<string> codeLines, int startIndex, int endIndex, ERACodeIfSegment ifSegment)
        {
            // 重写的方法，正确处理嵌套的IF结构
            List<(int index, string type, string? condition)> controlPoints = [];

            // 首先找出所有同级的ELSEIF和ELSE控制点
            int nestLevel = 0;
            for (int i = startIndex; i <= endIndex; i++)
            {
                string line = codeLines[i].TrimStart();

                if (line.StartsWith("IF "))
                {
                    nestLevel++;
                }
                else if (line == "ENDIF")
                {
                    nestLevel--;
                }
                // 只有当嵌套级别为0时，才考虑ELSEIF和ELSE
                else if (nestLevel == 0)
                {
                    if (line.StartsWith("ELSEIF "))
                    {
                        string condition = line[7..].Trim();
                        controlPoints.Add((i, "ELSEIF", condition));
                    }
                    else if (line == "ELSE")
                    {
                        controlPoints.Add((i, "ELSE", null));
                    }
                }
            }

            // 添加一个结束点以便处理最后一个块
            controlPoints.Add((endIndex + 1, "END", null));

            // 处理主IF块
            int currentStart = startIndex;

            // 处理每个块
            for (int i = 0; i < controlPoints.Count; i++)
            {
                var point = controlPoints[i];
                int blockEnd = point.index - 1;

                // 创建并解析当前块
                ERACodeMultiLines currentBlock = [];
                ParseCodeBlock(codeLines, currentStart, blockEnd, currentBlock);

                if (i == 0)
                {
                    // 这是主IF块
                    foreach (var code in currentBlock)
                    {
                        ifSegment.Add(code);
                    }
                }
                else
                {
                    var prevPoint = controlPoints[i - 1];
                    if (prevPoint.type == "ELSEIF")
                    {
                        // 这是ELSEIF块
                        ifSegment.AddElseIf(prevPoint.condition!, currentBlock);
                    }
                    else if (prevPoint.type == "ELSE")
                    {
                        // 这是ELSE块
                        ifSegment.AddElse(currentBlock);
                    }
                }

                // 更新下一个块的起始位置
                currentStart = point.index + 1;
            }
        }
        private static void ParseSelectCaseBlock(List<string> codeLines, int startIndex, int endIndex, ERACodeSelectCase selectCaseSegment)
        {
            // 改进后的方法，正确处理SELECTCASE和CASEELSE
            List<(int index, string type, string? value)> casePoints = [];
            
            // 首先找出同级别的所有CASE和CASEELSE点
            int nestingLevel = 0;
            for (int i = startIndex; i <= endIndex; i++)
            {
                string line = codeLines[i].TrimStart();

                if (line.StartsWith("SELECTCASE "))
                {
                    nestingLevel++;
                }
                else if (line == "ENDSELECT")
                {
                    if (nestingLevel > 0)
                        nestingLevel--;
                }
                // 只在最外层处理CASE和CASEELSE
                else if (nestingLevel == 0)
                {
                    if (line.StartsWith("CASE "))
                    {
                        string caseValue = line[5..].Trim();
                        casePoints.Add((i, "CASE", caseValue));
                    }
                    else if (line == "CASEELSE")
                    {
                        casePoints.Add((i, "CASEELSE", null));
                    }
                }
            }

            // 添加结束点，方便处理最后一个CASE或CASEELSE块
            casePoints.Add((endIndex + 1, "END", null));

            // 如果没有任何CASE或CASEELSE，直接返回
            if (casePoints.Count <= 1)
            {
                return;
            }

            // 依次处理每个CASE和CASEELSE块
            for (int i = 0; i < casePoints.Count - 1; i++)
            {
                var currentPoint = casePoints[i];
                var nextPoint = casePoints[i + 1];

                // 确定当前块的范围
                int blockStart = currentPoint.index + 1;
                int blockEnd = nextPoint.index - 1;

                // 确保范围有效
                if (blockStart <= blockEnd)
                {
                    if (currentPoint.type == "CASE")
                    {
                        // 处理CASE块
                        ERACodeSelectCaseSubCase caseSubCase = new(currentPoint.value);
                        
                        // 解析块内容
                        ParseCodeBlock(codeLines, blockStart, blockEnd, caseSubCase);
                        
                        // 添加到SELECTCASE段
                        selectCaseSegment.Add(caseSubCase);
                    }
                    else if (currentPoint.type == "CASEELSE")
                    {
                        // 处理CASEELSE块
                        ERACodeSelectCaseSubCase caseElseSubCase = new(null);
                        
                        // 解析块内容
                        ParseCodeBlock(codeLines, blockStart, blockEnd, caseElseSubCase);
                        
                        // 添加到SELECTCASE段
                        selectCaseSegment.Add(caseElseSubCase);
                    }
                }
            }
        }

        // 查找下一个同级别的CASE、CASEELSE或ENDSELECT - 保留以便向后兼容
        private static int FindNextCaseOrEnd(List<string> codeLines, int startIndex, int endIndex)
        {
            int nestingLevel = 0;

            for (int i = startIndex; i <= endIndex; i++)
            {
                string line = codeLines[i].TrimStart();

                if (line.StartsWith("SELECTCASE "))
                {
                    nestingLevel++;
                }
                else if (line == "ENDSELECT")
                {
                    if (nestingLevel == 0)
                        return i; // 找到同级别的ENDSELECT
                    nestingLevel--;
                }
                else if (nestingLevel == 0 && (line.StartsWith("CASE ") || line == "CASEELSE"))
                {
                    return i; // 找到同级别的CASE或CASEELSE
                }
            }

            return -1; // 没有找到
        }

        // 查找同级别的ENDSELECT - 保留以便向后兼容
        private static int FindNextEndSelectAtSameLevel(List<string> codeLines, int startIndex, int endIndex)
        {
            int nestingLevel = 0;

            for (int i = startIndex; i <= endIndex; i++)
            {
                string line = codeLines[i].TrimStart();

                if (line.StartsWith("SELECTCASE "))
                {
                    nestingLevel++;
                }
                else if (line == "ENDSELECT")
                {
                    if (nestingLevel == 0)
                        return i; // 找到同级别的ENDSELECT
                    nestingLevel--;
                }
            }

            return -1; // 没有找到
        }
        private static int FindMatchingEndKeyword(List<string> codeLines, int startIndex, int endIndex, string startKeyword, string endKeyword)
        {
            int nestingLevel = 1; // 已经遇到了一个开始关键字

            for (int i = startIndex; i <= endIndex; i++)
            {
                string line = codeLines[i].TrimStart();

                // 特殊情况处理：DO-LOOP结构
                if (startKeyword == "DO" && (line == "LOOP" || line.StartsWith("LOOP ")))
                {
                    nestingLevel--;
                    if (nestingLevel == 0)
                    {
                        return i; // 找到匹配的LOOP
                    }
                    continue;
                }

                // 特殊情况：SELECTCASE结构中的CASEELSE
                if (startKeyword == "SELECTCASE" && line == "CASEELSE")
                {
                    // CASEELSE不影响嵌套级别，跳过不处理
                    continue;
                }

                // 检查是否是开始关键字
                if (line == startKeyword || 
                    line.StartsWith(startKeyword + " ") || 
                    Regex.IsMatch(line, $"^{Regex.Escape(startKeyword)}\\b"))
                {
                    nestingLevel++;
                }
                // 检查是否是结束关键字
                else if (line == endKeyword || 
                        line.StartsWith(endKeyword + " ") || 
                        Regex.IsMatch(line, $"^{Regex.Escape(endKeyword)}\\b"))
                {
                    nestingLevel--;
                    if (nestingLevel == 0)
                    {
                        return i; // 找到匹配的结束关键字
                    }
                }
            }

            // 添加调试信息
            Console.WriteLine($"未找到匹配的结束关键字: {startKeyword} -> {endKeyword}, 搜索范围: {startIndex}-{endIndex}");
            
            return -1; // 没有找到匹配的结束关键字
        }
    }
}