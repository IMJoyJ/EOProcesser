using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOProcesser
{
    //Base ERA code lines.
    public class ERACodeLine(string codeLine) : ERACode
    {
        public virtual ERACodeLine FromLine(string line)
        {
            return new ERACodeLine(line);
        }
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
            return indentation + CodeLine.TrimStart();
        }
    }
    public class ERACodeCommentLine : ERACodeLine
    {
        public ERACodeCommentLine(string codeLine) : base(codeLine)
        {
        }

        public override ERACodeLine FromLine(string line)
        {
            if (!line.StartsWith(";"))
            {
            }
        }
    }

}
