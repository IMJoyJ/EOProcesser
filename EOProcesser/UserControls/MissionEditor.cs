using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EOProcesser.UserControls
{
    public partial class MissionEditor : UserControl
    {
        public MissionEditor()
        {
            InitializeComponent();
        }

        public void LoadFolder(string folder)
        {
            Utils.InitTreeViewByFolder(folder, tvMissionFolder, (tn, isFolder) =>
            {
                if (!isFolder && tn.Tag is string file)
                {
                    try
                    {
                        MissionFile mf = new(file);
                        tn.Tag = mf;
                        TreeNode node = new($"ID：{mf.Id}")
                        {
                            Tag = tn
                        };
                        tn.Nodes.Add(node);
                        node = new($"名前：{mf.Name}")
                        {
                            Tag = tn
                        };
                        tn.Nodes.Add(node);
                    }
                    catch { }
                }
            }, "*.erb");
        }

        private void btnSearchFile_Click(object sender, EventArgs e)
        {
            Utils.SearchTreeViewWithString(txtSearchFile.Text, tvMissionFolder);
        }
    }
}
