using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WF_iPMAC.Controller
{
    public class Log_Controller
    {
        string run_path;
        string exception_path;
        string run_file;
        string exception_file;
        string today;

        public Log_Controller()
        {
            this.today = DateTime.Now.ToString("yyyy-MM-dd");
            string run_file_prefix = "_RunLogs.txt";
            string exception_file_prefix = "_Logs.txt";
            string run_folder = "DebugRun";
            string exception_folder = "Debug";

            this.exception_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, exception_folder);
            this.run_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, run_folder);

            this.exception_file = Path.Combine(this.exception_path, today + exception_file_prefix);
            this.run_file = Path.Combine(this.run_path, today + run_file_prefix);
        }

        public void WriteLog(string msg, string title, bool IsError)
        {
            try
            {
                var path = IsError ? exception_path : run_path;
                var file = IsError ? exception_file : run_file;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                var sw = new StreamWriter(fs);
                sw.Close();
                fs.Close();

                var fs1 = new FileStream(file, FileMode.Append, FileAccess.Write);
                var sw1 = new StreamWriter(fs1);
                sw1.Write(title + ":" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + msg + Environment.NewLine);
                sw1.Close();
                fs1.Close();
            }
            catch(Exception ex)
            {

            }
            
        }
    }
}
