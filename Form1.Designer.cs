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
            groupBox1 = new GroupBox();
            filesChangedLoggerTextBox = new TextBox();
            groupBox2 = new GroupBox();
            portTextBox = new TextBox();
            label3 = new Label();
            hostTextBox = new TextBox();
            label2 = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 27);
            label1.Name = "label1";
            label1.Size = new Size(56, 15);
            label1.TabIndex = 0;
            label1.Text = "Location:";
            // 
            // appFoldertextBox
            // 
            appFoldertextBox.Location = new Point(65, 22);
            appFoldertextBox.Name = "appFoldertextBox";
            appFoldertextBox.ReadOnly = true;
            appFoldertextBox.Size = new Size(590, 23);
            appFoldertextBox.TabIndex = 1;
            // 
            // searchFolderButton
            // 
            searchFolderButton.Location = new Point(661, 11);
            searchFolderButton.Name = "searchFolderButton";
            searchFolderButton.Size = new Size(118, 54);
            searchFolderButton.TabIndex = 2;
            searchFolderButton.Text = "Search Folder";
            searchFolderButton.UseVisualStyleBackColor = true;
            searchFolderButton.Click += searchFolderButton_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(filesChangedLoggerTextBox);
            groupBox1.Location = new Point(12, 174);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(779, 272);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Files changed";
            // 
            // filesChangedLoggerTextBox
            // 
            filesChangedLoggerTextBox.Location = new Point(7, 22);
            filesChangedLoggerTextBox.Multiline = true;
            filesChangedLoggerTextBox.Name = "filesChangedLoggerTextBox";
            filesChangedLoggerTextBox.ReadOnly = true;
            filesChangedLoggerTextBox.ScrollBars = ScrollBars.Vertical;
            filesChangedLoggerTextBox.Size = new Size(762, 242);
            filesChangedLoggerTextBox.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(portTextBox);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(hostTextBox);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(appFoldertextBox);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(searchFolderButton);
            groupBox2.Location = new Point(12, 5);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(779, 163);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Text = "App Folder";
            // 
            // portTextBox
            // 
            portTextBox.Location = new Point(307, 53);
            portTextBox.Name = "portTextBox";
            portTextBox.Size = new Size(100, 23);
            portTextBox.TabIndex = 6;
            portTextBox.Text = "4502";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(270, 56);
            label3.Name = "label3";
            label3.Size = new Size(32, 15);
            label3.TabIndex = 5;
            label3.Text = "Port:";
            // 
            // hostTextBox
            // 
            hostTextBox.Location = new Point(66, 53);
            hostTextBox.Name = "hostTextBox";
            hostTextBox.Size = new Size(200, 23);
            hostTextBox.TabIndex = 4;
            hostTextBox.Text = "localhost";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(27, 57);
            label2.Name = "label2";
            label2.Size = new Size(35, 15);
            label2.TabIndex = 3;
            label2.Text = "Host:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "Form1";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private FolderBrowserDialog appBrowserDialog;
        private TextBox appFoldertextBox;
        private Button searchFolderButton;
        private GroupBox groupBox1;
        private TextBox filesChangedLoggerTextBox;
        private GroupBox groupBox2;
        private Label label2;
        private TextBox portTextBox;
        private Label label3;
        private TextBox hostTextBox;
    }
}
