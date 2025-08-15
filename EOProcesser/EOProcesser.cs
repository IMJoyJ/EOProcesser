using System.Diagnostics;
using System.Text.Json;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using EOProcesser.Settings;

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
            catch (Exception ex)
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

                    var nodes = script.GetTreeNode();
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
            catch (Exception ex)
            {
                // 捕获任何其他异常
            }
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
            if (e.Button == MouseButtons.Right && e.Node.Tag is string filePath)
            {
                // Set the event handler for the Open menu item
                openToolStripMenuItem.Click -= OpenFile_Click;
                openToolStripMenuItem.Click += OpenFile_Click;
                openToolStripMenuItem.Tag = filePath;

                // Show the context menu
                CodeViewMenuStrip.Show(tvFolderFiles, e.Location);
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
        }
        string? CurrentFile = null;
        private void treeCards_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is ERAOCGCardScript script)
            {
                if (MessageBox.Show("保存されてないデータが失われる。\n確認しますか？", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ClearAll();
                    listCardScriptCard.Items.Clear();
                    foreach (var card in script.Cards)
                    {
                        listCardScriptCard.Items.Add(card);
                    }
                    CurrentFile = script.ScriptFile;
                }
            }
            else if (e.Node.Tag is ERAOCGCard card)
            {
                if (MessageBox.Show("カード所属のスクリプトを開きます。\n保存されてないデータが失われる。\n確認しますか？", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ClearAll();
                    foreach (var c in card.CardScript.Cards)
                    {
                        listCardScriptCard.Items.Add(c);
                    }
                    CurrentFile = card.CardScript.ScriptFile;
                    tabCardEditPanel.SelectedIndex = 0;
                }
            }
        }

        ERAOCGCard? CurrentCard = null;
        private void listCardScriptCard_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            silent = true;
            if (listCardScriptCard.SelectedItem is ERAOCGCard card)
            {
                if (MessageBox.Show("保存されてないデータが失われる。確認しますか？", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    txtCardName.Text = card.Name;
                    txtShortName.Text = card.ShortName;
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
                if (CurrentFile == null || CurrentCard == null)
                {
                    return;
                }
                treeCardEffectList.Nodes.Clear();
                EOCardManagerCardEffect effect = EOCardManagerCardEffect.Parse(CurrentCard);
                foreach (var eff in effect)
                {
                    treeCardEffectList.Nodes.AddRange(eff.GetTreeNodes());
                }
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
            if (e.Node.Tag == null)
                return;

            if (e.Node.Tag is ERACode code)
            {
                // 如果节点包含ERACode，将其加载到编辑器
                eeCardManagerScriptEditor.LoadCode(code);
            }
            else if (e.Node.Tag is string strValue)
            {
                // 如果是字符串属性（如Condition），提示用户输入
                string propertyName = e.Node.Text;

                // 从节点文本中提取属性名称（例如从"条件: xxx"提取"条件"）
                if (propertyName.Contains(':'))
                {
                    propertyName = propertyName.Substring(0, propertyName.IndexOf(':'));
                }

                // 显示输入对话框
                string prompt = $"请输入新的{propertyName}值:";
                string title = $"修改{propertyName}";

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
            }
            else if (e.Node.Tag is EOCardManagerEffect effect)
            {
                // 如果点击的是效果节点本身，可以编辑效果编号或其他主要属性
                string? newEffectNo = Microsoft.VisualBasic.Interaction.InputBox(
                    "新しい効果計数を入力してください:", "効果計数", effect.EffectNo ?? "");

                if (!string.IsNullOrEmpty(newEffectNo))
                {
                    effect.EffectNo = newEffectNo;
                    e.Node.Text = $"效果{newEffectNo}";
                }
            }
        }

        private void treeCards_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Show context menu on right-click
            if (e.Button == MouseButtons.Right)
            {
                if (e.Node.Tag is ERAOCGCardScript script)
                {
                    // Set the event handler for the Open menu item
                    openToolStripMenuItem.Click -= OpenFile_Click;
                    openToolStripMenuItem.Click += OpenFile_Click;
                    openToolStripMenuItem.Tag = script.ScriptFile;
                }
                else if (e.Node.Tag is ERAOCGCard card)
                {
                    openToolStripMenuItem.Click -= OpenFile_Click;
                    openToolStripMenuItem.Click += OpenFile_Click;
                    openToolStripMenuItem.Tag = card.CardScript.ScriptFile;
                }
                else if (e.Node.Tag is string path)
                {
                    openToolStripMenuItem.Click -= OpenFile_Click;
                    openToolStripMenuItem.Click += OpenFile_Click;
                    openToolStripMenuItem.Tag = path;
                }
                // Show the context menu
                CodeViewMenuStrip.Show(tvFolderFiles, e.Location);
            }
        }
    }
}