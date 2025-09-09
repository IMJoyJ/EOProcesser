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
using EOProcesser.Settings;

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
            txtDeckFolder.Text = Settings.DeckFolder;
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
                txtRootFolder.Text = folderDialog.SelectedPath;
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
                txtCardFolder.Text = folderDialog.SelectedPath;
            }
        }

        private void btnSelectDeckFolder_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog folderDialog = new();
            folderDialog.Description = "Select Deck Folder";
            folderDialog.ShowNewFolderButton = true;

            // If there's already a root folder set, start from there
            if (!string.IsNullOrEmpty(Settings.DeckFolder) && Directory.Exists(Settings.DeckFolder))
            {
                folderDialog.SelectedPath = Settings.DeckFolder;
            }

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                txtDeckFolder.Text = folderDialog.SelectedPath;
            }
        }

        private void txtDeckFolder_TextChanged(object sender, EventArgs e)
        {
            Settings.DeckFolder = txtDeckFolder.Text;
        }
        private void btnQuickSetting_Click(object sender, EventArgs e)
        {
            using OpenFileDialog fileDialog = new();
            fileDialog.Title = "ゲーム実行ファイルを選択してください";
            fileDialog.Filter = "実行ファイル (*.exe)|*.exe";
            fileDialog.CheckFileExists = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // ゲーム実行ファイルのディレクトリを取得
                    string gameDirectory = Path.GetDirectoryName(fileDialog.FileName) ?? "";
                    
                    if (string.IsNullOrEmpty(gameDirectory))
                    {
                        MessageBox.Show("ゲームディレクトリを取得できませんでした。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // 各フォルダのパスを設定
                    string rootFolder = Path.Combine(gameDirectory, "ERB");
                    string cardFolder = Path.Combine(gameDirectory, "ERB", "10.デュエル関連", "02.CARDS");
                    string deckFolder = Path.Combine(gameDirectory, "ERB", "10.デュエル関連", "03.デッキ編集", "デッキリスト");

                    // フォルダの存在確認
                    bool rootExists = Directory.Exists(rootFolder);
                    bool cardExists = Directory.Exists(cardFolder);
                    bool deckExists = Directory.Exists(deckFolder);

                    // 結果メッセージを作成
                    StringBuilder message = new();
                    message.AppendLine("以下のフォルダを設定します：");
                    message.AppendLine($"Root Folder: {rootFolder} {(rootExists ? "（存在）" : "（存在しません）")}");
                    message.AppendLine($"Card Folder: {cardFolder} {(cardExists ? "（存在）" : "（存在しません）")}");
                    message.AppendLine($"Deck Folder: {deckFolder} {(deckExists ? "（存在）" : "（存在しません）")}");
                    message.AppendLine();
                    message.AppendLine("続行しますか？");

                    // 確認ダイアログを表示
                    DialogResult result = MessageBox.Show(
                        message.ToString(),
                        "フォルダ設定の確認",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        // 設定を更新
                        txtRootFolder.Text = rootFolder;
                        txtCardFolder.Text = cardFolder;
                        txtDeckFolder.Text = deckFolder;
                        
                        MessageBox.Show("フォルダ設定が完了しました。", "設定完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"エラーが発生しました：{ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
