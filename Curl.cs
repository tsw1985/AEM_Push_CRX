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
        public static String CONNECTION_RESULT_OK = "HTTP/1.1 200 OK";

        private Utils utils;
        private String userName;
        private String password;
        private String curlHeadCommand;
        private String curlHeadCheckConnCommand;

        public Curl(Utils utils , String userName , String password) { 
            this.utils = utils;
            this.userName = userName;
            this.password = password;
            this.curlHeadCommand = "curl -u " + userName  + ":" + password + " ";
            this.curlHeadCheckConnCommand = "curl -I -u " + userName + ":" + password + " ";
        }


        public bool CheckConnection(String host , String port)
        {
            bool connected = false;
            String outText = "";
            String checkCommand = curlHeadCheckConnCommand + " http://" + host + ":" + port;
            Debug.WriteLine(checkCommand);
            connected = ExecuteCurl(checkCommand, CONNECTION_RESULT_OK , out outText);
            Debug.WriteLine("OUT CHECK CONN : " + outText);
            return connected;
        }

        public bool UploadFile(String path, String relativePath, String timestamp , String host , String port)
        {
            bool resultUpload = false;
            bool resultInstall = false;
            bool resultDelete = false;
            String outText = "";

            try
            {
                if (utils.IsADialogXML(relativePath))
                {
                    relativePath = relativePath.Replace("_cq_dialog", "cqdialog");
                }

                string commandUploadZip = curlHeadCommand      + " -f -s -S -F package=@" + path + "  -F force=true http://" + host + ":" + port + "/crx/packmgr/service/.json?cmd=upload";
                Debug.WriteLine(commandUploadZip);
                resultUpload = ExecuteCurl(commandUploadZip, Curl.RESULT_OK , out outText);
                Debug.WriteLine("OUT UPLOADZIP: " + outText);

                outText = "";
                System.Threading.Thread.Sleep(500);

                string commandInstallZip = curlHeadCommand     + " -f -s -S -X POST http://" + host + ":" + port + "/crx/packmgr/service/.json/etc/packages/tmp/repo/repo" + relativePath.Replace("\\", "-") + "-" + timestamp + ".zip" + "?cmd=install";
                Debug.WriteLine(commandInstallZip);
                resultInstall = ExecuteCurl(commandInstallZip, Curl.RESULT_OK, out outText);
                Debug.WriteLine("OUT INSTALL: " + outText);
                System.Threading.Thread.Sleep(500);

                outText = "";

                string commandDeleteZip = curlHeadCommand      + " -f -s -S -X POST http://" + host + ":" + port + "/crx/packmgr/service/.json/etc/packages/tmp/repo/repo" + relativePath.Replace("\\", "-") + "-" + timestamp + ".zip" + "?cmd=delete";
                Debug.WriteLine(commandDeleteZip);
                resultDelete = ExecuteCurl(commandDeleteZip, Curl.RESULT_OK, out outText);
                Debug.WriteLine("OUT DELETE: " + outText);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error executing CURL command:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return resultInstall && resultDelete && resultUpload;

        }


        public bool downloadFile(String path , String host, String port , String timeStamp , String relativePath, String destinationFolder)
        {
            bool resultUpload = false;
            bool resultBuild = false;

            String outText = "";
            String commandUploadZip = curlHeadCommand + " -f -s -S -F package=@" + path + "  -F force=true http://" + host + ":" + port + "/crx/packmgr/service/.json?cmd=upload";
            Debug.WriteLine(commandUploadZip);
            resultUpload = ExecuteCurl(commandUploadZip, Curl.RESULT_OK , out outText);
            Debug.WriteLine("OUT UPLOAD: " + outText);

            System.Threading.Thread.Sleep(500);

            string commandBuildPackage = curlHeadCommand + " -f -s -S -X POST http://" + host + ":" + port + "/crx/packmgr/service/.json/etc/packages/tmp/repo/" + relativePath  + "-" + timeStamp + ".zip" + "?cmd=build";
            Debug.WriteLine( commandBuildPackage);
            resultBuild = ExecuteCurl(commandBuildPackage, Curl.RESULT_OK, out outText);
            Debug.WriteLine("OUT BUILD: " + outText);

            System.Threading.Thread.Sleep(500);

            string commanDownloadPackage = curlHeadCommand + " -f -s -S -o \"" + destinationFolder + "\\pkg.zip\"" + " http://" + host +  ":" + port + "/etc/packages/tmp/repo/" + relativePath + "-" + timeStamp + ".zip";
            Debug.WriteLine( commanDownloadPackage);
            ExecuteCurl(commanDownloadPackage, Curl.RESULT_OK, out outText);
            Debug.WriteLine("OUT Download: " + outText);

            return resultUpload && resultBuild;
        }


        private bool ExecuteCurl(String curlCommand , String expectedResult , out String outText)
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
                Debug.WriteLine("OUTPUT--->: " + output);

                outText = output;
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                Debug.WriteLine("Curl Command : " + curlCommand);
                
                if (output.Contains(expectedResult))
                {
                    result = true;
                }
            }
            return result;
        }
    }
}