using System.Collections;

namespace Athletica.Repositories
{
    public class MatchRepository : ActivityRepository
    {
        private readonly AthleticaServer _aServer;

        public MatchRepository(AthleticaServer aServer) : base(aServer)
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
            int gamesWon,
            int gamesLost
            )
        {
            var activityID = base.Create(date, timeOfDay, sport, time, locationID, kitID, comment, tags);

            const string strSQL = @"INSERT INTO dbo.DATA_Matches(
                ID, 
                GamesWon,
                GamesLost 
                ) 
                SELECT 
                @Param0 AS ID, 
                @Param1 AS GamesWon, 
                @Param2 AS GamesLost ";

            _aServer.Open();

            _aServer.ExecuteNonQuery(strSQL, new ArrayList { activityID, gamesWon, gamesLost });

            _aServer.Close();

            return 0;
        }
    }
}