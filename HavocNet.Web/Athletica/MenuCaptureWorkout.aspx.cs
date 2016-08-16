using System;
using System.Web.UI;
using Athletica;
using Athletica.Repositories;

namespace HavocNet.Web.Athletica
{
    public partial class MenuCaptureWorkout : Page
    {
        private Athletica _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (Athletica)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainCapture");

            var aServer = new AthleticaServer();
            aServer.Open();

            BindControls(aServer);

            BindButtons();

            BindActivities(aServer);

            aServer.Close();
        }

        private void BindControls(AthleticaServer aServer)
        {
            var ctrls = new AthleticaControls(_myMaster.aUser, aServer, _myMaster.SelectedSport, "Race");
            ctrls.BindLocation(this.cboLocation, this.txtNewLocation);
            ctrls.BindRoute(this.cboRoute);
            ctrls.BindKit(this.cboKit, this.txtNewKit);
            ctrls.BindUnits(this.lblUnits);

            AthleticaControls.BindDate(this.txtDate);
            AthleticaControls.BindTimeOfDay(this.cboTimeOfDay);
            AthleticaControls.BindTime(this.txtHours, this.txtMinutes, this.txtSeconds);
            AthleticaControls.BindTextBoxes(new[] { this.txtComment, this.txtDistance });

            aServer.Close();

            this.phHiddens.Controls.Clear();
            foreach (Control ctrl in ctrls.Hiddens)
            {
                this.phHiddens.Controls.Add(ctrl);
            }
        }

        private void BindButtons()
        {
            this.cmdUpload.Enabled = _myMaster.WriteAccess;
            this.cmdUpload.Click += cmdUpload_Click;
        }

        private void BindActivities(AthleticaServer aServer)
        {
            var sessionRepository = new SessionRepository(aServer);

            this.phActivities.Controls.Add(sessionRepository.Render(this.txtDate.Text));
        }

        private void cmdUpload_Click(object sender, ImageClickEventArgs e)
        {
            var aServer = new AthleticaServer();

            var repository = new WorkoutRepository(aServer);

            var strTime = (Request["ctl00$cphMain$txtHours"] == "" ? "00" : Request["ctl00$cphMain$txtHours"].PadLeft(2, '0')) + ":" +
              (Request["ctl00$cphMain$txtMinutes"] == "" ? "00" : Request["ctl00$cphMain$txtMinutes"].PadLeft(2,'0')) + ":" +
              (Request["ctl00$cphMain$txtSeconds"] == "" ? "00" : Request["ctl00$cphMain$txtSeconds"].PadLeft(2, '0'));

            double distance;
            int locationID;
            if (Request["ctl00$cphMain$txtDistance"] == "")
            {
                var routeRepository = new RouteRepository(aServer);
                var route = routeRepository.Get(int.Parse(Request["ctl00$cphMain$cboRoute"]));

                distance = route.Distance;
                locationID = route.LocationID;
            }
            else
            {
                distance = double.Parse(Request["ctl00$cphMain$txtDistance"]);

                if (Request["ctl00$cphMain$txtNewLocation"] != "")
                {
                    var locationRepository = new LocationRepository(aServer);

                    locationID = locationRepository.Create(
                        Request["ctl00$cphMain$txtNewLocation"],
                        int.Parse(Request["ctl00$cphMain$cboSport"]),
                        _myMaster.aUser.ID);

                    _myMaster.Audit("Create Location", "Name=" + Request["ctl00$cphMain$txtNewLocation"]);
                }
                else
                {
                    locationID = int.Parse(Request["ctl00$cphMain$cboLocation"]);
                }
            }

            int kitID;
            if (Request["ctl00$cphMain$txtNewKit"] != "")
            {
                var kitRepository = new KitRepository(aServer);

                kitID = kitRepository.Create(
                    Request["ctl00$cphMain$txtNewKit"],
                    int.Parse(Request["ctl00$cphMain$cboSport"]),
                    _myMaster.aUser.ID);

                _myMaster.Audit("Create Kit", "Name=" + Request["ctl00$cphMain$txtNewKit"]);
            }
            else
            {
                kitID = int.Parse(Request["ctl00$cphMain$cboKit"]);
            }

            repository.Create(
                Request["ctl00$cphMain$txtDate"],
                Request["ctl00$cphMain$cboTimeOfDay"],
                int.Parse(Request["ctl00$cphMain$cboSport"]),
                "1900-01-01 " + strTime,
                locationID,
                kitID,
                Request["ctl00$cphMain$txtComment"],
                Request["ctl00$cphMain$hidTags"],
                distance,
                int.Parse(Request["ctl00$cphMain$cboRoute"] ?? "0")
                );
        }
    }
}