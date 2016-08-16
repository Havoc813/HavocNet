using System;

namespace HavocNet.Web.Athletica
{
    public partial class MenuCalculators : System.Web.UI.Page
    {
        private Athletica _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (Athletica)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainTools");
        }
    }
}