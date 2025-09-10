using EOProcesser.Forms;
using EOProcesser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EOProcesser
{
    public partial class DeckEditor : UserControl
    {
        public DeckEditor()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                Visible = false;
            }
        }
        public void LoadFolder(string deckFolder, Dictionary<int, string> cardDic)
        {
            Visible = false;
            Utils.InitTreeViewByFolder(deckFolder, tvFolder, (t, isFolder) =>
            {
                if (!isFolder)
                {
                    try
                    {
                        if (t.Tag is not string file || !File.Exists(file))
                        {
                            return;
                        }
                        DeckEditorFile deFile = new(file);
                        t.Tag = deFile;
                        foreach (var deck in deFile)
                        {
                            TreeNode tn = new(deck.ToString())
                            {
                                Tag = deck
                            };
                            t.Nodes.Add(tn);
                        }
                        // 为文件节点添加右键菜单
                        AddNewFileContextMenu(t, Path.GetDirectoryName(file));
                    }
                    catch { }
                }
                else
                {
                    // 为文件夹节点添加右键菜单
                    AddNewFileContextMenu(t, t.Tag as string);
                }
            });
            sdeDeckEditor.InitCardDic(cardDic);
        }

        // 添加创建新文件的右键菜单
        private void AddNewFileContextMenu(TreeNode node, string? folderPath)
        {
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
                return;

            node.ContextMenuStrip = new ContextMenuStrip();
            var menuItem = new ToolStripMenuItem("新規デッキファイル");
            menuItem.Click += (s, e) =>
            {
                string fileName = Microsoft.VisualBasic.Interaction.InputBox("ファイル名を入力してください:", "新規ERBファイル", "");
                if (string.IsNullOrEmpty(fileName))
                    return;

                // 确保文件名有.erb扩展名
                if (!fileName.EndsWith(".erb", StringComparison.OrdinalIgnoreCase))
                    fileName += ".erb";

                string filePath = Path.Combine(folderPath, fileName);

                try
                {
                    // 创建空文件
                    File.WriteAllText(filePath, string.Empty);

                    // 添加新文件到树视图
                    TreeNode newNode = new(fileName);
                    newNode.Tag = new DeckEditorFile(filePath);

                    // 如果是文件夹节点，添加到其子节点
                    if (node.Tag is string)
                    {
                        node.Nodes.Add(newNode);
                        node.Expand();
                    }
                    // 如果是文件节点，添加到同一父节点下
                    else if (node.Parent != null)
                    {
                        node.Parent.Nodes.Add(newNode);
                        node.Parent.Expand();
                    }
                    // 如果没有父节点，直接添加到tvFolder根节点
                    else
                    {
                        tvFolder.Nodes.Add(newNode);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ファイル作成エラー: " + ex.Message, "エラー",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            node.ContextMenuStrip.Items.Add(menuItem);
        }
        public DeckEditor(string deckFolder, Dictionary<int, string> cardFileDic) : base()
        {
            LoadFolder(deckFolder, cardFileDic);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Utils.SearchTreeViewWithString(txtSearchFile.Text, tvFolder);
        }

        DeckEditorFile? CurrentDeckEditorFile = null;
        DeckEditorDeck? CurrentDeckEditorDeck = null;
        TreeNode? CurrentTreeNode = null;

        DeckEditorDeck? openDeck = null;
        private void tvFolder_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (openDeck == null && CurrentDeckEditorFile != null)
            {
                if (MessageBox.Show("現在開いているファイルを破棄しますか？",
                    "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
            }
            if (tvFolder.SelectedNode?.Tag is DeckEditorFile def)
            {
                CurrentTreeNode = tvFolder.SelectedNode;
                CurrentDeckEditorFile = def;
                CurrentDeckEditorDeck = null;
                listDeckList.Items.Clear();
                foreach (var deck in def)
                {
                    listDeckList.Items.Add(deck);
                }
                txtExtraFunctions.Text = def.GetExtraFunctionContent();
                if (openDeck != null)
                {
                    var tmp = openDeck;
                    openDeck = null;
                    if (listDeckList.Items.Contains(tmp))
                    {
                        listDeckList.SelectedItem = tmp;
                        listDeckList_DoubleClick(sender, e);
                    }
                }
            }
            else if (tvFolder.SelectedNode?.Tag is DeckEditorDeck deck)
            {
                if (tvFolder.SelectedNode?.Parent?.Tag is DeckEditorFile file)
                {
                    openDeck = deck;
                    tvFolder.SelectedNode = tvFolder.SelectedNode.Parent;
                    tvFolder_NodeMouseDoubleClick(sender, e);
                }
            }
        }

        private void listDeckList_DoubleClick(object sender, EventArgs e)
        {
            if (listDeckList.SelectedItem is DeckEditorDeck deck)
            {
                CurrentDeckEditorDeck = deck;
                sdeDeckEditor.LoadDeck(deck);
                tabDeckEditTabPage.SelectedIndex = 2;
            }
        }

        private void txtSearchFile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }

        private void btnAddDeck_Click(object sender, EventArgs e)
        {
            if (CurrentDeckEditorFile != null && CurrentTreeNode != null)
            {
                formNewDeck form = new();
                if (new formNewDeck().ShowDialog() == DialogResult.OK
                    && form.Result is DeckEditorDeck deck)
                {
                    CurrentDeckEditorFile.DeckEditorDecks.Add(deck);
                    listDeckList.Items.Add(deck);
                    TreeNode node = new($"({deck.DeckId}):{deck.DeckName}")
                    {
                        Tag = deck
                    };
                    CurrentTreeNode.Nodes.Add(node);
                }
            }
        }

        private void btnDeleteDeck_Click(object sender, EventArgs e)
        {
            if (CurrentDeckEditorFile != null && CurrentTreeNode != null && listDeckList.SelectedItem is DeckEditorDeck deck)
            {
                if (MessageBox.Show("本当にこのデッキを削除しますか？", "確認",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    CurrentDeckEditorFile.DeckEditorDecks.Remove(deck);
                    listDeckList.Items.Remove(deck);
                    for (int i = 0; i < CurrentTreeNode.Nodes.Count; i++)
                    {
                        if (CurrentTreeNode.Nodes[i].Tag == deck)
                        {
                            CurrentTreeNode.Nodes.RemoveAt(i);
                            i--;
                        }
                    }
                    if (CurrentDeckEditorDeck == deck)
                    {
                        sdeDeckEditor.UnloadDeck();
                    }
                }
            }
        }

        private void btnMoveUpDeck_Click(object sender, EventArgs e)
        {
            if (listDeckList.SelectedItem is DeckEditorDeck deck && CurrentDeckEditorFile != null)
            {
                int index = listDeckList.SelectedIndex;
                if (index > 0)
                {
                    // Move the deck up in the file's deck list
                    CurrentDeckEditorFile.DeckEditorDecks.Remove(deck);
                    CurrentDeckEditorFile.DeckEditorDecks.Insert(index - 1, deck);

                    // Update the listbox
                    listDeckList.Items.Remove(deck);
                    listDeckList.Items.Insert(index - 1, deck);
                    listDeckList.SelectedIndex = index - 1;
                }
            }
        }

        private void btnMoveDownDeck_Click(object sender, EventArgs e)
        {
            if (listDeckList.SelectedItem is DeckEditorDeck deck && CurrentDeckEditorFile != null)
            {
                int index = listDeckList.SelectedIndex;
                if (index < listDeckList.Items.Count - 1 && index >= 0)
                {
                    // Move the deck down in the file's deck list
                    CurrentDeckEditorFile.DeckEditorDecks.Remove(deck);
                    CurrentDeckEditorFile.DeckEditorDecks.Insert(index + 1, deck);

                    // Update the listbox
                    listDeckList.Items.Remove(deck);
                    listDeckList.Items.Insert(index + 1, deck);
                    listDeckList.SelectedIndex = index + 1;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CurrentDeckEditorFile != null && CurrentTreeNode != null)
            {
                CurrentDeckEditorFile.ExtraLines.Clear();
                CurrentDeckEditorFile.ExtraLines = [ERACodeAnalyzer.AnalyzeCode(txtExtraFunctions.Text.Replace("\r\n", "\n").Split('\n'))];
                File.WriteAllText(CurrentDeckEditorFile.DeckFile, CurrentDeckEditorFile.ToFileContent());
                CurrentDeckEditorFile = new(CurrentDeckEditorFile.DeckFile);
                CurrentTreeNode.Tag = CurrentDeckEditorFile;
            }
        }

        private void sdeDeckEditor_OnDeckChanged(object sender, EventArgs e)
        {
            if (sdeDeckEditor.Deck == null)
            {
                return;
            }
            int index = -1;
            for (int i = 0; i < listDeckList.Items.Count; i++)
            {
                if (listDeckList.Items[i] == sdeDeckEditor.Deck)
                {
                    index = i;
                    break;
                }
            }
            if (index > 0)
            {
                listDeckList.Items[index] = sdeDeckEditor.Deck;
            }
        }
    }
}
