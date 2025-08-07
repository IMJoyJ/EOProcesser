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
            button1 = new Button();
            label2 = new Label();
            txtCardFolder = new TextBox();
            SuspendLayout();
            // 
            // txtRootFolder
            // 
            txtRootFolder.Location = new Point(94, 6);
            txtRootFolder.Margin = new Padding(1, 2, 1, 2);
            txtRootFolder.Name = "txtRootFolder";
            txtRootFolder.Size = new Size(352, 27);
            txtRootFolder.TabIndex = 0;
            txtRootFolder.TextChanged += txtRootFolder_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 8);
            label1.Margin = new Padding(1, 0, 1, 0);
            label1.Name = "label1";
            label1.Size = new Size(85, 20);
            label1.TabIndex = 1;
            label1.Text = "Root folder";
            // 
            // btnSelectRootFolder
            // 
            btnSelectRootFolder.Location = new Point(447, 3);
            btnSelectRootFolder.Margin = new Padding(1, 2, 1, 2);
            btnSelectRootFolder.Name = "btnSelectRootFolder";
            btnSelectRootFolder.Size = new Size(31, 30);
            btnSelectRootFolder.TabIndex = 2;
            btnSelectRootFolder.Text = "...";
            btnSelectRootFolder.UseVisualStyleBackColor = true;
            btnSelectRootFolder.Click += btnSelectRootFolder_Click;
            // 
            // btnConfirm
            // 
            btnConfirm.DialogResult = DialogResult.OK;
            btnConfirm.Location = new Point(399, 75);
            btnConfirm.Margin = new Padding(1, 2, 1, 2);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(38, 30);
            btnConfirm.TabIndex = 3;
            btnConfirm.Text = "√";
            btnConfirm.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(440, 75);
            btnCancel.Margin = new Padding(1, 2, 1, 2);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(38, 30);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "×";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(447, 34);
            button1.Margin = new Padding(1, 2, 1, 2);
            button1.Name = "button1";
            button1.Size = new Size(31, 30);
            button1.TabIndex = 7;
            button1.Text = "...";
            button1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 39);
            label2.Margin = new Padding(1, 0, 1, 0);
            label2.Name = "label2";
            label2.Size = new Size(84, 20);
            label2.TabIndex = 6;
            label2.Text = "Card folder";
            // 
            // txtCardFolder
            // 
            txtCardFolder.Location = new Point(94, 37);
            txtCardFolder.Margin = new Padding(1, 2, 1, 2);
            txtCardFolder.Name = "txtCardFolder";
            txtCardFolder.Size = new Size(352, 27);
            txtCardFolder.TabIndex = 5;
            txtCardFolder.TextChanged += txtCardFolder_TextChanged;
            // 
            // formSettings
            // 
            AcceptButton = btnConfirm;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(483, 116);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(txtCardFolder);
            Controls.Add(btnCancel);
            Controls.Add(btnConfirm);
            Controls.Add(btnSelectRootFolder);
            Controls.Add(label1);
            Controls.Add(txtRootFolder);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
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
        private Button button1;
        private Label label2;
        private TextBox txtCardFolder;
    }
}