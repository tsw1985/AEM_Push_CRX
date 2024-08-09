using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AEM_Push_CRX
{
    internal class Curl
    {

        public Curl() { }

        public bool uploadFile(String path, String relativePath, String timestamp , String host , String port)
        {
            bool result = false;
            Debug.WriteLine("Uploading file: " + path);
            try
            {

                //check if we are uploading a dialog
                if (relativePath.Contains("_cq_dialog"))
                {
                    relativePath = relativePath.Replace("_cq_dialog", "cqdialog");
                }

               // Define el comando curl
                string commandUploadZip = "curl -u admin:admin -f -s -S -F package=@" + path + "  -F force=true http://" + host + ":" + port + "/crx/packmgr/service/.json?cmd=upload";

                // Configurar el proceso
                var processStartInfoUploadFile = new ProcessStartInfo("cmd", "/c " + commandUploadZip)
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                // Ejecutar el proceso UPLOAD
                using (var process = new Process { StartInfo = processStartInfoUploadFile })
                {
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    // Mostrar el resultado o error en la consola de depuración
                    Debug.WriteLine("Output: " + output);
                    Debug.WriteLine("Error: " + error);

                    // Mostrar el resultado en un cuadro de mensaje
                    Debug.WriteLine("Comando UPLOAD ejecutado exitosamente:" + output);
                }

                string commandInstallZip = "curl -u admin:admin -f -s -S -X POST http://" + host + ":" + port + "/crx/packmgr/service/.json/etc/packages/tmp/repo/repo" + relativePath.Replace("\\","-") + "-" + timestamp + ".zip" + "?cmd=install";

                Debug.WriteLine("COMMANDO : " + commandInstallZip);

                // Configurar el proceso
                var processStartInfoInstallFile = new ProcessStartInfo("cmd", "/c " + commandInstallZip)
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };


                using (var processInstall = new Process { StartInfo = processStartInfoInstallFile })
                {
                    processInstall.Start();
                    string output = processInstall.StandardOutput.ReadToEnd();
                    string error = processInstall.StandardError.ReadToEnd();
                    processInstall.WaitForExit();

                    // Mostrar el resultado o error en la consola de depuración
                    Debug.WriteLine("INSTALLING - Error Output: " + output);
                    Debug.WriteLine("INSTALLING - Error: " + error);

                    // Mostrar el resultado en un cuadro de mensaje
                    Debug.WriteLine("Comando INSTALL ejecutado exitosamente:\n" + output);
                }


                //DELETE PACKAGE

                string commandDeleteZip = "curl -u admin:admin -f -s -S -X POST http://" + host + ":" + port + "/crx/packmgr/service/.json/etc/packages/tmp/repo/repo" + relativePath.Replace("\\", "-") + "-" + timestamp + ".zip" + "?cmd=delete";

                Debug.WriteLine("COMMANDO DELETE: " + commandDeleteZip);

                // Configurar el proceso
                var processStartInfoDeleteFile = new ProcessStartInfo("cmd", "/c " + commandDeleteZip)
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };


                using (var processDelete = new Process { StartInfo = processStartInfoDeleteFile })
                {
                    processDelete.Start();
                    string output = processDelete.StandardOutput.ReadToEnd();
                    string error = processDelete.StandardError.ReadToEnd();
                    processDelete.WaitForExit();

                    // Mostrar el resultado o error en la consola de depuración
                    Debug.WriteLine("DELETING - Error Output: " + output);
                    Debug.WriteLine("DELETING - Error: " + error);

                    // Mostrar el resultado en un cuadro de mensaje
                    Debug.WriteLine("Comando DELETE ejecutado exitosamente:\n" + output);
                }

            }
            catch (Exception ex)
            {
                // Mostrar cualquier excepción en un cuadro de mensaje
                MessageBox.Show("Ocurrió un error al ejecutar el comando INSTALL:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            return result;
        }

        public bool downloadFile(String path)
        {
            bool result = false;
            Debug.WriteLine("Download fileee");


            // comando manual para descargar

            // 1 Build package
            // http://192.168.1.196:4502/crx/packmgr/service/.json/etc/packages/tmp/repo/repo-apps-icex-elena-components-content-comunity-.content.xml-1723147800.zip?cmd=build

            //2 Download
            //curl -u admin:admin -f -s -S -o /tmp/demo/pkg.zip http://192.168.1.196:4502/etc/packages/tmp/repo/repo-apps-icex-elena-components-content-comunity-.content.xml-1723147800.zip
            //curl -u admin:admin -f -s -S -o /tmp/demo/pkg.zip http://192.168.1.196:4502/etc/packages/tmp/repo/repo-apps-icex-elena-components-content-comunity-.content.xml-1723147800.zip











            return result;
        }
    }
}
