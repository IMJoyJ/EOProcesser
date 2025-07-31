using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EOProcesser
{
    public partial class formSettings : Form
    {
        public ERAOCGCardManagerSettings Settings;
        public formSettings(ERAOCGCardManagerSettings settings)
        {
            Settings = settings;
            InitializeComponent();
        }

        private void btnSelectRootFolder_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "Select Root Folder";
            folderDialog.ShowNewFolderButton = true;

            // If there's already a root folder set, start from there
            if (!string.IsNullOrEmpty(Settings.RootFolder) && Directory.Exists(Settings.RootFolder))
            {
                folderDialog.SelectedPath = Settings.RootFolder;
            }

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                Settings.RootFolder = folderDialog.SelectedPath;
                txtRootFolder.Text = Settings.RootFolder;
            }
        }
    }
}
