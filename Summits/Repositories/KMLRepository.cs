using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using Summits.Administration;

namespace Summits.Repositories
{
    public class KMLRepository
    {
        private readonly int _userID;
        private readonly SummitServer _server;

        public KMLRepository(int userID, SummitServer server)
        {
            _userID = userID;
            _server = server;
        }

        public KML Get(string type)
        {
            var kml = new KML("World");
            var countries = new List<string>();

            _server.Open();
            _server.SQLParams.Add(_userID);

            var strSQL = @"SELECT 
	                a.ID,
	                a.Code,
	                a.Name,
	                a.Continent,
	                a.Longitude,
	                a.Latitude,
	                a.Zoom,
	                c.Name AS PointName,
	                c.Longitude AS PointLongitude,
	                c.Latitude AS PointLatitude,
	                isnull(c.Height,0) AS Height,
	                isnull(c.Prominence,0) AS Prominence,
	                isnull(c.WikiLink,'') AS WikiLink,
	                isnull(c.SummitPostLink,'') AS SummitPostLink,
	                isnull(convert(varchar,d.SummitDate,103),'') AS ClimbDate,
	                b.IsHighpoint
                FROM 
	                dbo.Countries a
	                INNER JOIN dbo.PointsToCountries b ON a.ID = b.CountryID
	                INNER JOIN dbo.Points c ON b.PointID = c.ID
	                INNER JOIN dbo.TripPoints d ON c.ID = d.PointID 
	                INNER JOIN dbo.Trips e ON d.TripID = e.ID
                WHERE
	                e.UserID = @Param0";

            var reader = _server.ExecuteReader(strSQL);

            while (reader.Read())
            {
                if(!countries.Contains(reader["ID"].ToString())) countries.Add(reader["ID"].ToString());

                kml.Markers.Add(new Marker(
                    reader["PointName"].ToString(), 
                    MakeMountainInfoWindow(reader),
                    reader["PointLongitude"].ToString(),
                    reader["PointLatitude"].ToString(), reader["IsHighpoint"].ToString() == "0" ? "yellow" : "blue"
                    ));
            }
            reader.Close();

            var strCountries = countries.Aggregate("", (current, country) => current + ("," + country)).Substring(1);

            strSQL = @"SELECT *, cast(a.Longitude AS DECIMAL(12,4)) AS Longitude2, cast(a.Latitude AS DECIMAL(12,4)) AS Latitude2, b.Name AS CountryName
                    FROM dbo.Coordinates a
                    INNER JOIN dbo.Countries b ON a.CountryID = b.ID 
                    WHERE CountryID IN (" + strCountries + @")
                    ORDER BY MultiPathID,CoordinateID";

            reader = _server.ExecuteReader(strSQL);

            var currPath = "";
            var currSubPath = new Dictionary<int, string>();

            if (reader.HasRows)
            {
                reader.Read();
                var currPathID = int.Parse(reader["MultiPathID"].ToString());
                var currSubPathID = int.Parse(reader["SubMultiPathID"].ToString());
                var currCountry = reader["CountryName"].ToString();
                var currShape = new Shape(reader["CountryName"].ToString(), "", Color.Green, 1);

                do
                {
                    if (reader["CountryName"].ToString() == currCountry)
                    {
                        if (int.Parse(reader["MultiPathID"].ToString()) == currPathID)
                        {
                            if (int.Parse(reader["SubMultiPathID"].ToString()) == 0)
                            {
                                currPath += Path(reader);
                            }
                            else if (int.Parse(reader["SubMultiPathID"].ToString()) == currSubPathID)
                            {
                                if (!currSubPath.ContainsKey(int.Parse(reader["SubMultiPathID"].ToString())))
                                {
                                    currSubPath.Add(int.Parse(reader["SubMultiPathID"].ToString()), Path(reader));
                                }
                                else
                                {
                                    currSubPath[int.Parse(reader["SubMultiPathID"].ToString())] += Path(reader);
                                }
                            }
                            else
                            {
                                currSubPath.Add(int.Parse(reader["SubMultiPathID"].ToString()), Path(reader));
                                currSubPathID = int.Parse(reader["SubMultiPathID"].ToString());
                            }
                        }
                        else
                        {
                            var polygon = new Polygon();
                            polygon.border = currPath;
                            polygon.cutOuts = currSubPath;
                        
                            currShape.Polygons.Add(polygon);

                            currPath = reader["Longitude2"] + "," + reader["Latitude2"] + ",0 ";
                            currPathID = int.Parse(reader["MultiPathID"].ToString());
                            currSubPathID = 0;
                            currSubPath = new Dictionary<int, string>();
                        }                        
                    }
                    else
                    {
                        var polygon = new Polygon();
                        polygon.border = currPath;
                        polygon.cutOuts = currSubPath;

                        currShape.Polygons.Add(polygon);

                        currPath = reader["Longitude2"] + "," + reader["Latitude2"] + ",0 ";
                        currPathID = int.Parse(reader["MultiPathID"].ToString());
                        currSubPathID = 0;
                        currSubPath = new Dictionary<int, string>();

                        kml.Shapes.Add(currShape);

                        currShape = new Shape(reader["CountryName"].ToString(), "", Color.Green, 1);

                        if (int.Parse(reader["SubMultiPathID"].ToString()) == 0)
                        {
                            currPath += Path(reader);
                        }
                        else if (int.Parse(reader["SubMultiPathID"].ToString()) == currSubPathID)
                        {
                            if (!currSubPath.ContainsKey(int.Parse(reader["SubMultiPathID"].ToString())))
                            {
                                currSubPath.Add(int.Parse(reader["SubMultiPathID"].ToString()), Path(reader));
                            }
                            else
                            {
                                currSubPath[int.Parse(reader["SubMultiPathID"].ToString())] += Path(reader);
                            }
                        }
                        else
                        {
                            currSubPath.Add(int.Parse(reader["SubMultiPathID"].ToString()), Path(reader));
                            currSubPathID = int.Parse(reader["SubMultiPathID"].ToString());
                        }

                        currCountry = reader["CountryName"].ToString();
                    }

                } while (reader.Read());
            }
            reader.Close();

            _server.Close();

            return kml;
        }

