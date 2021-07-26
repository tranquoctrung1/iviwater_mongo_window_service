using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WF_iPMAC.Controller
{
    public class File_Controller
    {
        Log_Controller _log = new Log_Controller();
        public void Copy_Files(string sourcePath, string destPath)
        {
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            foreach (var fileName in Directory.GetFiles(sourcePath))
            {
                try
                {
                    var a = Path.Combine(destPath, Path.GetFileName(fileName));
                    File.Copy(fileName,a , true);
                }
                catch (Exception ex)
                {
                    _log.WriteLog(ex.ToString(), "error on copy file " + fileName, true);
                }
            }
        }
    }
}
