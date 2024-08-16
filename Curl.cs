using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AEM_Push_CRX
{
    internal class Curl
    {

        public static String RESULT_OK = "\"success\":true";
        private Utils utils;

        public Curl(Utils utils) { 
            this.utils = utils;
        }

        public bool uploadFile(String path, String relativePath, String timestamp , String host , String port)
        {
            bool resultUpload = false;
            bool resultInstall = false;
            bool resultDelete = false;
            
            try
            {
                //check if we are uploading a dialog
                if (utils.IsADialogXML(relativePath))
                {
                    relativePath = relativePath.Replace("_cq_dialog", "cqdialog");
                }

                // Define el comando curl
                string commandUploadZip = "curl -u admin:admin -f -s -S -F package=@" + path + "  -F force=true http://" + host + ":" + port + "/crx/packmgr/service/.json?cmd=upload";
                resultUpload = ExecuteCurl(commandUploadZip);

                string commandInstallZip = "curl -u admin:admin -f -s -S -X POST http://" + host + ":" + port + "/crx/packmgr/service/.json/etc/packages/tmp/repo/repo" + relativePath.Replace("\\", "-") + "-" + timestamp + ".zip" + "?cmd=install";
                resultInstall = ExecuteCurl(commandInstallZip);

                string commandDeleteZip = "curl -u admin:admin -f -s -S -X POST http://" + host + ":" + port + "/crx/packmgr/service/.json/etc/packages/tmp/repo/repo" + relativePath.Replace("\\", "-") + "-" + timestamp + ".zip" + "?cmd=delete";
                resultDelete = ExecuteCurl(commandDeleteZip);

            }
            catch (Exception ex)
            {
                // Mostrar cualquier excepción en un cuadro de mensaje
                MessageBox.Show("Ocurrió un error al ejecutar el comando:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return resultInstall && resultDelete && resultUpload;

        }


        public bool downloadFile(String path , String host, String port , String timeStamp , String relativePath)
        {
            bool resultUpload = false;
            bool resultBuild = false;
            Debug.WriteLine("Download fileee");

            //1 Upload package
            String commandUploadZip = "curl -u admin:admin -f -s -S -F package=@" + path + "  -F force=true http://" + host + ":" + port + "/crx/packmgr/service/.json?cmd=upload";
            resultUpload = ExecuteCurl(commandUploadZip);

            System.Threading.Thread.Sleep(1000);

            //2 build package
            string commandBuildPackage = "curl -u admin:admin -f -s -S -X POST http://" + host + ":" + port + "/crx/packmgr/service/.json/etc/packages/tmp/repo/" + relativePath                                           + "-" + timeStamp + ".zip" + "?cmd=build";
            resultBuild = ExecuteCurl(commandBuildPackage);

            //3 Download zip , extract it and put file on the same path
            // curl -u admin:admin -f -s -S -o /tmp/pkggggg.zip http://192.168.1.196:4502/etc/packages/tmp/repo/repo-apps-icex-elena-components-content-comunity-comunity.html-1723802917.zip


            return resultUpload;
        }


        private bool ExecuteCurl(String curlCommand)
        {
            bool result = false;

            var processStartInfoUploadFile = new ProcessStartInfo("cmd", "/c " + curlCommand)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = processStartInfoUploadFile })
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                Debug.WriteLine("Curl Command : " + curlCommand);
                Debug.WriteLine("Output: " + output);
                if (output.Contains(Curl.RESULT_OK))
                {
                    result = true;
                }
            }
            return result;
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

        
    }
}
