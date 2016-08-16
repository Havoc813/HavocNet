using System;

namespace HavocNet.Web
{
    public partial class MainRegatta : System.Web.UI.Page
    {
        private Regatta.Regatta _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (Regatta.Regatta)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainRegatta");
        }
    }
}