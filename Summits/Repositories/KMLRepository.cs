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

        public KML GetHighpointed()
        {
            var kml = new KML("World");
            var countries = new List<string>();
            var climbedCountries = new List<string>();

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
	                b.IsHighpoint,
                    isnull(UserID,0) AS UserID
                FROM 
	                dbo.Countries a
	                INNER JOIN dbo.PointsToCountries b ON a.ID = b.CountryID
	                INNER JOIN dbo.Points c ON b.PointID = c.ID
	                LEFT OUTER JOIN dbo.TripPoints d ON c.ID = d.PointID 
	                LEFT OUTER JOIN dbo.Trips e ON d.TripID = e.ID AND UserID = @Param0
                WHERE 
                    b.IsHighpoint = 1 ";

            var reader = _server.ExecuteReader(strSQL);

            while (reader.Read())
            {
                if(!countries.Contains(reader["ID"].ToString()) && reader["UserID"].ToString() != "0") countries.Add(reader["ID"].ToString());

                kml.Markers.Add(new Marker(
                    reader["PointName"].ToString(), 
                    MakeMountainInfoWindow(reader),
                    reader["PointLongitude"].ToString(),
                    reader["PointLatitude"].ToString(), 
                    reader["UserID"].ToString() == "0" ? "blue" : "green"
                    ));
            }
            reader.Close();

            var strCountries = countries.Aggregate("", (current, country) => current + ("," + country)).Substring(1);

            strSQL = @"SELECT 
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
	                b.IsHighpoint,
                    isnull(UserID,0) AS UserID
                FROM 
	                dbo.Countries a
	                INNER JOIN dbo.PointsToCountries b ON a.ID = b.CountryID
	                INNER JOIN dbo.Points c ON b.PointID = c.ID
	                LEFT OUTER JOIN dbo.TripPoints d ON c.ID = d.PointID 
	                LEFT OUTER JOIN dbo.Trips e ON d.TripID = e.ID AND UserID = 1
                WHERE 
                    b.IsHighpoint IS NULL
	                AND UserID = @Param0
	                AND CountryID NOT IN (" + strCountries + @")
                ORDER BY 
                    a.ID";

            reader = _server.ExecuteReader(strSQL);

            while (reader.Read())
            {
                if (!countries.Contains(reader["ID"].ToString()))
                {
                    countries.Add(reader["ID"].ToString());
                    climbedCountries.Add(reader["ID"].ToString());

                    kml.Markers.Add(new Marker(
                        reader["PointName"].ToString(),
                        MakeMountainInfoWindow(reader),
                        reader["PointLongitude"].ToString(),
                        reader["PointLatitude"].ToString(),
                        "orange"
                        ));
                }
            }
            reader.Close();

            strCountries = countries.Aggregate("", (current, country) => current + ("," + country)).Substring(1);

            strSQL = @"SELECT *, cast(a.Longitude AS DECIMAL(12,4)) AS Longitude2, cast(a.Latitude AS DECIMAL(12,4)) AS Latitude2, b.Name AS CountryName
                    FROM dbo.Coordinates a
                    INNER JOIN dbo.Countries b ON a.CountryID = b.ID 
                    WHERE CountryID IN (" + strCountries + @")
                    ORDER BY CountryID, MultiPathID,CoordinateID";

            reader = _server.ExecuteReader(strSQL);

            var currPath = "";
            var currSubPath = new Dictionary<int, string>();

            if (reader.HasRows)
            {
                reader.Read();
                var currPathID = int.Parse(reader["MultiPathID"].ToString());
                var currSubPathID = int.Parse(reader["SubMultiPathID"].ToString());
                var currCountry = reader["CountryName"].ToString();
                var currShape = new Shape(reader["CountryName"].ToString(), "", GetColour(reader, climbedCountries), 1);

                do
                {
                    if (reader["CountryName"].ToString() == currCountry)
                    {
                        if (int.Parse(reader["MultiPathID"].ToString()) == currPathID)
                        {
                            AddToPath(reader, ref currPath, ref currSubPath, ref currSubPathID);
                        }
                        else
                        {
                            AddToPolygons(reader, ref currPath, ref currSubPath, ref currShape, ref currPathID, ref currSubPathID);
                        }                        
                    }
                    else
                    {
                        AddToPolygons(reader, ref currPath, ref currSubPath, ref currShape, ref currPathID, ref currSubPathID);

                        kml.Shapes.Add(currShape);

                        currShape = new Shape(reader["CountryName"].ToString(), "", GetColour(reader, climbedCountries), 1);

                        AddToPath(reader, ref currPath, ref currSubPath, ref currSubPathID);

                        currCountry = reader["CountryName"].ToString();
                    }

                } while (reader.Read());

                var polygon = new Polygon();
                polygon.border = currPath;
                polygon.cutOuts = currSubPath;

                currShape.Polygons.Add(polygon);

                kml.Shapes.Add(currShape);
            }
            reader.Close();

            _server.Close();

            return kml;
        }

        private Color GetColour(DbDataReader reader, List<string> contains )
        {
            return contains.Contains(reader["CountryID"].ToString()) ? Color.Orange : Color.Green;
        }

        private void AddToPolygons(DbDataReader reader, ref string currPath, ref Dictionary<int, string> currSubPath, ref Shape currShape, ref int currPathID, ref int currSubPathID)
        {
            var polygon = new Polygon();
            polygon.border = currPath;
            polygon.cutOuts = currSubPath;

            currShape.Polygons.Add(polygon);

            currPath = Path(reader);
            currPathID = int.Parse(reader["MultiPathID"].ToString());
            currSubPathID = 0;
            currSubPath = new Dictionary<int, string>();
        }

        private void AddToPath(DbDataReader reader, ref string currPath, ref Dictionary<int, string> currSubPath, ref int currSubPathID)
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

        private string Path(DbDataReader reader)
        {
            return reader["Longitude2"] + "," + reader["Latitude2"] + ",0 ";
        }

        private string MakeMountainInfoWindow(DbDataReader reader)
        {
            var info = "<table>";

            info += "<tr><td>Highpoint Of:</td><td>" + reader["Name"] + "</td></tr>";
            info += "<tr><td>Height:</td><td>" + double.Parse(reader["Height"].ToString()).ToString("#,##") + "m (" + (double.Parse(reader["Height"].ToString()) * 3.2808399).ToString("#,###") + "ft)</td></tr>";

            if(int.Parse(reader["Prominence"].ToString()) != 0) info += "<tr><td>Prominence:</td><td>"  + double.Parse(reader["Prominence"].ToString()).ToString("#,##") + "m (" + (double.Parse(reader["prominence"].ToString()) * 3.2808399).ToString("#,###") + "ft)</td></tr>";
            if(reader["WikiLink"].ToString() != "") info += "<tr><td>Wikipedia:</td><td><a href='" + reader["WikiLink"] + "'>" + reader["WikiLink"] + "</a></td></tr>";
            if (reader["SummitPostLink"].ToString() != "") info += "<tr><td>Summit Post:</td><td><a href='" + reader["SummitPostLink"] + "'>" + reader["SummitPostLink"] + "</a></td></tr>";

            info += "</table>";

            return info;
        }

        public KML GetVisited()
        {
            var kml = new KML("World");
            
            _server.Open();
            _server.SQLParams.Add(_userID);

            var strSQL = @"SELECT 
	                    a.ID,
	                    a.Code,
	                    a.Name AS CountryName,
	                    a.Continent,
	                    a.Longitude,
	                    a.Latitude,
	                    a.Zoom,
	                    b.VisitTypeID, 
                        d.*,
	                    cast(d.Longitude AS DECIMAL(12,4)) AS Longitude2, 
	                    cast(d.Latitude AS DECIMAL(12,4)) AS Latitude2
                    FROM 
	                    dbo.Countries a
	                    INNER JOIN dbo.TripCountries b ON a.ID = b.CountryID
	                    INNER JOIN dbo.Trips c ON b.TripID = c.ID
	                    INNER JOIN dbo.Coordinates d ON a.ID = d.CountryID
                    WHERE 
	                    UserID = @Param0 
                    ORDER BY 
                        d.CountryID, MultiPathID,CoordinateID";

            var reader = _server.ExecuteReader(strSQL);

            var currPath = "";
            var currSubPath = new Dictionary<int, string>();

            if (reader.HasRows)
            {
                reader.Read();
                var currPathID = int.Parse(reader["MultiPathID"].ToString());
                var currSubPathID = int.Parse(reader["SubMultiPathID"].ToString());
                var currCountry = reader["CountryName"].ToString();
                var currShape = new Shape(reader["CountryName"].ToString(), "", GetVisitedColour(reader), 1);

                do
                {
                    if (reader["CountryName"].ToString() == currCountry)
                    {
                        if (int.Parse(reader["MultiPathID"].ToString()) == currPathID)
                        {
                            AddToPath(reader, ref currPath, ref currSubPath, ref currSubPathID);
                        }
                        else
                        {
                            AddToPolygons(reader, ref currPath, ref currSubPath, ref currShape, ref currPathID, ref currSubPathID);
                        }
                    }
                    else
                    {
                        AddToPolygons(reader, ref currPath, ref currSubPath, ref currShape, ref currPathID, ref currSubPathID);

                        kml.Shapes.Add(currShape);

                        currShape = new Shape(reader["CountryName"].ToString(), "", GetVisitedColour(reader), 1);

                        AddToPath(reader, ref currPath, ref currSubPath, ref currSubPathID);

                        currCountry = reader["CountryName"].ToString();
                    }

                } while (reader.Read());

                var polygon = new Polygon();
                polygon.border = currPath;
                polygon.cutOuts = currSubPath;

                currShape.Polygons.Add(polygon);

                kml.Shapes.Add(currShape);
            }
            reader.Close();

            _server.Close();

            return kml;
        }

        private Color GetVisitedColour(DbDataReader reader)
        {
            switch (reader["VisitTypeID"].ToString())
            {
                case "1":
                case "2":
                    return Color.Green;
                default:
                    return Color.Orange;
            }
        }

        public KML GetVisited(string countryName)
        {
            var kml = new KML(countryName);

            _server.Open();
            _server.SQLParams.Add(_userID);
            _server.SQLParams.Add(countryName);

            var strSQL = @"SELECT 
	                a.ID,
	                a.Name AS CountyName,
	                a.Longitude,
	                a.Latitude,
	                a.Zoom,
	                b.VisitTypeID, 
                    d.*,
	                cast(d.Longitude AS DECIMAL(12,4)) AS Longitude2, 
	                cast(d.Latitude AS DECIMAL(12,4)) AS Latitude2
                FROM 
	                dbo.Counties a
	                INNER JOIN dbo.TripCounties b ON a.ID = b.CountyID
	                INNER JOIN dbo.Trips c ON b.TripID = c.ID
	                INNER JOIN dbo.Coordinates d ON a.ID = d.CountryID
                WHERE 
	                UserID = @Param0
                    AND a.CountryID = (SELECT ID FROM dbo.Countries WHERE Name = @Param1)
                ORDER BY 
                    d.CountryID, MultiPathID,CoordinateID";

            var reader = _server.ExecuteReader(strSQL);

            var currPath = "";
            var currSubPath = new Dictionary<int, string>();

            if (reader.HasRows)
            {
                reader.Read();
                var currPathID = int.Parse(reader["MultiPathID"].ToString());
                var currSubPathID = int.Parse(reader["SubMultiPathID"].ToString());
                var currCounty = reader["CountyName"].ToString();
                var currShape = new Shape(reader["CountyName"].ToString(), "", GetVisitedColour(reader), 1);

                do
                {
                    if (reader["CountyName"].ToString() == currCounty)
                    {
                        if (int.Parse(reader["MultiPathID"].ToString()) == currPathID)
                        {
                            AddToPath(reader, ref currPath, ref currSubPath, ref currSubPathID);
                        }
                        else
                        {
                            AddToPolygons(reader, ref currPath, ref currSubPath, ref currShape, ref currPathID, ref currSubPathID);
                        }
                    }
                    else
                    {
                        AddToPolygons(reader, ref currPath, ref currSubPath, ref currShape, ref currPathID, ref currSubPathID);

                        kml.Shapes.Add(currShape);

                        currShape = new Shape(reader["CountyName"].ToString(), "", GetVisitedColour(reader), 1);

                        AddToPath(reader, ref currPath, ref currSubPath, ref currSubPathID);

                        currCounty = reader["CountyName"].ToString();
                    }

                } while (reader.Read());

                var polygon = new Polygon();
                polygon.border = currPath;
                polygon.cutOuts = currSubPath;

                currShape.Polygons.Add(polygon);

                kml.Shapes.Add(currShape);
            }
            reader.Close();

            _server.Close();

            return kml;
        }
        
        public KML GetHighpointed(string countryName)
        {
            var kml = new KML(countryName);
            var counties = new List<string>();
            var climbedCounties = new List<string>();

            _server.Open();
            _server.SQLParams.Add(_userID);
            _server.SQLParams.Add(countryName);

            var strSQL = @"SELECT 
	                a.ID,
	                a.Name,
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
	                b.IsHighpoint,
                    isnull(UserID,0) AS UserID
                FROM 
	                dbo.Counties a
	                INNER JOIN dbo.PointsToCounties b ON a.ID = b.CountyID
	                INNER JOIN dbo.Points c ON b.PointID = c.ID
	                LEFT OUTER JOIN dbo.TripPoints d ON c.ID = d.PointID 
	                LEFT OUTER JOIN dbo.Trips e ON d.TripID = e.ID AND UserID = @Param0
                WHERE 
                    b.IsHighpoint = 1
	                AND CountryID = (SELECT ID FROM dbo.Countries WHERE Name = @Param1) ";

            var reader = _server.ExecuteReader(strSQL);

            while (reader.Read())
            {
                if (!counties.Contains(reader["ID"].ToString()) && reader["UserID"].ToString() != "0") counties.Add(reader["ID"].ToString());

                kml.Markers.Add(new Marker(
                    reader["PointName"].ToString(),
                    MakeMountainInfoWindow(reader),
                    reader["PointLongitude"].ToString(),
                    reader["PointLatitude"].ToString(),
                    reader["UserID"].ToString() == "0" ? "blue" : "green"
                    ));
            }
            reader.Close();

            var strCounties = counties.Aggregate("", (current, county) => current + ("," + county)).Substring(1);

            strSQL = @"SELECT 
	                    a.ID,
	                    a.Name,
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
	                    b.IsHighpoint,
                        isnull(UserID,0) AS UserID
                    FROM 
	                    dbo.Counties a
	                    INNER JOIN dbo.PointsToCounties b ON a.ID = b.CountyID
	                    INNER JOIN dbo.Points c ON b.PointID = c.ID
	                    LEFT OUTER JOIN dbo.TripPoints d ON c.ID = d.PointID 
	                    LEFT OUTER JOIN dbo.Trips e ON d.TripID = e.ID AND UserID = @Param0
                    WHERE 
                        b.IsHighpoint IS NULL
	                    AND UserID = @Param0
	                    AND CountyID NOT IN (" + strCounties + @")
	                    AND CountryID = (SELECT ID FROM dbo.Countries WHERE Name = @Param1) 
                    ORDER BY 
                        a.ID";

            reader = _server.ExecuteReader(strSQL);

            while (reader.Read())
            {
                if (!counties.Contains(reader["ID"].ToString()))
                {
                    counties.Add(reader["ID"].ToString());
                    climbedCounties.Add(reader["ID"].ToString());

                    kml.Markers.Add(new Marker(
                        reader["PointName"].ToString(),
                        MakeMountainInfoWindow(reader),
                        reader["PointLongitude"].ToString(),
                        reader["PointLatitude"].ToString(),
                        "orange"
                        ));
                }
            }
            reader.Close();

            strCounties = counties.Aggregate("", (current, county) => current + ("," + county)).Substring(1);

            strSQL = @"SELECT *, cast(a.Longitude AS DECIMAL(12,4)) AS Longitude2, cast(a.Latitude AS DECIMAL(12,4)) AS Latitude2, b.Name AS CountyName
                    FROM dbo.Coordinates a
                    INNER JOIN dbo.Counties b ON a.CountryID = b.ID 
                    WHERE a.CountryID IN (" + strCounties + @")
                    ORDER BY a.CountryID, MultiPathID,CoordinateID";

            reader = _server.ExecuteReader(strSQL);

            var currPath = "";
            var currSubPath = new Dictionary<int, string>();

            if (reader.HasRows)
            {
                reader.Read();
                var currPathID = int.Parse(reader["MultiPathID"].ToString());
                var currSubPathID = int.Parse(reader["SubMultiPathID"].ToString());
                var currCounty = reader["CountyName"].ToString();
                var currShape = new Shape(reader["CountyName"].ToString(), "", GetColour(reader, climbedCounties), 1);

                do
                {
                    if (reader["CountyName"].ToString() == currCounty)
                    {
                        if (int.Parse(reader["MultiPathID"].ToString()) == currPathID)
                        {
                            AddToPath(reader, ref currPath, ref currSubPath, ref currSubPathID);
                        }
                        else
                        {
                            AddToPolygons(reader, ref currPath, ref currSubPath, ref currShape, ref currPathID, ref currSubPathID);
                        }
                    }
                    else
                    {
                        AddToPolygons(reader, ref currPath, ref currSubPath, ref currShape, ref currPathID, ref currSubPathID);

                        kml.Shapes.Add(currShape);

                        currShape = new Shape(reader["CountyName"].ToString(), "", GetColour(reader, climbedCounties), 1);

                        AddToPath(reader, ref currPath, ref currSubPath, ref currSubPathID);

                        currCounty = reader["CountyName"].ToString();
                    }

                } while (reader.Read());

                var polygon = new Polygon();
                polygon.border = currPath;
                polygon.cutOuts = currSubPath;

                currShape.Polygons.Add(polygon);

                kml.Shapes.Add(currShape);
            }
            reader.Close();

            _server.Close();

            return kml;
        }
    }
}
