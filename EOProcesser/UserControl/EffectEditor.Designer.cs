namespace EOProcesser
{
    partial class EffectEditor
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            scContainer = new SplitContainer();
            btnNodeMoveDown = new Button();
            btnNodeMoveUp = new Button();
            btnRemoveNode = new Button();
            btnAddNewNode = new Button();
            btnSaveFile = new Button();
            treeCodeTree = new TreeView();
            btnSave = new Button();
            txtCode = new TextBox();
            menuCodeMenu = new ContextMenuStrip(components);
            ParentAddCodeToolStripMenuItem = new ToolStripMenuItem();
            ParentAddCodeBeforeToolStripMenuItem = new ToolStripMenuItem();
            ParentAddCodeAfterToolStripMenuItem = new ToolStripMenuItem();
            ChildAddCodeToolStripMenuItem = new ToolStripMenuItem();
            EditCodeToolStripMenuItem = new ToolStripMenuItem();
            CodeDeleteToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)scContainer).BeginInit();
            scContainer.Panel1.SuspendLayout();
            scContainer.Panel2.SuspendLayout();
            scContainer.SuspendLayout();
            menuCodeMenu.SuspendLayout();
            SuspendLayout();
            // 
            // scContainer
            // 
            scContainer.Dock = DockStyle.Fill;
            scContainer.Location = new Point(0, 0);
            scContainer.Name = "scContainer";
            scContainer.Orientation = Orientation.Horizontal;
            // 
            // scContainer.Panel1
            // 
            scContainer.Panel1.Controls.Add(btnNodeMoveDown);
            scContainer.Panel1.Controls.Add(btnNodeMoveUp);
            scContainer.Panel1.Controls.Add(btnRemoveNode);
            scContainer.Panel1.Controls.Add(btnAddNewNode);
            scContainer.Panel1.Controls.Add(btnSaveFile);
            scContainer.Panel1.Controls.Add(treeCodeTree);
            // 
            // scContainer.Panel2
            // 
            scContainer.Panel2.Controls.Add(btnSave);
            scContainer.Panel2.Controls.Add(txtCode);
            scContainer.Size = new Size(633, 514);
            scContainer.SplitterDistance = 211;
            scContainer.TabIndex = 0;
            // 
            // btnNodeMoveDown
            // 
            btnNodeMoveDown.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnNodeMoveDown.Location = new Point(603, 179);
            btnNodeMoveDown.Name = "btnNodeMoveDown";
            btnNodeMoveDown.Size = new Size(27, 29);
            btnNodeMoveDown.TabIndex = 6;
            btnNodeMoveDown.Text = "↓";
            btnNodeMoveDown.UseVisualStyleBackColor = true;
            btnNodeMoveDown.Click += btnNodeMoveDown_Click;
            // 
            // btnNodeMoveUp
            // 
            btnNodeMoveUp.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnNodeMoveUp.Location = new Point(570, 179);
            btnNodeMoveUp.Name = "btnNodeMoveUp";
            btnNodeMoveUp.Size = new Size(27, 29);
            btnNodeMoveUp.TabIndex = 5;
            btnNodeMoveUp.Text = "↑";
            btnNodeMoveUp.UseVisualStyleBackColor = true;
            btnNodeMoveUp.Click += btnNodeMoveUp_Click;
            // 
            // btnRemoveNode
            // 
            btnRemoveNode.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRemoveNode.Location = new Point(537, 179);
            btnRemoveNode.Name = "btnRemoveNode";
            btnRemoveNode.Size = new Size(27, 29);
            btnRemoveNode.TabIndex = 4;
            btnRemoveNode.Text = "-";
            btnRemoveNode.UseVisualStyleBackColor = true;
            btnRemoveNode.Click += btnRemoveNode_Click;
            // 
            // btnAddNewNode
            // 
            btnAddNewNode.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAddNewNode.Location = new Point(504, 179);
            btnAddNewNode.Name = "btnAddNewNode";
            btnAddNewNode.Size = new Size(27, 29);
            btnAddNewNode.TabIndex = 3;
            btnAddNewNode.Text = "+";
            btnAddNewNode.UseVisualStyleBackColor = true;
            btnAddNewNode.Click += btnAddNewNode_Click;
            // 
            // btnSaveFile
            // 
            btnSaveFile.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSaveFile.Location = new Point(502, 144);
            btnSaveFile.Name = "btnSaveFile";
            btnSaveFile.Size = new Size(128, 29);
            btnSaveFile.TabIndex = 2;
            btnSaveFile.Text = "ファイル上書き";
            btnSaveFile.UseVisualStyleBackColor = true;
            btnSaveFile.Click += btnSaveFile_Click;
            // 
            // treeCodeTree
            // 
            treeCodeTree.Dock = DockStyle.Fill;
            treeCodeTree.Location = new Point(0, 0);
            treeCodeTree.Name = "treeCodeTree";
            treeCodeTree.Size = new Size(633, 211);
            treeCodeTree.TabIndex = 0;
            treeCodeTree.NodeMouseDoubleClick += treeCodeTree_NodeMouseDoubleClick;
            treeCodeTree.MouseClick += treeCodeTree_MouseClick;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.Location = new Point(549, 267);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(59, 29);
            btnSave.TabIndex = 1;
            btnSave.Text = "上書き";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // txtCode
            // 
            txtCode.AcceptsTab = true;
            txtCode.Dock = DockStyle.Fill;
            txtCode.Location = new Point(0, 0);
            txtCode.Multiline = true;
            txtCode.Name = "txtCode";
            txtCode.ScrollBars = ScrollBars.Vertical;
            txtCode.Size = new Size(633, 299);
            txtCode.TabIndex = 0;
            txtCode.TextChanged += txtCode_TextChanged;
            // 
            // menuCodeMenu
            // 
            menuCodeMenu.ImageScalingSize = new Size(20, 20);
            menuCodeMenu.Items.AddRange(new ToolStripItem[] { ParentAddCodeToolStripMenuItem, ChildAddCodeToolStripMenuItem, EditCodeToolStripMenuItem, CodeDeleteToolStripMenuItem });
            menuCodeMenu.Name = "menuCodeMenu";
            menuCodeMenu.Size = new Size(154, 100);
            // 
            // ParentAddCodeToolStripMenuItem
            // 
            ParentAddCodeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ParentAddCodeBeforeToolStripMenuItem, ParentAddCodeAfterToolStripMenuItem });
            ParentAddCodeToolStripMenuItem.Name = "ParentAddCodeToolStripMenuItem";
            ParentAddCodeToolStripMenuItem.Size = new Size(153, 24);
            ParentAddCodeToolStripMenuItem.Text = "追加";
            // 
            // ParentAddCodeBeforeToolStripMenuItem
            // 
            ParentAddCodeBeforeToolStripMenuItem.Name = "ParentAddCodeBeforeToolStripMenuItem";
            ParentAddCodeBeforeToolStripMenuItem.Size = new Size(122, 26);
            ParentAddCodeBeforeToolStripMenuItem.Text = "上方";
            ParentAddCodeBeforeToolStripMenuItem.Click += ParentAddCodeBeforeToolStripMenuItem_Click;
            // 
            // ParentAddCodeAfterToolStripMenuItem
            // 
            ParentAddCodeAfterToolStripMenuItem.Name = "ParentAddCodeAfterToolStripMenuItem";
            ParentAddCodeAfterToolStripMenuItem.Size = new Size(122, 26);
            ParentAddCodeAfterToolStripMenuItem.Text = "下方";
            ParentAddCodeAfterToolStripMenuItem.Click += ParentAddCodeAfterToolStripMenuItem_Click;
            // 
            // ChildAddCodeToolStripMenuItem
            // 
            ChildAddCodeToolStripMenuItem.Name = "ChildAddCodeToolStripMenuItem";
            ChildAddCodeToolStripMenuItem.Size = new Size(153, 24);
            ChildAddCodeToolStripMenuItem.Text = "追加（子）";
            ChildAddCodeToolStripMenuItem.Click += CodeChildAddToolStripMenuItem_Click;
            // 
            // EditCodeToolStripMenuItem
            // 
            EditCodeToolStripMenuItem.Name = "EditCodeToolStripMenuItem";
            EditCodeToolStripMenuItem.Size = new Size(153, 24);
            EditCodeToolStripMenuItem.Text = "編集";
            EditCodeToolStripMenuItem.Click += EditCodeToolStripMenuItem_Click;
            // 
            // CodeDeleteToolStripMenuItem
            // 
            CodeDeleteToolStripMenuItem.Name = "CodeDeleteToolStripMenuItem";
            CodeDeleteToolStripMenuItem.Size = new Size(153, 24);
            CodeDeleteToolStripMenuItem.Text = "削除";
            CodeDeleteToolStripMenuItem.Click += CodeDeleteToolStripMenuItem_Click;
            // 
            // EffectEditor
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(scContainer);
            Name = "EffectEditor";
            Size = new Size(633, 514);
            scContainer.Panel1.ResumeLayout(false);
            scContainer.Panel2.ResumeLayout(false);
            scContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)scContainer).EndInit();
            scContainer.ResumeLayout(false);
            menuCodeMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer scContainer;
        private TreeView treeCodeTree;
        private TextBox txtCode;
        private ContextMenuStrip menuCodeMenu;
        private ToolStripMenuItem ChildAddCodeToolStripMenuItem;
        private ToolStripMenuItem EditCodeToolStripMenuItem;
        private ToolStripMenuItem CodeDeleteToolStripMenuItem;
        private Button btnSave;
        private ToolStripMenuItem ParentAddCodeToolStripMenuItem;
        private ToolStripMenuItem ParentAddCodeBeforeToolStripMenuItem;
        private ToolStripMenuItem ParentAddCodeAfterToolStripMenuItem;
        private Button btnSaveFile;
        private Button btnNodeMoveDown;
        private Button btnNodeMoveUp;
        private Button btnRemoveNode;
        private Button btnAddNewNode;
    }
}
