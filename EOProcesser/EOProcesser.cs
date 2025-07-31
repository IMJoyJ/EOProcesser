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

        private void EOProcesser_Load(object sender, EventArgs e)
        {
            const string folder = @"D:\Git\ELauncher\ERB\10.ÉfÉÖÉGÉãä÷òA\02.CARDS";
            // Create a root node for the folder
            TreeNode rootNode = new TreeNode(Path.GetFileName(folder));
            rootNode.Tag = folder;
            tvFolderFiles.Nodes.Add(rootNode);

            // Populate the TreeView with .erb files
            PopulateTreeView(folder, rootNode);

            // Expand the root node
            rootNode.Expand();
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

        private void RefreshCodeView(string file)
        {

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
                    foreach(var code in codes)
                    {
                        tvCode.Nodes.AddRange([.. code.GetTreeNodes()]);
                    }
                }
                catch
#if DEBUG
#pragma warning disable CS8360 // ??äÌï\?éÆê•èÌó  ÅgfalseÅhÅB
                when (false)
#pragma warning restore CS8360 // ??äÌï\?éÆê•èÌó  ÅgfalseÅhÅB
#endif
                { }
            }
        }
    }
}
