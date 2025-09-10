using EOProcesser.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EOProcesser.DeckEditor;

namespace EOProcesser.UserControls
{
    public partial class SingleDeckEditor : UserControl
    {
        public SingleDeckEditor()
        {
            InitializeComponent();
            InitDragDrop();
        }

        List<EOCard> CardList = [];
        public void InitCardDic(Dictionary<int, string> cardDic)
        {
            CardList = [.. cardDic.Select(x => new EOCard(x.Key, x.Value))];
            listCardDictionary.DataSource = CardList;
        }
        public DeckEditorDeck? Deck = null;
        public void UnloadDeck()
        {
            ClearDeckInfo();
            Deck = null;
        }
        public void LoadDeck(DeckEditorDeck deck)
        {
            ClearDeckInfo();
            Deck = deck;
            numDeckId.Value = deck.DeckId;
            txtDeckName.Text = deck.DeckName;
            foreach (var cardId in deck.MainDeckContent)
            {
                var card = CardList.FirstOrDefault(x => x.CardId == cardId);
                listMainDeck.Items.Add(card ?? new EOCard(cardId, "（不明なカード）"));
            }
            foreach (var cardId in deck.ExtraDeckContent)
            {
                var card = CardList.FirstOrDefault(x => x.CardId == cardId);
                listExtraDeck.Items.Add(card ?? new EOCard(cardId, "（不明なカード）"));
            }
            RefreshDeckCount();
        }

        private void ClearDeckInfo()
        {
            listMainDeck.Items.Clear();
            listExtraDeck.Items.Clear();
            txtDeckName.Text = "";
            RefreshDeckCount();
        }

        private const string DragFormat = "EOProcesser.EOCardDrag";

        // 拖拽启动相关
        private Point _mouseDownPoint;
        private bool _mouseDown;
        private bool _isDragging;
        private ListBox? _dragSourceListBox;

        // 拖拽目标插入索引
        private int _pendingInsertIndex = -1;
        private ListBox? _pendingTargetListBox = null;

        public class DragEOCardData
        {
            public EOCard? Card { get; set; }
            public ListBox? SourceListBox { get; set; }
            public int SourceIndex { get; set; }
            public bool FromDictionary { get; set; }
        }

        private void InitDragDrop()
        {
            listCardDictionary.AllowDrop = true;
            listMainDeck.AllowDrop = true;
            listExtraDeck.AllowDrop = true;

            // 绑定鼠标事件用于启动拖拽
#pragma warning disable CS8622 // 参数类型中引用类型的为 Null 性与目标委托不匹配(可能是由于为 Null 性特性)。
            listCardDictionary.MouseDown += ListBox_MouseDown;
            listMainDeck.MouseDown += ListBox_MouseDown;
            listExtraDeck.MouseDown += ListBox_MouseDown;

            listCardDictionary.MouseMove += ListBox_MouseMove;
            listMainDeck.MouseMove += ListBox_MouseMove;
            listExtraDeck.MouseMove += ListBox_MouseMove;

            listCardDictionary.MouseUp += ListBox_MouseUp;
            listMainDeck.MouseUp += ListBox_MouseUp;
            listExtraDeck.MouseUp += ListBox_MouseUp;

            // 拖放事件
            listCardDictionary.DragEnter += ListBox_DragEnter;
            listMainDeck.DragEnter += ListBox_DragEnter;
            listExtraDeck.DragEnter += ListBox_DragEnter;

            listCardDictionary.DragOver += ListBox_DragOver;
            listMainDeck.DragOver += ListBox_DragOver;
            listExtraDeck.DragOver += ListBox_DragOver;

            listCardDictionary.DragDrop += ListBox_DragDrop;
            listMainDeck.DragDrop += ListBox_DragDrop;
            listExtraDeck.DragDrop += ListBox_DragDrop;

            listCardDictionary.DragLeave += ListBox_DragLeave;
            listMainDeck.DragLeave += ListBox_DragLeave;
            listExtraDeck.DragLeave += ListBox_DragLeave;
#pragma warning restore CS8622 // 参数类型中引用类型的为 Null 性与目标委托不匹配(可能是由于为 Null 性特性)。
        }

