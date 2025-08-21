using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOProcesser
{
    public static class Utils
    {
        private static readonly string[] separator = ["\r\n", "\n"];

        public static string TrimCode(string code)
        {
            string[] lines = code.Split(separator, StringSplitOptions.None);
            StringBuilder result = new();
            bool isSkipSection = false;
            bool previousLineWasEmpty = false;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                // Check if this is a skip section marker
                if (line.Trim() == "[SKIPSTART]")
                {
                    isSkipSection = true;
                    result.AppendLine(line);
                    continue;
                }
                else if (line.Trim() == "[SKIPEND]")
                {
                    isSkipSection = false;
                    result.AppendLine(line);
                    continue;
                }

                // Handle regular lines
                if (isSkipSection)
                {
                    // In skip section, preserve all lines as-is
                    result.AppendLine(line);
                }
                else
                {
                    bool isCurrentLineEmpty = string.IsNullOrWhiteSpace(line);

                    // If line is empty and previous line was also empty, skip it
                    if (isCurrentLineEmpty && previousLineWasEmpty)
                    {
                        continue;
                    }

                    // Trim empty lines, keep non-empty lines as-is
                    if (isCurrentLineEmpty)
                    {
                        result.AppendLine("");
                    }
                    else
                    {
                        result.AppendLine(line);
                    }

                    previousLineWasEmpty = isCurrentLineEmpty;
                }
            }

            return result.ToString();
        }
    }
}
