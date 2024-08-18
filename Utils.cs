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
        public bool IsADialogXML(string relativePath)
        {
            bool isADialogXML = false;
            // Obtener la ruta de la carpeta que contiene el archivo
            string directoryPath = Path.GetDirectoryName(relativePath);

            // Obtener la carpeta padre
            string parentFolderName = new DirectoryInfo(directoryPath).Name;
            if (parentFolderName.Equals("_cq_dialog"))
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
                // Copiar el archivo al nuevo directorio
                //String backUpDestinationFilePath = destinationFile;
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
                string onlyFromAppsFolder = sourceFile.Split("jcr_root")[1];
                string sourceRoot = Path.GetPathRoot(sourceFile);
                string destinationFile = destinationRoot + "\\jcr_root\\";
                string destinationDirectory = Path.GetDirectoryName(destinationFile);

                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                    Debug.WriteLine($"Directorio creado: {destinationDirectory}");
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
                string onlyFromAppsFolder = sourceFile.Split("jcr_root")[1];
                string sourceRoot = Path.GetPathRoot(sourceFile);
                string relativePath = onlyFromAppsFolder;// Path.GetRelativePath(onlyFromAppsFolder, sourceFile);
                string destinationFile = "";
                if (addRelativePath)
                {
                    destinationFile = destinationRoot + "\\jcr_root" + relativePath;
                }
                else
                {
                    destinationFile = destinationRoot + "\\jcr_root\\";
                }

                string destinationDirectory = Path.GetDirectoryName(destinationFile);
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                    Debug.WriteLine($"Directorio creado: {destinationDirectory}");
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
                string relativePath = sourceFile.Split("jcr_root")[1];
                //create XML FILE on vault.
                string destinationFile = destinationRoot + "\\jcr_root" + relativePath;
                destinationFile = destinationFile.Replace("\\jcr_root", "");
                destinationFile = destinationRoot + "\\META-INF\\vault\\";


                string destinationDirectory = Path.GetDirectoryName(destinationFile);

                destinationDirectory = Path.GetDirectoryName(destinationFile);
                Debug.WriteLine("DESTINATION VAULT DIRECTORY: " + destinationDirectory);

                // Crear los directorios necesarios en el destino
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                    Debug.WriteLine($"Directorio VAULT creado: {destinationDirectory}");
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

                //String currentTimeStamp = GetCurrentDateTimeStamp();
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

                Debug.WriteLine("------------ UTILS -----------------");
                Debug.WriteLine(filtersXML);
                Debug.WriteLine("------------ END UTILS -----------------");
                Debug.WriteLine(propertiesXML);

                //write files properties and filters.xml
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
            string relativePath = sourceFile.Split("jcr_root")[1];

            if (sourceFile.Contains("jcr_root"))
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
                //Zip the folder @"C:\windows\temp\aemtemp";
                String sourceZipFolder = @"C:\windows\temp\aemtemp";
                if (File.Exists(sourceZipFolder))
                {
                    File.Delete(sourceZipFolder);
                }

                String folderZippedFile = @"C:\windows\temp\aemtemp.zip";

                // Elimina el archivo .zip si ya existe
                if (File.Exists(folderZippedFile))
                {
                    File.Delete(folderZippedFile);
                }

                // Crea el archivo .zip desde el directorio especificado
                ZipFile.CreateFromDirectory(sourceZipFolder, folderZippedFile);

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

    }
}