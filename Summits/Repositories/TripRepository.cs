using System;
using System.Collections.Generic;
using Summits.Models;

namespace Summits.Repositories
{
    public class TripRepository
    {
        public List<Trip> GetAll(SummitServer server)
        {
            var trips = new List<Trip>();

            const string strSQL = @"SELECT * FROM dbo.Trips WHERE UserID = @Param0 ORDER BY EndDate DESC";

            var reader = server.ExecuteReader(strSQL);

            while (reader.Read())
            {
                var trip = new Trip(
                    int.Parse(reader["ID"].ToString()),
                    reader["name"].ToString(),
                    DateTime.Parse(reader["StartDate"].ToString()),
                    DateTime.Parse(reader["EndDate"].ToString())
                    );
                trips.Add(trip);
            }
            reader.Close();

            var repo = new MountainRepository(server);

            foreach (var trip in trips)
            {
                trip.Mountains = repo.GetTrip(trip.ID);
            }

            return trips;
        }
    }
}