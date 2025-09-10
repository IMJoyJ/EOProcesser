using EOProcesser.UserControls;
namespace EOProcesser
{
    partial class DeckEditor
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
            mainContainer = new SplitContainer();
            tvFolder = new TreeView();
            btnSearch = new Button();
            txtSearchFile = new TextBox();
            tabDeckEditTabPage = new TabControl();
            tabDeckSelection = new TabPage();
            btnSave = new Button();
            btnMoveDownDeck = new Button();
            btnMoveUpDeck = new Button();
            btnDeleteDeck = new Button();
            btnAddDeck = new Button();
            listDeckList = new ListBox();
            tabExtraFuncs = new TabPage();
            txtExtraFunctions = new TextBox();
            tabDeckEditor = new TabPage();
            sdeDeckEditor = new SingleDeckEditor();
            ((System.ComponentModel.ISupportInitialize)mainContainer).BeginInit();
            mainContainer.Panel1.SuspendLayout();
            mainContainer.Panel2.SuspendLayout();
            mainContainer.SuspendLayout();
            tabDeckEditTabPage.SuspendLayout();
            tabDeckSelection.SuspendLayout();
            tabExtraFuncs.SuspendLayout();
            tabDeckEditor.SuspendLayout();
            SuspendLayout();
            // 
            // mainContainer
            // 
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.Location = new Point(0, 0);
            mainContainer.Name = "mainContainer";
            // 
            // mainContainer.Panel1
            // 
            mainContainer.Panel1.Controls.Add(tvFolder);
            mainContainer.Panel1.Controls.Add(btnSearch);
            mainContainer.Panel1.Controls.Add(txtSearchFile);
            // 
            // mainContainer.Panel2
            // 
            mainContainer.Panel2.Controls.Add(tabDeckEditTabPage);
            mainContainer.Size = new Size(541, 393);
            mainContainer.SplitterDistance = 180;
            mainContainer.TabIndex = 0;
            // 
            // tvFolder
            // 
            tvFolder.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tvFolder.Location = new Point(3, 32);
            tvFolder.Name = "tvFolder";
            tvFolder.Size = new Size(174, 357);
            tvFolder.TabIndex = 3;
            tvFolder.NodeMouseDoubleClick += tvFolder_NodeMouseDoubleClick;
            // 
            // btnSearch
            // 
            btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSearch.Location = new Point(136, 4);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(41, 21);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "検索";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // txtSearchFile
            // 
            txtSearchFile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearchFile.Location = new Point(3, 3);
            txtSearchFile.Name = "txtSearchFile";
            txtSearchFile.Size = new Size(127, 23);
            txtSearchFile.TabIndex = 1;
            txtSearchFile.KeyDown += txtSearchFile_KeyDown;
            // 
            // tabDeckEditTabPage
            // 
            tabDeckEditTabPage.Controls.Add(tabDeckSelection);
            tabDeckEditTabPage.Controls.Add(tabExtraFuncs);
            tabDeckEditTabPage.Controls.Add(tabDeckEditor);
            tabDeckEditTabPage.Dock = DockStyle.Fill;
            tabDeckEditTabPage.Location = new Point(0, 0);
            tabDeckEditTabPage.Name = "tabDeckEditTabPage";
            tabDeckEditTabPage.SelectedIndex = 0;
            tabDeckEditTabPage.Size = new Size(357, 393);
            tabDeckEditTabPage.TabIndex = 0;
            // 
            // tabDeckSelection
            // 
            tabDeckSelection.Controls.Add(btnSave);
            tabDeckSelection.Controls.Add(btnMoveDownDeck);
            tabDeckSelection.Controls.Add(btnMoveUpDeck);
            tabDeckSelection.Controls.Add(btnDeleteDeck);
            tabDeckSelection.Controls.Add(btnAddDeck);
            tabDeckSelection.Controls.Add(listDeckList);
            tabDeckSelection.Location = new Point(4, 24);
            tabDeckSelection.Name = "tabDeckSelection";
            tabDeckSelection.Padding = new Padding(3);
            tabDeckSelection.Size = new Size(349, 365);
            tabDeckSelection.TabIndex = 0;
            tabDeckSelection.Text = "編集デッキ選択";
            tabDeckSelection.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.Location = new Point(212, 177);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(131, 37);
            btnSave.TabIndex = 1;
            btnSave.Text = "セーブ";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnMoveDownDeck
            // 
            btnMoveDownDeck.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMoveDownDeck.Location = new Point(212, 134);
            btnMoveDownDeck.Name = "btnMoveDownDeck";
            btnMoveDownDeck.Size = new Size(131, 37);
            btnMoveDownDeck.TabIndex = 1;
            btnMoveDownDeck.Text = "下へ移動";
            btnMoveDownDeck.UseVisualStyleBackColor = true;
            btnMoveDownDeck.Click += btnMoveDownDeck_Click;
            // 
            // btnMoveUpDeck
            // 
            btnMoveUpDeck.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMoveUpDeck.Location = new Point(212, 91);
            btnMoveUpDeck.Name = "btnMoveUpDeck";
            btnMoveUpDeck.Size = new Size(131, 37);
            btnMoveUpDeck.TabIndex = 1;
            btnMoveUpDeck.Text = "上へ移動";
            btnMoveUpDeck.UseVisualStyleBackColor = true;
            btnMoveUpDeck.Click += btnMoveUpDeck_Click;
            // 
            // btnDeleteDeck
            // 
            btnDeleteDeck.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDeleteDeck.Location = new Point(212, 48);
            btnDeleteDeck.Name = "btnDeleteDeck";
            btnDeleteDeck.Size = new Size(131, 37);
            btnDeleteDeck.TabIndex = 1;
            btnDeleteDeck.Text = "デッキ削除";
            btnDeleteDeck.UseVisualStyleBackColor = true;
            btnDeleteDeck.Click += btnDeleteDeck_Click;
            // 
            // btnAddDeck
            // 
            btnAddDeck.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddDeck.Location = new Point(212, 5);
            btnAddDeck.Name = "btnAddDeck";
            btnAddDeck.Size = new Size(131, 37);
            btnAddDeck.TabIndex = 1;
            btnAddDeck.Text = "デッキ追加";
            btnAddDeck.UseVisualStyleBackColor = true;
            btnAddDeck.Click += btnAddDeck_Click;
            // 
            // listDeckList
            // 
            listDeckList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listDeckList.FormattingEnabled = true;
            listDeckList.ItemHeight = 15;
            listDeckList.Location = new Point(6, 5);
            listDeckList.Name = "listDeckList";
            listDeckList.Size = new Size(200, 349);
            listDeckList.TabIndex = 0;
            listDeckList.DoubleClick += listDeckList_DoubleClick;
            // 
            // tabExtraFuncs
            // 
            tabExtraFuncs.Controls.Add(txtExtraFunctions);
            tabExtraFuncs.Location = new Point(4, 24);
            tabExtraFuncs.Name = "tabExtraFuncs";
            tabExtraFuncs.Padding = new Padding(3);
            tabExtraFuncs.Size = new Size(349, 365);
            tabExtraFuncs.TabIndex = 1;
            tabExtraFuncs.Text = "エクストラ関数定義";
            tabExtraFuncs.UseVisualStyleBackColor = true;
            // 
            // txtExtraFunctions
            // 
            txtExtraFunctions.Dock = DockStyle.Fill;
            txtExtraFunctions.Location = new Point(3, 3);
            txtExtraFunctions.Multiline = true;
            txtExtraFunctions.Name = "txtExtraFunctions";
            txtExtraFunctions.ScrollBars = ScrollBars.Vertical;
            txtExtraFunctions.Size = new Size(343, 359);
            txtExtraFunctions.TabIndex = 0;
            // 
            // tabDeckEditor
            // 
            tabDeckEditor.Controls.Add(sdeDeckEditor);
            tabDeckEditor.Location = new Point(4, 24);
            tabDeckEditor.Name = "tabDeckEditor";
            tabDeckEditor.Size = new Size(349, 365);
            tabDeckEditor.TabIndex = 2;
            tabDeckEditor.Text = "デッキ編集";
            tabDeckEditor.UseVisualStyleBackColor = true;
            // 
            // sdeDeckEditor
            // 
            sdeDeckEditor.Dock = DockStyle.Fill;
            sdeDeckEditor.Location = new Point(0, 0);
            sdeDeckEditor.Name = "sdeDeckEditor";
            sdeDeckEditor.Size = new Size(349, 365);
            sdeDeckEditor.TabIndex = 0;
            sdeDeckEditor.OnDeckChanged += sdeDeckEditor_OnDeckChanged;
            // 
            // DeckEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(mainContainer);
            Name = "DeckEditor";
            Size = new Size(541, 393);
            mainContainer.Panel1.ResumeLayout(false);
            mainContainer.Panel1.PerformLayout();
            mainContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mainContainer).EndInit();
            mainContainer.ResumeLayout(false);
            tabDeckEditTabPage.ResumeLayout(false);
            tabDeckSelection.ResumeLayout(false);
            tabExtraFuncs.ResumeLayout(false);
            tabExtraFuncs.PerformLayout();
            tabDeckEditor.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private SplitContainer mainContainer;
        private TabControl tabDeckEditTabPage;
        private TabPage tabDeckSelection;
        private TabPage tabExtraFuncs;
        private Button btnSearch;
        private TextBox txtSearchFile;
        private Button btnMoveDownDeck;
        private Button btnMoveUpDeck;
        private Button btnDeleteDeck;
        private Button btnAddDeck;
        private ListBox listDeckList;
        private TreeView tvFolder;
        private TabPage tabDeckEditor;
        private Button btnSave;
        private TextBox txtExtraFunctions;
        private Button btnOutputCode;
        private SingleDeckEditor sdeDeckEditor;
    }
}
