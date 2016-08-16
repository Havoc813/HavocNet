using System.Configuration;

namespace Phoenix.Core
{
    public static class FSConfig
    {
        public static string AppSettings(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }

        public static string ConnectionString(string name)
        {
            return FSEncrypt.Decrypt(ConfigurationManager.ConnectionStrings[name].ConnectionString);
        }
    }
}
