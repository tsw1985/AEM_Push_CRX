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

namespace AEM_Push_CRX
{
    public partial class Form1 : Form
    {
        private Curl curl;
        private Dictionary<string, string> fileHashes;

        public Form1()
        {
            InitializeComponent();
            //initFileWatcher();
            // Initialize the keyboard hook
            //InitHooking();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Desenganchar el hook cuando el formulario se cierre
            base.OnFormClosed(e);
        }

        // Inicializar FileSystemWatcher
        private void initFileWatcher(string filePath)
        {

            fileHashes = new Dictionary<string, string>();
            string path = filePath;


            FileSystemWatcher watcher = new FileSystemWatcher
            {
                Path = path,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName , IncludeSubdirectories = true
            };

            // Suscribirse a los eventos
            watcher.Changed += OnChanged;
            //watcher.Created += OnChanged;
            //watcher.Deleted += OnChanged;
            //watcher.Renamed += OnRenamed;

            // Comenzar a monitorear
            watcher.EnableRaisingEvents = true;
        }

        // Métodos de manejo de eventos para FileSystemWatcher
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                // Verificar si el contenido del archivo ha cambiado
                if (FileHasChanged(e.FullPath))
                {
                    Debug.WriteLine($"Archivo: {e.FullPath} {e.ChangeType}");

                    //TODO : Put when file is uploaded.


                    // pkg\jcr_root\apps\icex-elena\components\content\breadcrumb

                    CreateTempDirectory(e.FullPath);
                    UpdateTextBox(e.FullPath + " " + e.ChangeType);
                }
            }
        }

        private bool CreateTempDirectory(String path)
        {
            bool created = false;
            try
            {
                string sourceFile = path;
                string destinationDirectory = @"C:\windows\temp\aemtemp";
                CopyFileWithStructure(sourceFile, destinationDirectory);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("EXCEPTION !! " + ex.Message);
                created = false;
            }

            return created;
        }


        private static void CopyFileWithStructure(string sourceFile, string destinationRoot)
        {

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


                string filtersXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                                    "<workspaceFilter version=\"1.0\">\n" +
                                    "    <filter root=\"" + relativePath.Replace("\\" , "/") + "\"/>\n" +
                                    "</workspaceFilter>";

                Debug.WriteLine(filtersXML);

                //TODO:
                // write file filters with this content on target path
                // write properties.xml file
                // zip file
                // push curl and testing
              


            }
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
            using (var stream = File.OpenRead(filePath))
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
    }
}