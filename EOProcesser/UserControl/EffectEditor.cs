using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EOProcesser
{
    public partial class EffectEditor : UserControl
    {
        public EffectEditor()
        {
            InitializeComponent();
        }
        public void ClearAll()
        {
            TextHasChanged = false;
            treeCodeTree.Nodes.Clear();
            txtCode.Text = "";
            CurrentFile = null;
        }
        public void LoadCode(ERACode code)
        {
            treeCodeTree.Nodes.Clear();
            treeCodeTree.Nodes.AddRange([.. code.GetTreeNodes()]);
            txtCode.Text = code.ToString();
            txtCode.Enabled = false;
            TextHasChanged = false;
            CurrentFile = null;
            btnSaveFile.Visible = false;
            if (treeCodeTree.Nodes.Count > 0)
            {
                treeCodeTree.SelectedNode = treeCodeTree.Nodes[0];
                treeCodeTree_NodeMouseDoubleClick(this, new TreeNodeMouseClickEventArgs(treeCodeTree.SelectedNode, MouseButtons.Left, 1, 0, 0));
            }
        }
        public void LoadCode(string str, bool isFile = false)
        {
            if (!isFile)
            {
                CurrentFile = null;
            }
            treeCodeTree.Nodes.Clear();
            var ml = ERACodeAnalyzer.AnalyzeCode(str);
            treeCodeTree.Nodes.AddRange([.. ml.GetTreeNodes()]);
            txtCode.Text = str;
            txtCode.Enabled = false;
            TextHasChanged = false;
            btnSaveFile.Visible = (CurrentFile != null);
        }
        public string GetCodeString()
        {
            StringBuilder sb = new();
            foreach(TreeNode node in treeCodeTree.Nodes)
            {
                sb.AppendLine(node.Tag.ToString());
            }
            return sb.ToString();
        }
        public string? CurrentFile = null;
        public void LoadCodeFromFile(string file)
        {
            string content = File.ReadAllText(file);
            LoadCode(content, true);
            CurrentFile = file;
        }
        public void SaveToFile(string file)
        {
            StringBuilder sb = new();
            foreach (TreeNode node in treeCodeTree.Nodes)
            {
                sb.AppendLine(node.Tag.ToString());
            }
        }
        public void SaveToFile()
        {
            if (CurrentFile != null)
            {
                SaveToFile(CurrentFile);
            }
        }

        private void treeCodeTree_MouseClick(object sender, MouseEventArgs e)
        {
            var node = treeCodeTree.GetNodeAt(e.X, e.Y);
            if (e.Button == MouseButtons.Right && node != null)
            {
                // Check if the selected node is an ERACodeLine and disable the add child menu item if it is

                ChildAddCodeToolStripMenuItem.Enabled = node.Tag is ERABlockSegment;

                // Show context menu at the current mouse position
                treeCodeTree.SelectedNode = node;
                menuCodeMenu.Show(treeCodeTree, e.Location);
            }
        }

        private void CodeChildAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeCodeTree.SelectedNode.Tag is ERACodeLine)
            {
                MessageBox.Show("単一行のコードに子要素を追加することはできません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ERACodeGenericLine line = new("");
            treeCodeTree.SelectedNode.Nodes.AddRange([.. line.GetTreeNodes()]);
        }
        private void ParentAddCodeBeforeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeCodeTree.SelectedNode != null)
            {
                ERACodeCommentLine line = new("（新しい行）");
                TreeNode[] newNodes = [.. line.GetTreeNodes()];
                int index = treeCodeTree.SelectedNode.Index;
                TreeNode parent = treeCodeTree.SelectedNode.Parent;

                // Insert before the selected node
                foreach (var node in newNodes)
                {
                    if (parent != null)
                    {
                        parent.Nodes.Insert(index, node);
                    }
                    else
                    {
                        treeCodeTree.Nodes.Insert(index, node);
                    }
                }
            }
        }

        private void ParentAddCodeAfterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeCodeTree.SelectedNode != null)
            {
                ERACodeGenericLine line = new("");
                TreeNode[] newNodes = [.. line.GetTreeNodes()];
                int index = treeCodeTree.SelectedNode.Index + 1;
                TreeNode parent = treeCodeTree.SelectedNode.Parent;

                // Insert after the selected node
                foreach (var node in newNodes)
                {
                    if (parent != null)
                    {
                        parent.Nodes.Insert(index++, node);
                    }
                    else
                    {
                        treeCodeTree.Nodes.Insert(index++, node);
                    }
                }
            }
        }

        private void CodeDeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeCodeTree.SelectedNode != null)
            {
                DialogResult result = MessageBox.Show(
                    "削除してもよろしいですか？",
                    "確認",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    treeCodeTree.SelectedNode.Remove();
                }
            }
        }

        TreeNode? CurrentEditingNode = null;

        private void EditCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = treeCodeTree.SelectedNode;
            if (node == null)
            {
                return;
            }
            if (TextHasChanged)
            {
                DialogResult result = MessageBox.Show(
                    "コードが変更されています。保存しますか？",
                    "保存確認",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    btnSave_Click(sender, e);
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }
            CurrentEditingNode = node;
            string code = CurrentEditingNode.Tag.ToString() ?? "";
            
            // Split the code into lines
            string[] lines = code.Split(["\r\n", "\n"], StringSplitOptions.None);
            
            // Find the minimum number of leading tabs
            int minTabCount = int.MaxValue;
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                
                int tabCount = 0;
                foreach (char c in line)
                {
                    if (c == '\t') tabCount++;
                    else break;
                }
                
                if (tabCount < minTabCount)
                {
                    minTabCount = tabCount;
                }
            }
            
            // If we found some tabs to trim
            if (minTabCount > 0 && minTabCount < int.MaxValue)
            {
                // Remove the minimum number of tabs from each line
                for (int i = 0; i < lines.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(lines[i]))
                    {
                        lines[i] = lines[i][
                            Math.Min(minTabCount, lines[i].TakeWhile(c => c == '\t').Count())..];
                    }
                }
                
                // Join the lines back together
                code = string.Join(Environment.NewLine, lines);
            }
            
            txtCode.Text = code;
            txtCode.Enabled = true;
            TextHasChanged = false;
        }

        public event EventHandler? OnSave;
        private void btnSave_Click(object sender, EventArgs e)
        {
            TextHasChanged = false;
            var codes = ERACodeAnalyzer.AnalyzeCode(txtCode.Text);
            List<TreeNode> nodes = codes.GetTreeNodes();

            if (CurrentEditingNode != null && nodes.Count > 0)
            {
                TreeNode parentNode = CurrentEditingNode.Parent;
                int index = CurrentEditingNode.Index;

                CurrentEditingNode.Remove();

                if (parentNode != null)
                {
                    foreach (var node in nodes)
                    {
                        parentNode.Nodes.Insert(index++, node);
                    }
                    // 更新父节点及其所有上级节点的Tag属性
                    RefreshParentNodesTags(parentNode);
                }
                else
                {
                    foreach (var node in nodes)
                    {
                        treeCodeTree.Nodes.Insert(index++, node);
                    }
                }

                treeCodeTree.SelectedNode = nodes[0];
                EditCodeToolStripMenuItem_Click(sender, e);
            }
            
            OnSave?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 递归更新节点及其所有父节点的Tag属性
        /// </summary>
        /// <param name="node">要更新的节点</param>
        private void RefreshParentNodesTags(TreeNode node)
        {
            if (node == null) return;

            // 如果节点的Tag是ERACode类型，则更新它
            if (node.Tag is ERACode eraCode)
            {
                // 获取当前节点的所有子节点列表
                List<TreeNode> childNodes = [.. node.Nodes.Cast<TreeNode>()];

                // 调用ERACode的RefreshFromChildTreeNodes方法更新Tag
                if (node.Tag is ERABlockSegment block)
                {
                    block.RefreshFromChildTreeNodes(childNodes);
                }

                // 递归处理父节点
                RefreshParentNodesTags(node.Parent);
            }
        }

        bool TextHasChanged = false;

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            TextHasChanged = true;
        }

        private void treeCodeTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            EditCodeToolStripMenuItem_Click(sender, e);
        }

        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            if (CurrentFile != null)
            {
                SaveToFile();
            }
        }

        private void btnAddNewNode_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeCodeTree.SelectedNode;

            if (selectedNode == null)
            {
                // If no node is selected, add to the root
                ERACodeCommentLine line = new("新しい行");
                treeCodeTree.Nodes.AddRange([.. line.GetTreeNodes()]);
            }
            else if (selectedNode.Tag is ERABlockSegment)
            {
                // Ask whether to add as a child or as a sibling
                DialogResult addTypeResult = MessageBox.Show(
                    "新しい行を子要素として追加しますか？\n「いいえ」を選択すると、同じ階層に追加されます。",
                    "追加方法の選択",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (addTypeResult == DialogResult.Yes)
                {
                    // Add as a child
                    ERACodeCommentLine line = new("新しい行");
                    selectedNode.Nodes.AddRange([.. line.GetTreeNodes()]);
                    selectedNode.Expand();
                }
                else
                {
                    // Add as a sibling after the selected node
                    ERACodeCommentLine line = new("新しい行");
                    TreeNode[] newNodes = [.. line.GetTreeNodes()];
                    int index = selectedNode.Index + 1;
                    TreeNode parent = selectedNode.Parent;

                    foreach (var node in newNodes)
                    {
                        parent.Nodes.Insert(index++, node);
                    }
                    RefreshParentNodesTags(parent);
                }
            }
            else if (selectedNode.Parent != null)
            {
                // Add after the selected node if it's not a block
                ERACodeCommentLine line = new("新しい行");
                TreeNode[] newNodes = [.. line.GetTreeNodes()];
                int index = selectedNode.Index + 1;
                TreeNode parent = selectedNode.Parent;

                foreach (var node in newNodes)
                {
                    parent.Nodes.Insert(index++, node);
                }
                RefreshParentNodesTags(parent);
            }
            else
            {
                // Add after the selected node if it's a root node
                ERACodeCommentLine line = new("新しい行");
                TreeNode[] newNodes = [.. line.GetTreeNodes()];
                int index = selectedNode.Index + 1;

                foreach (var node in newNodes)
                {
                    treeCodeTree.Nodes.Insert(index++, node);
                }
            }
        }

        private void btnRemoveNode_Click(object sender, EventArgs e)
        {
            if (treeCodeTree.SelectedNode != null)
            {
                DialogResult result = MessageBox.Show(
                    "削除してもよろしいですか？",
                    "確認",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    TreeNode parent = treeCodeTree.SelectedNode.Parent;
                    treeCodeTree.SelectedNode.Remove();
                    
                    if (parent != null)
                    {
                        RefreshParentNodesTags(parent);
                    }
                }
            }
        }

        private void btnNodeMoveUp_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeCodeTree.SelectedNode;
            if (selectedNode != null)
            {
                TreeNode parent = selectedNode.Parent;
                int index = selectedNode.Index;
                
                if (index > 0)
                {
                    if (parent != null)
                    {
                        parent.Nodes.RemoveAt(index);
                        parent.Nodes.Insert(index - 1, selectedNode);
                        treeCodeTree.SelectedNode = selectedNode;
                    }
                    else
                    {
                        // Handle root level nodes
                        treeCodeTree.Nodes.RemoveAt(index);
                        treeCodeTree.Nodes.Insert(index - 1, selectedNode);
                        treeCodeTree.SelectedNode = selectedNode;
                    }
                    RefreshParentNodesTags(selectedNode);
                }
            }
        }

        private void btnNodeMoveDown_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeCodeTree.SelectedNode;
            if (selectedNode != null)
            {
                TreeNode parent = selectedNode.Parent;
                int index = selectedNode.Index;
                
                if (parent != null)
                {
                    if (index < parent.Nodes.Count - 1)
                    {
                        parent.Nodes.RemoveAt(index);
                        parent.Nodes.Insert(index + 1, selectedNode);
                        treeCodeTree.SelectedNode = selectedNode;
                        RefreshParentNodesTags(parent);
                    }
                }
                else
                {
                    // Handle root level nodes
                    if (index < treeCodeTree.Nodes.Count - 1)
                    {
                        treeCodeTree.Nodes.RemoveAt(index);
                        treeCodeTree.Nodes.Insert(index + 1, selectedNode);
                        treeCodeTree.SelectedNode = selectedNode;
                        RefreshParentNodesTags(selectedNode);
                    }
                }
            }
        }
    }
}
