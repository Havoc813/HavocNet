using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Athletica;
using Athletica.Repositories;

namespace HavocNet.Web.Athletica
{
    public partial class MenuCaptureRegatta : Page
    {
        private Athletica _myMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            _myMaster = (Athletica)Master;
            if (_myMaster == null) return;
            _myMaster.LoadPage("MainCapture");

            BindControls();

            BindButtons();
        }

        private void BindPlaces()
        {
            AthleticaControls.BindTextBox(txtPlace1);
            AthleticaControls.BindTextBox(txtPlaceParticipants1);

            if (Page.IsPostBack)
            {
                var i = 2;
                while (Request["ctl00$cphMain$txtPlace" + i] != null)
                {
                    var aRow = new HtmlTableRow();

                    aRow.Cells.Add(new HtmlTableCell { InnerText = "Race " + i + " : Placed" });
                    var aCell1 = new HtmlTableCell();
                    aCell1.Controls.Add(new TextBox { ID = "txtPlace" + i, Text = Request["ctl00$cphMain$txtPlace" + i] });
                    aRow.Cells.Add(aCell1);
                    aRow.Cells.Add(new HtmlTableCell { InnerText = "of" });
                    var aCell3 = new HtmlTableCell();
                    aCell3.Controls.Add(new TextBox { ID = "txtPlaceParticipants" + i, Text = Request["ctl00$cphMain$txtPlaceParticipants" + i] });
                    aRow.Cells.Add(aCell3);

                    this.tblRaces.Rows.Add(aRow);

                    i++;
                }
            }
        }

        private void BindControls()
        {
            AthleticaControls.BindDate(this.txtDate);
            AthleticaControls.BindTimeOfDay(this.cboTimeOfDay);
            AthleticaControls.BindTime(this.txtHours, this.txtMinutes, this.txtSeconds);
            AthleticaControls.BindTextBox(this.txtComment);

            BindPlaces();

            var aServer = new AthleticaServer();
            aServer.Open();

            var ctrls = new AthleticaControls(_myMaster.aUser, aServer, _myMaster.SelectedSport, "Regatta");
            ctrls.BindLocation(this.cboLocation, this.txtNewLocation);
            ctrls.BindKit(this.cboKit, this.txtNewKit);

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

        private void cmdUpload_Click(object sender, ImageClickEventArgs e)
        {
            var aServer = new AthleticaServer();

            var repository = new RegattaRepository(aServer);

            var strTime = (Request["ctl00$cphMain$txtHours"] == "" ? "00" : Request["ctl00$cphMain$txtHours"].PadLeft(2, '0')) + ":" +
              (Request["ctl00$cphMain$txtMinutes"] == "" ? "00" : Request["ctl00$cphMain$txtMinutes"].PadLeft(2, '0')) + ":" +
              (Request["ctl00$cphMain$txtSeconds"] == "" ? "00" : Request["ctl00$cphMain$txtSeconds"].PadLeft(2, '0'));

            int locationID;
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

            var places = "";
            var participants = "";
            var i = 0;
            while (Request["ctl00$cphMain$txtPlace" + i] != null)
            {
                places += Request["ctl00$cphMain$txtPlace" + i] + "|";
                participants += Request["ctl00$cphMain$txtParticipants" + i] + "|";

                i++;
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
                places,
                participants
                );
        }
    }
}