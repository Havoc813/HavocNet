using System;

namespace HavocNet.Web
{
    public partial class Login : System.Web.UI.Page
    {
        private HavocNetMaster _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (HavocNetMaster)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPageNoAuth("");
            _myMaster.ClearMenus();

        }
    }
}