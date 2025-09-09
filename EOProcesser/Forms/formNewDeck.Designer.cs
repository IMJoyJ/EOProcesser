namespace EOProcesser.Forms
{
    partial class formNewDeck
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            numId = new NumericUpDown();
            label1 = new Label();
            label2 = new Label();
            txtDeckName = new TextBox();
            btnConfirm = new Button();
            btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)numId).BeginInit();
            SuspendLayout();
            // 
            // numId
            // 
            numId.Location = new Point(67, 14);
            numId.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numId.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numId.Name = "numId";
            numId.Size = new Size(99, 23);
            numId.TabIndex = 0;
            numId.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 14);
            label1.Name = "label1";
            label1.Size = new Size(18, 15);
            label1.TabIndex = 1;
            label1.Text = "ID";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 48);
            label2.Name = "label2";
            label2.Size = new Size(45, 15);
            label2.TabIndex = 2;
            label2.Text = "デッキ名";
            // 
            // txtDeckName
            // 
            txtDeckName.Location = new Point(67, 48);
            txtDeckName.Name = "txtDeckName";
            txtDeckName.Size = new Size(99, 23);
            txtDeckName.TabIndex = 3;
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(189, 12);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(74, 25);
            btnConfirm.TabIndex = 4;
            btnConfirm.Text = "確定";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(189, 48);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(74, 25);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "破棄";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // formNewDeck
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(275, 84);
            Controls.Add(btnCancel);
            Controls.Add(btnConfirm);
            Controls.Add(txtDeckName);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(numId);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "formNewDeck";
            StartPosition = FormStartPosition.CenterParent;
            Text = "新デッキ";
            ((System.ComponentModel.ISupportInitialize)numId).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NumericUpDown numId;
        private Label label1;
        private Label label2;
        private TextBox txtDeckName;
        private Button btnConfirm;
        private Button btnCancel;
    }
}