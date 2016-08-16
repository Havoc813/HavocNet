using System.Collections.Generic;
using Core;

namespace Athletica.Repositories
{
    public class SportRepository
    {
        public Dictionary<int, string> GetByUser(AthleticaServer aServer)
        {
            var dict = new Dictionary<int, string>();

            var strSQL = new SQLHelper(aServer.SQLParams);

            strSQL.Selects.Add("a.ActivityID");
            strSQL.Selects.Add("a.ActivityName");

            strSQL.From = @"dbo.Activities a 
                    INNER JOIN dbo.UserActivities b ON a.ActivityID = b.ActivityID 
                    INNER JOIN dbo.ActivityClasses c ON a.ActivityClassID = c.ClassID";

            strSQL.Filters.Add("UserID = @Param0");
            strSQL.Filters.Add("Active = 1");
            strSQL.OptionalFilters.Add(1, "ClassName = @Param1");
            strSQL.Orders.Add("b.Ordering");

            var aReader = aServer.ExecuteReader(strSQL.MakeSQL());

            while (aReader.Read())
            {
                dict.Add(int.Parse(aReader["ActivityID"].ToString()), aReader["ActivityName"].ToString());
            }
            aReader.Close();

            return dict;
        }
    }
}
