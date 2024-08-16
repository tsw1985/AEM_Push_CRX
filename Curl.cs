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
        private String userName;
        private String password;
        private String curlHeadCommand;

        public Curl(Utils utils , String userName , String password) { 
            this.utils = utils;
            this.userName = userName;
            this.password = password;
            this.curlHeadCommand = "curl -u " + userName  + ":" + password + " ";
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
                string commandUploadZip = curlHeadCommand      + " -f -s -S -F package=@" + path + "  -F force=true http://" + host + ":" + port + "/crx/packmgr/service/.json?cmd=upload";
                resultUpload = ExecuteCurl(commandUploadZip);

                string commandInstallZip = curlHeadCommand     + " -f -s -S -X POST http://" + host + ":" + port + "/crx/packmgr/service/.json/etc/packages/tmp/repo/repo" + relativePath.Replace("\\", "-") + "-" + timestamp + ".zip" + "?cmd=install";
                resultInstall = ExecuteCurl(commandInstallZip);

                string commandDeleteZip = curlHeadCommand      + " -f -s -S -X POST http://" + host + ":" + port + "/crx/packmgr/service/.json/etc/packages/tmp/repo/repo" + relativePath.Replace("\\", "-") + "-" + timestamp + ".zip" + "?cmd=delete";
                resultDelete = ExecuteCurl(commandDeleteZip);

            }
            catch (Exception ex)
            {
                // Mostrar cualquier excepción en un cuadro de mensaje
                MessageBox.Show("Ocurrió un error al ejecutar el comando:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return resultInstall && resultDelete && resultUpload;

        }


        public bool downloadFile(String path , String host, String port , String timeStamp , String relativePath, String destinationFolder)
        {
            bool resultUpload = false;
            bool resultBuild = false;
            bool resultDownload = false;

            Debug.WriteLine("Download fileee");

            //1 Upload package
            String commandUploadZip = curlHeadCommand + " -f -s -S -F package=@" + path + "  -F force=true http://" + host + ":" + port + "/crx/packmgr/service/.json?cmd=upload";
            resultUpload = ExecuteCurl(commandUploadZip);

            System.Threading.Thread.Sleep(1000);

            //2 build package
            string commandBuildPackage = curlHeadCommand + " -f -s -S -X POST http://" + host + ":" + port + "/crx/packmgr/service/.json/etc/packages/tmp/repo/" + relativePath  + "-" + timeStamp + ".zip" + "?cmd=build";
            resultBuild = ExecuteCurl(commandBuildPackage);

            //3 Download zip , extract it and put file on the same path
            // Linux
            // curl -u admin:admin -f -s -S -o /tmp/pkggggg.zip http://192.168.1.196:4502/etc/packages/tmp/repo/repo-apps-icex-elena-components-content-comunity-comunity.html-1723802917.zip
            System.Threading.Thread.Sleep(1000);

            // Windows
            // curl -u admin:admin -f -s -S -o "c:\AEM\fichero.zip" http://192.168.1.196:4502/etc/packages/tmp/repo/repo-apps-icex-elena-components-content-breadcrumb-breadcrumb.html-1723802698.zip
            string commanDownloadPackage = curlHeadCommand + " -f -s -S -o \"" + destinationFolder + "\\pkg.zip\"" + " http://" + host +  ":" + port + "/etc/packages/tmp/repo/" + relativePath + "-" + timeStamp + ".zip";
            resultDownload = ExecuteCurl(commanDownloadPackage);


            return resultUpload && resultBuild;
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
    }
}