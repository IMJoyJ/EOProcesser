namespace EOProcesser.UserControls
{
    partial class MissionEditor
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
            splitContainer1 = new SplitContainer();
            btnSearchFile = new Button();
            txtSearchFile = new TextBox();
            tvMissionFolder = new TreeView();
            tabControl1 = new TabControl();
            tabBasicInfo = new TabPage();
            tabActFunction = new TabPage();
            tabDeck = new TabPage();
            tabExtraFuncs = new TabPage();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(btnSearchFile);
            splitContainer1.Panel1.Controls.Add(txtSearchFile);
            splitContainer1.Panel1.Controls.Add(tvMissionFolder);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tabControl1);
            splitContainer1.Size = new Size(586, 389);
            splitContainer1.SplitterDistance = 184;
            splitContainer1.TabIndex = 0;
            // 
            // btnSearchFile
            // 
            btnSearchFile.Location = new Point(139, 0);
            btnSearchFile.Name = "btnSearchFile";
            btnSearchFile.Size = new Size(42, 26);
            btnSearchFile.TabIndex = 2;
            btnSearchFile.Text = "検索";
            btnSearchFile.UseVisualStyleBackColor = true;
            btnSearchFile.Click += btnSearchFile_Click;
            // 
            // txtSearchFile
            // 
            txtSearchFile.Location = new Point(3, 3);
            txtSearchFile.Name = "txtSearchFile";
            txtSearchFile.Size = new Size(130, 23);
            txtSearchFile.TabIndex = 1;
            // 
            // tvMissionFolder
            // 
            tvMissionFolder.Dock = DockStyle.Bottom;
            tvMissionFolder.Location = new Point(0, 24);
            tvMissionFolder.Name = "tvMissionFolder";
            tvMissionFolder.Size = new Size(184, 365);
            tvMissionFolder.TabIndex = 0;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabBasicInfo);
            tabControl1.Controls.Add(tabActFunction);
            tabControl1.Controls.Add(tabDeck);
            tabControl1.Controls.Add(tabExtraFuncs);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(398, 389);
            tabControl1.TabIndex = 0;
            // 
            // tabBasicInfo
            // 
            tabBasicInfo.Location = new Point(4, 24);
            tabBasicInfo.Name = "tabBasicInfo";
            tabBasicInfo.Padding = new Padding(3);
            tabBasicInfo.Size = new Size(390, 361);
            tabBasicInfo.TabIndex = 0;
            tabBasicInfo.Text = "基本情報";
            tabBasicInfo.UseVisualStyleBackColor = true;
            // 
            // tabActFunction
            // 
            tabActFunction.Location = new Point(4, 24);
            tabActFunction.Name = "tabActFunction";
            tabActFunction.Padding = new Padding(3);
            tabActFunction.Size = new Size(390, 361);
            tabActFunction.TabIndex = 1;
            tabActFunction.Text = "イベント関数";
            tabActFunction.UseVisualStyleBackColor = true;
            // 
            // tabDeck
            // 
            tabDeck.Location = new Point(4, 24);
            tabDeck.Name = "tabDeck";
            tabDeck.Size = new Size(390, 361);
            tabDeck.TabIndex = 2;
            tabDeck.Text = "相手デッキ編集";
            tabDeck.UseVisualStyleBackColor = true;
            // 
            // tabExtraFuncs
            // 
            tabExtraFuncs.Location = new Point(4, 24);
            tabExtraFuncs.Name = "tabExtraFuncs";
            tabExtraFuncs.Size = new Size(390, 361);
            tabExtraFuncs.TabIndex = 3;
            tabExtraFuncs.Text = "追加関数";
            tabExtraFuncs.UseVisualStyleBackColor = true;
            // 
            // MissionEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Name = "MissionEditor";
            Size = new Size(586, 389);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private TreeView tvMissionFolder;
        private TabControl tabControl1;
        private TabPage tabBasicInfo;
        private TabPage tabActFunction;
        private TabPage tabDeck;
        private TabPage tabExtraFuncs;
        private Button btnSearchFile;
        private TextBox txtSearchFile;
    }
}
