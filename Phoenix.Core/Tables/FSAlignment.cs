using System.Web.UI.WebControls;

namespace Phoenix.Core.Tables
{
    public enum FSAlignment
    {
        Centre,
        Justify,
        Left,
        Right
    }

    public static class HTMLAlignment
    {
        public static HorizontalAlign FSAlignToHTMLAlign(FSAlignment horizontalAlignment)
        {
            switch (horizontalAlignment)
            {
                case FSAlignment.Centre:
                    return HorizontalAlign.Center;
                case FSAlignment.Justify:
                    return HorizontalAlign.Justify;
                case FSAlignment.Right:
                    return HorizontalAlign.Right;
            }

            return HorizontalAlign.Left;
        }
    }
}
