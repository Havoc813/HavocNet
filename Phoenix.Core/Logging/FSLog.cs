using System;
using System.Collections;
using System.IO;

namespace Phoenix.Core.Logging
{
    public static class FSLog
    {
        public static void WriteArrayToFile(string fileNameWithPath, ArrayList @params)
        {
            if (!String.IsNullOrEmpty(Path.GetDirectoryName(fileNameWithPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(fileNameWithPath));

            using (StreamWriter aFile = File.AppendText(fileNameWithPath))
            {
                foreach (var obj in @params)
                    aFile.WriteLine(obj.ToString());

                aFile.Close();    
            }
        }

        public static void WriteToFile(string fileNameWithPath, string line)
        {
            if (!String.IsNullOrEmpty(Path.GetDirectoryName(fileNameWithPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(fileNameWithPath));

            using (StreamWriter aFile = File.AppendText(fileNameWithPath))
            {
                aFile.WriteLine(line);
                aFile.Close();
            }
        }
    }
}
