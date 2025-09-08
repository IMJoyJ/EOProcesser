namespace EOProcesser
{
    partial class formSettings
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
            txtRootFolder = new TextBox();
            label1 = new Label();
            btnSelectRootFolder = new Button();
            btnConfirm = new Button();
            btnCancel = new Button();
            btnSelectCardFolder = new Button();
            label2 = new Label();
            txtCardFolder = new TextBox();
            btnSelectDeckFolder = new Button();
            label3 = new Label();
            txtDeckFolder = new TextBox();
            SuspendLayout();
            // 
            // txtRootFolder
            // 
            txtRootFolder.Location = new Point(82, 4);
            txtRootFolder.Margin = new Padding(1, 2, 1, 2);
            txtRootFolder.Name = "txtRootFolder";
            txtRootFolder.Size = new Size(308, 23);
            txtRootFolder.TabIndex = 0;
            txtRootFolder.TextChanged += txtRootFolder_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 6);
            label1.Margin = new Padding(1, 0, 1, 0);
            label1.Name = "label1";
            label1.Size = new Size(66, 15);
            label1.TabIndex = 1;
            label1.Text = "Root folder";
            // 
            // btnSelectRootFolder
            // 
            btnSelectRootFolder.Location = new Point(391, 2);
            btnSelectRootFolder.Margin = new Padding(1, 2, 1, 2);
            btnSelectRootFolder.Name = "btnSelectRootFolder";
            btnSelectRootFolder.Size = new Size(27, 22);
            btnSelectRootFolder.TabIndex = 2;
            btnSelectRootFolder.Text = "...";
            btnSelectRootFolder.UseVisualStyleBackColor = true;
            btnSelectRootFolder.Click += btnSelectRootFolder_Click;
            // 
            // btnConfirm
            // 
            btnConfirm.DialogResult = DialogResult.OK;
            btnConfirm.Location = new Point(349, 84);
            btnConfirm.Margin = new Padding(1, 2, 1, 2);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(33, 22);
            btnConfirm.TabIndex = 3;
            btnConfirm.Text = "⚪";
            btnConfirm.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(385, 84);
            btnCancel.Margin = new Padding(1, 2, 1, 2);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(33, 22);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "×";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSelectCardFolder
            // 
            btnSelectCardFolder.Location = new Point(391, 29);
            btnSelectCardFolder.Margin = new Padding(1, 2, 1, 2);
            btnSelectCardFolder.Name = "btnSelectCardFolder";
            btnSelectCardFolder.Size = new Size(27, 22);
            btnSelectCardFolder.TabIndex = 7;
            btnSelectCardFolder.Text = "...";
            btnSelectCardFolder.UseVisualStyleBackColor = true;
            btnSelectCardFolder.Click += btnSelectCardFolder_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(10, 29);
            label2.Margin = new Padding(1, 0, 1, 0);
            label2.Name = "label2";
            label2.Size = new Size(65, 15);
            label2.TabIndex = 6;
            label2.Text = "Card folder";
            // 
            // txtCardFolder
            // 
            txtCardFolder.Location = new Point(82, 28);
            txtCardFolder.Margin = new Padding(1, 2, 1, 2);
            txtCardFolder.Name = "txtCardFolder";
            txtCardFolder.Size = new Size(308, 23);
            txtCardFolder.TabIndex = 5;
            txtCardFolder.TextChanged += txtCardFolder_TextChanged;
            // 
            // btnSelectDeckFolder
            // 
            btnSelectDeckFolder.Location = new Point(391, 56);
            btnSelectDeckFolder.Margin = new Padding(1, 2, 1, 2);
            btnSelectDeckFolder.Name = "btnSelectDeckFolder";
            btnSelectDeckFolder.Size = new Size(27, 22);
            btnSelectDeckFolder.TabIndex = 10;
            btnSelectDeckFolder.Text = "...";
            btnSelectDeckFolder.UseVisualStyleBackColor = true;
            btnSelectDeckFolder.Click += btnSelectDeckFolder_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(10, 56);
            label3.Margin = new Padding(1, 0, 1, 0);
            label3.Name = "label3";
            label3.Size = new Size(67, 15);
            label3.TabIndex = 9;
            label3.Text = "Deck folder";
            // 
            // txtDeckFolder
            // 
            txtDeckFolder.Location = new Point(82, 55);
            txtDeckFolder.Margin = new Padding(1, 2, 1, 2);
            txtDeckFolder.Name = "txtDeckFolder";
            txtDeckFolder.Size = new Size(308, 23);
            txtDeckFolder.TabIndex = 8;
            txtDeckFolder.TextChanged += txtDeckFolder_TextChanged;
            // 
            // formSettings
            // 
            AcceptButton = btnConfirm;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(423, 117);
            Controls.Add(btnSelectDeckFolder);
            Controls.Add(label3);
            Controls.Add(txtDeckFolder);
            Controls.Add(btnSelectCardFolder);
            Controls.Add(label2);
            Controls.Add(txtCardFolder);
            Controls.Add(btnCancel);
            Controls.Add(btnConfirm);
            Controls.Add(btnSelectRootFolder);
            Controls.Add(label1);
            Controls.Add(txtRootFolder);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "formSettings";
            Text = "Settings";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtRootFolder;
        private Label label1;
        private Button btnSelectRootFolder;
        private Button btnConfirm;
        private Button btnCancel;
        private Button btnSelectCardFolder;
        private Label label2;
        private TextBox txtCardFolder;
        private Button btnSelectDeckFolder;
        private Label label3;
        private TextBox txtDeckFolder;
    }
}