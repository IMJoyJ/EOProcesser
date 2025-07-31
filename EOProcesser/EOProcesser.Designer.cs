namespace EOProcesser
{
    partial class EOProcesser
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            splitContainer = new SplitContainer();
            tvFolderFiles = new TreeView();
            splitContainer1 = new SplitContainer();
            tvCode = new TreeView();
            txtCode = new TextBox();
            menuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            tabControl = new TabControl();
            tabPageCardEdit = new TabPage();
            tabPageCodeView = new TabPage();
            CodeViewMenuStrip = new ContextMenuStrip(components);
            openToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            menuStrip.SuspendLayout();
            tabControl.SuspendLayout();
            tabPageCodeView.SuspendLayout();
            CodeViewMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(3, 3);
            splitContainer.Margin = new Padding(7, 6, 7, 6);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(tvFolderFiles);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(splitContainer1);
            splitContainer.Size = new Size(1693, 875);
            splitContainer.SplitterDistance = 563;
            splitContainer.SplitterWidth = 9;
            splitContainer.TabIndex = 0;
            // 
            // tvFolderFiles
            // 
            tvFolderFiles.Dock = DockStyle.Fill;
            tvFolderFiles.Location = new Point(0, 0);
            tvFolderFiles.Margin = new Padding(7, 6, 7, 6);
            tvFolderFiles.Name = "tvFolderFiles";
            tvFolderFiles.Size = new Size(563, 875);
            tvFolderFiles.TabIndex = 0;
            tvFolderFiles.NodeMouseClick += tvFolderFiles_NodeMouseClick;
            tvFolderFiles.NodeMouseDoubleClick += tvFolderFiles_NodeMouseDoubleClick;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tvCode);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(txtCode);
            splitContainer1.Size = new Size(1121, 875);
            splitContainer1.SplitterDistance = 437;
            splitContainer1.TabIndex = 1;
            // 
            // tvCode
            // 
            tvCode.Dock = DockStyle.Fill;
            tvCode.Location = new Point(0, 0);
            tvCode.Margin = new Padding(7, 6, 7, 6);
            tvCode.Name = "tvCode";
            tvCode.Size = new Size(1121, 437);
            tvCode.TabIndex = 0;
            // 
            // txtCode
            // 
            txtCode.Dock = DockStyle.Fill;
            txtCode.Location = new Point(0, 0);
            txtCode.Multiline = true;
            txtCode.Name = "txtCode";
            txtCode.ScrollBars = ScrollBars.Vertical;
            txtCode.Size = new Size(1121, 434);
            txtCode.TabIndex = 0;
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(14, 4, 0, 4);
            menuStrip.Size = new Size(1719, 51);
            menuStrip.TabIndex = 1;
            menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { settingsToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(90, 43);
            fileToolStripMenuItem.Text = "File";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(297, 54);
            settingsToolStripMenuItem.Text = "Settings";
            settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(297, 54);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabPageCardEdit);
            tabControl.Controls.Add(tabPageCodeView);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 51);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1719, 947);
            tabControl.TabIndex = 2;
            // 
            // tabPageCardEdit
            // 
            tabPageCardEdit.Location = new Point(10, 56);
            tabPageCardEdit.Name = "tabPageCardEdit";
            tabPageCardEdit.Padding = new Padding(3);
            tabPageCardEdit.Size = new Size(1699, 881);
            tabPageCardEdit.TabIndex = 1;
            tabPageCardEdit.Text = "Card Edit";
            tabPageCardEdit.UseVisualStyleBackColor = true;
            // 
            // tabPageCodeView
            // 
            tabPageCodeView.Controls.Add(splitContainer);
            tabPageCodeView.Location = new Point(10, 56);
            tabPageCodeView.Name = "tabPageCodeView";
            tabPageCodeView.Padding = new Padding(3);
            tabPageCodeView.Size = new Size(1699, 881);
            tabPageCodeView.TabIndex = 0;
            tabPageCodeView.Text = "Code View";
            tabPageCodeView.UseVisualStyleBackColor = true;
            // 
            // CodeViewMenuStrip
            // 
            CodeViewMenuStrip.ImageScalingSize = new Size(40, 40);
            CodeViewMenuStrip.Items.AddRange(new ToolStripItem[] { openToolStripMenuItem });
            CodeViewMenuStrip.Name = "CodeViewMenuStrip";
            CodeViewMenuStrip.Size = new Size(173, 50);
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(172, 46);
            openToolStripMenuItem.Text = "Open";
            // 
            // EOProcesser
            // 
            AutoScaleDimensions = new SizeF(18F, 39F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1719, 998);
            Controls.Add(tabControl);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Margin = new Padding(7, 6, 7, 6);
            Name = "EOProcesser";
            Text = "ERAOCG Card Manager v0.1 by JoyJ";
            Load += EOProcesser_Load;
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            tabControl.ResumeLayout(false);
            tabPageCodeView.ResumeLayout(false);
            CodeViewMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SplitContainer splitContainer;
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private TreeView tvFolderFiles;
        private TreeView tvCode;
        private TabControl tabControl;
        private TabPage tabPageCardEdit;
        private TabPage tabPageCodeView;
        private ContextMenuStrip CodeViewMenuStrip;
        private ToolStripMenuItem openToolStripMenuItem;
        private SplitContainer splitContainer1;
        private TextBox txtCode;
    }
}
