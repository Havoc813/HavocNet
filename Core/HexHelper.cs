using System;

namespace Core
{
    public static class HexHelper
    {
        public static string MyHex(int iVal)
        {
            return iVal == 16 ? "G" : iVal.ToString("X");
        }

        public static int UndoMyHex(string sVal)
        {
            return sVal == "G" ? 16 : Convert.ToInt32(sVal, 16);
        }
    }
}
