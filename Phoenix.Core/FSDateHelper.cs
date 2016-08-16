using System;
using System.Collections.Generic;
using Phoenix.Core.Models;
using Phoenix.Core.Repositories;

namespace Phoenix.Core
{
    public static class FSDateHelper
    {
        private enum Direction
        {
            Next = 1,
            Previous = -1
        }

        public static bool IsMorning(DateTime date)
        {
            return (date.Hour * 100 + date.Minute <= 1200);
        }

        public static bool IsAfternoon(DateTime date)
        {
            return (date.Hour * 100 + date.Minute >= 1200);
        }

        public static DateTime GetPrevWorkingDay(DateTime aDate)
        {
            return GetWorkingDay(aDate, new HolidayRepository().Get(), Direction.Previous, 1);
        }

        public static DateTime GetPrevWorkingDay(DateTime aDate, Dictionary<DateTime, String> holidays)
        {
            return GetWorkingDay(aDate, holidays, Direction.Previous, 1);
        }

        public static DateTime GetNextWorkingDay(DateTime aDate)
        {
            return GetWorkingDay(aDate, new HolidayRepository().Get(), Direction.Next, 1);
        }

        public static DateTime GetNextWorkingDay(DateTime aDate, Dictionary<DateTime, String> holidays)
        {
            return GetWorkingDay(aDate, holidays, Direction.Next, 1);
        }

        public static DateTime GetPrevWorkingDay(DateTime aDate, int dayBackCount)
        {
            return GetWorkingDay(aDate, new HolidayRepository().Get(), Direction.Previous, dayBackCount);
        }

        public static DateTime GetPrevWorkingDay(DateTime aDate, int dayBackCount, Dictionary<DateTime, String> holidays)
        {
            return GetWorkingDay(aDate, holidays, Direction.Previous, dayBackCount);
        }

        public static DateTime GetNextWorkingDay(DateTime aDate, int dayForeCount)
        {
            return GetWorkingDay(aDate, new HolidayRepository().Get(), Direction.Next, dayForeCount);
        }

        public static DateTime GetNextWorkingDay(DateTime aDate, int dayForeCount, Dictionary<DateTime, String> holidays)
        {
            return GetWorkingDay(aDate, holidays, Direction.Next, dayForeCount);
        }

        private static DateTime GetWorkingDay(DateTime aDate, Dictionary<DateTime,String> holidays, Direction aDirection, int dayCount)
        {
            //var holidays = new HolidayRepository().Get();
            var thisDate = DateTime.Parse(FSFormat.ShortDate(aDate));

            for (var count = 1; count <= dayCount; count++)
            {
                thisDate = thisDate.AddDays((Double) aDirection);
                while (holidays.ContainsKey(thisDate) || IsWeekend(thisDate))
                {
                    thisDate = thisDate.AddDays((Double)aDirection);
                }
            }
            return thisDate;
        }

        public static DateTime GetNextWeekDay(DateTime aDate)
        {
            while (IsWeekend(aDate))
                aDate = aDate.AddDays(1);

            return aDate;
        }

        public static bool IsWeekend(DateTime aDate)
        {
            return aDate.DayOfWeek == DayOfWeek.Saturday || aDate.DayOfWeek == DayOfWeek.Sunday;
        }
        
        public static bool IsWorkday(DateTime aDate, IHolidays holidays)
        {
            return ! (holidays.GetHolidays().ContainsKey(aDate) || IsWeekend(aDate));
        }

        public static string MonthName(int monthNum, string format = "MMMM")
        {
            var d = new DateTime(2000, monthNum, 1);
            return d.ToString(format);
        }

        public static DateTime GetLastDayOfMonth(DateTime aDate)
        {
            return aDate.AddMonths(1).AddDays(-1);
        }

        public static DateTime RoundUpFiveMins(DateTime d)
        {
            return RoundUp(d, TimeSpan.FromMinutes(5));
        }

        public static DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }
    }
}
