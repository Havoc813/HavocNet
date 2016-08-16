using System;
using System.Collections.Generic;
using Phoenix.Core.Servers;

namespace Phoenix.Core.Repositories
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly CoreServer _server;
        private const string LeaveSql = @"Select HolidayDate,
                      HolidayDescription
                      FROM 
                      dbo.Holidays ";

        public HolidayRepository()
        {
            _server = new CoreServer();
        }

        public Dictionary<DateTime, String> Get()
        {
            _server.Open();

            var holidays = new Dictionary<DateTime, String>();

            var aReader = _server.ExecuteReader(LeaveSql);

            using (aReader)
            {
                while (aReader.Read())
                    holidays.Add((DateTime)aReader["HolidayDate"], (String)aReader["HolidayDescription"]);
            }

            _server.Close();

            //return holidays.Values.OrderBy(holiday => holiday.Date);
            return holidays;
        }

        public void Add(DateTime holidayDate, String holidayDescription)
        {
            _server.Open();
            const string sql = "Insert Into Holidays (HolidayDate, HolidayDescription) Values (@Param0, @Param1)";
            _server.SQLParams.Add(holidayDate);
            _server.SQLParams.Add(holidayDescription);
            _server.ExecuteNonQuery(sql);
            _server.Close();
        }

        public bool Delete(DateTime holidayDate)
        {
            return false;
        }

        public void Update(DateTime holidayDate, String holidayDescription)
        {
            _server.Open();
            const string sql = "Update Holidays Set HolidayDescription=@Param1 Where HolidayDate=@Param0";
            _server.SQLParams.Add(holidayDate);
            _server.SQLParams.Add(holidayDescription);
            _server.ExecuteNonQuery(sql);
            _server.Close();
        }
    }
}
