using System.Collections;

namespace Athletica.Repositories
{
    public class RegattaRepository : ActivityRepository
    {
        private readonly AthleticaServer _aServer;

        public RegattaRepository(AthleticaServer aServer) : base(aServer)
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
            string racePlaces,
            string raceParticipants
            )
        {
            var activityID = base.Create(date, timeOfDay, sport, time, locationID, kitID, comment, tags);

            var places = racePlaces.Split('|');
            var participants = raceParticipants.Split('|');

            const string strSQL = @"INSERT INTO dbo.DATA_Matches(
                ID, 
                Place,
                Participants 
                ) 
                SELECT 
                @Param0 AS ID, 
                @Param1 AS GamesWon, 
                @Param2 AS GamesLost ";

            _aServer.Open();

            for (var i = 0; i < places.Length; i++)
            {
                _aServer.ExecuteNonQuery(strSQL, new ArrayList { activityID, places[i], participants[i] });
            }

            _aServer.Close();

            return 0;
        }
    }
}