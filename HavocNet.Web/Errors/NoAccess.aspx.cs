using System;
using System.Web;
using Core;
using Phoenix.Core.Logging;
using Phoenix.Core.Servers;

namespace HavocNet.Web.Errors
{
    public partial class NoAccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var masterPage = (HavocNetMaster) Master;
            if (masterPage == null) return;
            masterPage.LoadPageNoAuth("");

            masterPage.ClearMenus();
        
            var aError = new FSError(new HavocServer());
            aError.LogInTable(HttpContext.Current.User.Identity.Name, "HavocNet", Request.QueryString["aspxerrorpath"], new Exception("Access Denied"));

            //TODO: Email

        }
    }
}