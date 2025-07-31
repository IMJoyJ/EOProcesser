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
            splitContainer = new SplitContainer();
            tvFolderFiles = new TreeView();
            tvCode = new TreeView();
            menuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            tabControl = new TabControl();
            tabPageCodeEdit = new TabPage();
            tabPageCardEdit = new TabPage();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            menuStrip.SuspendLayout();
            tabControl.SuspendLayout();
            tabPageCodeEdit.SuspendLayout();
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
            splitContainer.Panel2.Controls.Add(tvCode);
            splitContainer.Size = new Size(1785, 802);
            splitContainer.SplitterDistance = 594;
            splitContainer.SplitterWidth = 9;
            splitContainer.TabIndex = 0;
            // 
            // tvFolderFiles
            // 
            tvFolderFiles.Dock = DockStyle.Fill;
            tvFolderFiles.Location = new Point(0, 0);
            tvFolderFiles.Margin = new Padding(7, 6, 7, 6);
            tvFolderFiles.Name = "tvFolderFiles";
            tvFolderFiles.Size = new Size(594, 802);
            tvFolderFiles.TabIndex = 0;
            tvFolderFiles.NodeMouseDoubleClick += tvFolderFiles_NodeMouseDoubleClick;
            // 
            // tvCode
            // 
            tvCode.Dock = DockStyle.Fill;
            tvCode.Location = new Point(0, 0);
            tvCode.Margin = new Padding(7, 6, 7, 6);
            tvCode.Name = "tvCode";
            tvCode.Size = new Size(1182, 802);
            tvCode.TabIndex = 0;
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(14, 4, 0, 4);
            menuStrip.Size = new Size(1811, 51);
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
            settingsToolStripMenuItem.Size = new Size(448, 54);
            settingsToolStripMenuItem.Text = "Settings";
            settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(448, 54);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabPageCardEdit);
            tabControl.Controls.Add(tabPageCodeEdit);
            tabControl.Dock = DockStyle.Bottom;
            tabControl.Location = new Point(0, 54);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1811, 874);
            tabControl.TabIndex = 2;
            // 
            // tabPageCodeEdit
            // 
            tabPageCodeEdit.Controls.Add(splitContainer);
            tabPageCodeEdit.Location = new Point(10, 56);
            tabPageCodeEdit.Name = "tabPageCodeEdit";
            tabPageCodeEdit.Padding = new Padding(3);
            tabPageCodeEdit.Size = new Size(1791, 808);
            tabPageCodeEdit.TabIndex = 0;
            tabPageCodeEdit.Text = "Code Edit";
            tabPageCodeEdit.UseVisualStyleBackColor = true;
            // 
            // tabPageCardEdit
            // 
            tabPageCardEdit.Location = new Point(10, 56);
            tabPageCardEdit.Name = "tabPageCardEdit";
            tabPageCardEdit.Padding = new Padding(3);
            tabPageCardEdit.Size = new Size(1791, 808);
            tabPageCardEdit.TabIndex = 1;
            tabPageCardEdit.Text = "Card Edit";
            tabPageCardEdit.UseVisualStyleBackColor = true;
            // 
            // EOProcesser
            // 
            AutoScaleDimensions = new SizeF(18F, 39F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1811, 928);
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
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            tabControl.ResumeLayout(false);
            tabPageCodeEdit.ResumeLayout(false);
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
        private TabPage tabPageCodeEdit;
    }
}
