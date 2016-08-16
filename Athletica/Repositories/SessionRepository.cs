using System;
using System.Web.UI.WebControls;
using Core;

namespace Athletica.Repositories
{
    public class SessionRepository
    {
        private readonly AthleticaServer _aServer;
        private DateTime _date;

        public SessionRepository(AthleticaServer aServer)
        {
            _aServer = aServer;
        }

        public Table Render(string dateStr)
        {
            _date = DateTime.Parse(dateStr);

            var tbl = new Table();

            _aServer.Open();

            _aServer.SQLParams.Add(_date);

            const string strSQL = @"SELECT 
                a.ID AS ID,
                TimeOfDay,
                ActivityID,
                Time, 
                Seconds, 
                LocationID,
                KitID,
                Comment,
                Tags,
                b.ID AS MatchID,
                b.GamesWon,
                b.GamesLost,
                c.ID AS RaceID,
                c.RaceName,
                c.PlaceOverall,
                c.ParticipantsOverall,
                c.PlaceGender,
                c.ParticipantsGender,
                c.PlaceCategory,
                c.ParticipantsCategory,
                c.CategoryName,
                d.ID AS WorkoutID,
                d.Distance,
                d.RouteID
                FROM 
                dbo.DATA_Activities a
                LEFT OUTER JOIN dbo.DATA_Matches b ON a.ID = b.ID
                LEFT OUTER JOIN dbo.DATA_Races c ON a.ID = c.ID
                LEFT OUTER JOIN dbo.DATA_Workouts d ON a.ID = d.ID
                WHERE
                [Date] = @Param0";

            var aReader = _aServer.ExecuteReader(strSQL);

            while (aReader.Read())
            {
                var tblRow = new TableRow();
                var tblCell = new TableCell();

                if (!string.IsNullOrEmpty(aReader["MatchID"].ToString()))
                {
                    tblCell.Controls.Add(new MatchRepository(new AthleticaServer()).Get(int.Parse(aReader["MatchID"].ToString())).Render());
                }
                else if (!string.IsNullOrEmpty(aReader["RaceID"].ToString()))
                {
                    tblCell.Controls.Add(new RaceRepository(new AthleticaServer()).Get(int.Parse(aReader["RaceID"].ToString())).Render());
                }
                else if (!string.IsNullOrEmpty(aReader["WorkoutID"].ToString()))
                {
                    tblCell.Controls.Add(new WorkoutRepository(new AthleticaServer()).Get(int.Parse(aReader["WorkoutID"].ToString())).Render());
                }
                else
                {
                    tblCell.Controls.Add(new ActivityRepository(new AthleticaServer()).Get(int.Parse(aReader["ID"].ToString())).Render());
                }

                tblRow.Cells.Add(tblCell);
                tbl.Rows.Add(tblRow);
            }
            aReader.Close();

            _aServer.Close();

            return tbl;
        }
    }
}