        private string Path(DbDataReader reader)
        {
            return reader["Longitude2"] + "," + reader["Latitude2"] + ",0";
        }

        private string MakeMountainInfoWindow(DbDataReader reader)
        {
            var info = "<table>";

            info += "<tr><td colspan='2'><b>" + reader["PointName"] + "</b></td></tr>";
            info += "<tr><td>Highpoint Of:</td><td>" + reader["Name"] + "</td></tr>";
            info += "<tr><td>Height:</td><td>" + double.Parse(reader["Height"].ToString()).ToString("#,##") + "m (" + (double.Parse(reader["Height"].ToString()) * 3.2808399).ToString("#,###") + "ft)</td></tr>";

            if(int.Parse(reader["prominence"].ToString()) != 0) info += "<tr><td>Prominence:</td><td>"  + double.Parse(reader["Prominence"].ToString()).ToString("#,##") + "m (" + (double.Parse(reader["prominence"].ToString()) * 3.2808399).ToString("#,###") + "ft)</td></tr>";
            if(reader["WikiLink"].ToString() != "") info += "<tr><td>Wikipedia:</td><td><a href='" + reader["WikiLink"] + "'>" + reader["WikiLink"] + "</a></td></tr>";
            if(reader["SummitPostLink"].ToString() != "") info += "<tr><td>Summit Post:</td><td><a href='" + reader["WikiLink"] + "'>" + reader["WikiLink"] + "</a></td></tr>";

            info += "</table>";

            return info;
        }

        public KML Get(string type, string countryName)
        {            
            var kml = new KML(countryName);

            //County
            //Fetch all county highpoints


            //Fetch country borders



            return kml;
        }
    }
}
