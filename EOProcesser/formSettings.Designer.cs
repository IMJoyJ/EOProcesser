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
            SuspendLayout();
            // 
            // txtRootFolder
            // 
            txtRootFolder.Location = new Point(212, 12);
            txtRootFolder.Name = "txtRootFolder";
            txtRootFolder.Size = new Size(787, 46);
            txtRootFolder.TabIndex = 0;
            txtRootFolder.TextChanged += txtRootFolder_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(27, 15);
            label1.Name = "label1";
            label1.Size = new Size(179, 39);
            label1.TabIndex = 1;
            label1.Text = "Root folder";
            // 
            // btnSelectRootFolder
            // 
            btnSelectRootFolder.Location = new Point(1005, 5);
            btnSelectRootFolder.Name = "btnSelectRootFolder";
            btnSelectRootFolder.Size = new Size(69, 58);
            btnSelectRootFolder.TabIndex = 2;
            btnSelectRootFolder.Text = "...";
            btnSelectRootFolder.UseVisualStyleBackColor = true;
            btnSelectRootFolder.Click += btnSelectRootFolder_Click;
            // 
            // btnConfirm
            // 
            btnConfirm.DialogResult = DialogResult.OK;
            btnConfirm.Location = new Point(896, 104);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(86, 58);
            btnConfirm.TabIndex = 3;
            btnConfirm.Text = "√";
            btnConfirm.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(988, 104);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(86, 58);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "×";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // formSettings
            // 
            AcceptButton = btnConfirm;
            AutoScaleDimensions = new SizeF(18F, 39F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(1086, 174);
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
    }
}