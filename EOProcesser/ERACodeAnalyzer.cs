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
        public static ERACodeMultiLines AnalyzeCode(IEnumerable<string> codes)
        {
            var codeLines = codes.ToList();
            var result = new ERACodeMultiLines();

            // 如果没有代码，返回空结果
            if (codeLines.Count == 0)
            {
                return result;
            }

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
                    ERAFuncSegment funcSegment = new(funcName);

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
                    result.Add(new ERACodeLine(codeLines[currentIndex]));
                    currentIndex++;
                }
            }

            return result;
        }

        private static void ParseCodeBlock(List<string> codeLines, int startIndex, int endIndex, ERACodeMultiLines parent)
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                string line = codeLines[i].TrimStart();

                // 跳过空行
                if (string.IsNullOrWhiteSpace(line))
                {
                    parent.Add(new ERACodeLine(line));
                    continue;
                }

                // 检查各种控制结构
                if (line.StartsWith("SIF "))
                {
                    // SIF 只影响下一行
                    string condition = line[4..].Trim();
                    ERACodeSIfSegment sifSegment = new(condition);

                    // 如果SIF后面还有行，添加到SIF段
                    if (i + 1 <= endIndex)
                    {
                        sifSegment.Add(new ERACodeLine(codeLines[i + 1]));
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
                        parent.Add(new ERACodeLine(codeLines[i]));
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
                    parent.Add(new ERACodeLine(codeLines[i]));
                }
            }
        }

        private static void ParseIfBlock(List<string> codeLines, int startIndex, int endIndex, ERACodeIfSegment ifSegment)
        {
            int currentIndex = startIndex;

            // 查找ELSEIF或ELSE
            while (currentIndex <= endIndex)
            {
                // 寻找下一个ELSEIF或ELSE
                int elseIfIndex = -1;
                int elseIndex = -1;

                for (int i = currentIndex; i <= endIndex; i++)
                {
                    string line = codeLines[i].TrimStart();
                    if (line.StartsWith("ELSEIF "))
                    {
                        elseIfIndex = i;
                        break;
                    }
                    else if (line == "ELSE")
                    {
                        elseIndex = i;
                        break;
                    }
                }

                // 处理IF部分直到ELSEIF/ELSE
                int endOfCurrentBlock = Math.Min(
                    elseIfIndex != -1 ? elseIfIndex - 1 : endIndex,
                    elseIndex != -1 ? elseIndex - 1 : endIndex
                );

                // 解析当前块
                ERACodeMultiLines currentBlock = new();
                ParseCodeBlock(codeLines, currentIndex, endOfCurrentBlock, currentBlock);

                // 将解析的块添加到适当的部分
                if (currentIndex == startIndex)
                {
                    // 这是主IF块
                    foreach (var code in currentBlock)
                    {
                        ifSegment.Add(code);
                    }
                }

                // 更新当前索引
                if (elseIfIndex != -1)
                {
                    // 处理ELSEIF
                    string condition = codeLines[elseIfIndex].TrimStart()[7..].Trim();
                    currentIndex = elseIfIndex + 1;

                    // 查找下一个ELSEIF/ELSE或结束
                    int nextElseIfOrElse = -1;
                    for (int i = currentIndex; i <= endIndex; i++)
                    {
                        string line = codeLines[i].TrimStart();
                        if (line.StartsWith("ELSEIF ") || line == "ELSE")
                        {
                            nextElseIfOrElse = i;
                            break;
                        }
                    }

                    if (nextElseIfOrElse == -1)
                        nextElseIfOrElse = endIndex + 1;

                    // 解析ELSEIF块
                    ERACodeMultiLines elseIfBlock = new();
                    ParseCodeBlock(codeLines, currentIndex, nextElseIfOrElse - 1, elseIfBlock);

                    // 添加到IF段
                    ifSegment.AddElseIf(condition, elseIfBlock);

                    currentIndex = nextElseIfOrElse;
                }
                else if (elseIndex != -1)
                {
                    // 处理ELSE
                    currentIndex = elseIndex + 1;

                    // 解析ELSE块
                    ERACodeMultiLines elseBlock = new();
                    ParseCodeBlock(codeLines, currentIndex, endIndex, elseBlock);

                    // 添加到IF段
                    ifSegment.AddElse(elseBlock);

                    break; // ELSE是最后一个块
                }
                else
                {
                    // 没有更多的ELSEIF或ELSE
                    break;
                }
            }
        }

        private static void ParseSelectCaseBlock(List<string> codeLines, int startIndex, int endIndex, ERACodeSelectCase selectCaseSegment)
        {
            int currentIndex = startIndex;

            while (currentIndex <= endIndex)
            {
                string line = codeLines[currentIndex].TrimStart();

                if (line.StartsWith("CASE "))
                {
                    string caseValue = line[5..].Trim();

                    // 查找下一个CASE或CASEELSE
                    int nextCaseIndex = -1;
                    for (int i = currentIndex + 1; i <= endIndex; i++)
                    {
                        string nextLine = codeLines[i].TrimStart();
                        if (nextLine.StartsWith("CASE ") || nextLine == "CASEELSE")
                        {
                            nextCaseIndex = i;
                            break;
                        }
                    }

                    if (nextCaseIndex == -1)
                        nextCaseIndex = endIndex + 1;

                    // 创建CASE子结构
                    ERACodeSelectCaseSubCase caseSubCase = new(caseValue);

                    // 解析CASE块内容并添加到caseSubCase
                    ParseCodeBlock(codeLines, currentIndex + 1, nextCaseIndex - 1, caseSubCase);

                    // 添加到SELECTCASE段
                    selectCaseSegment.Add(caseSubCase);

                    currentIndex = nextCaseIndex;
                }
                else if (line == "CASEELSE")
                {
                    // 创建CASEELSE子结构
                    ERACodeSelectCaseSubCase caseElseSubCase = new(null);

                    // 解析CASEELSE块内容并添加到caseElseSubCase
                    ParseCodeBlock(codeLines, currentIndex + 1, endIndex, caseElseSubCase);

                    // 添加到SELECTCASE段
                    selectCaseSegment.Add(caseElseSubCase);

                    break; // CASEELSE是最后一个块
                }
                else
                {
                    // 不是CASE或CASEELSE，可能是语法错误，跳过
                    currentIndex++;
                }
            }
        }

        private static int FindMatchingEndKeyword(List<string> codeLines, int startIndex, int endIndex, string startKeyword, string endKeyword)
        {
            int nestingLevel = 1; // 已经遇到了一个开始关键字

            for (int i = startIndex; i <= endIndex; i++)
            {
                string line = codeLines[i].TrimStart();

                // 检查是否是开始关键字
                if (line.StartsWith(startKeyword + " ") || line == startKeyword)
                {
                    nestingLevel++;
                }
                // 检查是否是结束关键字
                else if (line.StartsWith(endKeyword + " ") || line == endKeyword)
                {
                    nestingLevel--;
                    if (nestingLevel == 0)
                    {
                        return i; // 找到匹配的结束关键字
                    }
                }
                // 特殊情况：LOOP可能包含条件
                else if (startKeyword == "DO" && line.StartsWith("LOOP"))
                {
                    nestingLevel--;
                    if (nestingLevel == 0)
                    {
                        return i;
                    }
                }
            }

            return -1; // 没有找到匹配的结束关键字
        }
    }
}