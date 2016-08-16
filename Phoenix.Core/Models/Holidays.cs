using System;
using System.Collections.Generic;
using Phoenix.Core.Repositories;

namespace Phoenix.Core.Models
{
    public class Holidays : IHolidays
    {
        private readonly Dictionary<DateTime, String> _holidays;

        public Holidays()
        {
            _holidays = new HolidayRepository().Get();
        }

        public Holidays(Dictionary<DateTime, String> holidays)
        {
            _holidays = holidays;
        }

        public Dictionary<DateTime, String> GetHolidays()
        {
            return _holidays;
        }

        public int GetHolidayCount(DateTime dateTestFrom, DateTime dateTestTo)
        {
            var holidayCount = 0;
            foreach (DateTime bh in _holidays.Keys)
            {
                if (dateTestFrom.Date <= bh && bh <= dateTestTo.Date && bh.DayOfWeek != DayOfWeek.Saturday && bh.DayOfWeek != DayOfWeek.Sunday)
                    holidayCount += 1;
            }

            return holidayCount;
        }
    }
}
