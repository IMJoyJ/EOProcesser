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
            tabCardEditPanel = new TabControl();
            tabCardSelect = new TabPage();
            btnCardScriptMoveDown = new Button();
            btnCardScriptMoveUp = new Button();
            btnCardScriptRemoveCard = new Button();
            btnCardScriptAddCard = new Button();
            listCardScriptCard = new ListBox();
            tabCardInfoSettings = new TabPage();
            btnMoveDownCategory = new Button();
            btnMoveUpCategory = new Button();
            btnRemoveCategory = new Button();
            btnAddCategory = new Button();
            btnSaveSingleCardToScript = new Button();
            btnEditCardInfoValue = new Button();
            btnEditCardInfoKey = new Button();
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
            splitContainer1 = new SplitContainer();
            treeCardEffectList = new TreeView();
            eeCardManagerScriptEditor = new EffectEditor();
            label6 = new Label();
            radioCustomStandardEffect = new RadioButton();
            radioCMStandardEffect = new RadioButton();
            tabAA = new TabPage();
            eeCardSummonAA = new EffectEditor();
            tabExtraFuncs = new TabPage();
            eeExtraFuncs = new EffectEditor();
            tabEffectFunc = new TabPage();
            eeCardEffect = new EffectEditor();
            tabCardExplanation = new TabPage();
            eeCardExplanation = new EffectEditor();
            tabEffectCan = new TabPage();
            eeCardCan = new EffectEditor();
            tabPageCodeView = new TabPage();
            splitContainer = new SplitContainer();
            tvFolderFiles = new TreeView();
            eeCodeView = new EffectEditor();
            CodeViewMenuStrip = new ContextMenuStrip(components);
            openToolStripMenuItem = new ToolStripMenuItem();
            bwLoadCards = new System.ComponentModel.BackgroundWorker();
            menuStrip.SuspendLayout();
            tabControl.SuspendLayout();
            tabPageCardEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerCardEdit).BeginInit();
            splitContainerCardEdit.Panel1.SuspendLayout();
            splitContainerCardEdit.Panel2.SuspendLayout();
            splitContainerCardEdit.SuspendLayout();
            tabCardEditPanel.SuspendLayout();
            tabCardSelect.SuspendLayout();
            tabCardInfoSettings.SuspendLayout();
            tabEffect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabAA.SuspendLayout();
            tabExtraFuncs.SuspendLayout();
            tabEffectFunc.SuspendLayout();
            tabCardExplanation.SuspendLayout();
            tabEffectCan.SuspendLayout();
            tabPageCodeView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            CodeViewMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(5, 3, 0, 3);
            menuStrip.Size = new Size(658, 27);
            menuStrip.TabIndex = 1;
            menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { settingsToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(39, 21);
            fileToolStripMenuItem.Text = "File";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(122, 22);
            settingsToolStripMenuItem.Text = "Settings";
            settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(122, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // tabControl
            // 
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl.Controls.Add(tabPageCardEdit);
            tabControl.Controls.Add(tabPageCodeView);
            tabControl.Location = new Point(0, 25);
            tabControl.Margin = new Padding(1, 3, 1, 3);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(658, 423);
            tabControl.TabIndex = 2;
            // 
            // tabPageCardEdit
            // 
            tabPageCardEdit.Controls.Add(splitContainerCardEdit);
            tabPageCardEdit.Location = new Point(4, 26);
            tabPageCardEdit.Margin = new Padding(1, 3, 1, 3);
            tabPageCardEdit.Name = "tabPageCardEdit";
            tabPageCardEdit.Padding = new Padding(1, 3, 1, 3);
            tabPageCardEdit.Size = new Size(650, 393);
            tabPageCardEdit.TabIndex = 1;
            tabPageCardEdit.Text = "Card Edit";
            tabPageCardEdit.UseVisualStyleBackColor = true;
            // 
            // splitContainerCardEdit
            // 
            splitContainerCardEdit.Dock = DockStyle.Fill;
            splitContainerCardEdit.Location = new Point(1, 3);
            splitContainerCardEdit.Margin = new Padding(4, 3, 4, 3);
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
            splitContainerCardEdit.Panel2.Controls.Add(tabCardEditPanel);
            splitContainerCardEdit.Size = new Size(648, 387);
            splitContainerCardEdit.SplitterDistance = 143;
            splitContainerCardEdit.TabIndex = 0;
            // 
            // btnSearch
            // 
            btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSearch.Location = new Point(93, 5);
            btnSearch.Margin = new Padding(4, 3, 4, 3);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(46, 25);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "检索";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // txtSearchCard
            // 
            txtSearchCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearchCard.Location = new Point(4, 5);
            txtSearchCard.Margin = new Padding(4, 3, 4, 3);
            txtSearchCard.Name = "txtSearchCard";
            txtSearchCard.Size = new Size(81, 23);
            txtSearchCard.TabIndex = 1;
            txtSearchCard.KeyDown += txtSearchCard_KeyDown;
            // 
            // treeCards
            // 
            treeCards.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            treeCards.Location = new Point(0, 37);
            treeCards.Margin = new Padding(4, 3, 4, 3);
            treeCards.Name = "treeCards";
            treeCards.Size = new Size(143, 350);
            treeCards.TabIndex = 0;
            treeCards.NodeMouseClick += treeCards_NodeMouseClick;
            treeCards.NodeMouseDoubleClick += treeCards_NodeMouseDoubleClick;
            // 
            // tabCardEditPanel
            // 
            tabCardEditPanel.Controls.Add(tabCardSelect);
            tabCardEditPanel.Controls.Add(tabCardInfoSettings);
            tabCardEditPanel.Controls.Add(tabEffect);
            tabCardEditPanel.Controls.Add(tabAA);
            tabCardEditPanel.Controls.Add(tabExtraFuncs);
            tabCardEditPanel.Controls.Add(tabEffectFunc);
            tabCardEditPanel.Controls.Add(tabCardExplanation);
            tabCardEditPanel.Controls.Add(tabEffectCan);
            tabCardEditPanel.Dock = DockStyle.Fill;
            tabCardEditPanel.Location = new Point(0, 0);
            tabCardEditPanel.Margin = new Padding(1, 2, 1, 2);
            tabCardEditPanel.Name = "tabCardEditPanel";
            tabCardEditPanel.SelectedIndex = 0;
            tabCardEditPanel.Size = new Size(501, 387);
            tabCardEditPanel.TabIndex = 0;
            // 
            // tabCardSelect
            // 
            tabCardSelect.Controls.Add(btnCardScriptMoveDown);
            tabCardSelect.Controls.Add(btnCardScriptMoveUp);
            tabCardSelect.Controls.Add(btnCardScriptRemoveCard);
            tabCardSelect.Controls.Add(btnCardScriptAddCard);
            tabCardSelect.Controls.Add(listCardScriptCard);
            tabCardSelect.Location = new Point(4, 26);
            tabCardSelect.Margin = new Padding(1, 2, 1, 2);
            tabCardSelect.Name = "tabCardSelect";
            tabCardSelect.Size = new Size(493, 357);
            tabCardSelect.TabIndex = 7;
            tabCardSelect.Text = "カード選択";
            tabCardSelect.UseVisualStyleBackColor = true;
            // 
            // btnCardScriptMoveDown
            // 
            btnCardScriptMoveDown.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCardScriptMoveDown.Location = new Point(144, 325);
            btnCardScriptMoveDown.Margin = new Padding(1, 2, 1, 2);
            btnCardScriptMoveDown.Name = "btnCardScriptMoveDown";
            btnCardScriptMoveDown.Size = new Size(30, 25);
            btnCardScriptMoveDown.TabIndex = 19;
            btnCardScriptMoveDown.Text = "↓";
            btnCardScriptMoveDown.UseVisualStyleBackColor = true;
            btnCardScriptMoveDown.Click += btnCardScriptMoveDown_Click;
            // 
            // btnCardScriptMoveUp
            // 
            btnCardScriptMoveUp.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCardScriptMoveUp.Location = new Point(112, 325);
            btnCardScriptMoveUp.Margin = new Padding(1, 2, 1, 2);
            btnCardScriptMoveUp.Name = "btnCardScriptMoveUp";
            btnCardScriptMoveUp.Size = new Size(30, 25);
            btnCardScriptMoveUp.TabIndex = 18;
            btnCardScriptMoveUp.Text = "↑";
            btnCardScriptMoveUp.UseVisualStyleBackColor = true;
            btnCardScriptMoveUp.Click += btnCardScriptMoveUp_Click;
            // 
            // btnCardScriptRemoveCard
            // 
            btnCardScriptRemoveCard.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCardScriptRemoveCard.Location = new Point(59, 325);
            btnCardScriptRemoveCard.Margin = new Padding(1, 2, 1, 2);
            btnCardScriptRemoveCard.Name = "btnCardScriptRemoveCard";
            btnCardScriptRemoveCard.Size = new Size(50, 25);
            btnCardScriptRemoveCard.TabIndex = 17;
            btnCardScriptRemoveCard.Text = "削除";
            btnCardScriptRemoveCard.UseVisualStyleBackColor = true;
            btnCardScriptRemoveCard.Click += btnCardScriptRemoveCard_Click;
            // 
            // btnCardScriptAddCard
            // 
            btnCardScriptAddCard.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCardScriptAddCard.Location = new Point(7, 325);
            btnCardScriptAddCard.Margin = new Padding(1, 2, 1, 2);
            btnCardScriptAddCard.Name = "btnCardScriptAddCard";
            btnCardScriptAddCard.Size = new Size(50, 25);
            btnCardScriptAddCard.TabIndex = 16;
            btnCardScriptAddCard.Text = "追加";
            btnCardScriptAddCard.UseVisualStyleBackColor = true;
            btnCardScriptAddCard.Click += btnCardScriptAddCard_Click;
            // 
            // listCardScriptCard
            // 
            listCardScriptCard.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listCardScriptCard.FormattingEnabled = true;
            listCardScriptCard.ItemHeight = 17;
            listCardScriptCard.Location = new Point(7, 11);
            listCardScriptCard.Margin = new Padding(1, 2, 1, 2);
            listCardScriptCard.Name = "listCardScriptCard";
            listCardScriptCard.Size = new Size(478, 310);
            listCardScriptCard.TabIndex = 0;
            listCardScriptCard.MouseDoubleClick += listCardScriptCard_MouseDoubleClick;
            // 
            // tabCardInfoSettings
            // 
            tabCardInfoSettings.Controls.Add(btnMoveDownCategory);
            tabCardInfoSettings.Controls.Add(btnMoveUpCategory);
            tabCardInfoSettings.Controls.Add(btnRemoveCategory);
            tabCardInfoSettings.Controls.Add(btnAddCategory);
            tabCardInfoSettings.Controls.Add(btnSaveSingleCardToScript);
            tabCardInfoSettings.Controls.Add(btnEditCardInfoValue);
            tabCardInfoSettings.Controls.Add(btnEditCardInfoKey);
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
            tabCardInfoSettings.Location = new Point(4, 26);
            tabCardInfoSettings.Margin = new Padding(1, 2, 1, 2);
            tabCardInfoSettings.Name = "tabCardInfoSettings";
            tabCardInfoSettings.Padding = new Padding(1, 2, 1, 2);
            tabCardInfoSettings.Size = new Size(493, 357);
            tabCardInfoSettings.TabIndex = 0;
            tabCardInfoSettings.Text = "基本情報";
            tabCardInfoSettings.UseVisualStyleBackColor = true;
            // 
            // btnMoveDownCategory
            // 
            btnMoveDownCategory.Location = new Point(164, 157);
            btnMoveDownCategory.Margin = new Padding(1, 2, 1, 2);
            btnMoveDownCategory.Name = "btnMoveDownCategory";
            btnMoveDownCategory.Size = new Size(30, 25);
            btnMoveDownCategory.TabIndex = 20;
            btnMoveDownCategory.Text = "↓";
            btnMoveDownCategory.UseVisualStyleBackColor = true;
            btnMoveDownCategory.Click += btnMoveDownCategory_Click;
            // 
            // btnMoveUpCategory
            // 
            btnMoveUpCategory.Location = new Point(132, 157);
            btnMoveUpCategory.Margin = new Padding(1, 2, 1, 2);
            btnMoveUpCategory.Name = "btnMoveUpCategory";
            btnMoveUpCategory.Size = new Size(30, 25);
            btnMoveUpCategory.TabIndex = 19;
            btnMoveUpCategory.Text = "↑";
            btnMoveUpCategory.UseVisualStyleBackColor = true;
            btnMoveUpCategory.Click += btnMoveUpCategory_Click;
            // 
            // btnRemoveCategory
            // 
            btnRemoveCategory.Location = new Point(102, 157);
            btnRemoveCategory.Margin = new Padding(1, 2, 1, 2);
            btnRemoveCategory.Name = "btnRemoveCategory";
            btnRemoveCategory.Size = new Size(28, 25);
            btnRemoveCategory.TabIndex = 18;
            btnRemoveCategory.Text = "-";
            btnRemoveCategory.UseVisualStyleBackColor = true;
            btnRemoveCategory.Click += btnRemoveCategory_Click;
            // 
            // btnAddCategory
            // 
            btnAddCategory.Location = new Point(70, 157);
            btnAddCategory.Margin = new Padding(1, 2, 1, 2);
            btnAddCategory.Name = "btnAddCategory";
            btnAddCategory.Size = new Size(30, 25);
            btnAddCategory.TabIndex = 17;
            btnAddCategory.Text = "+";
            btnAddCategory.UseVisualStyleBackColor = true;
            btnAddCategory.Click += btnAddCategory_Click;
            // 
            // btnSaveSingleCardToScript
            // 
            btnSaveSingleCardToScript.Location = new Point(10, 296);
            btnSaveSingleCardToScript.Margin = new Padding(1, 2, 1, 2);
            btnSaveSingleCardToScript.Name = "btnSaveSingleCardToScript";
            btnSaveSingleCardToScript.Size = new Size(214, 44);
            btnSaveSingleCardToScript.TabIndex = 16;
            btnSaveSingleCardToScript.Text = "カード保存";
            btnSaveSingleCardToScript.UseVisualStyleBackColor = true;
            btnSaveSingleCardToScript.Click += btnSaveSingleCard_Click;
            // 
            // btnEditCardInfoValue
            // 
            btnEditCardInfoValue.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnEditCardInfoValue.Location = new Point(303, 315);
            btnEditCardInfoValue.Margin = new Padding(1, 2, 1, 2);
            btnEditCardInfoValue.Name = "btnEditCardInfoValue";
            btnEditCardInfoValue.Size = new Size(53, 25);
            btnEditCardInfoValue.TabIndex = 15;
            btnEditCardInfoValue.Text = "値編集";
            btnEditCardInfoValue.UseVisualStyleBackColor = true;
            btnEditCardInfoValue.Click += btnEditCardInfoValue_Click;
            // 
            // btnEditCardInfoKey
            // 
            btnEditCardInfoKey.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnEditCardInfoKey.Location = new Point(249, 315);
            btnEditCardInfoKey.Margin = new Padding(1, 2, 1, 2);
            btnEditCardInfoKey.Name = "btnEditCardInfoKey";
            btnEditCardInfoKey.Size = new Size(52, 25);
            btnEditCardInfoKey.TabIndex = 15;
            btnEditCardInfoKey.Text = "鍵編集";
            btnEditCardInfoKey.UseVisualStyleBackColor = true;
            btnEditCardInfoKey.Click += btnEditCardInfoKey_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(45, 240);
            label5.Margin = new Padding(1, 0, 1, 0);
            label5.Name = "label5";
            label5.Size = new Size(32, 34);
            label5.TabIndex = 14;
            label5.Text = "便利\r\n設定";
            // 
            // btnMoveDownCardInfo
            // 
            btnMoveDownCardInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnMoveDownCardInfo.Location = new Point(456, 315);
            btnMoveDownCardInfo.Margin = new Padding(1, 2, 1, 2);
            btnMoveDownCardInfo.Name = "btnMoveDownCardInfo";
            btnMoveDownCardInfo.Size = new Size(30, 25);
            btnMoveDownCardInfo.TabIndex = 13;
            btnMoveDownCardInfo.Text = "↓";
            btnMoveDownCardInfo.UseVisualStyleBackColor = true;
            btnMoveDownCardInfo.Click += btnMoveDownCardInfo_Click;
            // 
            // btnMoveUpCardInfo
            // 
            btnMoveUpCardInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnMoveUpCardInfo.Location = new Point(424, 315);
            btnMoveUpCardInfo.Margin = new Padding(1, 2, 1, 2);
            btnMoveUpCardInfo.Name = "btnMoveUpCardInfo";
            btnMoveUpCardInfo.Size = new Size(30, 25);
            btnMoveUpCardInfo.TabIndex = 12;
            btnMoveUpCardInfo.Text = "↑";
            btnMoveUpCardInfo.UseVisualStyleBackColor = true;
            btnMoveUpCardInfo.Click += btnMoveUpCardInfo_Click;
            // 
            // btnDeleteCardInfo
            // 
            btnDeleteCardInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDeleteCardInfo.Location = new Point(391, 315);
            btnDeleteCardInfo.Margin = new Padding(1, 2, 1, 2);
            btnDeleteCardInfo.Name = "btnDeleteCardInfo";
            btnDeleteCardInfo.Size = new Size(29, 25);
            btnDeleteCardInfo.TabIndex = 11;
            btnDeleteCardInfo.Text = "-";
            btnDeleteCardInfo.UseVisualStyleBackColor = true;
            btnDeleteCardInfo.Click += btnDeleteCardInfo_Click;
            // 
            // btnAddCardInfo
            // 
            btnAddCardInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAddCardInfo.Location = new Point(362, 315);
            btnAddCardInfo.Margin = new Padding(1, 2, 1, 2);
            btnAddCardInfo.Name = "btnAddCardInfo";
            btnAddCardInfo.Size = new Size(27, 25);
            btnAddCardInfo.TabIndex = 10;
            btnAddCardInfo.Text = "+";
            btnAddCardInfo.UseVisualStyleBackColor = true;
            btnAddCardInfo.Click += btnAddCardInfo_Click;
            // 
            // btnQuickSetSpellTrap
            // 
            btnQuickSetSpellTrap.Location = new Point(84, 259);
            btnQuickSetSpellTrap.Margin = new Padding(1, 2, 1, 2);
            btnQuickSetSpellTrap.Name = "btnQuickSetSpellTrap";
            btnQuickSetSpellTrap.Size = new Size(140, 33);
            btnQuickSetSpellTrap.TabIndex = 9;
            btnQuickSetSpellTrap.Text = "魔法・罠→";
            btnQuickSetSpellTrap.UseVisualStyleBackColor = true;
            // 
            // btnQuickSetMonster
            // 
            btnQuickSetMonster.Location = new Point(84, 220);
            btnQuickSetMonster.Margin = new Padding(1, 2, 1, 2);
            btnQuickSetMonster.Name = "btnQuickSetMonster";
            btnQuickSetMonster.Size = new Size(140, 35);
            btnQuickSetMonster.TabIndex = 8;
            btnQuickSetMonster.Text = "モンスター→";
            btnQuickSetMonster.UseVisualStyleBackColor = true;
            // 
            // listCardInfo
            // 
            listCardInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listCardInfo.FormattingEnabled = true;
            listCardInfo.ItemHeight = 17;
            listCardInfo.Location = new Point(232, 47);
            listCardInfo.Margin = new Padding(1, 2, 1, 2);
            listCardInfo.Name = "listCardInfo";
            listCardInfo.Size = new Size(259, 259);
            listCardInfo.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(200, 47);
            label4.Margin = new Padding(1, 0, 1, 0);
            label4.Name = "label4";
            label4.Size = new Size(32, 17);
            label4.TabIndex = 6;
            label4.Text = "情報";
            // 
            // listCategory
            // 
            listCategory.FormattingEnabled = true;
            listCategory.ItemHeight = 17;
            listCategory.Location = new Point(52, 47);
            listCategory.Margin = new Padding(1, 2, 1, 2);
            listCategory.Name = "listCategory";
            listCategory.Size = new Size(142, 106);
            listCategory.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(10, 47);
            label3.Margin = new Padding(1, 0, 1, 0);
            label3.Name = "label3";
            label3.Size = new Size(32, 17);
            label3.TabIndex = 4;
            label3.Text = "分類";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(324, 12);
            label2.Margin = new Padding(1, 0, 1, 0);
            label2.Name = "label2";
            label2.Size = new Size(32, 17);
            label2.TabIndex = 3;
            label2.Text = "略称";
            // 
            // txtShortName
            // 
            txtShortName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtShortName.Location = new Point(356, 12);
            txtShortName.Margin = new Padding(1, 2, 1, 2);
            txtShortName.Name = "txtShortName";
            txtShortName.Size = new Size(135, 23);
            txtShortName.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 12);
            label1.Margin = new Padding(1, 0, 1, 0);
            label1.Name = "label1";
            label1.Size = new Size(32, 17);
            label1.TabIndex = 1;
            label1.Text = "名前";
            // 
            // txtCardName
            // 
            txtCardName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtCardName.Location = new Point(52, 11);
            txtCardName.Margin = new Padding(1, 2, 1, 2);
            txtCardName.Name = "txtCardName";
            txtCardName.Size = new Size(267, 23);
            txtCardName.TabIndex = 0;
            // 
            // tabEffect
            // 
            tabEffect.Controls.Add(splitContainer1);
            tabEffect.Controls.Add(label6);
            tabEffect.Controls.Add(radioCustomStandardEffect);
            tabEffect.Controls.Add(radioCMStandardEffect);
            tabEffect.Location = new Point(4, 26);
            tabEffect.Margin = new Padding(1, 2, 1, 2);
            tabEffect.Name = "tabEffect";
            tabEffect.Size = new Size(493, 357);
            tabEffect.TabIndex = 3;
            tabEffect.Text = "効果設定";
            tabEffect.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(7, 70);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(treeCardEffectList);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(eeCardManagerScriptEditor);
            splitContainer1.Size = new Size(478, 276);
            splitContainer1.SplitterDistance = 157;
            splitContainer1.TabIndex = 4;
            // 
            // treeCardEffectList
            // 
            treeCardEffectList.Dock = DockStyle.Fill;
            treeCardEffectList.Location = new Point(0, 0);
            treeCardEffectList.Margin = new Padding(1, 2, 1, 2);
            treeCardEffectList.Name = "treeCardEffectList";
            treeCardEffectList.Size = new Size(157, 276);
            treeCardEffectList.TabIndex = 2;
            treeCardEffectList.NodeMouseClick += treeCardEffectList_NodeMouseClick;
            treeCardEffectList.NodeMouseDoubleClick += treeCardEffectList_NodeMouseDoubleClick;
            // 
            // eeCardManagerScriptEditor
            // 
            eeCardManagerScriptEditor.Dock = DockStyle.Fill;
            eeCardManagerScriptEditor.Location = new Point(0, 0);
            eeCardManagerScriptEditor.Margin = new Padding(6, 5, 6, 5);
            eeCardManagerScriptEditor.Name = "eeCardManagerScriptEditor";
            eeCardManagerScriptEditor.Size = new Size(317, 276);
            eeCardManagerScriptEditor.TabIndex = 0;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.SlateBlue;
            label6.ForeColor = Color.PaleGoldenrod;
            label6.Location = new Point(7, 33);
            label6.Margin = new Padding(1, 0, 1, 0);
            label6.Name = "label6";
            label6.Size = new Size(416, 34);
            label6.TabIndex = 3;
            label6.Text = "※Card Manager標準を使用すると、こちらの設定が優先され、\r\n右の「効果文」「効果関数」「効果可用性」タブの設定が無効になります。";
            // 
            // radioCustomStandardEffect
            // 
            radioCustomStandardEffect.AutoSize = true;
            radioCustomStandardEffect.Checked = true;
            radioCustomStandardEffect.Location = new Point(144, 11);
            radioCustomStandardEffect.Margin = new Padding(1, 2, 1, 2);
            radioCustomStandardEffect.Name = "radioCustomStandardEffect";
            radioCustomStandardEffect.Size = new Size(122, 21);
            radioCustomStandardEffect.TabIndex = 1;
            radioCustomStandardEffect.TabStop = true;
            radioCustomStandardEffect.Text = "高度な設定を使用";
            radioCustomStandardEffect.UseVisualStyleBackColor = true;
            // 
            // radioCMStandardEffect
            // 
            radioCMStandardEffect.AutoSize = true;
            radioCMStandardEffect.Location = new Point(7, 11);
            radioCMStandardEffect.Margin = new Padding(1, 2, 1, 2);
            radioCMStandardEffect.Name = "radioCMStandardEffect";
            radioCMStandardEffect.Size = new Size(135, 21);
            radioCMStandardEffect.TabIndex = 0;
            radioCMStandardEffect.Text = "Card Manager標準";
            radioCMStandardEffect.UseVisualStyleBackColor = true;
            radioCMStandardEffect.CheckedChanged += radioCMStandardEffect_CheckedChanged;
            // 
            // tabAA
            // 
            tabAA.Controls.Add(eeCardSummonAA);
            tabAA.Location = new Point(4, 26);
            tabAA.Margin = new Padding(1, 2, 1, 2);
            tabAA.Name = "tabAA";
            tabAA.Size = new Size(493, 357);
            tabAA.TabIndex = 4;
            tabAA.Text = "召喚AA";
            tabAA.UseVisualStyleBackColor = true;
            // 
            // eeCardSummonAA
            // 
            eeCardSummonAA.Dock = DockStyle.Fill;
            eeCardSummonAA.Location = new Point(0, 0);
            eeCardSummonAA.Margin = new Padding(6, 5, 6, 5);
            eeCardSummonAA.Name = "eeCardSummonAA";
            eeCardSummonAA.Size = new Size(493, 357);
            eeCardSummonAA.TabIndex = 1;
            // 
            // tabExtraFuncs
            // 
            tabExtraFuncs.Controls.Add(eeExtraFuncs);
            tabExtraFuncs.Location = new Point(4, 26);
            tabExtraFuncs.Margin = new Padding(1, 2, 1, 2);
            tabExtraFuncs.Name = "tabExtraFuncs";
            tabExtraFuncs.Size = new Size(493, 357);
            tabExtraFuncs.TabIndex = 5;
            tabExtraFuncs.Text = "追加関数";
            tabExtraFuncs.UseVisualStyleBackColor = true;
            // 
            // eeExtraFuncs
            // 
            eeExtraFuncs.Dock = DockStyle.Fill;
            eeExtraFuncs.Location = new Point(0, 0);
            eeExtraFuncs.Margin = new Padding(6, 5, 6, 5);
            eeExtraFuncs.Name = "eeExtraFuncs";
            eeExtraFuncs.Size = new Size(493, 357);
            eeExtraFuncs.TabIndex = 1;
            // 
            // tabEffectFunc
            // 
            tabEffectFunc.Controls.Add(eeCardEffect);
            tabEffectFunc.Location = new Point(4, 26);
            tabEffectFunc.Margin = new Padding(1, 2, 1, 2);
            tabEffectFunc.Name = "tabEffectFunc";
            tabEffectFunc.Size = new Size(493, 357);
            tabEffectFunc.TabIndex = 8;
            tabEffectFunc.Text = "効果関数";
            tabEffectFunc.UseVisualStyleBackColor = true;
            // 
            // eeCardEffect
            // 
            eeCardEffect.Dock = DockStyle.Fill;
            eeCardEffect.Location = new Point(0, 0);
            eeCardEffect.Margin = new Padding(6, 5, 6, 5);
            eeCardEffect.Name = "eeCardEffect";
            eeCardEffect.Size = new Size(493, 357);
            eeCardEffect.TabIndex = 0;
            // 
            // tabCardExplanation
            // 
            tabCardExplanation.Controls.Add(eeCardExplanation);
            tabCardExplanation.Location = new Point(4, 26);
            tabCardExplanation.Margin = new Padding(1, 2, 1, 2);
            tabCardExplanation.Name = "tabCardExplanation";
            tabCardExplanation.Size = new Size(493, 357);
            tabCardExplanation.TabIndex = 2;
            tabCardExplanation.Text = "効果文";
            tabCardExplanation.UseVisualStyleBackColor = true;
            // 
            // eeCardExplanation
            // 
            eeCardExplanation.Dock = DockStyle.Fill;
            eeCardExplanation.Location = new Point(0, 0);
            eeCardExplanation.Margin = new Padding(6, 5, 6, 5);
            eeCardExplanation.Name = "eeCardExplanation";
            eeCardExplanation.Size = new Size(493, 357);
            eeCardExplanation.TabIndex = 1;
            // 
            // tabEffectCan
            // 
            tabEffectCan.Controls.Add(eeCardCan);
            tabEffectCan.Location = new Point(4, 26);
            tabEffectCan.Margin = new Padding(1, 2, 1, 2);
            tabEffectCan.Name = "tabEffectCan";
            tabEffectCan.Size = new Size(493, 357);
            tabEffectCan.TabIndex = 6;
            tabEffectCan.Text = "効果可用性";
            tabEffectCan.UseVisualStyleBackColor = true;
            // 
            // eeCardCan
            // 
            eeCardCan.Dock = DockStyle.Fill;
            eeCardCan.Location = new Point(0, 0);
            eeCardCan.Margin = new Padding(6, 5, 6, 5);
            eeCardCan.Name = "eeCardCan";
            eeCardCan.Size = new Size(493, 357);
            eeCardCan.TabIndex = 1;
            // 
            // tabPageCodeView
            // 
            tabPageCodeView.Controls.Add(splitContainer);
            tabPageCodeView.Location = new Point(4, 26);
            tabPageCodeView.Margin = new Padding(1, 3, 1, 3);
            tabPageCodeView.Name = "tabPageCodeView";
            tabPageCodeView.Padding = new Padding(1, 3, 1, 3);
            tabPageCodeView.Size = new Size(650, 393);
            tabPageCodeView.TabIndex = 0;
            tabPageCodeView.Text = "Code View";
            tabPageCodeView.UseVisualStyleBackColor = true;
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(1, 3);
            splitContainer.Margin = new Padding(4, 3, 4, 3);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(tvFolderFiles);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(eeCodeView);
            splitContainer.Size = new Size(648, 387);
            splitContainer.SplitterDistance = 213;
            splitContainer.TabIndex = 0;
            // 
            // tvFolderFiles
            // 
            tvFolderFiles.Dock = DockStyle.Fill;
            tvFolderFiles.Location = new Point(0, 0);
            tvFolderFiles.Margin = new Padding(4, 3, 4, 3);
            tvFolderFiles.Name = "tvFolderFiles";
            tvFolderFiles.Size = new Size(213, 387);
            tvFolderFiles.TabIndex = 0;
            tvFolderFiles.NodeMouseClick += tvFolderFiles_NodeMouseClick;
            tvFolderFiles.NodeMouseDoubleClick += tvFolderFiles_NodeMouseDoubleClick;
            // 
            // eeCodeView
            // 
            eeCodeView.Dock = DockStyle.Fill;
            eeCodeView.Location = new Point(0, 0);
            eeCodeView.Margin = new Padding(6, 5, 6, 5);
            eeCodeView.Name = "eeCodeView";
            eeCodeView.Size = new Size(431, 387);
            eeCodeView.TabIndex = 0;
            // 
            // CodeViewMenuStrip
            // 
            CodeViewMenuStrip.ImageScalingSize = new Size(40, 40);
            CodeViewMenuStrip.Items.AddRange(new ToolStripItem[] { openToolStripMenuItem });
            CodeViewMenuStrip.Name = "CodeViewMenuStrip";
            CodeViewMenuStrip.Size = new Size(197, 26);
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(196, 22);
            openToolStripMenuItem.Text = "外部エディターで開く";
            // 
            // bwLoadCards
            // 
            bwLoadCards.WorkerReportsProgress = true;
            bwLoadCards.DoWork += bwLoadCards_DoWork;
            bwLoadCards.ProgressChanged += bwLoadCards_ProgressChanged;
            bwLoadCards.RunWorkerCompleted += bwLoadCards_RunWorkerCompleted;
            // 
            // EOProcesser
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(658, 448);
            Controls.Add(tabControl);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Margin = new Padding(4, 3, 4, 3);
            Name = "EOProcesser";
            Text = "ERAOCG AIO Manager v0.9.0 by JoyJ";
            Load += EOProcesser_Load;
            Shown += EOProcesser_Shown;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            tabControl.ResumeLayout(false);
            tabPageCardEdit.ResumeLayout(false);
            splitContainerCardEdit.Panel1.ResumeLayout(false);
            splitContainerCardEdit.Panel1.PerformLayout();
            splitContainerCardEdit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerCardEdit).EndInit();
            splitContainerCardEdit.ResumeLayout(false);
            tabCardEditPanel.ResumeLayout(false);
            tabCardSelect.ResumeLayout(false);
            tabCardInfoSettings.ResumeLayout(false);
            tabCardInfoSettings.PerformLayout();
            tabEffect.ResumeLayout(false);
            tabEffect.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabAA.ResumeLayout(false);
            tabExtraFuncs.ResumeLayout(false);
            tabEffectFunc.ResumeLayout(false);
            tabCardExplanation.ResumeLayout(false);
            tabEffectCan.ResumeLayout(false);
            tabPageCodeView.ResumeLayout(false);
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            CodeViewMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private TabControl tabControl;
        private TabPage tabPageCardEdit;
        private TabPage tabPageCodeView;
        private ContextMenuStrip CodeViewMenuStrip;
        private ToolStripMenuItem openToolStripMenuItem;
        private SplitContainer splitContainerCardEdit;
        private TextBox txtSearchCard;
        private TreeView treeCards;
        private System.ComponentModel.BackgroundWorker bwLoadCards;
        private Button btnSearch;
        private TabControl tabCardEditPanel;
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
        private Button btnEditCardInfoKey;
        private Button btnSaveSingleCardToScript;
        private TabPage tabExtraFuncs;
        private TabPage tabEffectCan;
        private TabPage tabCardSelect;
        private Button btnCardScriptMoveDown;
        private Button btnCardScriptMoveUp;
        private Button btnCardScriptRemoveCard;
        private Button btnCardScriptAddCard;
        private ListBox listCardScriptCard;
        private RadioButton radioCustomStandardEffect;
        private RadioButton radioCMStandardEffect;
        private TreeView treeCardEffectList;
        private Label label6;
        private TabPage tabEffectFunc;
        private SplitContainer splitContainer;
        private TreeView tvFolderFiles;
        private EffectEditor eeCodeView;
        private EffectEditor eeCardEffect;
        private EffectEditor eeCardExplanation;
        private EffectEditor eeCardCan;
        private EffectEditor eeCardSummonAA;
        private EffectEditor eeExtraFuncs;
        private SplitContainer splitContainer1;
        private EffectEditor eeCardManagerScriptEditor;
        private Button btnMoveDownCategory;
        private Button btnMoveUpCategory;
        private Button btnRemoveCategory;
        private Button btnAddCategory;
        private Button btnEditCardInfoValue;
    }
}
