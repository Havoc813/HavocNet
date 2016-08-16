using System;
using System.Collections;
using System.Web.UI.WebControls;
using Core;
using Phoenix.Core;

namespace Athletica.Repositories
{
    public class ActivityRepository
    {
        private readonly AthleticaServer _aServer;

        public ActivityRepository(AthleticaServer aServer)
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
            string tags
            )
        {
            _aServer.Open();

            const string strSQL = @"INSERT INTO dbo.DATA_Activities([Date], [TimeOfDay], [ActivityID], [Time], [Seconds], [LocationID], [KitID], [Comment], [Tags]) 
                SELECT 
                @Param0 AS Date, 
                @Param1 AS TimeOfDay,
                @Param2 AS ActivityID, 
                @Param3 AS Time,
                @Param8 AS Seconds,
                @Param4 AS LocationID,
                @Param5 AS KitID,
                @Param6 AS Comment,
                @Param7 AS Tags";

            var secs = DateTime.Parse(time).Subtract(DateTime.Parse("1900-01-01 00:00:00")).TotalSeconds;

            _aServer.ExecuteNonQuery(strSQL, new ArrayList {date, timeOfDay, sport, time, locationID, kitID, comment, tags, secs });

            var newActivityID = int.Parse(_aServer.ExecuteScalar("SELECT MAX(ID) FROM dbo.DATA_Activities").ToString());

            _aServer.Close();

            return newActivityID;
        }

        public Activity Get(int id)
        {
            const string strSQL = @"SELECT * FROM dbo.DATA_Activities WHERE ActivityID = @Param0";
            
            _aServer.Open();
            _aServer.SQLParams.Add(id);

            Activity activity = null;

            var aReader = _aServer.ExecuteReader(strSQL);

            if (aReader.HasRows)
            {
                aReader.Read();

                activity = new Activity(
                    id,
                    aReader["TimeOfDay"].ToString(),
                    int.Parse(aReader["ActivityID"].ToString()),
                    aReader["Time"].ToString(),
                    int.Parse(aReader["Seconds"].ToString()),
                    int.Parse(aReader["LocationID"].ToString()),
                    int.Parse(aReader["KitID"].ToString()),
                    aReader["Comments"].ToString(),
                    aReader["Tags"].ToString()
                    );

            }
            aReader.Close();

            _aServer.Close();

            return activity;
        }
    }

    public class Activity
    {
        public readonly int ID;
        public readonly string TimeOfDay;
        public readonly int ActivityID;
        public readonly string Time;
        public readonly int Seconds;
        public readonly int LocationID;
        public readonly int KitID;
        public readonly string Comment;
        public readonly string Tags;

        public Activity(
            int Id,
            string TimeOfDay,
            int ActivityID,
            string Time, 
            int Seconds, 
            int LocationID,
            int KitID,
            string Comment,
            string Tags
            )
        {
            ID = Id;
            this.TimeOfDay = TimeOfDay;
            this.ActivityID = ActivityID;
            this.Time = Time;
            this.Seconds = Seconds;
            this.LocationID = LocationID;
            this.KitID = KitID;
            this.Comment = Comment;
            this.Tags = Tags;
        }

        public Table Render()
        {
            var tbl = new Table();

            var aRow = new TableRow();

            aRow.Cells.Add(new FSTableCell(ActivityID.ToString(""),false));

            tbl.Rows.Add(aRow);

            return tbl;
        }
    }
}
