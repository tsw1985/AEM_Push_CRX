using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;


namespace AEM_Push_CRX
{
    public partial class AppForm : Form
    {

        private Curl curl;
        private Dictionary<string, string> fileHashes;
        private Utils utils;
        FileSystemWatcher watcher;
        public static string DESTINATION_DIRECTORY = @"C:\windows\temp\aemtemp";
        public static String FOLDER_ZIPPED_FILE = @"C:\windows\temp\aemtemp.zip";
        public static String JCR_ROOT = "jcr_root";

        public AppForm()
        {
            InitializeComponent();
            InitObjects();
        }

        private void InitObjects()
        {
            utils = new Utils();
            curl = new Curl(utils, adminUserTextBox.Text, adminPasswordTextBox.Text);
        }

        // Inicializar FileSystemWatcher
        private void initFileWatcher(string filePath)
        {

            if (!filePath.Equals(""))
            {

                filesChangedLoggerTextBox.Text = "Starting log files ... " + filePath;
                filesChangedLoggerTextBox.AppendText(Environment.NewLine);

                fileHashes = new Dictionary<string, string>();
                string path = filePath;


                watcher = new FileSystemWatcher
                {
                    Path = path,
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                    IncludeSubdirectories = true
                };

                //set events to listen
                watcher.Changed += OnChanged;
                //watcher.Created += OnChanged;
                //watcher.Deleted += OnChanged;
                //watcher.Renamed += OnRenamed;

                //Start watcher
                watcher.EnableRaisingEvents = true;

            }
        }

        // Métodos de manejo de eventos para FileSystemWatcher
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            /*System.Threading.Thread.Sleep(500); // Pequeño retraso para asegurar que el archivo se haya escrito completamente
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                if (utils.IsAllowedFile(e.FullPath) && !utils.IsTargetDirectoryPresent(e.FullPath))
                {
                    if (FileHasChanged(e.FullPath))
                    {
                        launchWorker(e);
                    }
                }
            }*/


