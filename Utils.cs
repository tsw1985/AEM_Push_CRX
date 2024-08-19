using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AEM_Push_CRX
{
    internal class Utils
    {

        private static String CQ_DIALOG = "_cq_dialog";
        private static String META_VAULT =  @"\META-INF\vault\" ;

        public bool IsADialogXML(string relativePath)
        {
            bool isADialogXML = false;
            string directoryPath = Path.GetDirectoryName(relativePath);

            string parentFolderName = new DirectoryInfo(directoryPath).Name;
            if (parentFolderName.Equals(CQ_DIALOG))
            {
                isADialogXML = true;
            }
            return isADialogXML;
        }

        public bool CopyTargetFileToPkgFolder(String sourceFile, String destinationFile)
        {
            bool fileCopied = true;
            try
            {
                File.Copy(sourceFile, destinationFile, true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error CopyTargetFileToPkgFolder "  + ex);
                fileCopied = false;
            }
            return fileCopied;
            
        }

        public bool CreateBasicFolders(String sourceFile, String destinationRoot, String currentTimeStamp)
        {

            bool basicFolderCreaded = true;

            try
            {
                string onlyFromAppsFolder = sourceFile.Split(AppForm.JCR_ROOT)[1];
                string sourceRoot = Path.GetPathRoot(sourceFile);
                string destinationFile = destinationRoot + "\\" + AppForm.JCR_ROOT + "\\";
                string destinationDirectory = Path.GetDirectoryName(destinationFile);

                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

            }
            catch (Exception e)
            {
                basicFolderCreaded = false;
                Debug.WriteLine("Problem creating basic folders " + e);
            }
            return basicFolderCreaded;
        }


        public bool CreateJcrRootTargetFolder(String sourceFile, String destinationRoot, String currentTimeStamp , bool addRelativePath)
        {
            bool copy = true;

            try
            {
                string onlyFromAppsFolder = sourceFile.Split(AppForm.JCR_ROOT)[1];
                string sourceRoot = Path.GetPathRoot(sourceFile);
                string relativePath = onlyFromAppsFolder;
                string destinationFile = "";
                if (addRelativePath)
                {
                    destinationFile = destinationRoot + "\\" + AppForm.JCR_ROOT + relativePath;
                }
                else
                {
                    destinationFile = destinationRoot + "\\" + AppForm.JCR_ROOT + "\\";
                }

                string destinationDirectory = Path.GetDirectoryName(destinationFile);
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Problem copying target file to basic folders pkg " + e);
                copy = false;
            }
            return copy;
        }


        public bool CreateFilterAndPropertiesFiles(String sourceFile, String destinationRoot, String currentTimeStamp)
        {

            bool filesCreated = true;
            try
            {
                string relativePath = sourceFile.Split(AppForm.JCR_ROOT)[1];
                string destinationFile = destinationRoot + "\\" + AppForm.JCR_ROOT + relativePath;
                destinationFile = destinationFile.Replace("\\" + AppForm.JCR_ROOT, "");
                destinationFile = destinationRoot + META_VAULT; 

                string destinationDirectory = Path.GetDirectoryName(destinationFile);
                destinationDirectory = Path.GetDirectoryName(destinationFile);

                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                String finalRelativePath = "";
                if (IsADialogXML(relativePath))
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

                String propertiesXML = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>\n" +
                                       "<!DOCTYPE properties SYSTEM \"http://java.sun.com/dtd/properties.dtd\">\n" +
                                       "<properties>\n" +
                                       "<entry key=\"name\">${replacePath}</entry>\n" +
                                       "<entry key=\"version\">${randomVersion}</entry>\n" +
                                       "<entry key=\"group\">tmp/repo</entry>\n" +
                                       "</properties>";

                if (IsADialogXML(relativePath))
                {
                    propertiesXML = propertiesXML.Replace("${replacePath}", GetPathName(relativePath))
                                                 .Replace("${randomVersion}", currentTimeStamp)
                                                 .Replace("_cq_dialog-", "cqdialog-");
                }
                else //if it is a .html .js file ...
                {
                    propertiesXML = propertiesXML.Replace("${replacePath}", GetPathName(relativePath))
                                                 .Replace("${randomVersion}", currentTimeStamp);
                }

                // It is a .content.xml for component name or other .content.xml file??
                if (relativePath.Contains("\\.content.xml"))
                {
                    filtersXML = filtersXML.Replace("/.content.xml", "");
                }

                String filtersFileXML = destinationDirectory + "\\filter.xml";
                String propertiesFileXML = destinationDirectory + "\\properties.xml";

                File.WriteAllText(filtersFileXML, filtersXML);
                File.WriteAllText(propertiesFileXML, propertiesXML);
            }
            catch (Exception e)
            {
                filesCreated = false;
            }

            return filesCreated;
        }

        public String CreateEmtpyZipPkg(String sourceFile, String destinationRoot , String currentTimeStamp)
        {
            string relativePath = sourceFile.Split(AppForm.JCR_ROOT)[1];

            if (sourceFile.Contains(AppForm.JCR_ROOT))
            {
                try
                {
                    if ( CreateBasicFolders(sourceFile, destinationRoot, currentTimeStamp))
                    {
                        if (CreateJcrRootTargetFolder(sourceFile, destinationRoot, currentTimeStamp , false))
                        {
                            if( CreateFilterAndPropertiesFiles(sourceFile, destinationRoot, currentTimeStamp))
                            {
                                ZipTempFolder();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error creating CreateEmtpyZipPkg folder " + ex);
                }
            }
            return GetPathName(relativePath);
        }

        public bool ZipTempFolder()
        {
            bool folderZiped = true;

            try
            {

                if (File.Exists(AppForm.DESTINATION_DIRECTORY))
                {
                    File.Delete(AppForm.DESTINATION_DIRECTORY);
                }

                if (File.Exists(AppForm.FOLDER_ZIPPED_FILE))
                {
                    File.Delete(AppForm.FOLDER_ZIPPED_FILE);
                }

                ZipFile.CreateFromDirectory(AppForm.DESTINATION_DIRECTORY, AppForm.FOLDER_ZIPPED_FILE);

            }
            catch (Exception e)
            {
                Debug.WriteLine("Error creating ZipTempFolder folder " + e);
            }
            return folderZiped;
        }

        public String GetPathName(String path)
        {
            return "repo" + path.Replace("\\", "-");
        }

        public String GetCurrentDateTimeStamp()
        {
            DateTime now = DateTime.UtcNow;
            long timestamp = ((DateTimeOffset)now).ToUnixTimeSeconds();
            return timestamp.ToString();
        }


        public bool IsAllowedFile(String filePath)
        {
            return filePath.EndsWith(".html") ||
                   filePath.EndsWith(".xml")  ||
                   filePath.EndsWith(".js")   ||
                   filePath.EndsWith(".css")  ||
                   filePath.EndsWith(".txt")  ||
                   filePath.EndsWith(".jsp")  ||
                   filePath.EndsWith(".less");
        }

    }
}