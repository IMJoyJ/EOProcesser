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
            SuspendLayout();
            // 
            // txtRootFolder
            // 
            txtRootFolder.Location = new Point(212, 12);
            txtRootFolder.Margin = new Padding(2, 4, 2, 4);
            txtRootFolder.Name = "txtRootFolder";
            txtRootFolder.Size = new Size(787, 46);
            txtRootFolder.TabIndex = 0;
            txtRootFolder.TextChanged += txtRootFolder_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(27, 16);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(179, 39);
            label1.TabIndex = 1;
            label1.Text = "Root folder";
            // 
            // btnSelectRootFolder
            // 
            btnSelectRootFolder.Location = new Point(1006, 6);
            btnSelectRootFolder.Margin = new Padding(2, 4, 2, 4);
            btnSelectRootFolder.Name = "btnSelectRootFolder";
            btnSelectRootFolder.Size = new Size(70, 58);
            btnSelectRootFolder.TabIndex = 2;
            btnSelectRootFolder.Text = "...";
            btnSelectRootFolder.UseVisualStyleBackColor = true;
            btnSelectRootFolder.Click += btnSelectRootFolder_Click;
            // 
            // btnConfirm
            // 
            btnConfirm.DialogResult = DialogResult.OK;
            btnConfirm.Location = new Point(898, 146);
            btnConfirm.Margin = new Padding(2, 4, 2, 4);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(86, 58);
            btnConfirm.TabIndex = 3;
            btnConfirm.Text = "√";
            btnConfirm.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(990, 146);
            btnCancel.Margin = new Padding(2, 4, 2, 4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(86, 58);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "×";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSelectCardFolder
            // 
            btnSelectCardFolder.Location = new Point(1006, 66);
            btnSelectCardFolder.Margin = new Padding(2, 4, 2, 4);
            btnSelectCardFolder.Name = "btnSelectCardFolder";
            btnSelectCardFolder.Size = new Size(70, 58);
            btnSelectCardFolder.TabIndex = 7;
            btnSelectCardFolder.Text = "...";
            btnSelectCardFolder.UseVisualStyleBackColor = true;
            btnSelectCardFolder.Click += btnSelectCardFolder_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(27, 76);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(177, 39);
            label2.TabIndex = 6;
            label2.Text = "Card folder";
            // 
            // txtCardFolder
            // 
            txtCardFolder.Location = new Point(212, 72);
            txtCardFolder.Margin = new Padding(2, 4, 2, 4);
            txtCardFolder.Name = "txtCardFolder";
            txtCardFolder.Size = new Size(787, 46);
            txtCardFolder.TabIndex = 5;
            txtCardFolder.TextChanged += txtCardFolder_TextChanged;
            // 
            // formSettings
            // 
            AcceptButton = btnConfirm;
            AutoScaleDimensions = new SizeF(18F, 39F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(1087, 226);
            Controls.Add(btnSelectCardFolder);
            Controls.Add(label2);
            Controls.Add(txtCardFolder);
            Controls.Add(btnCancel);
            Controls.Add(btnConfirm);
            Controls.Add(btnSelectRootFolder);
            Controls.Add(label1);
            Controls.Add(txtRootFolder);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(7, 6, 7, 6);
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
    }
}