using System;
using System.IO;
using System.Text;

namespace Phoenix.Core.Logging
{
    public static class FSLogMessage
    {
        public static void LogInFile(string fileName, string message)
        {
            var sb = new StringBuilder(@"****************************************************");
            sb.Append("\r\n");
            sb.Append(@"Date:    " + DateTime.Now.ToString("dd/MM/yyyy hh:mm"));
            sb.Append("\r\n");
            sb.Append("Message: " + message);
            sb.Append("\r\n");
            FSLog.WriteToFile(Path.Combine(FSLogPath.GetLogPath(), fileName), sb.ToString());
        }
    }
}
