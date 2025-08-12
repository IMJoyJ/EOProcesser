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
            splitContainerCardEdit = new SplitContainer();
            btnSearch = new Button();
            txtSearchCard = new TextBox();
            treeCards = new TreeView();
            tabCardEdit = new TabControl();
            tabCardSelect = new TabPage();
            btnCardScriptMoveDown = new Button();
            btnCardScriptMoveUp = new Button();
            btnCardScriptRemoveCard = new Button();
            btnCardScriptAddCard = new Button();
            listCardScriptCard = new ListBox();
            tabCardInfoSettings = new TabPage();
            btnImport = new Button();
            btnExport = new Button();
            btnSave = new Button();
            btnEditCardInfo = new Button();
            label5 = new Label();
            btnMoveDownCardInfo = new Button();
            btnMoveUpCardInfo = new Button();
            btnDeleteCardInfo = new Button();
            btnAddCardInfo = new Button();
            btnQuickSetSpellTrap = new Button();
            btnQuickSetMonster = new Button();
            listCardInfo = new ListBox();
            label4 = new Label();
            listCategory = new ListBox();
            label3 = new Label();
            label2 = new Label();
            txtShortName = new TextBox();
            label1 = new Label();
            txtCardName = new TextBox();
            tabEffect = new TabPage();
            treeView1 = new TreeView();
            radioCustomStandardEffect = new RadioButton();
            radioCMStandardEffect = new RadioButton();
            tabCardExplanation = new TabPage();
            btnReset = new Button();
            btnAddEffectText = new Button();
            treeEffectFunc = new TreeView();
            tabEffectCan = new TabPage();
            tabAA = new TabPage();
            tabExtraFuncs = new TabPage();
            tabPageCodeView = new TabPage();
            CodeViewMenuStrip = new ContextMenuStrip(components);
            openToolStripMenuItem = new ToolStripMenuItem();
            bwLoadCards = new System.ComponentModel.BackgroundWorker();
            tabEffectFunc = new TabPage();
            label6 = new Label();
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
            tabPageCardEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerCardEdit).BeginInit();
            splitContainerCardEdit.Panel1.SuspendLayout();
            splitContainerCardEdit.Panel2.SuspendLayout();
            splitContainerCardEdit.SuspendLayout();
            tabCardEdit.SuspendLayout();
            tabCardSelect.SuspendLayout();
            tabCardInfoSettings.SuspendLayout();
            tabEffect.SuspendLayout();
            tabCardExplanation.SuspendLayout();
            tabPageCodeView.SuspendLayout();
            CodeViewMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(3, 5);
            splitContainer.Margin = new Padding(8, 7, 8, 7);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(tvFolderFiles);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(splitContainer1);
            splitContainer.Size = new Size(1666, 895);
            splitContainer.SplitterDistance = 551;
            splitContainer.SplitterWidth = 10;
            splitContainer.TabIndex = 0;
            // 
            // tvFolderFiles
            // 
            tvFolderFiles.Dock = DockStyle.Fill;
            tvFolderFiles.Location = new Point(0, 0);
            tvFolderFiles.Margin = new Padding(8, 7, 8, 7);
            tvFolderFiles.Name = "tvFolderFiles";
            tvFolderFiles.Size = new Size(551, 895);
            tvFolderFiles.TabIndex = 0;
            tvFolderFiles.NodeMouseClick += tvFolderFiles_NodeMouseClick;
            tvFolderFiles.NodeMouseDoubleClick += tvFolderFiles_NodeMouseDoubleClick;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Margin = new Padding(3, 5, 3, 5);
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
            splitContainer1.Size = new Size(1105, 895);
            splitContainer1.SplitterDistance = 443;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 1;
            // 
            // tvCode
            // 
            tvCode.Dock = DockStyle.Fill;
            tvCode.Location = new Point(0, 0);
            tvCode.Margin = new Padding(8, 7, 8, 7);
            tvCode.Name = "tvCode";
            tvCode.Size = new Size(1105, 443);
            tvCode.TabIndex = 0;
            // 
            // txtCode
            // 
            txtCode.Dock = DockStyle.Fill;
            txtCode.Location = new Point(0, 0);
            txtCode.Margin = new Padding(3, 5, 3, 5);
            txtCode.Multiline = true;
            txtCode.Name = "txtCode";
            txtCode.ScrollBars = ScrollBars.Vertical;
            txtCode.Size = new Size(1105, 447);
            txtCode.TabIndex = 0;
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(13, 5, 0, 5);
            menuStrip.Size = new Size(1692, 53);
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
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl.Controls.Add(tabPageCardEdit);
            tabControl.Controls.Add(tabPageCodeView);
            tabControl.Location = new Point(0, 57);
            tabControl.Margin = new Padding(3, 5, 3, 5);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1692, 971);
            tabControl.TabIndex = 2;
            // 
            // tabPageCardEdit
            // 
            tabPageCardEdit.Controls.Add(splitContainerCardEdit);
            tabPageCardEdit.Location = new Point(10, 56);
            tabPageCardEdit.Margin = new Padding(3, 5, 3, 5);
            tabPageCardEdit.Name = "tabPageCardEdit";
            tabPageCardEdit.Padding = new Padding(3, 5, 3, 5);
            tabPageCardEdit.Size = new Size(1672, 905);
            tabPageCardEdit.TabIndex = 1;
            tabPageCardEdit.Text = "Card Edit";
            tabPageCardEdit.UseVisualStyleBackColor = true;
            // 
            // splitContainerCardEdit
            // 
            splitContainerCardEdit.Dock = DockStyle.Fill;
            splitContainerCardEdit.Location = new Point(3, 5);
            splitContainerCardEdit.Margin = new Padding(8, 7, 8, 7);
            splitContainerCardEdit.Name = "splitContainerCardEdit";
            // 
            // splitContainerCardEdit.Panel1
            // 
            splitContainerCardEdit.Panel1.Controls.Add(btnSearch);
            splitContainerCardEdit.Panel1.Controls.Add(txtSearchCard);
            splitContainerCardEdit.Panel1.Controls.Add(treeCards);
            // 
            // splitContainerCardEdit.Panel2
            // 
            splitContainerCardEdit.Panel2.Controls.Add(tabCardEdit);
            splitContainerCardEdit.Size = new Size(1666, 895);
            splitContainerCardEdit.SplitterDistance = 450;
            splitContainerCardEdit.SplitterWidth = 10;
            splitContainerCardEdit.TabIndex = 0;
            // 
            // btnSearch
            // 
            btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSearch.Location = new Point(327, 7);
            btnSearch.Margin = new Padding(8, 7, 8, 7);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(118, 57);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "检索";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // txtSearchCard
            // 
            txtSearchCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearchCard.Location = new Point(8, 11);
            txtSearchCard.Margin = new Padding(8, 7, 8, 7);
            txtSearchCard.Name = "txtSearchCard";
            txtSearchCard.Size = new Size(300, 46);
            txtSearchCard.TabIndex = 1;
            txtSearchCard.KeyDown += txtSearchCard_KeyDown;
            // 
            // treeCards
            // 
            treeCards.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            treeCards.Location = new Point(0, 81);
            treeCards.Margin = new Padding(8, 7, 8, 7);
            treeCards.Name = "treeCards";
            treeCards.Size = new Size(450, 814);
            treeCards.TabIndex = 0;
            treeCards.NodeMouseDoubleClick += treeCards_NodeMouseDoubleClick;
            // 
            // tabCardEdit
            // 
            tabCardEdit.Controls.Add(tabCardSelect);
            tabCardEdit.Controls.Add(tabCardInfoSettings);
            tabCardEdit.Controls.Add(tabEffect);
            tabCardEdit.Controls.Add(tabEffectFunc);
            tabCardEdit.Controls.Add(tabCardExplanation);
            tabCardEdit.Controls.Add(tabEffectCan);
            tabCardEdit.Controls.Add(tabAA);
            tabCardEdit.Controls.Add(tabExtraFuncs);
            tabCardEdit.Dock = DockStyle.Fill;
            tabCardEdit.Location = new Point(0, 0);
            tabCardEdit.Name = "tabCardEdit";
            tabCardEdit.SelectedIndex = 0;
            tabCardEdit.Size = new Size(1206, 895);
            tabCardEdit.TabIndex = 0;
            // 
            // tabCardSelect
            // 
            tabCardSelect.Controls.Add(btnCardScriptMoveDown);
            tabCardSelect.Controls.Add(btnCardScriptMoveUp);
            tabCardSelect.Controls.Add(btnCardScriptRemoveCard);
            tabCardSelect.Controls.Add(btnCardScriptAddCard);
            tabCardSelect.Controls.Add(listCardScriptCard);
            tabCardSelect.Location = new Point(10, 56);
            tabCardSelect.Name = "tabCardSelect";
            tabCardSelect.Size = new Size(1186, 829);
            tabCardSelect.TabIndex = 7;
            tabCardSelect.Text = "カード選択";
            tabCardSelect.UseVisualStyleBackColor = true;
            // 
            // btnCardScriptMoveDown
            // 
            btnCardScriptMoveDown.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCardScriptMoveDown.Location = new Point(330, 749);
            btnCardScriptMoveDown.Name = "btnCardScriptMoveDown";
            btnCardScriptMoveDown.Size = new Size(77, 56);
            btnCardScriptMoveDown.TabIndex = 19;
            btnCardScriptMoveDown.Text = "↓";
            btnCardScriptMoveDown.UseVisualStyleBackColor = true;
            // 
            // btnCardScriptMoveUp
            // 
            btnCardScriptMoveUp.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCardScriptMoveUp.Location = new Point(247, 749);
            btnCardScriptMoveUp.Name = "btnCardScriptMoveUp";
            btnCardScriptMoveUp.Size = new Size(77, 56);
            btnCardScriptMoveUp.TabIndex = 18;
            btnCardScriptMoveUp.Text = "↑";
            btnCardScriptMoveUp.UseVisualStyleBackColor = true;
            // 
            // btnCardScriptRemoveCard
            // 
            btnCardScriptRemoveCard.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCardScriptRemoveCard.Location = new Point(128, 749);
            btnCardScriptRemoveCard.Name = "btnCardScriptRemoveCard";
            btnCardScriptRemoveCard.Size = new Size(108, 56);
            btnCardScriptRemoveCard.TabIndex = 17;
            btnCardScriptRemoveCard.Text = "削除";
            btnCardScriptRemoveCard.UseVisualStyleBackColor = true;
            // 
            // btnCardScriptAddCard
            // 
            btnCardScriptAddCard.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCardScriptAddCard.Location = new Point(18, 749);
            btnCardScriptAddCard.Name = "btnCardScriptAddCard";
            btnCardScriptAddCard.Size = new Size(104, 56);
            btnCardScriptAddCard.TabIndex = 16;
            btnCardScriptAddCard.Text = "追加";
            btnCardScriptAddCard.UseVisualStyleBackColor = true;
            // 
            // listCardScriptCard
            // 
            listCardScriptCard.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listCardScriptCard.FormattingEnabled = true;
            listCardScriptCard.Location = new Point(18, 25);
            listCardScriptCard.Name = "listCardScriptCard";
            listCardScriptCard.Size = new Size(1144, 706);
            listCardScriptCard.TabIndex = 0;
            listCardScriptCard.MouseDoubleClick += listCardScriptCard_MouseDoubleClick;
            // 
            // tabCardInfoSettings
            // 
            tabCardInfoSettings.Controls.Add(btnImport);
            tabCardInfoSettings.Controls.Add(btnExport);
            tabCardInfoSettings.Controls.Add(btnSave);
            tabCardInfoSettings.Controls.Add(btnEditCardInfo);
            tabCardInfoSettings.Controls.Add(label5);
            tabCardInfoSettings.Controls.Add(btnMoveDownCardInfo);
            tabCardInfoSettings.Controls.Add(btnMoveUpCardInfo);
            tabCardInfoSettings.Controls.Add(btnDeleteCardInfo);
            tabCardInfoSettings.Controls.Add(btnAddCardInfo);
            tabCardInfoSettings.Controls.Add(btnQuickSetSpellTrap);
            tabCardInfoSettings.Controls.Add(btnQuickSetMonster);
            tabCardInfoSettings.Controls.Add(listCardInfo);
            tabCardInfoSettings.Controls.Add(label4);
            tabCardInfoSettings.Controls.Add(listCategory);
            tabCardInfoSettings.Controls.Add(label3);
            tabCardInfoSettings.Controls.Add(label2);
            tabCardInfoSettings.Controls.Add(txtShortName);
            tabCardInfoSettings.Controls.Add(label1);
            tabCardInfoSettings.Controls.Add(txtCardName);
            tabCardInfoSettings.Location = new Point(10, 56);
            tabCardInfoSettings.Name = "tabCardInfoSettings";
            tabCardInfoSettings.Padding = new Padding(3);
            tabCardInfoSettings.Size = new Size(1186, 829);
            tabCardInfoSettings.TabIndex = 0;
            tabCardInfoSettings.Text = "基本情報";
            tabCardInfoSettings.UseVisualStyleBackColor = true;
            // 
            // btnImport
            // 
            btnImport.Location = new Point(25, 496);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(469, 101);
            btnImport.TabIndex = 18;
            btnImport.Text = "ファイルから読み込む";
            btnImport.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(25, 603);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(469, 101);
            btnExport.TabIndex = 17;
            btnExport.Text = "別ファイルへ書き込む";
            btnExport.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(25, 710);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(469, 101);
            btnSave.TabIndex = 16;
            btnSave.Text = "上書き保存";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // btnEditCardInfo
            // 
            btnEditCardInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnEditCardInfo.Location = new Point(666, 755);
            btnEditCardInfo.Name = "btnEditCardInfo";
            btnEditCardInfo.Size = new Size(104, 56);
            btnEditCardInfo.TabIndex = 15;
            btnEditCardInfo.Text = "編集";
            btnEditCardInfo.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(25, 389);
            label5.Name = "label5";
            label5.Size = new Size(77, 78);
            label5.TabIndex = 14;
            label5.Text = "便利\r\n設定";
            // 
            // btnMoveDownCardInfo
            // 
            btnMoveDownCardInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnMoveDownCardInfo.Location = new Point(1088, 755);
            btnMoveDownCardInfo.Name = "btnMoveDownCardInfo";
            btnMoveDownCardInfo.Size = new Size(77, 56);
            btnMoveDownCardInfo.TabIndex = 13;
            btnMoveDownCardInfo.Text = "↓";
            btnMoveDownCardInfo.UseVisualStyleBackColor = true;
            // 
            // btnMoveUpCardInfo
            // 
            btnMoveUpCardInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnMoveUpCardInfo.Location = new Point(1005, 755);
            btnMoveUpCardInfo.Name = "btnMoveUpCardInfo";
            btnMoveUpCardInfo.Size = new Size(77, 56);
            btnMoveUpCardInfo.TabIndex = 12;
            btnMoveUpCardInfo.Text = "↑";
            btnMoveUpCardInfo.UseVisualStyleBackColor = true;
            // 
            // btnDeleteCardInfo
            // 
            btnDeleteCardInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDeleteCardInfo.Location = new Point(886, 755);
            btnDeleteCardInfo.Name = "btnDeleteCardInfo";
            btnDeleteCardInfo.Size = new Size(108, 56);
            btnDeleteCardInfo.TabIndex = 11;
            btnDeleteCardInfo.Text = "削除";
            btnDeleteCardInfo.UseVisualStyleBackColor = true;
            // 
            // btnAddCardInfo
            // 
            btnAddCardInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAddCardInfo.Location = new Point(776, 755);
            btnAddCardInfo.Name = "btnAddCardInfo";
            btnAddCardInfo.Size = new Size(104, 56);
            btnAddCardInfo.TabIndex = 10;
            btnAddCardInfo.Text = "追加";
            btnAddCardInfo.UseVisualStyleBackColor = true;
            // 
            // btnQuickSetSpellTrap
            // 
            btnQuickSetSpellTrap.Location = new Point(135, 431);
            btnQuickSetSpellTrap.Name = "btnQuickSetSpellTrap";
            btnQuickSetSpellTrap.Size = new Size(359, 56);
            btnQuickSetSpellTrap.TabIndex = 9;
            btnQuickSetSpellTrap.Text = "魔法・罠";
            btnQuickSetSpellTrap.UseVisualStyleBackColor = true;
            // 
            // btnQuickSetMonster
            // 
            btnQuickSetMonster.Location = new Point(135, 369);
            btnQuickSetMonster.Name = "btnQuickSetMonster";
            btnQuickSetMonster.Size = new Size(359, 56);
            btnQuickSetMonster.TabIndex = 8;
            btnQuickSetMonster.Text = "モンスター";
            btnQuickSetMonster.UseVisualStyleBackColor = true;
            // 
            // listCardInfo
            // 
            listCardInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listCardInfo.FormattingEnabled = true;
            listCardInfo.Location = new Point(597, 107);
            listCardInfo.Name = "listCardInfo";
            listCardInfo.Size = new Size(568, 628);
            listCardInfo.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(514, 107);
            label4.Name = "label4";
            label4.Size = new Size(77, 39);
            label4.TabIndex = 6;
            label4.Text = "情報";
            // 
            // listCategory
            // 
            listCategory.FormattingEnabled = true;
            listCategory.Location = new Point(135, 107);
            listCategory.Name = "listCategory";
            listCategory.Size = new Size(359, 238);
            listCategory.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(25, 107);
            label3.Name = "label3";
            label3.Size = new Size(77, 39);
            label3.TabIndex = 4;
            label3.Text = "分類";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(832, 28);
            label2.Name = "label2";
            label2.Size = new Size(77, 39);
            label2.TabIndex = 3;
            label2.Text = "略称";
            // 
            // txtShortName
            // 
            txtShortName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtShortName.Location = new Point(915, 28);
            txtShortName.Name = "txtShortName";
            txtShortName.Size = new Size(250, 46);
            txtShortName.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(25, 28);
            label1.Name = "label1";
            label1.Size = new Size(77, 39);
            label1.TabIndex = 1;
            label1.Text = "名前";
            // 
            // txtCardName
            // 
            txtCardName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtCardName.Location = new Point(135, 25);
            txtCardName.Name = "txtCardName";
            txtCardName.Size = new Size(680, 46);
            txtCardName.TabIndex = 0;
            // 
            // tabEffect
            // 
            tabEffect.Controls.Add(label6);
            tabEffect.Controls.Add(treeView1);
            tabEffect.Controls.Add(radioCustomStandardEffect);
            tabEffect.Controls.Add(radioCMStandardEffect);
            tabEffect.Location = new Point(10, 56);
            tabEffect.Name = "tabEffect";
            tabEffect.Size = new Size(1186, 829);
            tabEffect.TabIndex = 3;
            tabEffect.Text = "効果設定";
            tabEffect.UseVisualStyleBackColor = true;
            // 
            // treeView1
            // 
            treeView1.Location = new Point(19, 199);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(557, 609);
            treeView1.TabIndex = 2;
            // 
            // radioCustomStandardEffect
            // 
            radioCustomStandardEffect.AutoSize = true;
            radioCustomStandardEffect.Checked = true;
            radioCustomStandardEffect.Location = new Point(343, 25);
            radioCustomStandardEffect.Name = "radioCustomStandardEffect";
            radioCustomStandardEffect.Size = new Size(294, 43);
            radioCustomStandardEffect.TabIndex = 1;
            radioCustomStandardEffect.TabStop = true;
            radioCustomStandardEffect.Text = "高度な設定を使用";
            radioCustomStandardEffect.UseVisualStyleBackColor = true;
            // 
            // radioCMStandardEffect
            // 
            radioCMStandardEffect.AutoSize = true;
            radioCMStandardEffect.Location = new Point(19, 25);
            radioCMStandardEffect.Name = "radioCMStandardEffect";
            radioCMStandardEffect.Size = new Size(318, 43);
            radioCMStandardEffect.TabIndex = 0;
            radioCMStandardEffect.Text = "Card Manager標準";
            radioCMStandardEffect.UseVisualStyleBackColor = true;
            // 
            // tabCardExplanation
            // 
            tabCardExplanation.Controls.Add(btnReset);
            tabCardExplanation.Controls.Add(btnAddEffectText);
            tabCardExplanation.Controls.Add(treeEffectFunc);
            tabCardExplanation.Location = new Point(10, 56);
            tabCardExplanation.Name = "tabCardExplanation";
            tabCardExplanation.Size = new Size(1186, 829);
            tabCardExplanation.TabIndex = 2;
            tabCardExplanation.Text = "効果文";
            tabCardExplanation.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            btnReset.Location = new Point(39, 25);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(292, 95);
            btnReset.TabIndex = 2;
            btnReset.Text = "リセット";
            btnReset.UseVisualStyleBackColor = true;
            // 
            // btnAddEffectText
            // 
            btnAddEffectText.Location = new Point(39, 126);
            btnAddEffectText.Name = "btnAddEffectText";
            btnAddEffectText.Size = new Size(292, 95);
            btnAddEffectText.TabIndex = 1;
            btnAddEffectText.Text = "効果追加";
            btnAddEffectText.UseVisualStyleBackColor = true;
            // 
            // treeEffectFunc
            // 
            treeEffectFunc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            treeEffectFunc.Location = new Point(363, 3);
            treeEffectFunc.Name = "treeEffectFunc";
            treeEffectFunc.Size = new Size(823, 826);
            treeEffectFunc.TabIndex = 0;
            // 
            // tabEffectCan
            // 
            tabEffectCan.Location = new Point(10, 56);
            tabEffectCan.Name = "tabEffectCan";
            tabEffectCan.Size = new Size(1186, 829);
            tabEffectCan.TabIndex = 6;
            tabEffectCan.Text = "効果可用性";
            tabEffectCan.UseVisualStyleBackColor = true;
            // 
            // tabAA
            // 
            tabAA.Location = new Point(10, 56);
            tabAA.Name = "tabAA";
            tabAA.Size = new Size(1186, 829);
            tabAA.TabIndex = 4;
            tabAA.Text = "召喚AA";
            tabAA.UseVisualStyleBackColor = true;
            // 
            // tabExtraFuncs
            // 
            tabExtraFuncs.Location = new Point(10, 56);
            tabExtraFuncs.Name = "tabExtraFuncs";
            tabExtraFuncs.Size = new Size(1186, 829);
            tabExtraFuncs.TabIndex = 5;
            tabExtraFuncs.Text = "追加関数";
            tabExtraFuncs.UseVisualStyleBackColor = true;
            // 
            // tabPageCodeView
            // 
            tabPageCodeView.Controls.Add(splitContainer);
            tabPageCodeView.Location = new Point(10, 56);
            tabPageCodeView.Margin = new Padding(3, 5, 3, 5);
            tabPageCodeView.Name = "tabPageCodeView";
            tabPageCodeView.Padding = new Padding(3, 5, 3, 5);
            tabPageCodeView.Size = new Size(1672, 905);
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
            // bwLoadCards
            // 
            bwLoadCards.WorkerReportsProgress = true;
            bwLoadCards.DoWork += bwLoadCards_DoWork;
            bwLoadCards.ProgressChanged += bwLoadCards_ProgressChanged;
            bwLoadCards.RunWorkerCompleted += bwLoadCards_RunWorkerCompleted;
            // 
            // tabEffectFunc
            // 
            tabEffectFunc.Location = new Point(10, 56);
            tabEffectFunc.Name = "tabEffectFunc";
            tabEffectFunc.Size = new Size(1186, 829);
            tabEffectFunc.TabIndex = 8;
            tabEffectFunc.Text = "効果関数";
            tabEffectFunc.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.SlateBlue;
            label6.ForeColor = Color.PaleGoldenrod;
            label6.Location = new Point(19, 71);
            label6.Name = "label6";
            label6.Size = new Size(557, 117);
            label6.TabIndex = 3;
            label6.Text = "※Card Manager標準を使用すると\r\n「効果関数」「効果文」「効果可用性」\r\nタブの設定が無効になります。";
            // 
            // EOProcesser
            // 
            AutoScaleDimensions = new SizeF(18F, 39F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1692, 1028);
            Controls.Add(tabControl);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Margin = new Padding(8, 7, 8, 7);
            Name = "EOProcesser";
            Text = "ERAOCG Card Manager v1.0.0 by JoyJ";
            Load += EOProcesser_Load;
            Shown += EOProcesser_Shown;
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
            tabPageCardEdit.ResumeLayout(false);
            splitContainerCardEdit.Panel1.ResumeLayout(false);
            splitContainerCardEdit.Panel1.PerformLayout();
            splitContainerCardEdit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerCardEdit).EndInit();
            splitContainerCardEdit.ResumeLayout(false);
            tabCardEdit.ResumeLayout(false);
            tabCardSelect.ResumeLayout(false);
            tabCardInfoSettings.ResumeLayout(false);
            tabCardInfoSettings.PerformLayout();
            tabEffect.ResumeLayout(false);
            tabEffect.PerformLayout();
            tabCardExplanation.ResumeLayout(false);
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
        private SplitContainer splitContainerCardEdit;
        private TextBox txtSearchCard;
        private TreeView treeCards;
        private System.ComponentModel.BackgroundWorker bwLoadCards;
        private Button btnSearch;
        private TabControl tabCardEdit;
        private TabPage tabCardInfoSettings;
        private TabPage tabCardInfo;
        private TabPage tabCardExplanation;
        private TabPage tabEffect;
        private TabPage tabAA;
        private ListBox listCategory;
        private Label label3;
        private Label label2;
        private TextBox txtShortName;
        private Label label1;
        private TextBox txtCardName;
        private ListBox listCardInfo;
        private Label label4;
        private Label label5;
        private Button btnMoveDownCardInfo;
        private Button btnMoveUpCardInfo;
        private Button btnDeleteCardInfo;
        private Button btnAddCardInfo;
        private Button btnQuickSetMonster;
        private Button btnQuickSetSpellTrap;
        private Button btnEditCardInfo;
        private Button btnImport;
        private Button btnExport;
        private Button btnSave;
        private TabPage tabExtraFuncs;
        private TabPage tabEffectCan;
        private TreeView treeEffectFunc;
        private Button btnReset;
        private Button btnAddEffectText;
        private TabPage tabCardSelect;
        private Button btnCardScriptMoveDown;
        private Button btnCardScriptMoveUp;
        private Button btnCardScriptRemoveCard;
        private Button btnCardScriptAddCard;
        private ListBox listCardScriptCard;
        private RadioButton radioCustomStandardEffect;
        private RadioButton radioCMStandardEffect;
        private TreeView treeView1;
        private Label label6;
        private TabPage tabEffectFunc;
    }
}
