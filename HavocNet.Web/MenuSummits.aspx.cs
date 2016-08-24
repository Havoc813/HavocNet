using System;
using System.Web.UI;
using Core.Repositories;

namespace HavocNet.Web
{
    public partial class MenuSummits : Page
    {
        private HavocNetMaster _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (HavocNetMaster)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainAdministration");

            LoadPage();
        }

        private void LoadPage()
        {
            
        }
    }
}