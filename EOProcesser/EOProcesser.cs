using System.Diagnostics;
using System.Text.Json;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using EOProcesser.Settings;
using System.Reflection.Emit;
using System.Text;
using EOProcesser.Forms;

namespace EOProcesser
{
    public partial class EOProcesser : Form
    {
        public EOProcesser()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        ERAOCGCardManagerSettings settings = new();

        private void EOProcesser_Load(object sender, EventArgs e)
        {
            try
            {
                settings = JsonSerializer.Deserialize<ERAOCGCardManagerSettings>(File.ReadAllText("settings.json")) ?? settings;
            }
            catch { }

            LoadCodeFolderTreeView();
        }

        int loadedCards = 0;
        int allCards = 0;
        private readonly SemaphoreSlim semaphore = new(16); // 限制最大并行度为16
        private readonly ConcurrentBag<(string FilePath, TreeNode[] Nodes)> processedScripts = [];

        private async Task<TreeNode?> LoadCardTreeView()
        {
            if (string.IsNullOrEmpty(settings.CardFolder))
                return null;
            try
            {

                // 重置计数器和结果集合
                loadedCards = 0;
                processedScripts.Clear();

                // 收集所有.erb文件
                var allErbFiles = new List<string>();
                CollectAllErbFiles(settings.CardFolder, allErbFiles);
                allCards = allErbFiles.Count;

                // 获取所有子目录
                var allDirectories = Directory.GetDirectories(settings.CardFolder, "*", SearchOption.AllDirectories)
                    .Append(settings.CardFolder) // 包括根目录
                    .ToDictionary(dir => dir, dir => new TreeNode(Path.GetFileName(dir)) { Tag = dir });

                // 建立目录树结构
                foreach (var dir in allDirectories.Keys.OrderBy(d => d.Length))
                {
                    if (dir == settings.CardFolder) continue; // 根目录已经创建

                    var parentDir = Path.GetDirectoryName(dir);
                    if (parentDir != null && allDirectories.TryGetValue(parentDir, out var parentNode))
                    {
                        parentNode.Nodes.Add(allDirectories[dir]);
                    }
                }

                // 并行处理所有文件
                var tasks = allErbFiles.Select(file => ProcessCardFileAsync(file));
                await Task.WhenAll(tasks);

                // 将处理好的文件添加到对应的目录节点
                foreach (var (filePath, nodes) in processedScripts)
                {
                    var dirPath = Path.GetDirectoryName(filePath);
                    if (dirPath != null && allDirectories.TryGetValue(dirPath, out var dirNode))
                    {
                        dirNode.Nodes.AddRange(nodes);
                    }
                }

                // 返回根节点
                return allDirectories[settings.CardFolder];

            }
#pragma warning disable CS0168 // 声明了变量，但从未使用过
            catch (Exception ex)
#pragma warning restore CS0168 // 声明了变量，但从未使用过
#if DEBUG
#pragma warning disable CS8360 // 筛选器表达式是常量 “false”。
            when (false)
#pragma warning restore CS8360 // 筛选器表达式是常量 “false”。
#endif
            {
#pragma warning disable CS0162 // 检测到无法访问的代码
                return null;
#pragma warning restore CS0162 // 检测到无法访问的代码
            }
        }

