using System;

namespace HavocNet.Web
{
    public partial class MainChecklist : System.Web.UI.Page
    {
        private Checklist _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (Checklist)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainChecklist");
        }
    }
}