using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EOProcesser
{
    public partial class formSettings : Form
    {
        public ERAOCGCardManagerSettings Settings;
        public formSettings(ERAOCGCardManagerSettings settings)
        {
            Settings = new ERAOCGCardManagerSettings();
            // Create a deep copy of the settings using JSON serialization/deserialization
            if (settings != null)
            {
                string json = JsonSerializer.Serialize(settings);
                Settings = JsonSerializer.Deserialize<ERAOCGCardManagerSettings>(json) ?? Settings;
            }
            InitializeComponent();
            txtRootFolder.Text = Settings.RootFolder;
            txtCardFolder.Text = Settings.CardFolder;
        }

        private void btnSelectRootFolder_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog folderDialog = new();
            folderDialog.Description = "Select Root Folder";
            folderDialog.ShowNewFolderButton = true;

            // If there's already a root folder set, start from there
            if (!string.IsNullOrEmpty(Settings.RootFolder) && Directory.Exists(Settings.RootFolder))
            {
                folderDialog.SelectedPath = Settings.RootFolder;
            }

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                txtRootFolder.Text = Settings.RootFolder;
            }
        }

        private void txtRootFolder_TextChanged(object sender, EventArgs e)
        {
            Settings.RootFolder = txtRootFolder.Text;
        }

        private void txtCardFolder_TextChanged(object sender, EventArgs e)
        {
            Settings.CardFolder = txtCardFolder.Text;
        }

        private void btnSelectCardFolder_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog folderDialog = new();
            folderDialog.Description = "Select Card Folder";
            folderDialog.ShowNewFolderButton = true;

            // If there's already a root folder set, start from there
            if (!string.IsNullOrEmpty(Settings.CardFolder) && Directory.Exists(Settings.CardFolder))
            {
                folderDialog.SelectedPath = Settings.CardFolder;
            }

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                txtCardFolder.Text = Settings.CardFolder;
            }
        }
    }
}
