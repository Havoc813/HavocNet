using System;
using System.IO;
using HavocNet.Web;
using Summits;

public partial class UnpackKML : System.Web.UI.Page
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
        cmdUnpack.Click += CmdUnpackClick;
    }

    void CmdUnpackClick(object sender, EventArgs e)
    {
        var server = new SummitServer();
        server.Open();

        var filename = Server.MapPath("../../App_Data/Uploads/" + upFile.FileName);

        upFile.SaveAs(filename);

        server.ExecuteNonQuery("TRUNCATE TABLE Working_Countries");
        server.ExecuteNonQuery("TRUNCATE TABLE Working_Coordinates");

        var reader = new StreamReader(filename);
        var countryID = 0;

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();

            if (line.Contains(@"CTYUA15NM"""))
            {
                line = line.Replace(@"<SimpleData name=""CTYUA15NM"">", "").Replace("</SimpleData>", "").Trim();

                server.SQLParams.Clear();
                server.SQLParams.Add("");
                server.SQLParams.Add(line);
                server.SQLParams.Add("");
                server.SQLParams.Add(0.0);
                server.SQLParams.Add(0.0);
                server.SQLParams.Add(14);

                if (line.Length < 50)
                {
                    countryID = int.Parse(server.ExecuteScalar(@"INSERT INTO Working_Countries (Code, Name, Continent, Longitude, Latitude, Zoom) SELECT @Param0, @Param1, @Param2, @Param3, @Param4, @Param5 SELECT ID FROM Working_Countries WHERE Name = @Param1").ToString());
                }                
            }
            else if (line.Contains("MultiGeometry"))
            {
                line = line.Replace("<MultiGeometry>", "").Replace("</MultiGeometry>", "");

                var i = 1;
                foreach (var miniLine in line.Split(new string[] {@"</Polygon><Polygon>"}, StringSplitOptions.None))
                {
                    var corrected = miniLine.Replace("<Polygon>", "").Replace("</Polygon>", "").Split(new string[] { @"</outerBoundaryIs><innerBoundaryIs>" }, StringSplitOptions.None);

                    var outerbit = corrected[0].Replace("<outerBoundaryIs><LinearRing><coordinates>", "").Replace("</coordinates></LinearRing>", "").Replace("</outerBoundaryIs>", "");

                    foreach (var lineSegment in outerbit.Trim().Split(' '))
                    {
                        server.SQLParams.Clear();
                        server.SQLParams.Add(countryID);
                        server.SQLParams.Add(i);
                        server.SQLParams.Add(float.Parse(lineSegment.Split(',')[0].Trim()));
                        server.SQLParams.Add(float.Parse(lineSegment.Split(',')[1].Trim()));
                        server.SQLParams.Add(0);

                        server.ExecuteNonQuery(@"INSERT INTO Working_Coordinates (CountryID, MultiPathID, Longitude, Latitude, SubMultiPathID) SELECT @Param0, @Param1, @Param2, @Param3, @Param4");
                    }

                    if (corrected.Length > 1)
                    {
                        var innerbits = corrected[1].Split(new string[] { @"</innerBoundaryIs><innerBoundaryIs>" }, StringSplitOptions.None);
                        var j = 1;
                        foreach (var innerbit in innerbits)
                        {
                            var correctedInnerbit = innerbit.Replace("<innerBoundaryIs>", "")
                                                            .Replace("</innerBoundaryIs>", "")
                                                            .Replace("<LinearRing>", "")
                                                            .Replace("</LinearRing>", "")
                                                            .Replace("<coordinates>", "")
                                                            .Replace("</coordinates>", "");

                            foreach (var lineSegment in correctedInnerbit.Trim().Split(' '))
                            {
                                server.SQLParams.Clear();
                                server.SQLParams.Add(countryID);
                                server.SQLParams.Add(i);
                                server.SQLParams.Add(float.Parse(lineSegment.Split(',')[0].Trim()));
                                server.SQLParams.Add(float.Parse(lineSegment.Split(',')[1].Trim()));
                                server.SQLParams.Add(j);

                                server.ExecuteNonQuery(@"INSERT INTO Working_Coordinates (CountryID, MultiPathID, Longitude, Latitude, SubMultiPathID) SELECT @Param0, @Param1, @Param2, @Param3, @Param4");
                            }
                            j++;
                        }
                    }

                    i++;
                }
            }
            else if (line.Contains("coordinates"))
            {
                var corrected = line.Replace("<Polygon>", "").Replace("</Polygon>", "").Split(new string[] { @"</outerBoundaryIs><innerBoundaryIs>" }, StringSplitOptions.None);

                var outerbit = corrected[0].Replace("<outerBoundaryIs><LinearRing><coordinates>", "").Replace("</coordinates></LinearRing>", "").Replace("</outerBoundaryIs>", "");

                foreach (var lineSegment in outerbit.Trim().Split(' '))
                {
                    server.SQLParams.Clear();
                    server.SQLParams.Add(countryID);
                    server.SQLParams.Add(1);
                    server.SQLParams.Add(float.Parse(lineSegment.Split(',')[0].Trim()));
                    server.SQLParams.Add(float.Parse(lineSegment.Split(',')[1].Trim()));
                    server.SQLParams.Add(0);

                    server.ExecuteNonQuery(@"INSERT INTO Working_Coordinates (CountryID, MultiPathID, Longitude, Latitude, SubMultiPathID) SELECT @Param0, @Param1, @Param2, @Param3, @Param4");
                }

                if (corrected.Length > 1)
                {
                    var innerbits = corrected[1].Split(new string[] { @"</innerBoundaryIs><innerBoundaryIs>" }, StringSplitOptions.None);
                    var j = 1;
                    foreach (var innerbit in innerbits)
                    {
                        var correctedInnerbit = innerbit.Replace("<innerBoundaryIs>", "")
                                                        .Replace("</innerBoundaryIs>", "")
                                                        .Replace("<LinearRing>", "")
                                                        .Replace("</LinearRing>", "")
                                                        .Replace("<coordinates>", "")
                                                        .Replace("</coordinates>", "");

                        foreach (var lineSegment in correctedInnerbit.Trim().Split(' '))
                        {
                            server.SQLParams.Clear();
                            server.SQLParams.Add(countryID);
                            server.SQLParams.Add(1);
                            server.SQLParams.Add(float.Parse(lineSegment.Split(',')[0].Trim()));
                            server.SQLParams.Add(float.Parse(lineSegment.Split(',')[1].Trim()));
                            server.SQLParams.Add(j);

                            server.ExecuteNonQuery(@"INSERT INTO Working_Coordinates (CountryID, MultiPathID, Longitude, Latitude, SubMultiPathID) SELECT @Param0, @Param1, @Param2, @Param3, @Param4");
                        }
                        j++;
                    }
                }
            }
        }
        reader.Close();

        server.Close();
    }
}