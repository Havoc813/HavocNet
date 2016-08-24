using System;
using System.Web.UI.WebControls;
using HavocNet.Web;
using Summits;
using Summits.Repositories;

public partial class GenerateKML : System.Web.UI.Page
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
        var server = new SummitServer();
        server.Open();

        BindUsers(server);

        BindCountries();

        cmdGenerate.Click += cmdGenerate_Click;

        server.Close();
    }

    private void BindCountries()
    {
        this.cboCountries.Items.Add(new ListItem("World", "Countries"));
        this.cboCountries.Items.Add(new ListItem("England", "England"));
        this.cboCountries.Items.Add(new ListItem("Scotland", "Scotland"));
        this.cboCountries.Items.Add(new ListItem("Wales", "Wales"));
        this.cboCountries.Items.Add(new ListItem("Northern Ireland", "NorthernIreland"));
        this.cboCountries.Items.Add(new ListItem("Ireland", "Ireland"));
        this.cboCountries.Items.Add(new ListItem("USA", "USA"));
        this.cboCountries.Items.Add(new ListItem("Canada", "Canada"));

        if (Page.IsPostBack) this.cboCountries.Items.FindByValue(Request["ctl00$cphMain$cboCountries"]).Selected = true;
        else this.cboCountries.Items[0].Selected = true;
    }

    private void BindUsers(SummitServer server)
    {
        var reader = server.ExecuteReader("SELECT ID AS Value, Surname + ', ' + FirstName + ' (' + Username + ')' AS [Text] FROM HavocNet..tblUsers");

        this.cboUsers.DataSource = reader;
        this.cboUsers.DataTextField = "Text";
        this.cboUsers.DataValueField = "Value";
        this.cboUsers.DataBind();

        if (Page.IsPostBack) this.cboUsers.Items.FindByValue(Request["ctl00$cphMain$cboUsers"]).Selected = true;
        else this.cboUsers.Items[0].Selected = true;

    }

    void cmdGenerate_Click(object sender, EventArgs e)
    {
        var repo = new KMLRepository(int.Parse(this.cboUsers.SelectedValue), new SummitServer());

        if (this.cboCountries.SelectedValue == "Countries")
        {
            var kml = repo.GetHighpointed();
            kml.Generate(Server.MapPath("../KML/" + this.cboUsers.SelectedValue + "/" + this.cboCountries.SelectedValue + "_Highpointed_borders.kml"));

            kml = repo.GetVisited();
            kml.Generate(Server.MapPath("../KML/" + this.cboUsers.SelectedValue + "/" + this.cboCountries.SelectedValue + "_Visited_borders.kml"));
        }
        else
        {
            var kml = repo.GetHighpointed(this.cboCountries.SelectedItem.Text);
            kml.Generate(Server.MapPath("../KML/" + this.cboUsers.SelectedValue + "/" + this.cboCountries.SelectedValue + "_Highpointed_borders.kml"));

            kml = repo.GetVisited(this.cboCountries.SelectedItem.Text);
            kml.Generate(Server.MapPath("../KML/" + this.cboUsers.SelectedValue + "/" + this.cboCountries.SelectedValue + "_Visited_borders.kml"));
        }

        var server = new SummitServer();
        server.Open();

        server.SQLParams.Add(this.cboUsers.SelectedValue);

        server.ExecuteNonQuery("UPDATE HavocNet..tblUsers SET Version = ISNULL(Version, 0) + 1 WHERE ID = @Param0");

        server.Close();
    }
}