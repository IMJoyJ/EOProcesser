
namespace EOProcesser.UserControls
{
    partial class SingleDeckEditor
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
            btnCopyCode = new Button();
            btnExportToFile = new Button();
            btnLoadDeckFromFile = new Button();
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
            ((System.ComponentModel.ISupportInitialize)numDeckId).BeginInit();
            SuspendLayout();
            // 
            // btnCopyCode
            // 
            btnCopyCode.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCopyCode.Location = new Point(5, 331);
            btnCopyCode.Name = "btnCopyCode";
            btnCopyCode.Size = new Size(151, 23);
            btnCopyCode.TabIndex = 32;
            btnCopyCode.Text = "コードをコピーする";
            btnCopyCode.UseVisualStyleBackColor = true;
            btnCopyCode.Click += btnCopyCode_Click;
            // 
            // btnExportToFile
            // 
            btnExportToFile.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnExportToFile.Location = new Point(81, 302);
            btnExportToFile.Name = "btnExportToFile";
            btnExportToFile.Size = new Size(75, 23);
            btnExportToFile.TabIndex = 31;
            btnExportToFile.Text = "ファイル出力";
            btnExportToFile.UseVisualStyleBackColor = true;
            btnExportToFile.Click += btnExportToFile_Click;
            // 
            // btnLoadDeckFromFile
            // 
            btnLoadDeckFromFile.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnLoadDeckFromFile.Location = new Point(5, 302);
            btnLoadDeckFromFile.Name = "btnLoadDeckFromFile";
            btnLoadDeckFromFile.Size = new Size(75, 23);
            btnLoadDeckFromFile.TabIndex = 30;
            btnLoadDeckFromFile.Text = "ファイル読込";
            btnLoadDeckFromFile.UseVisualStyleBackColor = true;
            btnLoadDeckFromFile.Click += btnLoadDeckFromFile_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(171, 43);
            label2.Name = "label2";
            label2.Size = new Size(45, 15);
            label2.TabIndex = 29;
            label2.Text = "デッキ名";
            // 
            // txtDeckName
            // 
            txtDeckName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtDeckName.Location = new Point(222, 40);
            txtDeckName.Name = "txtDeckName";
            txtDeckName.Size = new Size(121, 23);
            txtDeckName.TabIndex = 28;
            txtDeckName.TextChanged += txtDeckName_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(195, 13);
            label1.Name = "label1";
            label1.Size = new Size(18, 15);
            label1.TabIndex = 27;
            label1.Text = "ID";
            // 
            // numDeckId
            // 
            numDeckId.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            numDeckId.Location = new Point(222, 11);
            numDeckId.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numDeckId.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numDeckId.Name = "numDeckId";
            numDeckId.Size = new Size(121, 23);
            numDeckId.TabIndex = 26;
            numDeckId.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numDeckId.ValueChanged += numDeckId_ValueChanged;
            // 
            // btnSearchCard
            // 
            btnSearchCard.Location = new Point(113, 9);
            btnSearchCard.Name = "btnSearchCard";
            btnSearchCard.Size = new Size(43, 22);
            btnSearchCard.TabIndex = 25;
            btnSearchCard.Text = "検索";
            btnSearchCard.UseVisualStyleBackColor = true;
            btnSearchCard.Click += btnSearchCard_Click;
            // 
            // txtSearchCard
            // 
            txtSearchCard.Location = new Point(5, 9);
            txtSearchCard.Name = "txtSearchCard";
            txtSearchCard.Size = new Size(102, 23);
            txtSearchCard.TabIndex = 24;
            txtSearchCard.KeyDown += txtSearchCard_KeyDown;
            // 
            // btnRemoveFromExtraDeck
            // 
            btnRemoveFromExtraDeck.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnRemoveFromExtraDeck.Location = new Point(162, 285);
            btnRemoveFromExtraDeck.Name = "btnRemoveFromExtraDeck";
            btnRemoveFromExtraDeck.Size = new Size(27, 23);
            btnRemoveFromExtraDeck.TabIndex = 23;
            btnRemoveFromExtraDeck.Text = "←";
            btnRemoveFromExtraDeck.UseVisualStyleBackColor = true;
            btnRemoveFromExtraDeck.Click += btnRemoveFromExtraDeck_Click;
            // 
            // btnMinus1ToExtraDeck
            // 
            btnMinus1ToExtraDeck.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnMinus1ToExtraDeck.Location = new Point(162, 314);
            btnMinus1ToExtraDeck.Name = "btnMinus1ToExtraDeck";
            btnMinus1ToExtraDeck.Size = new Size(27, 23);
            btnMinus1ToExtraDeck.TabIndex = 22;
            btnMinus1ToExtraDeck.Text = "-1";
            btnMinus1ToExtraDeck.UseVisualStyleBackColor = true;
            btnMinus1ToExtraDeck.Click += btnMinus1ToExtraDeck_Click;
            // 
            // btnMinus1ToMainDeck
            // 
            btnMinus1ToMainDeck.Location = new Point(162, 171);
            btnMinus1ToMainDeck.Name = "btnMinus1ToMainDeck";
            btnMinus1ToMainDeck.Size = new Size(27, 23);
            btnMinus1ToMainDeck.TabIndex = 21;
            btnMinus1ToMainDeck.Text = "-1";
            btnMinus1ToMainDeck.UseVisualStyleBackColor = true;
            btnMinus1ToMainDeck.Click += btnMinus1ToMainDeck_Click;
            // 
            // btnRemoveFromMainDeck
            // 
            btnRemoveFromMainDeck.Location = new Point(162, 142);
            btnRemoveFromMainDeck.Name = "btnRemoveFromMainDeck";
            btnRemoveFromMainDeck.Size = new Size(27, 23);
            btnRemoveFromMainDeck.TabIndex = 20;
            btnRemoveFromMainDeck.Text = "←";
            btnRemoveFromMainDeck.UseVisualStyleBackColor = true;
            btnRemoveFromMainDeck.Click += btnRemoveFromMainDeck_Click;
            // 
            // btnAddToExtraDeck
            // 
            btnAddToExtraDeck.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAddToExtraDeck.Location = new Point(162, 256);
            btnAddToExtraDeck.Name = "btnAddToExtraDeck";
            btnAddToExtraDeck.Size = new Size(27, 23);
            btnAddToExtraDeck.TabIndex = 19;
            btnAddToExtraDeck.Text = "→";
            btnAddToExtraDeck.UseVisualStyleBackColor = true;
            btnAddToExtraDeck.Click += btnAddToExtraDeck_Click;
            // 
            // btnAddToMainDeck
            // 
            btnAddToMainDeck.Location = new Point(162, 113);
            btnAddToMainDeck.Name = "btnAddToMainDeck";
            btnAddToMainDeck.Size = new Size(27, 23);
            btnAddToMainDeck.TabIndex = 18;
            btnAddToMainDeck.Text = "→";
            btnAddToMainDeck.UseVisualStyleBackColor = true;
            btnAddToMainDeck.Click += btnAddToMainDeck_Click;
            // 
            // lbExtraDeckCount
            // 
            lbExtraDeckCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lbExtraDeckCount.AutoSize = true;
            lbExtraDeckCount.Location = new Point(195, 214);
            lbExtraDeckCount.Name = "lbExtraDeckCount";
            lbExtraDeckCount.Size = new Size(67, 15);
            lbExtraDeckCount.TabIndex = 17;
            lbExtraDeckCount.Text = "現在枚数：";
            // 
            // lbMainDeckCount
            // 
            lbMainDeckCount.AutoSize = true;
            lbMainDeckCount.Location = new Point(195, 67);
            lbMainDeckCount.Name = "lbMainDeckCount";
            lbMainDeckCount.Size = new Size(67, 15);
            lbMainDeckCount.TabIndex = 16;
            lbMainDeckCount.Text = "現在枚数：";
            // 
            // listExtraDeck
            // 
            listExtraDeck.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listExtraDeck.FormattingEnabled = true;
            listExtraDeck.ItemHeight = 15;
            listExtraDeck.Location = new Point(195, 232);
            listExtraDeck.Name = "listExtraDeck";
            listExtraDeck.Size = new Size(153, 124);
            listExtraDeck.TabIndex = 15;
            listExtraDeck.DoubleClick += listExtraDeck_DoubleClick;
            listExtraDeck.KeyDown += listExtraDeck_KeyDown;
            // 
            // listMainDeck
            // 
            listMainDeck.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listMainDeck.FormattingEnabled = true;
            listMainDeck.ItemHeight = 15;
            listMainDeck.Location = new Point(195, 85);
            listMainDeck.Name = "listMainDeck";
            listMainDeck.Size = new Size(153, 124);
            listMainDeck.TabIndex = 14;
            listMainDeck.DoubleClick += listMainDeck_DoubleClick;
            listMainDeck.KeyDown += listMainDeck_KeyDown;
            // 
            // listCardDictionary
            // 
            listCardDictionary.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listCardDictionary.FormattingEnabled = true;
            listCardDictionary.ItemHeight = 15;
            listCardDictionary.Location = new Point(5, 37);
            listCardDictionary.Name = "listCardDictionary";
            listCardDictionary.Size = new Size(151, 259);
            listCardDictionary.TabIndex = 13;
            // 
            // SingleDeckEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnCopyCode);
            Controls.Add(btnExportToFile);
            Controls.Add(btnLoadDeckFromFile);
            Controls.Add(label2);
            Controls.Add(txtDeckName);
            Controls.Add(label1);
            Controls.Add(numDeckId);
            Controls.Add(btnSearchCard);
            Controls.Add(txtSearchCard);
            Controls.Add(btnRemoveFromExtraDeck);
            Controls.Add(btnMinus1ToExtraDeck);
            Controls.Add(btnMinus1ToMainDeck);
            Controls.Add(btnRemoveFromMainDeck);
            Controls.Add(btnAddToExtraDeck);
            Controls.Add(btnAddToMainDeck);
            Controls.Add(lbExtraDeckCount);
            Controls.Add(lbMainDeckCount);
            Controls.Add(listExtraDeck);
            Controls.Add(listMainDeck);
            Controls.Add(listCardDictionary);
            Name = "SingleDeckEditor";
            Size = new Size(353, 364);
            ((System.ComponentModel.ISupportInitialize)numDeckId).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnCopyCode;
        private Button btnExportToFile;
        private Button btnLoadDeckFromFile;
        private Label label2;
        private TextBox txtDeckName;
        private Label label1;
        private NumericUpDown numDeckId;
        private Button btnSearchCard;
        private TextBox txtSearchCard;
        private Button btnRemoveFromExtraDeck;
        private Button btnMinus1ToExtraDeck;
        private Button btnMinus1ToMainDeck;
        private Button btnRemoveFromMainDeck;
        private Button btnAddToExtraDeck;
        private Button btnAddToMainDeck;
        private Label lbExtraDeckCount;
        private Label lbMainDeckCount;
        private ListBox listExtraDeck;
        private ListBox listMainDeck;
        private ListBox listCardDictionary;
    }
}
