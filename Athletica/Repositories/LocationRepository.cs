using System.Collections;

namespace Athletica.Repositories
{
    public class LocationRepository
    {
        private readonly AthleticaServer _aServer;

        public LocationRepository(AthleticaServer aServer)
        {
            _aServer = aServer;
        }

        public int Create(
            string locationName, 
            int activityID,
            int userID
            )
        {
            _aServer.Open();

            const string strSQL = @"INSERT INTO dbo.Location(LocationName, ActivityID, UserID, Ordering, IsActive)
                    SELECT 
                    @Param0 AS LocationName,
                    @Param1 AS ActivityID,
                    @Param2 AS UserID,
                    (SELECT MAX(isnull(Ordering, 1)) FROM dbo.Location WHERE UserID = @Param2),
                    1 AS IsActive ";

            _aServer.ExecuteNonQuery(strSQL, new ArrayList { locationName, activityID, userID });

            var newID = int.Parse(_aServer.ExecuteScalar("SELECT MAX(LocationID) FROM dbo.Location").ToString());

            _aServer.Close();

            return newID;
        }
    }
}
