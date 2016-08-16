using System.Drawing;

namespace Phoenix.Core
{
    public static class FSColor
    {
        public static Color ForeColor(Color backColor)
        {
            var avg = (backColor.R + backColor.G + backColor.B) / 3;
            return avg > 255 / 2 ? Color.Black : Color.White;
        }
    }
}
