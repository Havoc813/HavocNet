using System;
using System.Web;

namespace HavocNet.Web.Errors
{
    public partial class FourOhFour : System.Web.UI.Page
    {
        private HavocNetMaster _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (HavocNetMaster) Master;
            if (_myMaster == null) return;
            _myMaster.LoadPageNoAuth("");
            _myMaster.ClearMenus();
        
            //var aError = new FSError(new FSMServer());
            //aError.LogInTable(HttpContext.Current.User.Identity.Name, "FSM", Request.QueryString["aspxerrorpath"], new Exception("Page Not Found"));
        }
    }
}