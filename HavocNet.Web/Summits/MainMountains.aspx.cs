using System;
using Newtonsoft.Json;
using Summits;
using Summits.Repositories;

namespace HavocNet.Web.Summits
{
    public partial class MainMountains : System.Web.UI.Page
    {
        private Summits _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (Summits)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainMountains");

            LoadPage();
        }

        private void LoadPage()
        {
            var server = new SummitServer();
            server.Open();

            server.SQLParams.Add(_myMaster.aUser.ID);

            var repo = new TripRepository();
            var trips = repo.GetAll(server);

            this.data.InnerHtml = JsonConvert.SerializeObject(trips);

            server.Close();
        }
    }
}