        private void CollectAllErbFiles(string rootFolder, List<string> files)
        {
            try
            {
                // 添加当前目录下的所有.erb文件
                files.AddRange(Directory.GetFiles(rootFolder, "*.erb"));

                // 递归处理所有子目录
                foreach (var dir in Directory.GetDirectories(rootFolder))
                {
                    CollectAllErbFiles(dir, files);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error collecting files: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ProcessCardFileAsync(string file)
        {
            try
            {
                await semaphore.WaitAsync(); // 获取信号量，限制并行度

                try
                {
                    ERAOCGCardScript script = new(file);
                    Interlocked.Increment(ref loadedCards);
                    bwLoadCards.ReportProgress(0);

                    var nodes = script.GetTreeNodes();
                    if (nodes != null)
                    {
                        processedScripts.Add((file, nodes.ToArray()));
                    }
                }
                catch
                {
                    // 处理单个文件的异常，但继续处理其他文件
                }
                finally
                {
                    semaphore.Release(); // 释放信号量
                }
            }
#pragma warning disable CS0168 // 声明了变量，但从未使用过
            catch (Exception ex) { }
#pragma warning restore CS0168 // 声明了变量，但从未使用过
        }

        private void LoadCodeFolderTreeView()
        {
            tvFolderFiles.Nodes.Clear();

            if (!string.IsNullOrEmpty(settings.RootFolder))
            {
                try
                {
                    // Create a root node for the folder
                    TreeNode rootNode = new(Path.GetFileName(settings.RootFolder))
                    {
                        Tag = settings.RootFolder
                    };
                    tvFolderFiles.Nodes.Add(rootNode);

                    // Populate the TreeView with .erb files
                    PopulateTreeView(settings.RootFolder, rootNode);

                    // Expand the root node
                    rootNode.Expand();
                }
                catch { }
            }
        }

        private void PopulateTreeView(string folderPath, TreeNode parentNode)
        {
            try
            {
                // Add all files with .erb extension
                foreach (string file in Directory.GetFiles(folderPath, "*.erb"))
                {
                    string fileName = Path.GetFileName(file);
                    TreeNode fileNode = new(fileName)
                    {
                        Tag = file
                    };
                    parentNode.Nodes.Add(fileNode);
                }

                // Process subdirectories
                foreach (string subDir in Directory.GetDirectories(folderPath))
                {
                    string dirName = Path.GetFileName(subDir);
                    TreeNode dirNode = new(dirName)
                    {
                        Tag = subDir
                    };
                    parentNode.Nodes.Add(dirNode);

                    // Recursively process subdirectories
                    PopulateTreeView(subDir, dirNode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error accessing directory: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tvFolderFiles_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is string str)
            {
                try
                {
                    if (!File.Exists(str))
                    {
                        return;
                    }
                    eeCodeView.LoadCodeFromFile(str);
                }
                catch
#if DEBUG
#pragma warning disable CS8360
                when (false)
#pragma warning restore CS8360
#endif
                { }
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string previousRootFolder = settings.RootFolder;
            formSettings formSettings = new(settings);
            if (formSettings.ShowDialog() == DialogResult.OK)
            {
                settings = formSettings.Settings;

                // Save the updated settings
                try
                {
                    File.WriteAllText("settings.json", JsonSerializer.Serialize(settings));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving settings: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // If RootFolder changed, reload the TreeView
                if (settings.RootFolder != previousRootFolder)
                {
                    LoadCodeFolderTreeView();
                }
            }
        }
        private void tvFolderFiles_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Show context menu on right-click
            if (e.Button == MouseButtons.Right && e.Node.Tag is string path)
            {
                ContextMenuStrip menu = new();
                if (File.Exists(path))
                {
                    // ファイルの場合は編集オプションを表示
                    var editMenuItem = menu.Items.Add("コードを編集");
                    editMenuItem.Click += (_, _) => eeCodeView.LoadCodeFromFile(path);

                    var renameMenuItem = menu.Items.Add("名前を変更");
                    renameMenuItem.Click += (_, _) =>
                    {
                        string newName = Microsoft.VisualBasic.Interaction.InputBox(
                            "新しいファイル名を入力してください:", "名前変更", Path.GetFileNameWithoutExtension(path));
                        if (!string.IsNullOrEmpty(newName))
                        {
                            try
                            {
                                string directory = Path.GetDirectoryName(path) ?? "";
                                string extension = Path.GetExtension(path);
                                string newPath = Path.Combine(directory, newName + extension);

                                if (File.Exists(newPath))
                                {
                                    MessageBox.Show("同名のファイルが既に存在します。", "エラー",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                File.Move(path, newPath);
                                e.Node.Text = Path.GetFileName(newPath);
                                e.Node.Tag = newPath;
                                MessageBox.Show("ファイル名を変更しました。", "成功",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"ファイル名の変更に失敗しました: {ex.Message}", "エラー",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    };

                    var openExternalMenuItem = menu.Items.Add("外部エディターで開く");
                    openExternalMenuItem.Tag = path;
                    openExternalMenuItem.Click += (s, args) =>
                    {
                        if (s is ToolStripMenuItem menuItem && menuItem.Tag is string filePath)
                        {
                            try
                            {
                                if (File.Exists(filePath))
                                {
                                    Process.Start(new ProcessStartInfo
                                    {
                                        FileName = filePath,
                                        UseShellExecute = true
                                    });
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error opening file: {ex.Message}", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    };
                }
                else if (Directory.Exists(path))
                {
                    // ディレクトリの場合は新規作成オプションを表示
                    var newScriptMenuItem = menu.Items.Add("新しいスクリプト作成");
                    newScriptMenuItem.Click += (_, _) =>
                    {
                        try
                        {
                            string fileName = Microsoft.VisualBasic.Interaction.InputBox(
                                "新しいファイル名を入力してください:", "ファイル作成", "新規スクリプト.erb");
                            if (!string.IsNullOrEmpty(fileName))
                            {
                                if (!fileName.EndsWith(".erb", StringComparison.OrdinalIgnoreCase))
                                    fileName += ".erb";
                                string newFilePath = Path.Combine(path, fileName);
                                File.WriteAllText(newFilePath, "");

                                // 新しいノードをツリービューに追加
                                TreeNode newNode = new(fileName) { Tag = newFilePath };
                                e.Node.Nodes.Add(newNode);
                                e.Node.Expand();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"ファイル作成エラー: {ex.Message}", "エラー",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };

                    var newFolderMenuItem = menu.Items.Add("新しいフォルダ作成");
                    newFolderMenuItem.Click += (_, _) =>
                    {
                        try
                        {
                            string folderName = Microsoft.VisualBasic.Interaction.InputBox(
                                "新しいフォルダ名を入力してください:", "フォルダ作成", "新規フォルダ");
                            if (!string.IsNullOrEmpty(folderName))
                            {
                                string newFolderPath = Path.Combine(path, folderName);
                                Directory.CreateDirectory(newFolderPath);

                                // 新しいノードをツリービューに追加
                                TreeNode newNode = new(folderName) { Tag = newFolderPath };
                                e.Node.Nodes.Add(newNode);
                                e.Node.Expand();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"フォルダ作成エラー: {ex.Message}", "エラー",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };

                    var renameMenuItem = menu.Items.Add("フォルダー名を変更");
                    renameMenuItem.Click += (_, _) =>
                    {
                        string newName = Microsoft.VisualBasic.Interaction.InputBox(
                            "新しいフォルダー名を入力してください:", "名前変更", Path.GetFileName(path));
                        if (!string.IsNullOrEmpty(newName))
                        {
                            try
                            {
                                string? parentPath = Directory.GetParent(path)?.FullName;
                                if (parentPath == null)
                                {
                                    MessageBox.Show("親フォルダーを特定できませんでした。", "エラー",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                string newPath = Path.Combine(parentPath, newName);

                                if (Directory.Exists(newPath))
                                {
                                    MessageBox.Show("同名のフォルダーが既に存在します。", "エラー",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                Directory.Move(path, newPath);
                                e.Node.Text = newName;
                                e.Node.Tag = newPath;
                                MessageBox.Show("フォルダー名を変更しました。", "成功",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"フォルダー名の変更に失敗しました: {ex.Message}", "エラー",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    };

                    var openExplorerMenuItem = menu.Items.Add("エクスプローラーで開く");
                    openExplorerMenuItem.Click += (_, _) =>
                    {
                        try
                        {
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = path,
                                UseShellExecute = true
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"フォルダを開けませんでした: {ex.Message}", "エラー",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };
                }
            }
        }

        private void OpenFile_Click(object? sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem && menuItem.Tag is string filePath)
            {
                try
                {
                    if (File.Exists(filePath))
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = filePath,
                            UseShellExecute = true
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void EOProcesser_Shown(object sender, EventArgs e)
        {
            bwLoadCards.RunWorkerAsync();
        }

        private async void bwLoadCards_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            loadedCards = 0;
            var node = await LoadCardTreeView();
            if (node != null)
            {
                Invoke(new Action(() =>
                {
                    treeCards.Nodes.Clear();
                    treeCards.Nodes.Add(node);
                    node.Expand();
                }));
            }
        }

        private void bwLoadCards_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            txtSearchCard.Enabled = false;
            txtSearchCard.Text = $"Loaded {loadedCards}/{allCards} cards...";
        }

        private void bwLoadCards_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            txtSearchCard.Text = "";
            txtSearchCard.Enabled = true;
        }

        private void txtSearchCard_TextChanged(object sender, EventArgs e)
        {
        }

        private void ShowAllNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                // Reset all visual indicators
                node.ForeColor = SystemColors.WindowText;  // Reset color to default
                node.BackColor = SystemColors.Window;      // Reset background color
                node.Checked = false;                      // Uncheck the node

                // Recursively reset all child nodes
                if (node.Nodes.Count > 0)
                {
                    ShowAllNodes(node.Nodes);
                    node.Collapse();  // Collapse after processing children for proper state reset
                }
            }
        }

        private bool SearchNodes(TreeNode node, string searchText)
        {
            // Flag to track if this node or any child nodes match the search
            bool anyMatches = false;

            // Check if the current node text matches the search criteria
            bool currentNodeMatches = node.Text.Contains(searchText, StringComparison.CurrentCultureIgnoreCase);
            if (currentNodeMatches)
            {
                node.ForeColor = Color.Blue;  // Highlight matching nodes
                node.BackColor = Color.LightYellow;
                node.Checked = true;
                anyMatches = true;
            }
            else
            {
                // Reset this node's appearance if it doesn't match
                node.ForeColor = SystemColors.WindowText;
                node.BackColor = SystemColors.Window;
                node.Checked = false;
            }

            // Recursively search child nodes
            bool childrenMatch = false;
            foreach (TreeNode childNode in node.Nodes)
            {
                if (SearchNodes(childNode, searchText))
                {
                    childrenMatch = true;
                }
            }

            // If any children match, expand this node and mark it as having matches
            if (childrenMatch)
            {
                node.Expand();
                anyMatches = true;
            }
            else if (!currentNodeMatches)
            {
                node.Collapse();
            }

            return anyMatches;
        }

        private void txtSearchCard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;  // Prevent the Enter key from creating a new line
                btnSearch_Click(sender, e);  // Trigger the search button click
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!txtSearchCard.Enabled)
            {
                return;
            }
            string searchText = txtSearchCard.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                // If search text is empty, show all nodes
                ShowAllNodes(treeCards.Nodes);
                return;
            }

            // Start the search process
            foreach (TreeNode node in treeCards.Nodes)
            {
                SearchNodes(node, searchText);
            }
        }
        private void ClearAll()
        {
            checkIsRogueCard.Checked = false;
            listCardScriptCard.Items.Clear();
            listCategory.Items.Clear();
            listCardInfo.Items.Clear();
            txtCardName.Text = "";
            txtShortName.Text = "";
            treeCardEffectList.Nodes.Clear();
            eeCardCan.ClearAll();
            eeCardEffect.ClearAll();
            eeCardExplanation.ClearAll();
            eeCardManagerScriptEditor.ClearAll();
            eeCardSummonAA.ClearAll();
            eeExtraFuncs.ClearAll();
            CurrentCard = null;
        }
        ERAOCGCardScript? CurrentCardScript = null;
        private void treeCards_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is ERAOCGCardScript script)
            {
                if (noConfirm || MessageBox.Show("保存されてないデータが失われる。\n確認しますか？", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ClearAll();
                    listCardScriptCard.Items.Clear();
                    foreach (var card in script.Cards)
                    {
                        listCardScriptCard.Items.Add(card);
                    }
                    CurrentCardScript = script;
                    CurrentEditingTreeNode = e.Node;
                    tabCardEditPanel.SelectedIndex = 0;
                }
            }
            else if (e.Node.Tag is ERAOCGCard card)
            {
                if (noConfirm || MessageBox.Show("カード所属のスクリプトを開きます。\n保存されてないデータが失われる。\n確認しますか？", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ClearAll();
                    foreach (var c in card.CardScript.Cards)
                    {
                        listCardScriptCard.Items.Add(c);
                    }
                    CurrentCardScript = card.CardScript;
                    CurrentEditingTreeNode = e.Node.Parent!;
                    if (listCardScriptCard.Items.Count != 1)
                    {
                        tabCardEditPanel.SelectedIndex = 0;
                    }
                }
            }
            if (listCardScriptCard.Items.Count == 1)
            {
                noConfirm = true;
                listCardScriptCard.SelectedIndex = 0;
                listCardScriptCard_MouseDoubleClick(sender, e);
            }
            noConfirm = false;
        }

        bool noConfirm = false;

        ERAOCGCard? CurrentCard = null;
        private void listCardScriptCard_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            silent = true;
            if (listCardScriptCard.SelectedItem is ERAOCGCard card)
            {
                if (noConfirm || MessageBox.Show("保存されてないデータが失われる。確認しますか？", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    noConfirm = false;
                    txtCardName.Text = card.Name;
                    txtShortName.Text = card.ShortName;
                    checkIsRogueCard.Checked = card.IsRogueCard();
                    listCardInfo.Items.Clear();
                    listCategory.Items.Clear();
                    listCategory.Items.AddRange([.. card.GetCardCategory()]);
                    var func = card.GetCardInfoFunc();
                    if (func != null)
                    {
                        var sel = (func.FirstOrDefault((f) => f is ERACodeSelectCase)
                            as ERACodeSelectCase)
                            ?? new ERACodeSelectCase("参照先");
                        foreach (ERACodeSelectCaseSubCase caseVal in sel.Cast<ERACodeSelectCaseSubCase>())
                        {
                            listCardInfo.Items.Add(new ERACodeSelectCaseSubCaseListItem(caseVal));
                        }
                    }
                    func = card.GetCardEffectFunc();
                    if (func != null)
                    {
                        eeCardEffect.LoadCode(func);
                    }
                    func = card.GetCardCanFunc();
                    if (func != null)
                    {
                        eeCardCan.LoadCode(func);
                    }
                    func = card.GetCardExplanationFunc();
                    if (func != null)
                    {
                        eeCardExplanation.LoadCode(func);
                    }
                    func = card.GetCardAAFunc();
                    if (func != null)
                    {
                        eeCardSummonAA.LoadCode(func);
                    }

                    CurrentCard = card;
                    tabCardEditPanel.SelectedIndex = 1;
                    //トリガー
                    radioCMStandardEffect.Checked = false;
                    radioCMStandardEffect.Checked = true;
                }
            }
            silent = false;
        }

        class ERACodeSelectCaseSubCaseListItem
        {
            public ERACodeSelectCaseSubCase CaseValue;
            public ERACodeSelectCaseSubCaseListItem(ERACodeSelectCaseSubCase caseValue)
            {
                CaseValue = caseValue;
            }
            public override string ToString()
            {
                return $"{CaseValue.CaseCondition?.TrimStart('"').TrimEnd('"')} : {CaseValue.GetValue()}";
            }
        }

        bool silent = true;
        bool stopCheckChangeProcess = false;
        EOCardManagerCardEffect? CurrentCMEffects = null;
        private void radioCMStandardEffect_CheckedChanged(object sender, EventArgs e)
        {
            if (stopCheckChangeProcess)
            {
                return;
            }
            if (!radioCMStandardEffect.Checked)
            {
                return;
            }
            try
            {
                if (CurrentCardScript == null || CurrentCard == null)
                {
                    return;
                }
                treeCardEffectList.Nodes.Clear();
                CurrentCMEffects = EOCardManagerCardEffect.Parse(CurrentCard);
                treeCardEffectList.Nodes.AddRange([.. CurrentCMEffects.GetTreeNodes()]);
            }
            catch (Exception ex)
            {
                if (!silent)
                {
                    MessageBox.Show("非標準カードなので変換できません：" + ex.ToString(),
                        "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    stopCheckChangeProcess = true;
                    // Reset to custom standard effect
                    radioCMStandardEffect.Checked = false;
                    radioCustomStandardEffect.Checked = true;
                    stopCheckChangeProcess = false;
                }
            }
        }
        private void treeCardEffectList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            switch (e.Node.Tag)
            {
                case null:
                    return;
                case IEnumerable<ERACode> or ERACode:
                    if (e.Node.Tag is IEnumerable<ERACode> codes)
                    {
                        eeCardManagerScriptEditor.LoadCode(new ERACodeMultiLines(codes));
                    }
                    else if (e.Node.Tag is ERACode code)
                    {
                        eeCardManagerScriptEditor.LoadCode(code);
                    }
                    eeCardManagerScriptEditor.ClearOnSaveEvent();
                    eeCardManagerScriptEditor.OnSave += (sdr, ee) =>
                    {
                        ERACodeMultiLines lines = eeCardManagerScriptEditor.GetCodeMultiLines();
                        List<TreeNode> nodeList = lines.GetTreeNodes();
                        var parent = e.Node.Parent;
                        // Replace e.Node with the new nodes from nodeList
                        if (parent != null)
                        {
                            if (parent.Tag is EOCardManagerEffect)
                            {
                                e.Node.Nodes.Clear();
                                e.Node.Nodes.AddRange([.. nodeList]);
                            }
                            else
                            {
                                // If the node has a parent, replace the node in parent's collection
                                int nodeIndex = parent.Nodes.IndexOf(e.Node);
                                if (nodeIndex >= 0)
                                {
                                    parent.Nodes.RemoveAt(nodeIndex);
                                    foreach (TreeNode node in nodeList)
                                    {
                                        parent.Nodes.Insert(nodeIndex++, node);
                                    }
                                }
                            }
                        }
                        else
                        {
                            // If this is a root node (no parent), replace in the TreeView.Nodes collection
                            int nodeIndex = treeCardEffectList.Nodes.IndexOf(e.Node);
                            if (nodeIndex >= 0)
                            {
                                treeCardEffectList.Nodes.RemoveAt(nodeIndex);
                                foreach (TreeNode newNode in nodeList)
                                {
                                    treeCardEffectList.Nodes.Insert(nodeIndex++, newNode);
                                }
                            }
                        }
                    };
                    break;
                case string strValue:
                    // 如果是字符串属性（如Condition），提示用户输入
                    string propertyName = e.Node.Text;

                    // 从节点文本中提取属性名称（例如从"条件: xxx"提取"条件"）
                    if (propertyName.Contains(':'))
                    {
                        propertyName = propertyName.Substring(0, propertyName.IndexOf(':'));
                    }

                    // 显示输入对话框
                    string prompt = $"新しい{propertyName}値を入力してください:";
                    string title = $"{propertyName}の変更";

                    string? newValue = Microsoft.VisualBasic.Interaction.InputBox(prompt, title, strValue);

                    // 如果用户输入了新值（不为空且不同于原值）
                    if (!string.IsNullOrEmpty(newValue) && newValue != strValue)
                    {
                        // 更新节点显示的文本
                        e.Node.Text = $"{propertyName}: {newValue}";

                        // 更新节点的Tag值
                        e.Node.Tag = newValue;

                        // 如果节点是某个效果对象的属性，还需要更新对应的对象
                        if (e.Node.Parent?.Tag is EOCardManagerEffect effect)
                        {
                            // 根据属性名更新对应的效果属性
                            if (propertyName.Trim() == "条件")
                            {
                                effect.Condition = newValue;
                            }
                            // 可以根据需要添加其他属性的处理
                        }
                    }
                    break;
                case EOCardManagerEffect effect:
                    // 如果点击的是效果节点本身，可以编辑效果编号或其他主要属性
                    string? newEffectNo = Microsoft.VisualBasic.Interaction.InputBox(
                        "新しい効果計数を入力してください\n（auto・自動を入力すると自動数えになる）:", "効果計数", effect.EffectNo ?? "");
                    if (newEffectNo == "auto" || newEffectNo == "自動")
                    {
                        newEffectNo = null;
                    }
                    effect.EffectNo = newEffectNo;
                    e.Node.Text = $"效果{newEffectNo ?? "(自動数え)"}";
                    break;
            }
        }

        private void treeCards_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Show context menu on right-click
            if (e.Button == MouseButtons.Right)
            {
                switch (e.Node.Tag)
                {
                    case ERAOCGCardScript script:
                        ContextMenuStrip menu = new();
                        var ms = menu.Items.Add("編集");
                        ms.Click += (_, _) =>
                        {
                            treeCards_NodeMouseDoubleClick(sender, e);
                        };
                        ms = menu.Items.Add("外部エディターで開く");
                        ms.Click += (_, _) =>
                        {
                            // Get the actual code to edit
                            if (script.ScriptFile != null)
                            {
                                try
                                {
                                    Process.Start(new ProcessStartInfo
                                    {
                                        FileName = script.ScriptFile,
                                        UseShellExecute = true
                                    });
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"ファイルを開けませんでした: {ex.Message}", "エラー",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        };
                        menu.Show(tvFolderFiles, e.Location);
                        break;
                    case ERAOCGCard card:
                        ContextMenuStrip cardMenu = new();
                        var cardMs = cardMenu.Items.Add("編集");
                        cardMs.Click += (_, _) =>
                        {
                            treeCards_NodeMouseDoubleClick(sender, e);
                        };
                        cardMs = cardMenu.Items.Add("外部エディターで開く");
                        cardMs.Click += (_, _) =>
                        {
                            // Get the actual code to edit
                            if (card.CardScript?.ScriptFile != null)
                            {
                                try
                                {
                                    Process.Start(new ProcessStartInfo
                                    {
                                        FileName = card.CardScript.ScriptFile,
                                        UseShellExecute = true
                                    });
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"ファイルを開けませんでした: {ex.Message}", "エラー",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        };
                        cardMenu.Show(tvFolderFiles, e.Location);
                        break;
                    case string path:
                        ContextMenuStrip pathMenu = new();
                        var pathMs = pathMenu.Items.Add("開く");
                        pathMs.Click += (_, _) =>
                        {
                            try
                            {
                                if (File.Exists(path))
                                {
                                    Process.Start(new ProcessStartInfo
                                    {
                                        FileName = path,
                                        UseShellExecute = true
                                    });
                                }
                                else if (Directory.Exists(path))
                                {
                                    Process.Start(new ProcessStartInfo
                                    {
                                        FileName = path,
                                        UseShellExecute = true
                                    });
                                }
                                else
                                {
                                    MessageBox.Show("ファイルまたはフォルダが存在しません。", "エラー",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"ファイルを開けませんでした: {ex.Message}", "エラー",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        };
                        var deletePathMs = pathMenu.Items.Add("削除");
                        deletePathMs.Click += (_, _) =>
                        {
                            try
                            {
                                string confirmMessage = File.Exists(path) ?
                                    "このファイルを削除してもよろしいですか？" :
                                    "このフォルダとその中のすべてのコンテンツを削除してもよろしいですか？";

                                if (MessageBox.Show(confirmMessage, "削除の確認",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                                {
                                    if (File.Exists(path))
                                    {
                                        File.Delete(path);
                                        MessageBox.Show("ファイルが削除されました。", "削除完了",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else if (Directory.Exists(path))
                                    {
                                        Directory.Delete(path, true); // 再帰的に削除
                                        MessageBox.Show("フォルダが削除されました。", "削除完了",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }

                                    // 親ノードから現在のノードを削除
                                    e.Node.Parent?.Nodes.Remove(e.Node);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"削除中にエラーが発生しました: {ex.Message}", "エラー",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        };
                        var newFolderMs = pathMenu.Items.Add("新しいフォルダ");
                        newFolderMs.Click += (_, _) =>
                        {
                            try
                            {
                                if (Directory.Exists(path))
                                {
                                    string newFolderName = Microsoft.VisualBasic.Interaction.InputBox(
                                        "新しいフォルダ名を入力してください:", "フォルダ作成", "");

                                    if (!string.IsNullOrEmpty(newFolderName))
                                    {
                                        // 新しいフォルダパスを作成
                                        string newFolderPath = Path.Combine(path, newFolderName);

                                        // 新しいフォルダを作成
                                        Directory.CreateDirectory(newFolderPath);

                                        // 新しいノードを作成
                                        TreeNode newNode = new(newFolderName)
                                        {
                                            Tag = newFolderPath
                                        };
                                        e.Node.Nodes.Add(newNode);
                                        e.Node.Expand();

                                        MessageBox.Show("フォルダが作成されました。", "作成完了",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"フォルダの作成中にエラーが発生しました: {ex.Message}", "エラー",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        };
                        var newScriptMs = pathMenu.Items.Add("新しいカードスクリプト");
                        newScriptMs.Click += (_, _) =>
                        {
                            try
                            {
                                if (Directory.Exists(path))
                                {
                                    // まずカードIDを取得
                                    string cardId = Microsoft.VisualBasic.Interaction.InputBox(
                                        "カードIDを入力してください（数字のみ）:",
                                        "スクリプト作成", "10000");

                                    // IDが入力されたか確認
                                    if (!string.IsNullOrEmpty(cardId) && int.TryParse(cardId, out int id))
                                    {
                                        // 次にカード名を取得
                                        string cardName = Microsoft.VisualBasic.Interaction.InputBox(
                                            "カード名を入力してください:",
                                            "スクリプト作成", "新しいカード");

                                        if (!string.IsNullOrEmpty(cardName))
                                        {
                                            // モンスターカードかどうか確認
                                            DialogResult isMonsterResult = MessageBox.Show(
                                                "モンスターカードですか？",
                                                "カードタイプ選択",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);
                                            bool isMonster = isMonsterResult == DialogResult.Yes;
                                            // ファイル名を作成（ID_カード名.ERB）
                                            string newScriptName = $"{id}_{cardName}.ERB";
                                            // 新しいスクリプトパスを作成
                                            string newScriptPath = Path.Combine(path, newScriptName);

                                            // ERAOCGCardScriptを作成（コンストラクタ内でファイルが自動生成される）
                                            ERAOCGCardScript script = new(id, cardName, path, isMonster);
                                            // 新しいノードを作成
                                            e.Node.Nodes.AddRange([.. script.GetTreeNodes()]);
                                            MessageBox.Show("スクリプトが作成されました。", "作成完了",
                                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("有効なカードIDを入力してください。", "エラー",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"スクリプトの作成中にエラーが発生しました: {ex.Message}", "エラー",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        };
                        var renameMs = pathMenu.Items.Add("名前を変更");
                        renameMs.Click += (_, _) =>
                        {
                            try
                            {
                                string currentName = Path.GetFileName(path);
                                string newName = Microsoft.VisualBasic.Interaction.InputBox(
                                    "新しい名前を入力してください:", "名前の変更", currentName);

                                if (!string.IsNullOrEmpty(newName) && newName != currentName)
                                {
                                    string? parentPath = Path.GetDirectoryName(path);
                                    if (parentPath == null)
                                    {
                                        MessageBox.Show("親パスを取得できませんでした。", "エラー",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                    string newPath = Path.Combine(parentPath, newName);

                                    if (File.Exists(path))
                                    {
                                        // ファイルの場合
                                        if (File.Exists(newPath))
                                        {
                                            MessageBox.Show("同じ名前のファイルが既に存在します。", "エラー",
                                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }
                                        File.Move(path, newPath);
                                    }
                                    else if (Directory.Exists(path))
                                    {
                                        // フォルダの場合
                                        if (Directory.Exists(newPath))
                                        {
                                            MessageBox.Show("同じ名前のフォルダが既に存在します。", "エラー",
                                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }
                                        Directory.Move(path, newPath);
                                    }

                                    // ノードの更新
                                    e.Node.Text = newName;
                                    e.Node.Tag = newPath;

                                    MessageBox.Show("名前が変更されました。", "完了",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"名前の変更中にエラーが発生しました: {ex.Message}", "エラー",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        };
                        pathMenu.Show(tvFolderFiles, e.Location);
                        break;
                }
            }
        }

        private void btnSaveSingleCard_Click(object sender, EventArgs e)
        {
            SaveCurrentCard();
        }

        private void treeCardEffectList_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                switch (e.Node.Tag)
                {
                    case ERAOCGCard card:
                        ContextMenuStrip cardMenu = new();
                        var cardMs = cardMenu.Items.Add("編集");
                        cardMs.Click += (_, _) =>
                        {
                            treeCardEffectList_NodeMouseDoubleClick(sender, e);
                        };
                        cardMs = cardMenu.Items.Add("外部エディターで開く");
                        cardMs.Click += (_, _) =>
                        {
                            // Get the actual code to edit
                            if (card.CardScript?.ScriptFile != null)
                            {
                                try
                                {
                                    Process.Start(new ProcessStartInfo
                                    {
                                        FileName = card.CardScript.ScriptFile,
                                        UseShellExecute = true
                                    });
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"ファイルを開けませんでした: {ex.Message}", "エラー",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        };
                        cardMenu.Show(treeCardEffectList, e.Location);
                        break;
                    case string path:
                        ContextMenuStrip pathMenu = new();
                        var pathMs = pathMenu.Items.Add("開く");
                        pathMs.Click += (_, _) =>
                        {
                            try
                            {
                                if (File.Exists(path))
                                {
                                    Process.Start(new ProcessStartInfo
                                    {
                                        FileName = path,
                                        UseShellExecute = true
                                    });
                                }
                                else if (Directory.Exists(path))
                                {
                                    Process.Start(new ProcessStartInfo
                                    {
                                        FileName = path,
                                        UseShellExecute = true
                                    });
                                }
                                else
                                {
                                    MessageBox.Show("ファイルまたはフォルダが存在しません。", "エラー",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"ファイルを開けませんでした: {ex.Message}", "エラー",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        };
                        pathMenu.Show(treeCardEffectList, e.Location);
                        break;
                    case List<ERACodeFuncSegment> funcList:
                        ContextMenuStrip funcAddMenu = new();
                        var funcAdd = funcAddMenu.Items.Add("関数追加");

                        funcAdd.Click += (_, _) =>
                        {
                            // Ask the user for a new function name
                            string newFuncName = Microsoft.VisualBasic.Interaction.InputBox(
                                "新しい関数名を入力してください:", "関数追加", "");

                            // If the user provided a name, create and add the function
                            if (!string.IsNullOrEmpty(newFuncName))
                            {
                                // Create a new function with the given name
                                var newFunc = new ERACodeFuncSegment(newFuncName);

                                // Add the function to the list
                                funcList.Add(newFunc);

                                // Add a node for the new function
                                TreeNode newNode = new TreeNode(newFuncName)
                                {
                                    Tag = newFunc
                                };
                                e.Node.Nodes.Add(newNode);

                                // Expand the parent node to show the new function
                                e.Node.Expand();

                                // Load the new function in the editor
                                eeCardManagerScriptEditor.LoadCode(newFunc);
                            }
                        };
                        funcAddMenu.Show(treeCardEffectList, e.Location);
                        break;
                    case ERACodeFuncSegment funcSegment:
                        ContextMenuStrip funcSegmentMenu = new();
                        var deleteFuncMs = funcSegmentMenu.Items.Add("関数削除");
                        deleteFuncMs.Click += (_, _) =>
                        {
                            // Confirm with the user before deleting
                            if (MessageBox.Show("この関数を削除してもよろしいですか？",
                                               "関数の削除",
                                               MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                // Remove the node from its parent
                                e.Node.Parent?.Nodes.Remove(e.Node);
                            }
                        };
                        funcSegmentMenu.Show(treeCardEffectList, e.Location);
                        break;
                    case List<EOCardManagerEffect> effectList:
                        ContextMenuStrip effectListMenu = new();
                        var addEffectMs = effectListMenu.Items.Add("効果追加");
                        addEffectMs.Click += (_, _) =>
                        {
                            // Prompt user for the condition
                            string condition = Microsoft.VisualBasic.Interaction.InputBox(
                                "効果の条件を入力してください:", "効果追加", "");

                            if (!string.IsNullOrEmpty(condition))
                            {
                                // Create a new effect with the given condition
                                var newEffect = new EOCardManagerEffect(null, null, condition);

                                // Add the effect to the list
                                effectList.Add(newEffect);

                                e.Node.Nodes.Clear();
                                foreach (var eff in effectList)
                                {
                                    e.Node.Nodes.AddRange([.. eff.GetTreeNodes()]);
                                }

                                // Expand the parent node to show the new effect
                                e.Node.Expand();
                                foreach (TreeNode node in e.Node.Nodes)
                                {
                                    node.Expand();
                                }
                            }
                        };
                        effectListMenu.Show(treeCardEffectList, e.Location);
                        break;
                    case EOCardManagerEffect effect:
                        ContextMenuStrip effectMenu = new();

                        var deleteMs = effectMenu.Items.Add("効果削除");
                        deleteMs.Click += (_, _) =>
                        {
                            // Confirm with the user before deleting
                            if (MessageBox.Show("この効果を削除してもよろしいですか？",
                                               "効果の削除",
                                               MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                // Remove the node from its parent
                                e.Node.Parent?.Nodes.Remove(e.Node);
                                (e.Node.Parent?.Tag as List<EOCardManagerEffect>)?.Remove(effect);
                            }
                        };
                        effectMenu.Show(treeCardEffectList, e.Location);
                        var moveUpMs = effectMenu.Items.Add("効果上へ移動");
                        moveUpMs.Click += (_, _) =>
                        {
                            // Get the parent node and its tag which should be the list of effects
                            TreeNode parent = e.Node.Parent;
                            if (parent == null)
                            {
                                return;
                            }

                            if (parent?.Tag is List<EOCardManagerEffect> effectsList)
                            {
                                int index = effectsList.IndexOf(effect);
                                if (index > 0) // Make sure we're not at the top already
                                {
                                    // Swap in the effects list
                                    effectsList[index] = effectsList[index - 1];
                                    effectsList[index - 1] = effect;

                                    // Refresh the tree view nodes
                                    parent.Nodes.Clear();
                                    foreach (var eff in effectsList)
                                    {
                                        parent.Nodes.AddRange([.. eff.GetTreeNodes()]);
                                    }

                                    // Expand the parent node
                                    parent.Expand();
                                    foreach (TreeNode node in parent.Nodes)
                                    {
                                        node.Expand();
                                    }
                                }
                            }
                        };

                        var moveDownMs = effectMenu.Items.Add("効果下へ移動");
                        moveDownMs.Click += (_, _) =>
                        {
                            // Get the parent node and its tag which should be the list of effects
                            TreeNode parent = e.Node.Parent;
                            if (parent == null)
                            {
                                return;
                            }

                            if (parent?.Tag is List<EOCardManagerEffect> effectsList)
                            {
                                int index = effectsList.IndexOf(effect);
                                if (index < effectsList.Count - 1) // Make sure we're not at the bottom already
                                {
                                    // Swap in the effects list
                                    effectsList[index] = effectsList[index + 1];
                                    effectsList[index + 1] = effect;

                                    // Refresh the tree view nodes
                                    parent.Nodes.Clear();
                                    foreach (var eff in effectsList)
                                    {
                                        parent.Nodes.AddRange([.. eff.GetTreeNodes()]);
                                    }

                                    // Expand the parent node
                                    parent.Expand();
                                    foreach (TreeNode node in parent.Nodes)
                                    {
                                        node.Expand();
                                    }
                                }
                            }
                        };
                        break;
                    case ERACode code:
                        ContextMenuStrip menu = new();
                        var ms = menu.Items.Add("編集");
                        ms.Click += (sender, e) =>
                        {
                            eeCardManagerScriptEditor.LoadCode(code);
                        };
                        menu.Show(treeCardEffectList, e.Location);

                        // Check if the parent node's tag is IEnumerable<ERACode>
                        if (e.Node.Parent?.Tag is ERACodeMultiLines codeList)
                        {
                            var deleteCodeItem = menu.Items.Add("削除");
                            deleteCodeItem.Click += (_, _) =>
                            {
                                // Confirm with the user before deleting
                                if (MessageBox.Show("このコードを削除してもよろしいですか？",
                                                  "コードの削除",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    // Find and remove the code from the code list
                                    codeList.Remove(code);

                                    // Remove the node from its parent
                                    e.Node.Parent?.Nodes.Remove(e.Node);
                                }
                            };
                        }
                        break;
                }
            }
        }

        TreeNode? CurrentEditingTreeNode = null;

        private void btnCardScriptAddCard_Click(object sender, EventArgs e)
        {
            if (CurrentEditingTreeNode != null && CurrentCardScript != null)
            {
                ERAOCGCard card = new(CurrentCardScript, CurrentCardScript.Cards[0].CardId + 100000);
                CurrentCardScript.Cards.Add(card);
                listCardScriptCard.Items.Add(card);
                // 刷新TreeView
                var parent = CurrentEditingTreeNode.Parent;
                if (parent != null)
                {
                    parent.Nodes.AddRange([.. card.GetTreeNodes()]);
                }
                else
                {
                    treeCards.Nodes.AddRange([.. card.GetTreeNodes()]);
                }

                SaveCurrentCard();
            }
        }

        private void btnCardScriptRemoveCard_Click(object sender, EventArgs e)
        {
            if (listCardScriptCard.SelectedItem is ERAOCGCard selectedCard && CurrentCardScript != null)
            {
                // Don't remove the last card
                if (CurrentCardScript.Cards.Count <= 1)
                {
                    MessageBox.Show("最後のカードは削除できません。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Confirm with the user before deleting
                if (MessageBox.Show($"このカード「{selectedCard.Name}」を削除してもよろしいですか？",
                                    "カードの削除",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Remove from the script's card collection
                    CurrentCardScript.Cards.Remove(selectedCard);

                    // Remove from the list view
                    listCardScriptCard.Items.Remove(selectedCard);

                    // If the tree view is showing this card, find and remove its node
                    if (CurrentEditingTreeNode != null)
                    {
                        // Look for the card node in tree to remove it
                        foreach (TreeNode node in CurrentEditingTreeNode.Nodes)
                        {
                            if (node.Tag == selectedCard)
                            {
                                CurrentEditingTreeNode.Nodes.Remove(node);
                                break;
                            }
                        }
                    }

                    // Clear the editing fields if we were editing this card
                    if (CurrentCard == selectedCard)
                    {
                        ClearAll();
                    }

                    SaveCurrentCard();
                }
            }
        }

        private void btnCardScriptMoveUp_Click(object sender, EventArgs e)
        {
            if (listCardScriptCard.SelectedItem is ERAOCGCard selectedCard && CurrentCardScript != null)
            {
                int index = listCardScriptCard.SelectedIndex;
                if (index > 0)
                {
                    // Swap in the cards list
                    CurrentCardScript.Cards[index] = CurrentCardScript.Cards[index - 1];
                    CurrentCardScript.Cards[index - 1] = selectedCard;

                    // Refresh the list view
                    listCardScriptCard.Items.Clear();
                    foreach (var card in CurrentCardScript.Cards)
                    {
                        listCardScriptCard.Items.Add(card);
                    }

                    // Restore selection
                    listCardScriptCard.SelectedIndex = index - 1;

                    // Update the tree view if necessary
                    if (CurrentEditingTreeNode != null)
                    {
                        CurrentEditingTreeNode.Nodes.Clear();
                        foreach (var card in CurrentCardScript.Cards)
                        {
                            CurrentEditingTreeNode.Nodes.AddRange([.. card.GetTreeNodes()]);
                        }
                    }

                    SaveCurrentCard();
                }
            }
        }

        private void btnCardScriptMoveDown_Click(object sender, EventArgs e)
        {
            if (listCardScriptCard.SelectedItem is ERAOCGCard selectedCard && CurrentCardScript != null)
            {
                int index = listCardScriptCard.SelectedIndex;
                if (index < listCardScriptCard.Items.Count - 1)
                {
                    // Swap in the cards list
                    CurrentCardScript.Cards[index] = CurrentCardScript.Cards[index + 1];
                    CurrentCardScript.Cards[index + 1] = selectedCard;

                    // Refresh the list view
                    listCardScriptCard.Items.Clear();
                    foreach (var card in CurrentCardScript.Cards)
                    {
                        listCardScriptCard.Items.Add(card);
                    }

                    // Restore selection
                    listCardScriptCard.SelectedIndex = index + 1;

                    // Update the tree view if necessary
                    if (CurrentEditingTreeNode != null)
                    {
                        CurrentEditingTreeNode.Nodes.Clear();
                        foreach (var card in CurrentCardScript.Cards)
                        {
                            CurrentEditingTreeNode.Nodes.AddRange([.. card.GetTreeNodes()]);
                        }
                    }
                    SaveCurrentCard();
                }
            }
        }

        private void SaveCurrentCard()
        {
            StringBuilder sb = new();
            if (CurrentCardScript == null || CurrentCard == null)
            {
                return;
            }
            foreach (var card in CurrentCardScript.Cards)
            {
                if (CurrentCard.CardId == card.CardId)
                {
                    //Card Name
                    sb.AppendLine($"""
                    {GetCardNameScriptFromForm()}
                    {GetCardInfoScriptFromForm()}
                    {GetAllEffectContent()}
                    {GetCardAAScriptFromForm()}
                    """);

                    string GetAllEffectContent()
                    {
                        if (radioCMStandardEffect.Checked
                            && treeCardEffectList.Nodes.Count > 0
                            && treeCardEffectList.Nodes[0].Nodes.Count == 5)
                        {
                            List<TreeNode> effectNodes = [.. treeCardEffectList.Nodes[0].Nodes[4].Nodes.Cast<TreeNode>()];
                            ERACodeFuncSegment explanationFunc = new($"CARD_EXPLANATION_{CurrentCard.CardId}(種類)");
                            //効果文定義
                            foreach (TreeNode node in treeCardEffectList.Nodes[0].Nodes[0].Nodes)
                            {
                                if (node.Tag is ERACode code)
                                {
                                    explanationFunc.Add(code);
                                }
                            }

                            int effectIndex = 0;
                            foreach (var node in effectNodes)
                            {
                                string str = "";
                                if (node.Tag is EOCardManagerEffect effect)
                                {
                                    if (effect.EffectNo != null)
                                    {
                                        str = effect.EffectNo;
                                    }
                                    else
                                    {
                                        str = EOCardManagerCardEffect.NumString[effectIndex].ToString();
                                        effectIndex++;
                                    }
                                }
                                bool prefixAdded = false;
                                for (int i = 0; i < node.Nodes[1].Nodes.Count; i++)
                                {
                                    TreeNode childNode = node.Nodes[1].Nodes[i];
                                    if (childNode.Tag is ERACode code)
                                    {
                                        if (!prefixAdded && code is ERACodePrintLine codeLine)
                                        {
                                            prefixAdded = true;
                                            ERACodePrintLine newCodeLine =
                                                new($"PRINT{codeLine.PrintType} {str}：{codeLine.Content}");
                                            childNode.Tag = newCodeLine;
                                            explanationFunc.Add(newCodeLine);
                                        }
                                        else
                                        {
                                            explanationFunc.Add(code);
                                        }
                                    }
                                }
                            }

                            ERACodeFuncSegment canFunc =
                                new($"CARDCAN_{CurrentCard.CardId}(決闘者,種類,ゾーン,場所)");
                            //効果可用性定義
                            foreach (TreeNode node in treeCardEffectList.Nodes[0].Nodes[1].Nodes)
                            {
                                if (node.Tag is ERACode code)
                                {
                                    canFunc.Add(code);
                                }
                            }
                            if (effectNodes.Count > 0)
                            {
                                ERACodeIfSegment segment = new(effectNodes[0].Nodes[0].Tag.ToString() ?? "1 == 1");
                                // 効果可用性
                                foreach (TreeNode node in effectNodes[0].Nodes[2].Nodes)
                                {
                                    if (node.Tag is ERACode code)
                                    {
                                        segment.Add(code);
                                    }
                                }
                                for (int i = 1; i < effectNodes.Count; i++)
                                {
                                    ERACodeMultiLines lines = [];
                                    foreach (TreeNode node in effectNodes[i].Nodes[2].Nodes)
                                    {
                                        if (node.Tag is ERACode code)
                                        {
                                            lines.Add(code);
                                        }
                                    }
                                    if (lines.Count() > 0)
                                    {
                                        string cond = effectNodes[i].Nodes[0].Tag.ToString() ?? "1 == 1";
                                        segment.AddElseIf(cond, lines);
                                    }
                                }
                                canFunc.Add(segment);
                            }

                            ERACodeFuncSegment effectFunc =
                                new($"CARDEFFECT_{CurrentCard.CardId}(決闘者,種類,ゾーン,場所)");
                            //効果関数定義
                            foreach (TreeNode node in treeCardEffectList.Nodes[0].Nodes[2].Nodes)
                            {
                                if (node.Tag is ERACode code)
                                {
                                    effectFunc.Add(code);
                                }
                            }

                            if (effectNodes.Count > 0)
                            {
                                ERACodeIfSegment segment = new(effectNodes[0].Nodes[0].Tag.ToString() ?? "1 == 1");
                                //効果関数
                                foreach (TreeNode node in effectNodes[0].Nodes[3].Nodes)
                                {
                                    if (node.Tag is ERACode code)
                                    {
                                        segment.Add(code);
                                    }
                                }
                                for (int i = 1; i < effectNodes.Count; i++)
                                {
                                    ERACodeMultiLines lines = [];
                                    foreach (TreeNode node in effectNodes[i].Nodes[3].Nodes)
                                    {
                                        if (node.Tag is ERACode code)
                                        {
                                            lines.Add(code);
                                        }
                                    }
                                    if (lines.Count() > 0)
                                    {
                                        string cond = effectNodes[i].Nodes[0].Tag.ToString() ?? "1 == 1";
                                        segment.AddElseIf(cond, lines);
                                    }
                                }
                                effectFunc.Add(segment);
                            }

                            ERACodeMultiLines extraFuncs = [];
                            //追加関数
                            foreach (TreeNode tn in treeCardEffectList.Nodes[0].Nodes[3].Nodes)
                            {
                                if (tn.Tag is ERACode code)
                                {
                                    extraFuncs.Add(code);
                                }
                            }

                            return Utils.TrimCode($"""
                                {explanationFunc}
                                {canFunc}
                                {effectFunc}
                                {extraFuncs}
                                """);
                        }
                        else
                        {
                            return Utils.TrimCode($"""
                            {GetCardExplanationScriptFromForm()}
                            {GetCardCanScriptFromForm()}
                            {GetCardEffectScriptFromForm()}
                            {GetCardExtraScriptFromForm()}
                            """);
                        }
                    }

                    string GetCardNameScriptFromForm()
                    {
                        StringBuilder categories = new();
                        int i = 0;
                        foreach (var c in listCategory.Items)
                        {
                            categories.AppendLine($"        RESULTS:{i} = {c}");
                        }
                        return $"""
                        @CARDNAME_{CurrentCard.CardId},参照先
                        ;ココで指定カードの名前、略称を返す予定
                        #DIMS DYNAMIC 参照先

                        VARSET RESULT
                        VARSET RESULTS

                        SELECTCASE 参照先
                            CASE "名前"
                                RESULTS = {txtCardName.Text}
                            CASE "略称"
                                RESULTS = {txtShortName.Text}
                            CASE "カテゴリ"
                        {categories}
                        ENDSELECT

                        """;
                    }
                    string GetCardInfoScriptFromForm()
                    {
                        ERACodeSelectCase sc = new("参照先");
                        foreach (ERACodeSelectCaseSubCaseListItem item in listCardInfo.Items)
                        {
                            sc.AddCase(item.CaseValue);
                        }
                        return $"""
                        @CARD_{CurrentCard.CardId}(参照先)

                        #DIMS DYNAMIC 参照先
                        VARSET RESULT
                        VARSET RESULTS

                        {sc}

                        RETURN 0

                        """;
                    }
                    string GetCardAAScriptFromForm()
                    {
                        var str = eeCardSummonAA.GetCodeString();
                        if (string.IsNullOrWhiteSpace(str))
                        {
                            return $"""
                            @CARDSUMMON_AA_{CurrentCard!.CardId}
                                RETURN 0
                            
                            """;
                        }
                        return str;
                    }
                    string GetCardExplanationScriptFromForm()
                    {
                        var str = eeCardExplanation.GetCodeString();
                        if (string.IsNullOrWhiteSpace(str))
                        {
                            return $"""
                            @CARD_EXPLANATION_{CurrentCard.CardId}(種類)
                                PRINTL 

                            """;
                        }
                        return str;
                    }
                    string GetCardCanScriptFromForm()
                    {
                        var str = eeCardCan.GetCodeString();
                        if (string.IsNullOrWhiteSpace(str))
                        {
                            return $"""
                                @CARDCAN_{CurrentCard.CardId}(決闘者,種類,ゾーン,場所)
                                #DIMS DYNAMIC 決闘者
                                #DIMS DYNAMIC ゾーン
                                #DIM DYNAMIC 種類
                                #DIM DYNAMIC 場所

                                CALL CARD_NEGATE(決闘者,種類,ゾーン,場所,2585)
                                SIF RESULT == 1
                                    RETURN 0
                                
                            """;
                        }
                        return str;
                    }
                    string GetCardEffectScriptFromForm()
                    {
                        var str = eeCardEffect.GetCodeString();
                        if (string.IsNullOrWhiteSpace(str))
                        {
                            return $"""
                            @CARDEFFECT_{CurrentCard.CardId}(決闘者,種類,ゾーン,場所)
                                #DIMS DYNAMIC 決闘者
                                #DIMS DYNAMIC 対面者
                                #DIMS DYNAMIC ゾーン
                                #DIM DYNAMIC 種類
                                #DIM DYNAMIC 場所
                                #DIM DYNAMIC カウンタ
                                #DIM DYNAMIC 攻撃力修正

                                CALL 対面者判定(決闘者)
                                対面者 = %RESULTS%
                            
                            """;
                        }
                        return str;
                    }
                    string GetCardExtraScriptFromForm()
                    {
                        return "";
                    }
                }
                else
                {
                    sb.AppendLine(card.GetCardNameFunc().ToString());
                    sb.AppendLine(card.GetCardInfoFunc().ToString());
                    sb.AppendLine(card.GetCardAAFunc().ToString());
                    sb.AppendLine(card.GetCardExplanationFunc().ToString());
                    sb.AppendLine(card.GetCardCanFunc().ToString());
                    sb.AppendLine(card.GetCardEffectFunc().ToString());
                }
            }
            silent = true;
            File.WriteAllText(CurrentCardScript.ScriptFile, Utils.TrimCode(sb.ToString()));
            CurrentCardScript = new(CurrentCardScript.ScriptFile);
            if (CurrentEditingTreeNode != null)
            {
                // Replace existing nodes with the new nodes from CurrentCardScript
                var newNodes = CurrentCardScript.GetTreeNodes();
                var parent = CurrentEditingTreeNode.Parent;

                if (parent != null)
                {
                    // Find the index of the CurrentEditingTreeNode in the parent's collection
                    int nodeIndex = parent.Nodes.IndexOf(CurrentEditingTreeNode);
                    if (nodeIndex >= 0)
                    {
                        // Remove the old node
                        parent.Nodes.RemoveAt(nodeIndex);

                        // Insert the new nodes at the same position
                        foreach (TreeNode newNode in newNodes)
                        {
                            parent.Nodes.Insert(nodeIndex++, newNode);
                        }
                    }
                }
                else
                {
                    // If it's a root node, find its index in the treeView's root collection
                    int nodeIndex = treeCards.Nodes.IndexOf(CurrentEditingTreeNode);
                    if (nodeIndex >= 0)
                    {
                        // Remove the old node
                        treeCards.Nodes.RemoveAt(nodeIndex);

                        // Insert the new nodes at the same position
                        foreach (TreeNode newNode in newNodes)
                        {
                            treeCards.Nodes.Insert(nodeIndex++, newNode);
                        }
                    }
                }
            }
            ClearAll();
            silent = false;
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string newCategory = Microsoft.VisualBasic.Interaction.InputBox(
                "新しいカテゴリを入力してください:", "カテゴリ追加", "");

            if (!string.IsNullOrEmpty(newCategory))
            {
                // Check if the category already exists
                if (!listCategory.Items.Contains(newCategory))
                {
                    listCategory.Items.Add(newCategory);
                }
                else
                {
                    MessageBox.Show("このカテゴリは既に存在します。", "警告",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnRemoveCategory_Click(object sender, EventArgs e)
        {
            if (listCategory.SelectedIndex != -1)
            {
                if (MessageBox.Show("選択したカテゴリを削除してもよろしいですか？", "カテゴリの削除",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    listCategory.Items.RemoveAt(listCategory.SelectedIndex);
                }
            }
            else
            {
                MessageBox.Show("削除するカテゴリを選択してください。", "警告",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnMoveUpCategory_Click(object sender, EventArgs e)
        {
            if (listCategory.SelectedItem != null)
            {
                int selectedIndex = listCategory.SelectedIndex;
                if (selectedIndex > 0)
                {
                    // Get the selected item
                    object selectedItem = listCategory.SelectedItem;

                    // Remove it from the current position
                    listCategory.Items.RemoveAt(selectedIndex);

                    // Insert it at the new position
                    listCategory.Items.Insert(selectedIndex - 1, selectedItem);

                    // Keep the item selected
                    listCategory.SelectedIndex = selectedIndex - 1;
                }
            }
        }

        private void btnMoveDownCategory_Click(object sender, EventArgs e)
        {
            if (listCategory.SelectedItem != null)
            {
                int selectedIndex = listCategory.SelectedIndex;
                if (selectedIndex < listCategory.Items.Count - 1)
                {
                    // Get the selected item
                    object selectedItem = listCategory.SelectedItem;

                    // Remove it from the current position
                    listCategory.Items.RemoveAt(selectedIndex);

                    // Insert it at the new position
                    listCategory.Items.Insert(selectedIndex + 1, selectedItem);

                    // Keep the item selected
                    listCategory.SelectedIndex = selectedIndex + 1;
                }
            }
        }

        private void btnEditCardInfoKey_Click(object sender, EventArgs e)
        {
            if (listCardInfo.SelectedItem is ERACodeSelectCaseSubCaseListItem selectedItem)
            {
                string key = Microsoft.VisualBasic.Interaction.InputBox(
                    "キーを入力してください:", "カード情報の編集",
                    selectedItem.CaseValue.CaseCondition?.TrimStart('"').TrimEnd('"') ?? "");

                if (!string.IsNullOrEmpty(key))
                {
                    int selectedIndex = listCardInfo.SelectedIndex;
                    listCardInfo.Items.RemoveAt(selectedIndex);
                    listCardInfo.Items.Insert(selectedIndex,
                        new ERACodeSelectCaseSubCaseListItem(
                            new ERACodeSelectCaseSubCase($"\"{key}\"", selectedItem.CaseValue.GetValue() ?? "")));
                    listCardInfo.SelectedIndex = selectedIndex;
                }
            }
            else
            {
                MessageBox.Show("編集するカード情報を選択してください。", "警告",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEditCardInfoValue_Click(object sender, EventArgs e)
        {
            if (listCardInfo.SelectedItem is ERACodeSelectCaseSubCaseListItem selectedItem)
            {
                formStringSelection form = new(
                    Models.CardInfoSettings.GetValueListForKey(
                        selectedItem.CaseValue.CaseCondition ?? "") ?? [],
                        selectedItem.CaseValue.GetValue());

                if (form.ShowDialog() == DialogResult.OK)
                {
                    string value = form.ResultString ?? "";
                    int selectedIndex = listCardInfo.SelectedIndex;
                    listCardInfo.Items.RemoveAt(selectedIndex);
                    listCardInfo.Items.Insert(selectedIndex,
                        new ERACodeSelectCaseSubCaseListItem(
                            new ERACodeSelectCaseSubCase(selectedItem.CaseValue.CaseCondition ?? @"", value)));
                    listCardInfo.SelectedIndex = selectedIndex;
                }
            }
            else
            {
                MessageBox.Show("編集するカード情報を選択してください。", "警告",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAddCardInfo_Click(object sender, EventArgs e)
        {
            List<string> keyList = Models.CardInfoSettings.GetKeyList();
            formStringSelection form = new(keyList, "種類");
            if (form.ShowDialog() == DialogResult.OK)
            {
                string key = form.ResultString ?? "";
                if (!string.IsNullOrWhiteSpace(key))
                {
                    // Check if the key already exists in the list
                    bool keyExists = listCardInfo.Items.Cast<ERACodeSelectCaseSubCaseListItem>()
                        .Any(item => (item.CaseValue.CaseCondition ?? "") == key);

                    if (keyExists)
                    {
                        MessageBox.Show($"キー「{key}」は既に存在します。", "警告",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        // 選択したキーに基づいて適切なデフォルト値を取得
                        var valueList = Models.CardInfoSettings.GetValueListForKey(key);
                        string defaultValue = valueList?.FirstOrDefault() ?? "";

                        // 新しいカード情報項目を作成して追加
                        listCardInfo.Items.Add(
                            new ERACodeSelectCaseSubCaseListItem(
                                new ERACodeSelectCaseSubCase($"\"{key}\"", defaultValue)));
                    }
                }
            }
        }

        private void btnDeleteCardInfo_Click(object sender, EventArgs e)
        {
            if (listCardInfo.SelectedIndex != -1)
            {
                if (MessageBox.Show("選択した情報を削除してもよろしいですか？", "情報の削除",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    listCardInfo.Items.RemoveAt(listCardInfo.SelectedIndex);
                }
            }
            else
            {
                MessageBox.Show("削除する情報を選択してください。", "警告",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnMoveUpCardInfo_Click(object sender, EventArgs e)
        {
            if (listCardInfo.SelectedItem != null)
            {
                int selectedIndex = listCardInfo.SelectedIndex;
                if (selectedIndex > 0)
                {
                    // Get the selected item
                    object selectedItem = listCardInfo.SelectedItem;

                    // Remove it from the current position
                    listCardInfo.Items.RemoveAt(selectedIndex);

                    // Insert it at the new position
                    listCardInfo.Items.Insert(selectedIndex - 1, selectedItem);

                    // Keep the item selected
                    listCardInfo.SelectedIndex = selectedIndex - 1;
                }
            }
        }

        private void btnMoveDownCardInfo_Click(object sender, EventArgs e)
        {
            if (listCardInfo.SelectedItem != null)
            {
                int selectedIndex = listCardInfo.SelectedIndex;
                if (selectedIndex < listCardInfo.Items.Count - 1)
                {
                    // Get the selected item
                    object selectedItem = listCardInfo.SelectedItem;

                    // Remove it from the current position
                    listCardInfo.Items.RemoveAt(selectedIndex);

                    // Insert it at the new position
                    listCardInfo.Items.Insert(selectedIndex + 1, selectedItem);

                    // Keep the item selected
                    listCardInfo.SelectedIndex = selectedIndex + 1;
                }
            }
        }

        private void btnQuickSetMonster_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("既存のリストをクリアして、\nモンスターカード用の基本情報を設定しますか？", "確認",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            listCardInfo.Items.Clear();
            listCardInfo.Items.Add(
                new ERACodeSelectCaseSubCaseListItem(
                    new ERACodeSelectCaseSubCase(@"""種類""", "効果モン")));
            listCardInfo.Items.Add(
                new ERACodeSelectCaseSubCaseListItem(
                    new ERACodeSelectCaseSubCase(@"""属性""", "水属性")));
            listCardInfo.Items.Add(
                new ERACodeSelectCaseSubCaseListItem(
                    new ERACodeSelectCaseSubCase(@"""種族""", "植物族")));
            listCardInfo.Items.Add(
                new ERACodeSelectCaseSubCaseListItem(
                    new ERACodeSelectCaseSubCase(@"""レベル""", "6")));
            listCardInfo.Items.Add(
                new ERACodeSelectCaseSubCaseListItem(
                    new ERACodeSelectCaseSubCase(@"""攻撃力""", "1000")));
            listCardInfo.Items.Add(
                new ERACodeSelectCaseSubCaseListItem(
                    new ERACodeSelectCaseSubCase(@"""守備力""", "2400")));
            listCardInfo.Items.Add(
                new ERACodeSelectCaseSubCaseListItem(
                    new ERACodeSelectCaseSubCase(@"""性別""", "牝性")));
        }

        private void btnQuickSetSpellTrap_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("既存のリストをクリアして、\n魔法・罠カード用の基本情報を設定しますか？", "確認",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            listCardInfo.Items.Clear();
            listCardInfo.Items.Add(
                new ERACodeSelectCaseSubCaseListItem(
                    new ERACodeSelectCaseSubCase(@"""種類""", "フィールド")));

        }

        private void listCardInfo_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listCardInfo.SelectedItem != null)
            {
                btnEditCardInfoValue_Click(sender, e);
            }
        }
    }
}