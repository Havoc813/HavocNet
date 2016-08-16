using System;
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
        cmdGenerate.Click += cmdGenerate_Click;
    }

    void cmdGenerate_Click(object sender, EventArgs e)
    {
        var repo = new KMLRepository(1, new SummitServer());
        var kml = repo.Get("");
        kml.Generate(Server.MapPath("../KML/"));

        //Test

    }
}