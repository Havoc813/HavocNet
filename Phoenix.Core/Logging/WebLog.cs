using System.Collections;
using System.Web;

namespace Phoenix.Core.Logging
{
    public class WebLog
    {
        public void WriteArrayToFile(string fileName, ArrayList arr)
        {
            FSLog.WriteArrayToFile(HttpContext.Current.Server.MapPath("~/App_Data/Logs/" + fileName), arr);
        }

        public void WriteToFile(string fileName, string line)
        {
            FSLog.WriteToFile(HttpContext.Current.Server.MapPath("~/App_Data/Logs/" + fileName), line);
        }
    }
}
