using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace KC_Bugtracker.Helpers
{
    public class AttachmentHelper
    {
        public static string GetIcon(string fileName)
        {
            var imgPath = "";
            var ext = Path.GetExtension(fileName);
            switch(ext)
            {
                case ".pdf":
                    imgPath = "/Images/pdf.png";
                    break;
                case ".doc":
                    imgPath = "/Images/doc.png";
                    break;
                case ".docx":
                    imgPath = "/Images/docx.png";
                    break;
                case ".xls":
                    imgPath = "/Images/xls.png";
                    break;
                case ".xlsx":
                    imgPath = "/Images/xlsx.png";
                    break;
                case ".txt":
                    imgPath = "/Images/txt.png";
                    break;
                case ".zip":
                case ".rar":
                case ".7z":
                    imgPath = "/Images/zip.png";
                    break;
                case ".xml":
                    imgPath = "/Images/xml.png";
                    break;
                case ".jpg":
                case ".gif":
                case ".png":
                    imgPath = fileName;
                    break;
                default:
                    imgPath = "/Images/blank.png";
                    break;
            }
            return imgPath;
        }
    }
}