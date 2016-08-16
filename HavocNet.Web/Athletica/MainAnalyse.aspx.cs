using System;

namespace HavocNet.Web.Athletica
{
    public partial class MainAnalyse : System.Web.UI.Page
    {
        private Web.Athletica.Athletica _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            //_myMaster = (Web.Athletica.Athletica)Master;
            //if (_myMaster == null) return;
            //_myMaster.LoadPage("MainAnalyse");
            //_myMaster.BindTypeMenu("MainAnalyse", "Athletica/MenuAnalysis.aspx", "ActivityID=" + Request.QueryString["ActivityID"], "Analysis");

            Response.Redirect(@"\Athletica\MenuAnalysis.aspx?ActivityID=0");
        }
    }
}