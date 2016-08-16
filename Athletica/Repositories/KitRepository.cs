using System.Collections;

namespace Athletica.Repositories
{
    public class KitRepository
    {
        private readonly AthleticaServer _aServer;

        public KitRepository(AthleticaServer aServer)
        {
            _aServer = aServer;
        }

        public int Create(
            string kitName, 
            int activityID,
            int userID
            )
        {
            _aServer.Open();

            const string strSQL = @"INSERT INTO dbo.Kit(KitName, ActivityID, UserID, Ordering, IsDefault, IsActive)
                    SELECT 
                    @Param0 AS KitName,
                    @Param1 AS ActivityID,
                    @Param2 AS UserID,
                    (SELECT MAX(isnull(Ordering, 1)) FROM dbo.Location WHERE UserID = @Param2),
                    1 AS IsDefault,
                    1 AS IsActive ";

            _aServer.ExecuteNonQuery("UPDATE dbo.Kit SET IsDefault = 0 WHERE IsDefault = 1 AND UserID = @Param2 AND ActivityID = @Param1");

            _aServer.ExecuteNonQuery(strSQL, new ArrayList { kitName, activityID, userID });

            var newID = int.Parse(_aServer.ExecuteScalar("SELECT MAX(LocationID) FROM dbo.Location").ToString());

            _aServer.Close();

            return newID;
        }
    }
}
