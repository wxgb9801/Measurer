using System;

namespace LIB_DB
{
    public class XMLDBFileHelper
    {
        public static eXmlFileType GetFileType(string fileName)
        {
            eXmlFileType eType = eXmlFileType.Other;
            string[] filetmp = fileName.Split('.');
            string type = filetmp[1].ToUpper();
            bool r = Enum.TryParse<eXmlFileType>(type, out eType);
            return eType;
        }
        public static string GetFullFileName(string fileFolder, eXmlFileType fileType, string fileName)
        {
            return string.Format(@"{0}\{1}.{2}", fileFolder, fileName.Split('.')[0], fileType.ToString());
        }
    }
}
