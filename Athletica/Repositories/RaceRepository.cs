using System.Collections;

namespace Athletica.Repositories
{
    public class RaceRepository : WorkoutRepository
    {
        private readonly AthleticaServer _aServer;

        public RaceRepository(AthleticaServer aServer) : base(aServer)
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
            string raceName, 
            int placeOverall,
            int participantsOverall,
            int placeGender,
            int participantsGender,
            int placeCategory,
            int participantsCategory,
            int categoryName,
            double distance,
            int routeID
            )
        {
            var activityID = base.Create(date, timeOfDay, sport, time, locationID, kitID, comment, tags, distance, routeID);

            const string strSQL = @"INSERT INTO dbo.DATA_Races(
                ID, 
                RaceName, 
                PlaceOverall, 
                ParticipantsOverall, 
                PlaceGender, 
                ParticipantsGender, 
                PlaceCategory, 
                ParticipantsCategory, 
                CategoryName) 
                SELECT 
                @Param0 AS ID, 
                @Param1 AS RaceName, 
                @Param2 AS PlaceOverall, 
                @Param3 AS ParticipantsOverall, 
                @Param4 AS PlaceGender, 
                @Param5 AS ParticipantsGender, 
                @Param6 AS PlaceCategory, 
                @Param7 AS ParticipantsCategory,
                @Param8 AS CategoryName ";

            _aServer.Open();

            _aServer.ExecuteNonQuery(strSQL, 
                new ArrayList
                    {
                        activityID, 
                        raceName, 
                        placeOverall, 
                        participantsOverall, 
                        placeGender, 
                        participantsGender, 
                        placeCategory, 
                        participantsCategory, 
                        categoryName
                    });

            _aServer.Close();

            return 0;
        }
    }
}