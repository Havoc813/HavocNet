using System;
using System.IO;

namespace Phoenix.Core.Logging
{
    public static class FSLogPath
    {
        public static string GetLogPath()
        {
            return GetLogPath(null);
        }

        public static string GetLogPath(string appSettingsPathKey)
        {
            string defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (String.IsNullOrEmpty(defaultPath))
                defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            defaultPath = Path.Combine(defaultPath, "Phoenix");
            if (String.IsNullOrEmpty(appSettingsPathKey)) return defaultPath;

            string logPath = FSConfig.AppSettings(appSettingsPathKey);
            if (string.IsNullOrEmpty(logPath)) logPath = defaultPath;
            return logPath;
        }
    }
}
