namespace EOProcesser.Forms
{
    partial class formStringSelection
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
            comboSelection = new ComboBox();
            btnConfirm = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // comboSelection
            // 
            comboSelection.FormattingEnabled = true;
            comboSelection.Location = new Point(12, 12);
            comboSelection.Name = "comboSelection";
            comboSelection.Size = new Size(232, 28);
            comboSelection.TabIndex = 0;
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(250, 11);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(42, 29);
            btnConfirm.TabIndex = 1;
            btnConfirm.Text = "⚪";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(298, 12);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(42, 29);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "×";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // formStringSelection
            // 
            AcceptButton = btnConfirm;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(354, 61);
            Controls.Add(btnCancel);
            Controls.Add(btnConfirm);
            Controls.Add(comboSelection);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "formStringSelection";
            StartPosition = FormStartPosition.CenterParent;
            Text = "選択";
            ResumeLayout(false);
        }

        #endregion

        private ComboBox comboSelection;
        private Button btnConfirm;
        private Button btnCancel;
    }
}