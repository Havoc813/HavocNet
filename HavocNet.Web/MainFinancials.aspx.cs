using System;

namespace HavocNet.Web
{
    public partial class MainFinancials : System.Web.UI.Page
    {
        private Financials _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (Financials)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainFinancials");
        }
    }
}