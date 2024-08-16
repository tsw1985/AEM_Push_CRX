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
    public partial class Form1 : Form
    {
        private Curl curl;
        private Dictionary<string, string> fileHashes;
        private Utils utils;
        FileSystemWatcher watcher;
        private static string DESTINATION_DIRECTORY = @"C:\windows\temp\aemtemp";
        private static String FOLDER_ZIPPED_FILE = @"C:\windows\temp\aemtemp.zip";

        public Form1()
        {
            InitializeComponent();
            utils = new Utils();
            curl = new Curl(utils);
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
            System.Threading.Thread.Sleep(200); // Pequeño retraso para asegurar que el archivo se haya escrito completamente
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                String filePath = e.FullPath;
                if (filePath.EndsWith(".html") || filePath.EndsWith(".xml") || filePath.EndsWith(".js"))
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
                created = CopyFileWithStructure(sourceFile, DESTINATION_DIRECTORY);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("EXCEPTION !! " + ex.Message);
                created = false;
            }

            return created;
        }


        private bool CopyFileWithStructure(String sourceFile, String destinationRoot)
        {

            Boolean fileUploaded = false;

            if (sourceFile.Contains("jcr_root"))
            {
                // *************************** Copy html file *****************************
                string onlyFromAppsFolder = sourceFile.Split("jcr_root")[1];
                //sourceFile = onlyFromAppsFolder;

                // Obtener el directorio raíz de la ruta de origen
                string sourceRoot = Path.GetPathRoot(sourceFile);
                Debug.WriteLine("SOURCE ROOT: " + sourceRoot);

                // Obtener la ruta relativa del archivo desde el directorio raíz de origen
                //string relativePath = Path.GetRelativePath(sourceRoot, sourceFile);
                string relativePath = onlyFromAppsFolder;// Path.GetRelativePath(onlyFromAppsFolder, sourceFile);
                Debug.WriteLine("RELATIVE PATH: " + relativePath);

                // Construir la ruta completa en el destino
                string destinationFile = destinationRoot + "\\jcr_root" + relativePath;
                Debug.WriteLine("DESTINATION PATH: " + destinationFile);

                // Obtener el directorio de destino
                string destinationDirectory = Path.GetDirectoryName(destinationFile);
                Debug.WriteLine("DESTINATION DIRECTORY: " + destinationDirectory);

                // Crear los directorios necesarios en el destino
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                    Debug.WriteLine($"Directorio creado: {destinationDirectory}");
                }

                // Copiar el archivo al nuevo directorio
                //String backUpDestinationFilePath = destinationFile;
                File.Copy(sourceFile, destinationFile, true);
                Debug.WriteLine($"Archivo copiado a: {destinationFile}");
                // *************************** END Copy html file *****************************


                //create XML FILE on vault.
                destinationFile = destinationFile.Replace("\\jcr_root", "");
                destinationFile = destinationRoot + "\\META-INF\\vault\\";
                destinationDirectory = Path.GetDirectoryName(destinationFile);
                Debug.WriteLine("DESTINATION VAULT DIRECTORY: " + destinationDirectory);

                // Crear los directorios necesarios en el destino
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                    Debug.WriteLine($"Directorio VAULT creado: {destinationDirectory}");
                }

                String finalRelativePath = "";
                if (utils.IsADialogXML(relativePath))
                {
                    finalRelativePath = relativePath.Replace("\\", "/");
                    finalRelativePath = finalRelativePath.Replace("_cq_dialog/.content.xml", "cq:dialog");
                }
                else
                {
                    finalRelativePath = relativePath.Replace("\\", "/");
                }

                String filtersXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                                    "<workspaceFilter version=\"1.0\">\n" +
                                    "    <filter root=\"" + finalRelativePath + "\"/>\n" +
                                    "</workspaceFilter>";

                filtersXML = filtersXML.Replace("\\", "/");

                String currentTimeStamp = utils.GetCurrentDateTimeStamp();
                String propertiesXML = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>\n" +
                                       "<!DOCTYPE properties SYSTEM \"http://java.sun.com/dtd/properties.dtd\">\n" +
                                       "<properties>\n" +
                                       "<entry key=\"name\">${replacePath}</entry>\n" +
                                       "<entry key=\"version\">${randomVersion}</entry>\n" +
                                       "<entry key=\"group\">tmp/repo</entry>\n" +
                                       "</properties>";

                if (utils.IsADialogXML(relativePath))
                {
                    propertiesXML = propertiesXML.Replace("${replacePath}", utils.GetPathName(relativePath))
                                        .Replace("${randomVersion}", currentTimeStamp)
                                        .Replace("_cq_dialog-", "cqdialog-");
                }
                else //if it is a .html .js file ...
                {
                    propertiesXML = propertiesXML.Replace("${replacePath}", utils.GetPathName(relativePath))
                                        .Replace("${randomVersion}", currentTimeStamp);
                }

                // It is a .content.xml for component name or other .content.xml file??
                if (relativePath.Contains("\\.content.xml"))
                {
                    filtersXML = filtersXML.Replace("/.content.xml", "");
                }

                Debug.WriteLine(filtersXML);
                Debug.WriteLine("------------------------------------");
                Debug.WriteLine(propertiesXML);

                //write files properties and filters.xml
                String filtersFileXML = destinationDirectory + "\\filter.xml";
                String propertiesFileXML = destinationDirectory + "\\properties.xml";

                File.WriteAllText(filtersFileXML, filtersXML);
                File.WriteAllText(propertiesFileXML, propertiesXML);


                //Zip the folder @"C:\windows\temp\aemtemp";
                String sourceZipFolder = @"C:\windows\temp\aemtemp";
                if (File.Exists(sourceZipFolder))
                {
                    File.Delete(sourceZipFolder);
                }


                //String folderZippedFile = @"C:\windows\temp\aemtemp.zip";

                // Elimina el archivo .zip si ya existe
                if (File.Exists(FOLDER_ZIPPED_FILE))
                {
                    File.Delete(FOLDER_ZIPPED_FILE);
                }

                // Crea el archivo .zip desde el directorio especificado
                ZipFile.CreateFromDirectory(sourceZipFolder, FOLDER_ZIPPED_FILE);
                fileUploaded = curl.uploadFile(FOLDER_ZIPPED_FILE, relativePath, currentTimeStamp,
                    hostTextBox.Text, portTextBox.Text);
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
                if (PullFile(pathPullFileTextBox.Text, DESTINATION_DIRECTORY)) //DESTINATION_DIRECTORY
                {
                    resultLabel.Text = "Package done on : " + folderZipFileTextBox.Text + "\\pkg.zip";
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