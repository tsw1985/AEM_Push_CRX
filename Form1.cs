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
        private static string DESTINATION_DIRECTORY = @"C:\windows\temp\aemtemp";
        private static String FOLDER_ZIPPED_FILE = @"C:\windows\temp\aemtemp.zip";

        public AppForm()
        {
            InitializeComponent();
            utils = new Utils();
            curl = new Curl(utils,adminUserTextBox.Text , adminPasswordTextBox.Text);
        }

        // Inicializar FileSystemWatcher
        private void initFileWatcher(string filePath)
        {

            if (!filePath.Equals(""))
            {

                filesChangedLoggerTextBox.Text = "Starting log on : " + filePath;
                filesChangedLoggerTextBox.AppendText(Environment.NewLine);

                fileHashes = new Dictionary<string, string>();
                string path = filePath;


                watcher = new FileSystemWatcher
                {
                    Path = path,
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                    IncludeSubdirectories = true
                };

                // Suscribirse a los eventos
                watcher.Changed += OnChanged;
                //watcher.Created += OnChanged;
                //watcher.Deleted += OnChanged;
                //watcher.Renamed += OnRenamed;

                // Comenzar a monitorear
                watcher.EnableRaisingEvents = true;

            }
        }

        // Métodos de manejo de eventos para FileSystemWatcher
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            System.Threading.Thread.Sleep(500); // Pequeño retraso para asegurar que el archivo se haya escrito completamente
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                String filePath = e.FullPath;
                if (filePath.EndsWith(".html") || filePath.EndsWith(".xml") || 
                    filePath.EndsWith(".js") || filePath.EndsWith(".css") ||
                        filePath.EndsWith(".less"))
                {
                    // Verificar si el contenido del archivo ha cambiado
                    if (FileHasChanged(filePath))
                    {
                        Debug.WriteLine($"Archivo: {e.FullPath} {e.ChangeType}");
                        UploadFile(filePath);
                        UpdateTextBox(filePath + " " + e.ChangeType);
                    }
                }
            }
        }

        private bool UploadFile(String path)
        {
            bool created = false;
            try
            {
                string sourceFile = path;
                //string destinationDirectory = @"C:\windows\temp\aemtemp";
                created = CreatePkgZip(sourceFile, DESTINATION_DIRECTORY);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("EXCEPTION !! " + ex.Message);
                created = false;
            }

            return created;
        }

        private bool CreatePkgZip(String sourceFile, String destinationRoot)
        {

            Boolean fileUploaded = false;

            if (sourceFile.Contains("jcr_root"))
            {

                try
                {
                    String currentTimeStamp = utils.GetCurrentDateTimeStamp();
                    String relativePath = sourceFile.Split("jcr_root")[1];

                    if ( utils.CreateBasicFolders(sourceFile, destinationRoot, currentTimeStamp))
                    {
                        if (utils.CreateJcrRootTargetFolder(sourceFile, destinationRoot, currentTimeStamp, true))
                        {

                            string destinationFile = destinationRoot + "\\jcr_root" + relativePath;
                            utils.CopyTargetFileToPkgFolder(sourceFile, destinationFile);

                            if (utils.CreateFilterAndPropertiesFiles(sourceFile, destinationRoot, currentTimeStamp))
                            {
                                utils.ZipTempFolder();
                                
                                fileUploaded = curl.uploadFile(FOLDER_ZIPPED_FILE, relativePath, currentTimeStamp,
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
                // Si estamos en un hilo diferente, usar Invoke para llamar a este método en el hilo de la UI
                filesChangedLoggerTextBox.Invoke(new Action<string>(UpdateTextBox), text);
            }
            else
            {
                // Estamos en el hilo de la UI, podemos actualizar el TextBox directamente
                filesChangedLoggerTextBox.AppendText(text);
                filesChangedLoggerTextBox.AppendText(Environment.NewLine);
            }
        }

        private bool FileHasChanged(string filePath)
        {
            try
            {
                string newHash = ComputeFileHash(filePath);

                if (fileHashes.TryGetValue(filePath, out string oldHash))
                {
                    if (newHash == oldHash)
                    {
                        // El contenido no ha cambiado
                        return false;
                    }
                }

                // Actualizar el hash almacenado
                fileHashes[filePath] = newHash;
                return true;
            }
            catch (IOException)
            {
                // Manejar archivos que puedan estar siendo utilizados por otros procesos
                return false;
            }
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


        private void OnRenamed(object source, RenamedEventArgs e)
        {
            Debug.WriteLine($"Archivo renombrado de {e.OldFullPath} a {e.FullPath}");
        }

        private void searchFolderButton_Click(object sender, EventArgs e)
        {
            appBrowserDialog.ShowDialog(this);
            appFoldertextBox.Text = appBrowserDialog.SelectedPath;
            initFileWatcher(appBrowserDialog.SelectedPath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            filesChangedLoggerTextBox.Text = "";
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

                resultLabel.Text = "Creating package ... Please wait";

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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void open_folder_to_save_zip_button_Click(object sender, EventArgs e)
        {
            if (saveZipFileFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                folderZipFileTextBox.Text = saveZipFileFolderBrowserDialog.SelectedPath;
            }
        }
    }
}