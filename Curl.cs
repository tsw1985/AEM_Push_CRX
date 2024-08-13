using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
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
            bool resultUpload = false;
            bool resultInstall = false;
            bool resultDelete = false;
            String resultOK = "\"success\":true";

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

                    if (output.Contains(resultOK))
                    {
                        resultUpload = true;
                    }

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

                    if (output.Contains(resultOK))
                    {
                        resultInstall = true;
                    }

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


                    if (output.Contains(resultOK))
                    {
                        resultDelete  = true;
                    }

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
            return resultInstall && resultDelete && resultUpload;
        }

        /*
            /home/gabriel/DEVELOPER/code/JAVA/tools-repo-1.4/repo/repo get -f /home/gabriel/DEVELOPER/code/JAVA/icex-portalelena/ui.apps/src/main/content/jcr_root/apps/icex-elena/components/content/comunity/.content.xml
            downloading /apps/icex-elena/components/content/comunity/.content.xml from http://192.168.1.196:4502
            TEMP FOLDER ==> /tmp/repo.0KD
            EXCLUDES ===> /tmp/repo.0KD/.excludes
            parametro 2:  -F package =@/tmp/repo.0KD/pkg.zip -F force = true http://192.168.1.196:4502/crx/packmgr/service/.json?cmd=upload
            **** CURL COMMAND: curl -u admin:admin -f -s -S -F package =@/tmp/repo.0KD/pkg.zip -F force = true http://192.168.1.196:4502/crx/packmgr/service/.json?cmd=upload 
            parametro 2:  -X POST http://192.168.1.196:4502/crx/packmgr/service/.json/etc/packages/tmp/repo/repo-apps-icex-elena-components-content-comunity-.content.xml-1723558257.zip?cmd=build
            **** CURL COMMAND: curl -u admin:admin -f -s -S -X POST http://192.168.1.196:4502/crx/packmgr/service/.json/etc/packages/tmp/repo/repo-apps-icex-elena-components-content-comunity-.content.xml-1723558257.zip?cmd=build 
            DOWNLOAD PACKAGE pkg : tmp/repo/repo-apps-icex-elena-components-content-comunity-.content.xml-1723558257.zip
            DOWNLOAD PACKAGE /tmp/repo.0KD/pkg.zip : /tmp/repo.0KD/pkg.zip
            download_pkg_funtion params : -o /tmp/repo.0KD/pkg.zip http://192.168.1.196:4502/etc/packages/tmp/repo/repo-apps-icex-elena-components-content-comunity-.content.xml-1723558257.zip
            **** CURL COMMAND: curl -u admin:admin -f -s -S -o /tmp/repo.0KD/pkg.zip http://192.168.1.196:4502/etc/packages/tmp/repo/repo-apps-icex-elena-components-content-comunity-.content.xml-1723558257.zip 
        */

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
