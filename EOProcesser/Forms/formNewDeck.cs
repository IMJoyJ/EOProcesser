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

namespace EOProcesser.Forms
{
    public partial class formNewDeck : Form
    {
        public DeckEditorDeck? Result = null;
        public formNewDeck()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDeckName.Text))
            {
                MessageBox.Show("無効なデッキ名です。");
                return;
            }
            Result = new DeckEditorDeck(Convert.ToInt32(numId.Value), txtDeckName.Text.Trim());
            DialogResult = DialogResult.OK;
        }
    }
}
