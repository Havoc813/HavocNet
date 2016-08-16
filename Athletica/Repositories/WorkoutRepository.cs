using System.Collections;

namespace Athletica.Repositories
{
    public class WorkoutRepository : ActivityRepository
    {
        private readonly AthleticaServer _aServer;

        public WorkoutRepository(AthleticaServer aServer) : base(aServer)
        {
            _aServer = aServer;
        }

        public int Create(
            string date,
            string timeOfDay,
            int sport,
            string time,
            int locationID,
            int kitID,
            string comment,
            string tags,
            double distance,
            int routeID
            )
        {
            var activityID = base.Create(date, timeOfDay, sport, time, locationID, kitID, comment, tags);

            const string strSQL = @"INSERT INTO dbo.DATA_Workouts(
                ID, 
                Distance, 
                RouteID 
                ) 
                SELECT 
                @Param0 AS ID, 
                @Param1 AS Distance, 
                @Param2 AS RouteID ";

            _aServer.Open();

            _aServer.ExecuteNonQuery(strSQL, new ArrayList { activityID, distance, routeID });

            _aServer.Close();

            return 0;
        }
    }
}