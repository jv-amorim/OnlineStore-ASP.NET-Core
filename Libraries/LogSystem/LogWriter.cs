using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace OnlineStore.Libraries.LogSystem
{
    public class LogWriter
    {
        public static void WriteNewDataInLogFile(string path, string content)
        {
            if (!File.Exists(path))
            {
                FileStream file = File.Create(path);
                file.Close();
            }

            StreamWriter fileWriter = File.AppendText(path);
            fileWriter.WriteLine($"[{DateTime.Now.ToString()}]: {content}");
            fileWriter.Close();
        }
    }
}