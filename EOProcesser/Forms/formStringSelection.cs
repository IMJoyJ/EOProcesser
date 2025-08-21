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
    public partial class formStringSelection : Form
    {
        public formStringSelection(IEnumerable<object> selection, string? defaultValue = null)
        {
            InitializeComponent();
            comboSelection.Items.AddRange([.. selection.Select((obj) => obj.ToString())]);
            comboSelection.SelectedValue = defaultValue;
        }
        public string? ResultString = null;

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            ResultString = comboSelection.SelectedItem as string;
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
