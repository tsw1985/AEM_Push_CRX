using System;
using System.Collections.Generic;
using System.Linq;
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


    }
}
