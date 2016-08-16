using System;

namespace HavocNet.Web.Athletica
{
    public partial class MainDashboard : System.Web.UI.Page
    {
        private Athletica _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            //_myMaster = (Athletica) Master;
            //if (_myMaster == null) return;
            //_myMaster.LoadPage("MainDashboard", false);
            //_myMaster.BindTypeMenu("MainDashboard", "Athletica/MenuDashboard.aspx", "ActivityID=" + Request.QueryString["ActivityID"], "Dashboard");

            Response.Redirect(@"\Athletica\MenuDashboard.aspx?ActivityID=0");
        }
    }
}