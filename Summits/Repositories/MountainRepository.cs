using System.Collections.Generic;
using Summits.Models;

namespace Summits.Repositories
{
    public class MountainRepository
    {
        public Dictionary<int, List<Mountain>> AllMountains = new Dictionary<int, List<Mountain>>();

        public MountainRepository(SummitServer server)
        {
            var reader = server.ExecuteReader(@"
                SELECT TripID, PointID AS ID, c.Name, c.Longitude, c.Latitude, c.Zoom
                FROM Trips a 
                INNER JOIN TripPoints b on a.ID = b.TripID 
                INNER JOIN Points c on b.PointID = c.ID
                WHERE UserID = @Param0
                ORDER BY TripID, SummitDate DESC");

            while (reader.Read())
            {
                if (!AllMountains.ContainsKey(int.Parse(reader["TripID"].ToString()))) AllMountains.Add(int.Parse(reader["TripID"].ToString()), new List<Mountain>());

                var mountain = new Mountain(
                    int.Parse(reader["id"].ToString()),
                    reader["name"].ToString(),
                    double.Parse(reader["longitude"].ToString()),
                    double.Parse(reader["latitude"].ToString()),
                    int.Parse(reader["zoom"].ToString())
                    );

                AllMountains[int.Parse(reader["TripID"].ToString())].Add(mountain);
            }
            reader.Close();
        }

        public List<Mountain> GetTrip(int tripID)
        {
            return AllMountains[tripID];
        }
    }
}
