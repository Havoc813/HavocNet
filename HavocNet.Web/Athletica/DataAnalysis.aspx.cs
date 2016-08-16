using System;

namespace HavocNet.Web.Athletica
{
    public partial class DataAnalysis : System.Web.UI.Page
    {
        private Athletica _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (Athletica)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainAnalyse");
            _myMaster.BindTypeMenu("MainAnalyse", @"Athletica\MenuAnalysis.aspx", "ActivityID=" + Request.QueryString["ActivityID"], "Analyse");

            _myMaster.BindAnalyse(@"Athletica\DataAnalysis.aspx", "ActivityID=" + Request.QueryString["ActivityID"]);
        }
    }
}