using System;
using System.IO;
using System.Text;

namespace Phoenix.Core.Logging
{
    public static class FSLogException
    {

        public static void LogInFile(string fileName, Exception exception)
        {
            LogInFile(fileName, FSLogPath.GetLogPath(), exception);
        }

        public static void LogInFile(string fileName, string path, Exception exception)
        {
            var sb = new StringBuilder(@"****************************************************");
            sb.Append("\r\n");
            sb.Append(@"Date:    " + DateTime.Now.ToString("dd/MM/yyyy hh:mm"));
            sb.Append("\r\n");
            sb.Append("Source:  " + exception.Source);
            sb.Append("\r\n");
            sb.Append("Message: " + exception.Message);
            sb.Append("\r\n");
            sb.Append("Stack:   " + exception.StackTrace);
            sb.Append("\r\n");
            FSLog.WriteToFile(Path.Combine(path, fileName), sb.ToString());
        }
    }
}
