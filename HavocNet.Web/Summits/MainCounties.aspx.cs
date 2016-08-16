using System;
using Newtonsoft.Json;
using Summits;
using Summits.Repositories;

namespace HavocNet.Web
{
    public partial class MainCounties : System.Web.UI.Page
    {
        private Summits.Summits _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (Summits.Summits)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainCounties");

            LoadPage();
        }

        private void LoadPage()
        {
            var server = new SummitServer();
            server.Open();

            server.SQLParams.Add(_myMaster.aUser.ID);

            var repo = new StatRepository();
            var stats = repo.GetAllCounty(server);

            this.data.InnerHtml = JsonConvert.SerializeObject(stats);

            server.Close();
        }
    }
}