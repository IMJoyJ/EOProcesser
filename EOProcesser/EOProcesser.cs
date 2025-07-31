using System.Diagnostics;
using System.Text.Json;

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

        ERAOCGCardManagerSettings settings = new ERAOCGCardManagerSettings();

        private void EOProcesser_Load(object sender, EventArgs e)
        {
            try
            {
                settings = JsonSerializer.Deserialize<ERAOCGCardManagerSettings>(File.ReadAllText("settings.json")) ?? settings;
            }
            catch { }

            LoadFolderTreeView();
        }

        private void LoadFolderTreeView()
        {
            tvFolderFiles.Nodes.Clear();

            if (!string.IsNullOrEmpty(settings.RootFolder))
            {
                // Create a root node for the folder
                TreeNode rootNode = new TreeNode(Path.GetFileName(settings.RootFolder));
                rootNode.Tag = settings.RootFolder;
                tvFolderFiles.Nodes.Add(rootNode);

                // Populate the TreeView with .erb files
                PopulateTreeView(settings.RootFolder, rootNode);

                // Expand the root node
                rootNode.Expand();
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
                    TreeNode fileNode = new TreeNode(fileName);
                    fileNode.Tag = file;
                    parentNode.Nodes.Add(fileNode);
                }

                // Process subdirectories
                foreach (string subDir in Directory.GetDirectories(folderPath))
                {
                    string dirName = Path.GetFileName(subDir);
                    TreeNode dirNode = new TreeNode(dirName);
                    dirNode.Tag = subDir;
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
                    LoadFolderTreeView();
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
    }
}