            System.Threading.Thread.Sleep(500); // Pequeño retraso para asegurar que el archivo se haya escrito completamente
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                if (utils.IsAllowedFile(e.FullPath) && !utils.IsTargetDirectoryPresent(e.FullPath))
                {
                    if (FileHasChanged(e.FullPath))
                    {
                        UploadFile(e.FullPath);
                        UpdateTextBox(e.FullPath + " " + e.ChangeType);
                    }
                }
            }

        }

        private void launchWorker(FileSystemEventArgs e)
        {
            //by background worker
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync(e);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Debug.WriteLine("UPLOADING!!");
                BackgroundWorker helperBW = sender as BackgroundWorker;
                FileSystemEventArgs argument = (FileSystemEventArgs)e.Argument;

                String fullPath = argument.FullPath;
                String changeType = argument.ChangeType.ToString();

                if(UploadFile(fullPath))
                {
                    UpdateTextBox(fullPath + " | " + changeType);
                }
                else
                {
                    UpdateTextBox(fullPath + " | " + " ERROR");
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception UPLOADING!! backgroundWorker1_DoWork()");
            }
            
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Debug.WriteLine("UPLOADING FINISHED !!");
        }


        private bool UploadFile(String path)
        {
            bool created = false;
            try
            {
                string sourceFile = path;
                created = CreatePkgZip(sourceFile, DESTINATION_DIRECTORY);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("EXCEPTION UploadFile() " + ex.Message);
                created = false;
            }

            return created;
        }

        private bool CreatePkgZip(String sourceFile, String destinationRoot)
        {
            Boolean fileUploaded = false;

            if (sourceFile.Contains(JCR_ROOT))
            {

                try
                {
                    String currentTimeStamp = utils.GetCurrentDateTimeStamp();
                    String relativePath = sourceFile.Split(JCR_ROOT)[1];

                    if (utils.CreateBasicFolders(sourceFile, destinationRoot, currentTimeStamp))
                    {
                        if (utils.CreateJcrRootTargetFolder(sourceFile, destinationRoot, currentTimeStamp, true))
                        {
                            string destinationFile = destinationRoot + "\\" + JCR_ROOT + relativePath;
                            utils.CopyTargetFileToPkgFolder(sourceFile, destinationFile);

                            if (utils.CreateFilterAndPropertiesFiles(sourceFile, destinationRoot, currentTimeStamp))
                            {
                                utils.ZipTempFolder();
                                fileUploaded = curl.UploadFile(FOLDER_ZIPPED_FILE, relativePath, currentTimeStamp,
                                hostTextBox.Text, portTextBox.Text);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error creating CreateEmtpyZipPkg folder " + ex);
                }
            }

            return fileUploaded;
        }


        private void UpdateTextBox(string text)
        {
            if (filesChangedLoggerTextBox.InvokeRequired)
            {
                filesChangedLoggerTextBox.Invoke(new Action<string>(UpdateTextBox), text);
            }
            else
            {
                filesChangedLoggerTextBox.AppendText(text);
                filesChangedLoggerTextBox.AppendText(Environment.NewLine);
            }
        }

        private bool FileHasChanged(string filePath)
        {
            bool fileChanged = true;
            try
            {
                string newHash = ComputeFileHash(filePath);

                if (fileHashes.TryGetValue(filePath, out string oldHash))
                {
                    if (newHash == oldHash)
                    {
                        fileChanged = false;
                    }
                }

                fileHashes[filePath] = newHash;

            }
            catch (IOException io)
            {
                Debug.WriteLine("Error FileHasChanged() " + io);
            }

            return fileChanged;
        }

        private string ComputeFileHash(string filePath)
        {
            using (var md5 = MD5.Create())
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }


        private void searchFolderButton_Click(object sender, EventArgs e)
        {
            if (appBrowserDialog.ShowDialog() == DialogResult.OK)
            {

                appFoldertextBox.Text = appBrowserDialog.SelectedPath;
                initFileWatcher(appBrowserDialog.SelectedPath);

                /*if (curl.CheckConnection(hostTextBox.Text, portTextBox.Text))
                {
                    appFoldertextBox.Text = appBrowserDialog.SelectedPath;
                    initFileWatcher(appBrowserDialog.SelectedPath);
                }
                else
                {
                    MessageBox.Show("INSTANCE NOT FOUNDED");
                }*/
            }
        }

        private void searchFileButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pathPullFileTextBox.Text = openFileDialog.FileName;
                pullFromCrxButton.Enabled = true;

            }
        }

        private void pullFromCrxButton_Click(object sender, EventArgs e)
        {
            if (!pathPullFileTextBox.Text.Equals("") && !folderZipFileTextBox.Text.Equals(""))
            {

                resultLabel.Text = "Creating package ... Please wait ...";

                if (PullFile(pathPullFileTextBox.Text, DESTINATION_DIRECTORY)) //DESTINATION_DIRECTORY
                {
                    resultLabel.Text = "Package created in : " + folderZipFileTextBox.Text + "\\pkg.zip";
                }
                else
                {
                    resultLabel.Text = "[ERROR] Creating package :(";
                }
            }
        }


        private bool PullFile(String path, String destinationRootPath)
        {
            bool created = false;
            try
            {
                String timeStamp = utils.GetCurrentDateTimeStamp();
                String relativePath = utils.CreateEmtpyZipPkg(path, destinationRootPath, timeStamp);

                if (!relativePath.Equals(""))
                {
                    created = curl.downloadFile(FOLDER_ZIPPED_FILE, hostTextBox.Text, portTextBox.Text, timeStamp, relativePath, folderZipFileTextBox.Text);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Pull FILE EXCEPTION !! " + ex.Message);
                created = false;
            }

            return created;
        }

        private void open_folder_to_save_zip_button_Click(object sender, EventArgs e)
        {
            if (saveZipFileFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                folderZipFileTextBox.Text = saveZipFileFolderBrowserDialog.SelectedPath;
            }
        }

        private void clean_log_Click(object sender, EventArgs e)
        {
            filesChangedLoggerTextBox.Text = "";
        }

        
    }
}