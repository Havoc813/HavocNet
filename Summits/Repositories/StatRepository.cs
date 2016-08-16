using System.Collections.Generic;
using Summits.Models;

namespace Summits.Repositories
{
    public class StatRepository
    {
        public List<Stat> GetAllCountry(SummitServer server)
        {
            var stats = new List<Stat>();

            var strSQL = @"SELECT Total.Continent as Name, 
                            isnull(CountryCount,0) AS CountryCount, 
                            isnull(SteppedInCount, 0) AS SteppedInCount, 
                            isnull(VisitedCount, 0) AS VisitedCount, 
                            isnull(HighpointedCount, 0) AS HighpointedCount, 
                            isnull(ClimbedInCount, 0) AS ClimbedInCount 
                    FROM 
                    (select Continent, count(*) as CountryCount from Countries group by Continent) Total
                    left outer join 
                    (select Continent, count(distinct CountryID) as SteppedInCount from dbo.Trips a
                    full outer join dbo.TripCountries b on a.ID = b.TripID 
                    inner join dbo.Countries c on b.CountryID = c.ID
                    where UserID = @Param0
                    group by Continent) SteppedIn on Total.Continent = SteppedIn.Continent
                    left outer join
                    (select Continent, count(distinct CountryID) as VisitedCount from dbo.Trips a
                    full outer join dbo.TripCountries b on a.ID = b.TripID 
                    inner join dbo.Countries c on b.CountryID = c.ID
                    where VisitTypeID not in (3,4) and UserID = @Param0
                    group by Continent) Visited on Total.Continent = Visited.Continent
                    left outer join
                    (select Continent, count(*) as HighpointedCount from 
                    dbo.Countries a
                    inner join dbo.PointsToCountries b on a.ID = b.CountryID
                    inner join dbo.TripPoints c on b.PointID = c.PointID
                    inner join dbo.Trips d on d.ID = c.TripID
                    where UserID = @Param0 AND IsHighpoint = 1
                    group by Continent) Highpointed on Total.Continent = Highpointed.Continent
                    left outer join
                    (select Continent, count(distinct CountryID) as ClimbedInCount from dbo.Trips t inner join dbo.TripPoints a on t.ID = a.TripID
                    inner join dbo.Points b on PointID = b.ID
                    inner join dbo.PointsToCountries c on b.ID = c.PointID
                    inner join dbo.Countries d on c.CountryID = d.ID
                    where UserID = @Param0
                    group by Continent) ClimbedIn on Total.Continent = ClimbedIn.Continent";

            var reader = server.ExecuteReader(strSQL);

            while (reader.Read())
            {
                var stat = new Stat(
                    reader["Name"].ToString(),
                    int.Parse(reader["CountryCount"].ToString()),
                    int.Parse(reader["VisitedCount"].ToString()),
                    int.Parse(reader["SteppedInCount"].ToString()),
                    int.Parse(reader["HighpointedCount"].ToString()),
                    int.Parse(reader["ClimbedInCount"].ToString())
                    );
                stats.Add(stat);
            }
            reader.Close();

            return stats;
        }

        public List<Stat> GetAllCounty(SummitServer server)
        {
            var stats = new List<Stat>();

            var strSQL = @"SELECT Total.County as Name, 
                                isnull(CountyCount,0) AS CountyCount, 
                                isnull(SteppedInCount, 0) AS SteppedInCount, 
                                isnull(VisitedCount, 0) AS VisitedCount, 
                                isnull(HighpointedCount, 0) AS HighpointedCount, 
                                isnull(ClimbedInCount, 0) AS ClimbedInCount 
                        FROM 
                        (select b.Name AS County, count(*) as CountyCount from Counties a inner join Countries b on a.CountryID = b.ID group by b.Name) Total
                        left outer join 
                        (select c.Name AS County, count(distinct CountyID) as SteppedInCount from dbo.Trips a
                        full outer join dbo.TripCounties b on a.ID = b.TripID 
                        inner join dbo.Countries c on b.CountyID = c.ID
                        where UserID = @Param0
                        group by c.Name) SteppedIn on Total.County = SteppedIn.County
                        left outer join
                        (select c.Name AS County, count(distinct CountyID) as VisitedCount from dbo.Trips a
                        full outer join dbo.TripCounties b on a.ID = b.TripID 
                        inner join dbo.Countries c on b.CountyID = c.ID
                        where VisitTypeID not in (3,4) and UserID = @Param0
                        group by c.Name) Visited on Total.County = Visited.County
                        left outer join
                        (select b.Name AS County, count(*) as HighpointedCount from 
                        Counties a 
                        inner join Countries b on a.CountryID = b.ID
                        inner join PointsToCounties c on a.ID = c.CountyID
                        inner join TripPoints d on d.PointID = c.PointID
                        inner join dbo.Trips e on d.TripID = e.ID
                        where UserID = @Param0
                        and IsHighpoint = 1
                        group by b.Name) Highpointed on Total.County = Highpointed.County
                        left outer join
                        (select d.Name AS County, count(distinct CountryID) as ClimbedInCount from 
                        dbo.Trips t 
                        inner join dbo.TripPoints a on t.ID = a.TripID
                        inner join dbo.Points b on PointID = b.ID
                        inner join dbo.PointsToCountries c on b.ID = c.PointID
                        inner join dbo.Countries d on c.CountryID = d.ID
                        where UserID = @Param0
                        group by d.Name) ClimbedIn on Total.County = ClimbedIn.County";

            var reader = server.ExecuteReader(strSQL);

            while (reader.Read())
            {
                var stat = new Stat(
                    reader["Name"].ToString(),
                    int.Parse(reader["CountyCount"].ToString()),
                    int.Parse(reader["VisitedCount"].ToString()),
                    int.Parse(reader["SteppedInCount"].ToString()),
                    int.Parse(reader["HighpointedCount"].ToString()),
                    int.Parse(reader["ClimbedInCount"].ToString())
                    );
                stats.Add(stat);
            }
            reader.Close();

            return stats;
        }
    }
    
}