using System.IO;

namespace Edrive.FtpRecordsScheduler
{
    public class LoggerHelper
    {
        public string FileName { get; set; }

        public LoggerHelper(string fileName)
        {
            FileName = fileName;
        }

        public void AddLog(string message)
        {
            using (StreamWriter streamWriter = File.AppendText(FileName))
            {
                streamWriter.WriteLine(message);
            }
        }
    }
}