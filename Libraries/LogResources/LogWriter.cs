using System;
using System.IO;

namespace OnlineStore.Libraries.LogResources
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