        private void ListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            var lb = (ListBox)sender;
            int index = lb.IndexFromPoint(e.Location);
            if (index == ListBox.NoMatches) return;

            // 设置选中项（确保拖拽项被选中）
            lb.SelectedIndex = index;

            _mouseDown = true;
            _mouseDownPoint = e.Location;
            _dragSourceListBox = lb;
            _isDragging = false;
        }

        private void ListBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_mouseDown || _isDragging) return;
            if (_dragSourceListBox == null) return;
            if (_dragSourceListBox.SelectedIndex < 0) return;

            // 判断是否超过拖拽阈值
            var dx = Math.Abs(e.X - _mouseDownPoint.X);
            var dy = Math.Abs(e.Y - _mouseDownPoint.Y);
            // 你可以用 SystemInformation.DragSize，也可以用简单阈值
            if (dx < SystemInformation.DragSize.Width / 2 &&
                dy < SystemInformation.DragSize.Height / 2)
            {
                return;
            }

            // 启动拖拽
            StartDragFromListBox(_dragSourceListBox);
        }

        private void ListBox_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
            _dragSourceListBox = null;
        }

        private void StartDragFromListBox(ListBox lb)
        {
            if (_isDragging) return;
            if (lb.SelectedItem is not EOCard card) return;

            _isDragging = true;

            var data = new DragEOCardData
            {
                Card = card,
                SourceListBox = lb,
                SourceIndex = lb.SelectedIndex,
                FromDictionary = (lb == listCardDictionary)
            };
            var dataObj = new DataObject();
            dataObj.SetData(DragFormat, data);

            // 允许 Copy / Move，实际结果由 DragEnter/Over 决定
            var allowed = DragDropEffects.Copy | DragDropEffects.Move;
            lb.DoDragDrop(dataObj, allowed);

            // 拖拽结束清理状态
            _isDragging = false;
            _mouseDown = false;
            _dragSourceListBox = null;
        }

        private void ListBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DragFormat))
            {
                var payload = (DragEOCardData?)e.Data.GetData(DragFormat);
                var target = (ListBox)sender;
                e.Effect = DecideEffect(payload, target);
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void ListBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data == null || !e.Data.GetDataPresent(DragFormat))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            var payload = (DragEOCardData?)e.Data.GetData(DragFormat);
            var target = (ListBox)sender;
            var effect = DecideEffect(payload, target);
            e.Effect = effect;

            if (effect == DragDropEffects.None || target == listCardDictionary)
            {
                ClearPendingInsert();
                return;
            }

            Point clientPoint = target.PointToClient(new Point(e.X, e.Y));
            int insertIndex = ComputeInsertIndex(target, clientPoint);

            _pendingInsertIndex = insertIndex;
            _pendingTargetListBox = target;
        }

        private void ListBox_DragLeave(object sender, EventArgs e)
        {
            ClearPendingInsert();
        }

        private void ListBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data == null || !e.Data.GetDataPresent(DragFormat)) return;

            var payload = (DragEOCardData?)e.Data.GetData(DragFormat);
            var target = (ListBox)sender;

            if (payload == null || payload.SourceListBox == null || payload.Card == null)
            {
                return;
            }

            var effect = DecideEffect(payload, target);
            if (effect == DragDropEffects.None) { ClearPendingInsert(); return; }

            bool sameList = payload.SourceListBox == target;

            // 拖到字典 => 删除
            if (target == listCardDictionary && !payload.FromDictionary)
            {
                RemoveFromModelAndUI(payload.SourceListBox, payload.SourceIndex);
                ClearPendingInsert();
                return;
            }

            int insertIndex;
            if (_pendingTargetListBox == target && _pendingInsertIndex >= 0)
                insertIndex = _pendingInsertIndex;
            else
            {
                Point clientPoint = target.PointToClient(new Point(e.X, e.Y));
                insertIndex = ComputeInsertIndex(target, clientPoint);
            }

            if (effect == DragDropEffects.Move && !payload.FromDictionary)
            {
                if (sameList)
                {
                    int oldIndex = payload.SourceIndex;
                    if (insertIndex > oldIndex) insertIndex--;
                    RemoveFromModelAndUI(payload.SourceListBox, oldIndex);
                }
                else
                {
                    RemoveFromModelAndUI(payload.SourceListBox, payload.SourceIndex);
                }
            }

            EOCard cardToAdd = payload.FromDictionary ? new EOCard(payload.Card) : payload.Card;

            InsertIntoModelAndUI(target, insertIndex, cardToAdd);

            ClearPendingInsert();

            RefreshDeckCount();
        }

        private DragDropEffects DecideEffect(DragEOCardData? payload, ListBox target)
        {
            if (payload == null || payload.FromDictionary && target == payload.SourceListBox)
                return DragDropEffects.None;

            if (target == listCardDictionary && !payload.FromDictionary)
                return DragDropEffects.Move;

            if (payload.FromDictionary && (target == listMainDeck || target == listExtraDeck))
                return DragDropEffects.Copy;

            if (!payload.FromDictionary && (target == listMainDeck || target == listExtraDeck))
                return DragDropEffects.Move;

            return DragDropEffects.None;
        }

        private static int ComputeInsertIndex(ListBox lb, Point clientPoint)
        {
            int index = lb.IndexFromPoint(clientPoint);
            if (index == ListBox.NoMatches)
                return lb.Items.Count;

            Rectangle itemRect = lb.GetItemRectangle(index);
            if (clientPoint.Y > itemRect.Top + itemRect.Height / 2)
                index++;

            if (index < 0) index = 0;
            if (index > lb.Items.Count) index = lb.Items.Count;
            return index;
        }

        private void ClearPendingInsert()
        {
            _pendingInsertIndex = -1;
            _pendingTargetListBox = null;
        }

        private List<int>? GetModelListForListBox(ListBox lb)
        {
            if (lb == listMainDeck) return Deck?.MainDeckContent;
            else if (lb == listExtraDeck) return Deck?.ExtraDeckContent;
            return null;
        }

        private void RemoveFromModelAndUI(ListBox lb, int index)
        {
            if (index < 0 || index >= lb.Items.Count) return;
            var card = (EOCard)lb.Items[index];
            lb.Items.RemoveAt(index);
            var model = GetModelListForListBox(lb);
            if (model != null)
            {
                int modelIndex = model.IndexOf(card.CardId);
                if (modelIndex >= 0)
                {
                    model.RemoveAt(modelIndex);
                }
            }
        }

        private void InsertIntoModelAndUI(ListBox lb, int index, EOCard card)
        {
            if (index < 0) index = 0;
            if (index > lb.Items.Count) index = lb.Items.Count;
            lb.Items.Insert(index, card);

            var model = GetModelListForListBox(lb);
            if (model != null)
            {
                if (index > model.Count) index = model.Count;
                model.Insert(index, card.CardId);
            }
        }

        private void btnSearchCard_Click(object sender, EventArgs e)
        {
            Utils.SearchListBoxWithString<EOCard>(txtSearchCard.Text, listCardDictionary,
                (card, searchPattern) =>
                {
                    return card.CardName.Contains(searchPattern.Trim()) ||
                        card.CardId.ToString() == searchPattern.Trim();
                });
        }

        private void RefreshDeckCount()
        {
            lbMainDeckCount.Text = $"現在枚数：{listMainDeck.Items.Count}";
            lbExtraDeckCount.Text = $"現在枚数：{listExtraDeck.Items.Count}";
        }

        private void btnAddToMainDeck_Click(object sender, EventArgs e)
        {
            if (Deck is not null &&
                listCardDictionary.SelectedItem is EOCard card)
            {
                Deck.MainDeckContent.Add(card.CardId);
                listMainDeck.Items.Add(new EOCard(card.CardId, card.CardName));
                RefreshDeckCount();
            }
        }

        private void btnRemoveFromMainDeck_Click(object sender, EventArgs e)
        {
            if (Deck is not null)
            {
                foreach (EOCard card in listMainDeck.SelectedItems)
                {
                    Deck.MainDeckContent.Remove(card.CardId);
                }

                for (int i = listMainDeck.SelectedItems.Count - 1; i >= 0; i--)
                {
                    listMainDeck.Items.Remove(listMainDeck.SelectedItems[i]!);
                }
                RefreshDeckCount();
            }
        }

        private void btnMinus1ToMainDeck_Click(object sender, EventArgs e)
        {
            if (Deck is not null)
            {
                Deck.MainDeckContent.Add(-1);
                listMainDeck.Items.Add(new EOCard(-1, "（不明なカード）"));
                RefreshDeckCount();
            }
        }

        private void btnAddToExtraDeck_Click(object sender, EventArgs e)
        {
            if (Deck is not null &&
                listCardDictionary.SelectedItem is EOCard card)
            {
                Deck.ExtraDeckContent.Add(card.CardId);
                listExtraDeck.Items.Add(new EOCard(card.CardId, card.CardName));
                RefreshDeckCount();
            }
        }

        private void btnRemoveFromExtraDeck_Click(object sender, EventArgs e)
        {
            if (Deck is not null)
            {
                foreach (EOCard card in listExtraDeck.SelectedItems)
                {
                    Deck.ExtraDeckContent.Remove(card.CardId);
                }

                for (int i = listExtraDeck.SelectedItems.Count - 1; i >= 0; i--)
                {
                    listExtraDeck.Items.Remove(listExtraDeck.SelectedItems[i]!);
                }
                RefreshDeckCount();
            }
        }

        private void btnMinus1ToExtraDeck_Click(object sender, EventArgs e)
        {
            if (Deck is not null)
            {
                Deck.ExtraDeckContent.Add(-1);
                listExtraDeck.Items.Add(new EOCard(-1, "（不明なカード）"));
                RefreshDeckCount();
            }
        }

        private void txtSearchCard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearchCard_Click(sender, e);
            }
        }

        private void listMainDeck_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Left)
            {
                btnRemoveFromMainDeck_Click(sender, e);
            }
        }

        private void listExtraDeck_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Left)
            {
                btnRemoveFromExtraDeck_Click(sender, e);
            }
        }

        private void listMainDeck_DoubleClick(object sender, EventArgs e)
        {
            btnRemoveFromMainDeck_Click(sender, e);
        }

        private void listExtraDeck_DoubleClick(object sender, EventArgs e)
        {
            btnRemoveFromExtraDeck_Click(sender, e);
        }

        private void btnExportToFile_Click(object sender, EventArgs e)
        {
            if (Deck != null)
            {
                SaveFileDialog saveDialog = new()
                {
                    Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                    Title = "デッキ出力先",
                    DefaultExt = "txt"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using StreamWriter writer = new(saveDialog.FileName);
                        // メインデッキのカードIDを書き込む
                        foreach (var cardId in Deck.MainDeckContent)
                        {
                            writer.WriteLine(cardId);
                        }

                        // メインデッキとエクストラデッキの間に2行の空行を挿入
                        writer.WriteLine();
                        writer.WriteLine();

                        // エクストラデッキのカードIDを書き込む
                        foreach (var cardId in Deck.ExtraDeckContent)
                        {
                            writer.WriteLine(cardId);
                        }

                        MessageBox.Show("デッキが正常にエクスポートされました。", "エクスポート完了",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"エクスポート中にエラーが発生しました: {ex.Message}", "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("エクスポートするデッキが選択されていません。", "エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLoadDeckFromFile_Click(object sender, EventArgs e)
        {
            if (Deck != null)
            {
                OpenFileDialog openDialog = new()
                {
                    Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                    Title = "デッキファイル選択"
                };

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string[] lines = File.ReadAllLines(openDialog.FileName);

                        // 空行が2行以上連続する位置を探してMain/Extraの区切りとする
                        int separatorIndex = -1;
                        int emptyLineCount = 0;

                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (string.IsNullOrWhiteSpace(lines[i]))
                            {
                                emptyLineCount++;
                                if (emptyLineCount >= 2)
                                {
                                    separatorIndex = i - 1;
                                    break;
                                }
                            }
                            else
                            {
                                emptyLineCount = 0;
                            }
                        }

                        // ユーザーに現在のデッキをクリアするか確認
                        DialogResult dResult = MessageBox.Show(
                            "現在のデッキをクリアしますか？\n「いいえ」を選択すると、カードが追加されます。",
                            "デッキ読み込み",
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Question);

                        switch (dResult)
                        {
                            case DialogResult.Yes:
                                Deck.MainDeckContent.Clear();
                                Deck.ExtraDeckContent.Clear();
                                listMainDeck.Items.Clear();
                                listExtraDeck.Items.Clear();
                                break;
                            case DialogResult.Cancel:
                                return;
                        }

                        // メインデッキのカードを読み込む
                        for (int i = 0; i < lines.Length && (separatorIndex == -1 || i < separatorIndex); i++)
                        {
                            string line = lines[i].Trim();
                            if (string.IsNullOrWhiteSpace(line)) continue;

                            if (int.TryParse(line, out int cardId))
                            {
                                Deck.MainDeckContent.Add(cardId);
                                var card = CardList.FirstOrDefault(x => x.CardId == cardId);
                                listMainDeck.Items.Add(card != null ? new EOCard(card) : new EOCard(cardId, "（不明なカード）"));
                            }
                        }

                        // エクストラデッキのカードを読み込む
                        if (separatorIndex != -1)
                        {
                            for (int i = separatorIndex + 2; i < lines.Length; i++) // +2 で空行をスキップ
                            {
                                string line = lines[i].Trim();
                                if (string.IsNullOrWhiteSpace(line)) continue;

                                if (int.TryParse(line, out int cardId))
                                {
                                    Deck.ExtraDeckContent.Add(cardId);
                                    var card = CardList.FirstOrDefault(x => x.CardId == cardId);
                                    listExtraDeck.Items.Add(card != null ? new EOCard(card) : new EOCard(cardId, "（不明なカード）"));
                                }
                            }
                        }

                        RefreshDeckCount();
                        MessageBox.Show("デッキを正常に読み込みました。", "読み込み完了",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"デッキ読み込み中にエラーが発生しました: {ex.Message}", "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("デッキが選択されていません。", "エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void btnCopyCode_Click(object sender, EventArgs e)
        {
            if (Deck != null)
            {
                Clipboard.SetText(Deck.ToFunction().ToString());
            }
        }

        private void txtDeckName_TextChanged(object sender, EventArgs e)
        {
            if (Deck != null)
            {
                Deck.DeckName = txtDeckName.Text;
            }
            NotifyDeckChanged();
        }

        public event EventHandler? OnDeckChanged;

        private void NotifyDeckChanged()
        {
            OnDeckChanged?.Invoke(this, EventArgs.Empty);
        }

        private void numDeckId_ValueChanged(object sender, EventArgs e)
        {
            if (Deck != null)
            {
                Deck.DeckId = Convert.ToInt32(numDeckId.Value);
            }
            NotifyDeckChanged();
        }
    }
}
