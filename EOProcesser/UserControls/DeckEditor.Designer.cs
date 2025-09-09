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
            label2 = new Label();
            txtDeckName = new TextBox();
            label1 = new Label();
            numDeckId = new NumericUpDown();
            btnSearchCard = new Button();
            txtSearchCard = new TextBox();
            btnRemoveFromExtraDeck = new Button();
            btnMinus1ToExtraDeck = new Button();
            btnMinus1ToMainDeck = new Button();
            btnRemoveFromMainDeck = new Button();
            btnAddToExtraDeck = new Button();
            btnAddToMainDeck = new Button();
            lbExtraDeckCount = new Label();
            lbMainDeckCount = new Label();
            listExtraDeck = new ListBox();
            listMainDeck = new ListBox();
            listCardDictionary = new ListBox();
            btnLoadDeckFromFile = new Button();
            btnExportToFile = new Button();
            ((System.ComponentModel.ISupportInitialize)mainContainer).BeginInit();
            mainContainer.Panel1.SuspendLayout();
            mainContainer.Panel2.SuspendLayout();
            mainContainer.SuspendLayout();
            tabDeckEditTabPage.SuspendLayout();
            tabDeckSelection.SuspendLayout();
            tabExtraFuncs.SuspendLayout();
            tabDeckEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDeckId).BeginInit();
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
            tabDeckEditor.Controls.Add(btnExportToFile);
            tabDeckEditor.Controls.Add(btnLoadDeckFromFile);
            tabDeckEditor.Controls.Add(label2);
            tabDeckEditor.Controls.Add(txtDeckName);
            tabDeckEditor.Controls.Add(label1);
            tabDeckEditor.Controls.Add(numDeckId);
            tabDeckEditor.Controls.Add(btnSearchCard);
            tabDeckEditor.Controls.Add(txtSearchCard);
            tabDeckEditor.Controls.Add(btnRemoveFromExtraDeck);
            tabDeckEditor.Controls.Add(btnMinus1ToExtraDeck);
            tabDeckEditor.Controls.Add(btnMinus1ToMainDeck);
            tabDeckEditor.Controls.Add(btnRemoveFromMainDeck);
            tabDeckEditor.Controls.Add(btnAddToExtraDeck);
            tabDeckEditor.Controls.Add(btnAddToMainDeck);
            tabDeckEditor.Controls.Add(lbExtraDeckCount);
            tabDeckEditor.Controls.Add(lbMainDeckCount);
            tabDeckEditor.Controls.Add(listExtraDeck);
            tabDeckEditor.Controls.Add(listMainDeck);
            tabDeckEditor.Controls.Add(listCardDictionary);
            tabDeckEditor.Location = new Point(4, 24);
            tabDeckEditor.Name = "tabDeckEditor";
            tabDeckEditor.Size = new Size(349, 365);
            tabDeckEditor.TabIndex = 2;
            tabDeckEditor.Text = "デッキ編集";
            tabDeckEditor.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(169, 44);
            label2.Name = "label2";
            label2.Size = new Size(45, 15);
            label2.TabIndex = 11;
            label2.Text = "デッキ名";
            // 
            // txtDeckName
            // 
            txtDeckName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtDeckName.Location = new Point(220, 41);
            txtDeckName.Name = "txtDeckName";
            txtDeckName.Size = new Size(121, 23);
            txtDeckName.TabIndex = 10;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(193, 14);
            label1.Name = "label1";
            label1.Size = new Size(18, 15);
            label1.TabIndex = 9;
            label1.Text = "ID";
            // 
            // numDeckId
            // 
            numDeckId.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            numDeckId.Location = new Point(220, 12);
            numDeckId.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numDeckId.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numDeckId.Name = "numDeckId";
            numDeckId.Size = new Size(121, 23);
            numDeckId.TabIndex = 8;
            numDeckId.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnSearchCard
            // 
            btnSearchCard.Location = new Point(111, 10);
            btnSearchCard.Name = "btnSearchCard";
            btnSearchCard.Size = new Size(43, 22);
            btnSearchCard.TabIndex = 7;
            btnSearchCard.Text = "検索";
            btnSearchCard.UseVisualStyleBackColor = true;
            btnSearchCard.Click += btnSearchCard_Click;
            // 
            // txtSearchCard
            // 
            txtSearchCard.Location = new Point(3, 10);
            txtSearchCard.Name = "txtSearchCard";
            txtSearchCard.Size = new Size(102, 23);
            txtSearchCard.TabIndex = 6;
            txtSearchCard.KeyDown += txtSearchCard_KeyDown;
            // 
            // btnRemoveFromExtraDeck
            // 
            btnRemoveFromExtraDeck.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnRemoveFromExtraDeck.Location = new Point(160, 286);
            btnRemoveFromExtraDeck.Name = "btnRemoveFromExtraDeck";
            btnRemoveFromExtraDeck.Size = new Size(27, 23);
            btnRemoveFromExtraDeck.TabIndex = 5;
            btnRemoveFromExtraDeck.Text = "←";
            btnRemoveFromExtraDeck.UseVisualStyleBackColor = true;
            btnRemoveFromExtraDeck.Click += btnRemoveFromExtraDeck_Click;
            // 
            // btnMinus1ToExtraDeck
            // 
            btnMinus1ToExtraDeck.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnMinus1ToExtraDeck.Location = new Point(160, 315);
            btnMinus1ToExtraDeck.Name = "btnMinus1ToExtraDeck";
            btnMinus1ToExtraDeck.Size = new Size(27, 23);
            btnMinus1ToExtraDeck.TabIndex = 5;
            btnMinus1ToExtraDeck.Text = "-1";
            btnMinus1ToExtraDeck.UseVisualStyleBackColor = true;
            btnMinus1ToExtraDeck.Click += btnMinus1ToExtraDeck_Click;
            // 
            // btnMinus1ToMainDeck
            // 
            btnMinus1ToMainDeck.Location = new Point(160, 172);
            btnMinus1ToMainDeck.Name = "btnMinus1ToMainDeck";
            btnMinus1ToMainDeck.Size = new Size(27, 23);
            btnMinus1ToMainDeck.TabIndex = 5;
            btnMinus1ToMainDeck.Text = "-1";
            btnMinus1ToMainDeck.UseVisualStyleBackColor = true;
            btnMinus1ToMainDeck.Click += btnMinus1ToMainDeck_Click;
            // 
            // btnRemoveFromMainDeck
            // 
            btnRemoveFromMainDeck.Location = new Point(160, 143);
            btnRemoveFromMainDeck.Name = "btnRemoveFromMainDeck";
            btnRemoveFromMainDeck.Size = new Size(27, 23);
            btnRemoveFromMainDeck.TabIndex = 5;
            btnRemoveFromMainDeck.Text = "←";
            btnRemoveFromMainDeck.UseVisualStyleBackColor = true;
            btnRemoveFromMainDeck.Click += btnRemoveFromMainDeck_Click;
            // 
            // btnAddToExtraDeck
            // 
            btnAddToExtraDeck.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAddToExtraDeck.Location = new Point(160, 257);
            btnAddToExtraDeck.Name = "btnAddToExtraDeck";
            btnAddToExtraDeck.Size = new Size(27, 23);
            btnAddToExtraDeck.TabIndex = 5;
            btnAddToExtraDeck.Text = "→";
            btnAddToExtraDeck.UseVisualStyleBackColor = true;
            btnAddToExtraDeck.Click += btnAddToExtraDeck_Click;
            // 
            // btnAddToMainDeck
            // 
            btnAddToMainDeck.Location = new Point(160, 114);
            btnAddToMainDeck.Name = "btnAddToMainDeck";
            btnAddToMainDeck.Size = new Size(27, 23);
            btnAddToMainDeck.TabIndex = 5;
            btnAddToMainDeck.Text = "→";
            btnAddToMainDeck.UseVisualStyleBackColor = true;
            btnAddToMainDeck.Click += btnAddToMainDeck_Click;
            // 
            // lbExtraDeckCount
            // 
            lbExtraDeckCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lbExtraDeckCount.AutoSize = true;
            lbExtraDeckCount.Location = new Point(193, 215);
            lbExtraDeckCount.Name = "lbExtraDeckCount";
            lbExtraDeckCount.Size = new Size(67, 15);
            lbExtraDeckCount.TabIndex = 4;
            lbExtraDeckCount.Text = "現在枚数：";
            // 
            // lbMainDeckCount
            // 
            lbMainDeckCount.AutoSize = true;
            lbMainDeckCount.Location = new Point(193, 68);
            lbMainDeckCount.Name = "lbMainDeckCount";
            lbMainDeckCount.Size = new Size(67, 15);
            lbMainDeckCount.TabIndex = 3;
            lbMainDeckCount.Text = "現在枚数：";
            // 
            // listExtraDeck
            // 
            listExtraDeck.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listExtraDeck.FormattingEnabled = true;
            listExtraDeck.ItemHeight = 15;
            listExtraDeck.Location = new Point(193, 233);
            listExtraDeck.Name = "listExtraDeck";
            listExtraDeck.SelectionMode = SelectionMode.MultiExtended;
            listExtraDeck.Size = new Size(153, 124);
            listExtraDeck.TabIndex = 2;
            listExtraDeck.DoubleClick += listExtraDeck_DoubleClick;
            listExtraDeck.KeyDown += listExtraDeck_KeyDown;
            // 
            // listMainDeck
            // 
            listMainDeck.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listMainDeck.FormattingEnabled = true;
            listMainDeck.ItemHeight = 15;
            listMainDeck.Location = new Point(193, 86);
            listMainDeck.Name = "listMainDeck";
            listMainDeck.SelectionMode = SelectionMode.MultiExtended;
            listMainDeck.Size = new Size(153, 124);
            listMainDeck.TabIndex = 1;
            listMainDeck.DoubleClick += listMainDeck_DoubleClick;
            listMainDeck.KeyDown += listMainDeck_KeyDown;
            // 
            // listCardDictionary
            // 
            listCardDictionary.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listCardDictionary.FormattingEnabled = true;
            listCardDictionary.ItemHeight = 15;
            listCardDictionary.Location = new Point(3, 38);
            listCardDictionary.Name = "listCardDictionary";
            listCardDictionary.Size = new Size(151, 289);
            listCardDictionary.TabIndex = 0;
            // 
            // btnLoadDeckFromFile
            // 
            btnLoadDeckFromFile.Location = new Point(3, 333);
            btnLoadDeckFromFile.Name = "btnLoadDeckFromFile";
            btnLoadDeckFromFile.Size = new Size(75, 23);
            btnLoadDeckFromFile.TabIndex = 12;
            btnLoadDeckFromFile.Text = "ファイル読込";
            btnLoadDeckFromFile.UseVisualStyleBackColor = true;
            btnLoadDeckFromFile.Click += btnLoadDeckFromFile_Click;
            // 
            // btnExportToFile
            // 
            btnExportToFile.Location = new Point(79, 333);
            btnExportToFile.Name = "btnExportToFile";
            btnExportToFile.Size = new Size(75, 23);
            btnExportToFile.TabIndex = 12;
            btnExportToFile.Text = "ファイル出力";
            btnExportToFile.UseVisualStyleBackColor = true;
            btnExportToFile.Click += btnExportToFile_Click;
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
            tabDeckEditor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDeckId).EndInit();
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
        private ListBox listCardDictionary;
        private Button btnRemoveFromExtraDeck;
        private Button btnRemoveFromMainDeck;
        private Button btnAddToExtraDeck;
        private Button btnAddToMainDeck;
        private Label lbExtraDeckCount;
        private Label lbMainDeckCount;
        private ListBox listExtraDeck;
        private ListBox listMainDeck;
        private Button btnSearchCard;
        private TextBox txtSearchCard;
        private Label label2;
        private TextBox txtDeckName;
        private Label label1;
        private NumericUpDown numDeckId;
        private Button btnSave;
        private TextBox txtExtraFunctions;
        private Button btnMinus1ToMainDeck;
        private Button btnMinus1ToExtraDeck;
        private Button btnExportToFile;
        private Button btnLoadDeckFromFile;
    }
}
