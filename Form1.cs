using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
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

        public Form1()
        {
            InitializeComponent();
            initFileWatcher();

            // Initialize the keyboard hook
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
        private void initFileWatcher()
        {
            string path = @"C:\utils";
            FileSystemWatcher watcher = new FileSystemWatcher
            {
                Path = path,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName
            };

            // Suscribirse a los eventos
            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnChanged;
            watcher.Renamed += OnRenamed;

            // Comenzar a monitorear
            watcher.EnableRaisingEvents = true;
        }

        // Métodos de manejo de eventos para FileSystemWatcher
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            Debug.WriteLine($"Archivo: {e.FullPath} {e.ChangeType}");
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            Debug.WriteLine($"Archivo renombrado de {e.OldFullPath} a {e.FullPath}");
        }

        private void searchFolderButton_Click(object sender, EventArgs e)
        {
            appBrowserDialog.ShowDialog(this);
            appFoldertextBox.Text = appBrowserDialog.SelectedPath;

        }
    }
}
