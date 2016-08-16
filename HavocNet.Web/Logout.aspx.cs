using System;

namespace HavocNet.Web
{
    public partial class Logout : System.Web.UI.Page
    {
        private HavocNetMaster _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();

            _myMaster = (HavocNetMaster)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPageNoAuth("");
            _myMaster.ClearMenus();
        }
    }
}