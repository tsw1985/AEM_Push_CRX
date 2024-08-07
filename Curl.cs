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

        public bool uploadFile(String path, String relativePath, String timestamp)
        {
            bool result = false;
            //curl -u admin:admin -f -s -S -F package=@/tmp/repo.kLt/pkg.zip -F force=true http://192.168.1.196:4502/crx/packmgr/service/.json?cmd=upload 
            Debug.WriteLine("Uploading file: " + path);

            try
            {
                // Define el comando curl
                string commandUploadZip = "curl -u admin:admin -f -s -S -F package=@" + path + "  -F force=true http://192.168.1.196:4502/crx/packmgr/service/.json?cmd=upload";

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


                // Ejecutar el proceso INSTALL
                // Define el comando curl

                //funciona a mano :        curl -u admin:admin -f -s -S -X POST http://192.168.1.196:4502/crx/packmgr/service/.json/etc/packages/tmp/repo/repo-apps-icex-elena-components-content-breadcrumb-breadcrumb.html-1723065259.zip?cmd=install
                //                         curl -u admin:admin -f -s -S -X POST http://192.168.1.196:4502/crx/packmgr/service/.json/etc/packages/tmp/repo/repo-apps-icex-elena-components-content-breadcrumb-breadcrumb.html-1723065372.zip?cmd=install


                Thread.Sleep(1000);

                string commandInstallZip = "curl -u admin:admin -f -s -S -X POST http://192.168.1.196:4502/crx/packmgr/service/.json/etc/packages/tmp/repo/repo" + relativePath.Replace("\\","-") + "-" + timestamp + ".zip" + "?cmd=install";

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
            return result;
        }
    }
}
