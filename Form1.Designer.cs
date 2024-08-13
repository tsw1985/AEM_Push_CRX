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
            label5 = new Label();
            resultPullFileTextBox = new TextBox();
            searchFileButton = new Button();
            pathPullFileTextBox = new TextBox();
            label4 = new Label();
            portTextBox = new TextBox();
            label3 = new Label();
            hostTextBox = new TextBox();
            label2 = new Label();
            groupBox3 = new GroupBox();
            openFileDialog = new OpenFileDialog();
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
            groupBox2.Text = "App Folder";
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
            groupBox4.Controls.Add(label5);
            groupBox4.Controls.Add(resultPullFileTextBox);
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
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(10, 65);
            label5.Name = "label5";
            label5.RightToLeft = RightToLeft.Yes;
            label5.Size = new Size(39, 15);
            label5.TabIndex = 4;
            label5.Text = "Result";
            // 
            // resultPullFileTextBox
            // 
            resultPullFileTextBox.Location = new Point(54, 60);
            resultPullFileTextBox.Name = "resultPullFileTextBox";
            resultPullFileTextBox.Size = new Size(592, 23);
            resultPullFileTextBox.TabIndex = 3;
            // 
            // searchFileButton
            // 
            searchFileButton.Location = new Point(652, 19);
            searchFileButton.Name = "searchFileButton";
            searchFileButton.Size = new Size(97, 32);
            searchFileButton.TabIndex = 2;
            searchFileButton.Text = "Search file";
            searchFileButton.UseVisualStyleBackColor = true;
            searchFileButton.Click += searchFileButton_Click;
            // 
            // pathPullFileTextBox
            // 
            pathPullFileTextBox.Location = new Point(54, 24);
            pathPullFileTextBox.Name = "pathPullFileTextBox";
            pathPullFileTextBox.Size = new Size(592, 23);
            pathPullFileTextBox.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(10, 28);
            label4.Name = "label4";
            label4.Size = new Size(25, 15);
            label4.TabIndex = 0;
            label4.Text = "File";
            // 
            // portTextBox
            // 
            portTextBox.Location = new Point(287, 21);
            portTextBox.Name = "portTextBox";
            portTextBox.Size = new Size(38, 23);
            portTextBox.TabIndex = 6;
            portTextBox.Text = "4502";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(253, 23);
            label3.Name = "label3";
            label3.Size = new Size(32, 15);
            label3.TabIndex = 5;
            label3.Text = "Port:";
            // 
            // hostTextBox
            // 
            hostTextBox.Location = new Point(47, 20);
            hostTextBox.Name = "hostTextBox";
            hostTextBox.Size = new Size(200, 23);
            hostTextBox.TabIndex = 4;
            hostTextBox.Text = "192.168.1.196";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(7, 23);
            label2.Name = "label2";
            label2.Size = new Size(35, 15);
            label2.TabIndex = 3;
            label2.Text = "Host:";
            // 
            // groupBox3
            // 
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
            // openFileDialog
            // 
            openFileDialog.FileName = "filePath";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(groupBox3);
            Controls.Add(tabControl1);
            Name = "Form1";
            Text = "Form1";
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
        private Label label5;
        private TextBox resultPullFileTextBox;
    }
}
