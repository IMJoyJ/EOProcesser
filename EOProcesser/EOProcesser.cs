using System.Diagnostics;
using System.Text.Json;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

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
                    tvCode.Nodes.Clear();
                    var codes = ERACodeAnalyzer.AnalyzeCode(File.ReadAllLines(str));
                    foreach (var code in codes)
                    {
                        tvCode.Nodes.AddRange([.. code.GetTreeNodes()]);
                    }
                    txtCode.Text = codes.ToString();
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

        private void treeCards_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is ERAOCGCardScript script)
            {
                if (MessageBox.Show("保存されてないデータが失われる。\n確認しますか？", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    listCardScriptCard.Items.Clear();
                    foreach (var card in script.Cards)
                    {
                        listCardScriptCard.Items.Add(card);
                    }
                }
            }
            else if (e.Node.Tag is ERAOCGCard card)
            {
                if (MessageBox.Show("カード所属のスクリプトを開きます。\n保存されてないデータが失われる。\n確認しますか？", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    listCardScriptCard.Items.Clear();
                    foreach (var c in card.CardScript.Cards)
                    {
                        listCardScriptCard.Items.Add(c);
                    }
                }
            }
        }
        private void listCardScriptCard_MouseDoubleClick(object sender, MouseEventArgs e)
        {
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
                        foreach(ERACodeSelectCaseSubCase caseVal in sel.Cast<ERACodeSelectCaseSubCase>())
                        {
                            listCardInfo.Items.Add(caseVal);
                        }
                    }
                }
            }
        }
    }
}