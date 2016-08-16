using System;

namespace HavocNet.Web
{
    public partial class MainMountains : System.Web.UI.Page
    {
        private Summits.Summits _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (Summits.Summits)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainTrips");

            LoadPage();
        }

        private void LoadPage()
        {
            
        }
    }
}