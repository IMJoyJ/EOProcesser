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

        public static ERAOCGCard ReadFromFile(string file)
        {
            ERAOCGCard result = new();

            string[] lines = File.ReadAllLines(file);

            Dictionary<string, ERAFuncSegment> extraFuncDic = [];

            string currentFunc = "";
            List<ERACode> currentFuncLines = [];

            foreach (string line in lines)
            {
                //!!!DO NOT TRIM ANY LINE!!!
                if (line.StartsWith('@'))
                {
                    // If we have collected lines for a previous function, add them to the dictionary
                    if (!string.IsNullOrEmpty(currentFunc) && currentFuncLines.Count > 0)
                    {
                        extraFuncDic[currentFunc] = new(currentFunc,currentFuncLines);
                        currentFuncLines.Clear();
                    }

                    // Start a new function
                    currentFunc = line[1..]; // Remove the @ symbol
                }
                else if (!string.IsNullOrEmpty(currentFunc) && !string.IsNullOrEmpty(line))
                {
                    // Add line to current function
                    currentFuncLines.Add(new ERACodeLine(line));
                }
            }

            // Add the last function if there is one
            if (!string.IsNullOrEmpty(currentFunc))
            {
                extraFuncDic[currentFunc] = new(currentFunc, currentFuncLines);
            }

            return result;
        }

        override public string ToString()
        {
            return $"[{Id}]{Name}";
        }
    }

    public class ERAOCGCardEffect
    {
    }

    public class ERAOCGCardDescription
    {

    }

    public class ERAOCGCardFunc
    {
    }
}
