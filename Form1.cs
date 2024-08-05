using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace AEM_Push_CRX
{
    public partial class Form1 : Form
    {
        // Declaraciones de la API de Windows
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        // Delegado y constantes para el hook
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private LowLevelKeyboardProc _proc;
        private IntPtr _hookID = IntPtr.Zero;

        private Curl curl;
        private Dictionary<string, string> fileHashes;

        public Form1()
        {
            InitializeComponent();
            //initFileWatcher();
            // Initialize the keyboard hook
            //InitHooking();
        }

        private void InitHooking()
        {   
            _proc = HookCallback;
            _hookID = SetHook(_proc);
        }


        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;

                // Verificar si se presionan Control + S
                if (Control.ModifierKeys == Keys.Control && key == Keys.S)
                {
                    Debug.WriteLine("Pulsado Control + S");
                }
            }
            // Pasar el evento al siguiente hook en la cadena
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Desenganchar el hook cuando el formulario se cierre
            UnhookWindowsHookEx(_hookID);
            base.OnFormClosed(e);
        }

        // Inicializar FileSystemWatcher
        private void initFileWatcher(string filePath)
        {

            fileHashes = new Dictionary<string, string>();
            //string path = @"C:\utils";
            string path = filePath;  //@"C:\Utils\FileZilla_3.67.1_src\filezilla-3.67.1\data";


            FileSystemWatcher watcher = new FileSystemWatcher
            {
                Path = path,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName
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
            // Obtener el directorio raíz de la ruta de origen
            string sourceRoot = Path.GetPathRoot(sourceFile);
            Debug.WriteLine("SOURCE ROOT: " + sourceRoot);

            // Obtener la ruta relativa del archivo desde el directorio raíz de origen
            string relativePath = Path.GetRelativePath(sourceRoot, sourceFile);
            Debug.WriteLine("RELATIVE PATH: " + relativePath);

            // Construir la ruta completa en el destino
            string destinationFile = Path.Combine(destinationRoot, relativePath);
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
        }

        /*private bool CreateTempDirectory(String path)
        {
            bool created = false;
            try
            {
                string sourceFile = path;
                string destinationDirectory = @"C:\windows\temp\aemtemp";
                string destinationFile = Path.Combine(destinationDirectory, Path.GetFileName(sourceFile));

                if (!Directory.Exists(destinationDirectory))
                {
                    // Crear el directorio de destino
                    Directory.CreateDirectory(destinationDirectory);
                    Debug.WriteLine($"Directorio creado: {destinationDirectory}");
                }

                // Copiar el archivo al directorio de destino
                File.Copy(sourceFile, destinationFile, true);
                Debug.WriteLine($"Archivo copiado a: {destinationFile}");

            }
            catch (Exception ex)
            {
                Debug.WriteLine("EXCEPTION !! " + ex.Message);
                created = false;
            }

            return created;
        }*/


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
