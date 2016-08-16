using System;

namespace Phoenix.Core
{
    public static class FSGuid
    {
        public static string GetNewGuid()
        {
            return Guid.NewGuid().ToString();
        }

        public static bool CheckGuid(string guid)
        {
            Guid newGuid;
            return Guid.TryParse(guid, out newGuid);
        }
    }
}
