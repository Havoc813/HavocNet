using System.Drawing;
using System.Web.UI.WebControls;

namespace Core
{
    public static class MessageHelper
    {
        public static void SetLabel(ref Label lbl, string strMessage, Color colour)
        {
            lbl.Text = strMessage;
            lbl.ForeColor = colour;
        }
    }
}
