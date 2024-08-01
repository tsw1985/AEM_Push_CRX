namespace AEM_Push_CRX
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            appBrowserDialog = new FolderBrowserDialog();
            appFoldertextBox = new TextBox();
            searchFolderButton = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 23);
            label1.Name = "label1";
            label1.Size = new Size(102, 15);
            label1.TabIndex = 0;
            label1.Text = "Select App Folder:";
            // 
            // appFoldertextBox
            // 
            appFoldertextBox.Location = new Point(118, 18);
            appFoldertextBox.Name = "appFoldertextBox";
            appFoldertextBox.ReadOnly = true;
            appFoldertextBox.Size = new Size(542, 23);
            appFoldertextBox.TabIndex = 1;
            // 
            // searchFolderButton
            // 
            searchFolderButton.Location = new Point(664, 18);
            searchFolderButton.Name = "searchFolderButton";
            searchFolderButton.Size = new Size(133, 23);
            searchFolderButton.TabIndex = 2;
            searchFolderButton.Text = "Search Folder";
            searchFolderButton.UseVisualStyleBackColor = true;
            searchFolderButton.Click += searchFolderButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(searchFolderButton);
            Controls.Add(appFoldertextBox);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private FolderBrowserDialog appBrowserDialog;
        private TextBox appFoldertextBox;
        private Button searchFolderButton;
    }
}
