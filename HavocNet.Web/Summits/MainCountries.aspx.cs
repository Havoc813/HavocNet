using System;
using Newtonsoft.Json;
using Summits;
using Summits.Repositories;

namespace HavocNet.Web.Summits
{
    public partial class MainCountries : System.Web.UI.Page
    {
        private Summits _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (Summits)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainCountries");

            LoadPage();
        }

        private void LoadPage()
        {
            var server = new SummitServer();
            server.Open();

            server.SQLParams.Add(_myMaster.aUser.ID);

            var repo = new StatRepository();
            var stats = repo.GetAllCountry(server);

            this.data.InnerHtml = JsonConvert.SerializeObject(stats);
            
            server.Close();
        }
    }
}