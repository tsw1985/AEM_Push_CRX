namespace AEM_Push_CRX
{
    partial class AppForm
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
            appBrowserDialog = new FolderBrowserDialog();
            groupBox1 = new GroupBox();
            button1 = new Button();
            filesChangedLoggerTextBox = new TextBox();
            tabControl1 = new TabControl();
            pushTabPage = new TabPage();
            groupBox2 = new GroupBox();
            appFoldertextBox = new TextBox();
            label1 = new Label();
            searchFolderButton = new Button();
            pullTabPage = new TabPage();
            groupBox4 = new GroupBox();
            folderZipFileTextBox = new TextBox();
            button2 = new Button();
            label8 = new Label();
            pullFromCrxButton = new Button();
            resultLabel = new Label();
            searchFileButton = new Button();
            pathPullFileTextBox = new TextBox();
            label4 = new Label();
            portTextBox = new TextBox();
            label3 = new Label();
            hostTextBox = new TextBox();
            label2 = new Label();
            groupBox3 = new GroupBox();
            adminPasswordTextBox = new TextBox();
            label7 = new Label();
            adminUserTextBox = new TextBox();
            label6 = new Label();
            openFileDialog = new OpenFileDialog();
            saveZipFileFolderBrowserDialog = new FolderBrowserDialog();
            groupBox1.SuspendLayout();
            tabControl1.SuspendLayout();
            pushTabPage.SuspendLayout();
            groupBox2.SuspendLayout();
            pullTabPage.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(filesChangedLoggerTextBox);
            groupBox1.Location = new Point(6, 65);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(756, 265);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Files changed";
            // 
            // button1
            // 
            button1.Location = new Point(8, 20);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 1;
            button1.Text = "Clean log";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // filesChangedLoggerTextBox
            // 
            filesChangedLoggerTextBox.Location = new Point(7, 52);
            filesChangedLoggerTextBox.Multiline = true;
            filesChangedLoggerTextBox.Name = "filesChangedLoggerTextBox";
            filesChangedLoggerTextBox.ReadOnly = true;
            filesChangedLoggerTextBox.ScrollBars = ScrollBars.Vertical;
            filesChangedLoggerTextBox.Size = new Size(729, 212);
            filesChangedLoggerTextBox.TabIndex = 0;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(pushTabPage);
            tabControl1.Controls.Add(pullTabPage);
            tabControl1.Location = new Point(12, 74);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(776, 364);
            tabControl1.TabIndex = 5;
            // 
            // pushTabPage
            // 
            pushTabPage.Controls.Add(groupBox2);
            pushTabPage.Controls.Add(groupBox1);
            pushTabPage.Location = new Point(4, 24);
            pushTabPage.Name = "pushTabPage";
            pushTabPage.Padding = new Padding(3);
            pushTabPage.Size = new Size(768, 336);
            pushTabPage.TabIndex = 0;
            pushTabPage.Text = "PUSH FILE";
            pushTabPage.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(appFoldertextBox);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(searchFolderButton);
            groupBox2.Location = new Point(6, 6);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(756, 56);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Text = "App Folder to listen files";
            // 
            // appFoldertextBox
            // 
            appFoldertextBox.Location = new Point(65, 22);
            appFoldertextBox.Name = "appFoldertextBox";
            appFoldertextBox.ReadOnly = true;
            appFoldertextBox.Size = new Size(592, 23);
            appFoldertextBox.TabIndex = 1;
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
            // searchFolderButton
            // 
            searchFolderButton.Location = new Point(663, 17);
            searchFolderButton.Name = "searchFolderButton";
            searchFolderButton.Size = new Size(87, 34);
            searchFolderButton.TabIndex = 2;
            searchFolderButton.Text = "Search Folder";
            searchFolderButton.UseVisualStyleBackColor = true;
            searchFolderButton.Click += searchFolderButton_Click;
            // 
            // pullTabPage
            // 
            pullTabPage.Controls.Add(groupBox4);
            pullTabPage.Location = new Point(4, 24);
            pullTabPage.Name = "pullTabPage";
            pullTabPage.Padding = new Padding(3);
            pullTabPage.Size = new Size(768, 336);
            pullTabPage.TabIndex = 1;
            pullTabPage.Text = "PULL FILE";
            pullTabPage.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(folderZipFileTextBox);
            groupBox4.Controls.Add(button2);
            groupBox4.Controls.Add(label8);
            groupBox4.Controls.Add(pullFromCrxButton);
            groupBox4.Controls.Add(resultLabel);
            groupBox4.Controls.Add(searchFileButton);
            groupBox4.Controls.Add(pathPullFileTextBox);
            groupBox4.Controls.Add(label4);
            groupBox4.Location = new Point(7, 6);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(755, 324);
            groupBox4.TabIndex = 0;
            groupBox4.TabStop = false;
            groupBox4.Text = "Select file to get it from CRX";
            // 
            // folderZipFileTextBox
            // 
            folderZipFileTextBox.Location = new Point(81, 55);
            folderZipFileTextBox.Name = "folderZipFileTextBox";
            folderZipFileTextBox.Size = new Size(565, 23);
            folderZipFileTextBox.TabIndex = 8;
            // 
            // button2
            // 
            button2.Location = new Point(650, 56);
            button2.Name = "button2";
            button2.Size = new Size(97, 23);
            button2.TabIndex = 7;
            button2.Text = "Set folder";
            button2.UseVisualStyleBackColor = true;
            button2.Click += open_folder_to_save_zip_button_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(9, 59);
            label8.Name = "label8";
            label8.Size = new Size(66, 15);
            label8.TabIndex = 6;
            label8.Text = "Save file in:";
            // 
            // pullFromCrxButton
            // 
            pullFromCrxButton.Enabled = false;
            pullFromCrxButton.Location = new Point(80, 83);
            pullFromCrxButton.Name = "pullFromCrxButton";
            pullFromCrxButton.Size = new Size(566, 24);
            pullFromCrxButton.TabIndex = 5;
            pullFromCrxButton.Text = "Pull file";
            pullFromCrxButton.UseVisualStyleBackColor = true;
            pullFromCrxButton.Click += pullFromCrxButton_Click;
            // 
            // resultLabel
            // 
            resultLabel.AutoSize = true;
            resultLabel.Location = new Point(81, 114);
            resultLabel.Name = "resultLabel";
            resultLabel.RightToLeft = RightToLeft.Yes;
            resultLabel.Size = new Size(0, 15);
            resultLabel.TabIndex = 4;
            // 
            // searchFileButton
            // 
            searchFileButton.Location = new Point(650, 27);
            searchFileButton.Name = "searchFileButton";
            searchFileButton.Size = new Size(97, 24);
            searchFileButton.TabIndex = 2;
            searchFileButton.Text = "Search file";
            searchFileButton.UseVisualStyleBackColor = true;
            searchFileButton.Click += searchFileButton_Click;
            // 
            // pathPullFileTextBox
            // 
            pathPullFileTextBox.Location = new Point(81, 28);
            pathPullFileTextBox.Name = "pathPullFileTextBox";
            pathPullFileTextBox.Size = new Size(565, 23);
            pathPullFileTextBox.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(46, 36);
            label4.Name = "label4";
            label4.Size = new Size(25, 15);
            label4.TabIndex = 0;
            label4.Text = "File";
            // 
            // portTextBox
            // 
            portTextBox.Location = new Point(287, 20);
            portTextBox.Name = "portTextBox";
            portTextBox.Size = new Size(38, 23);
            portTextBox.TabIndex = 6;
            portTextBox.Text = "4502";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(252, 23);
            label3.Name = "label3";
            label3.Size = new Size(32, 15);
            label3.TabIndex = 5;
            label3.Text = "Port:";
            // 
            // hostTextBox
            // 
            hostTextBox.Location = new Point(46, 20);
            hostTextBox.Name = "hostTextBox";
            hostTextBox.Size = new Size(200, 23);
            hostTextBox.TabIndex = 4;
            hostTextBox.Text = "192.168.1.196";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(8, 23);
            label2.Name = "label2";
            label2.Size = new Size(35, 15);
            label2.TabIndex = 3;
            label2.Text = "Host:";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(adminPasswordTextBox);
            groupBox3.Controls.Add(label7);
            groupBox3.Controls.Add(adminUserTextBox);
            groupBox3.Controls.Add(label6);
            groupBox3.Controls.Add(portTextBox);
            groupBox3.Controls.Add(label2);
            groupBox3.Controls.Add(label3);
            groupBox3.Controls.Add(hostTextBox);
            groupBox3.Location = new Point(16, 12);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(768, 56);
            groupBox3.TabIndex = 6;
            groupBox3.TabStop = false;
            groupBox3.Text = "AEM Instance config";
            // 
            // adminPasswordTextBox
            // 
            adminPasswordTextBox.Location = new Point(626, 20);
            adminPasswordTextBox.Name = "adminPasswordTextBox";
            adminPasswordTextBox.Size = new Size(136, 23);
            adminPasswordTextBox.TabIndex = 10;
            adminPasswordTextBox.Text = "admin";
            adminPasswordTextBox.TextChanged += textBox2_TextChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(563, 23);
            label7.Name = "label7";
            label7.Size = new Size(60, 15);
            label7.TabIndex = 9;
            label7.Text = "Password:";
            // 
            // adminUserTextBox
            // 
            adminUserTextBox.Location = new Point(404, 20);
            adminUserTextBox.Name = "adminUserTextBox";
            adminUserTextBox.Size = new Size(139, 23);
            adminUserTextBox.TabIndex = 8;
            adminUserTextBox.Text = "admin";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(338, 23);
            label6.Name = "label6";
            label6.Size = new Size(63, 15);
            label6.TabIndex = 7;
            label6.Text = "Username:";
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "filePath";
            // 
            // AppForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(groupBox3);
            Controls.Add(tabControl1);
            Name = "AppForm";
            Text = "G85 - CRX Auto Save and download 1.0";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabControl1.ResumeLayout(false);
            pushTabPage.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            pullTabPage.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private FolderBrowserDialog appBrowserDialog;
        private GroupBox groupBox1;
        private TextBox filesChangedLoggerTextBox;
        private Button button1;
        private TabControl tabControl1;
        private TabPage pushTabPage;
        private TabPage pullTabPage;
        private Button searchFolderButton;
        private Label label1;
        private TextBox appFoldertextBox;
        private Label label2;
        private TextBox hostTextBox;
        private Label label3;
        private TextBox portTextBox;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private Button searchFileButton;
        private TextBox pathPullFileTextBox;
        private Label label4;
        private OpenFileDialog openFileDialog;
        private Label resultLabel;
        private Button pullFromCrxButton;
        private TextBox adminPasswordTextBox;
        private Label label7;
        private TextBox adminUserTextBox;
        private Label label6;
        private Button button2;
        private Label label8;
        private TextBox folderZipFileTextBox;
        private FolderBrowserDialog saveZipFileFolderBrowserDialog;
    }
}
