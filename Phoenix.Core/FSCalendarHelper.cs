using System;

namespace Phoenix.Core
{
    public class FSCalendarHelper
    {
        public enum CalendarViewMode { Day, WorkWeek, Week, Month }
        public CalendarViewMode CurrentMode { get; set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public DayOfWeek FirstDayOfWeek = DayOfWeek.Monday;
        public DayOfWeek FirstDayOfWorkWeek = DayOfWeek.Monday;

        public FSCalendarHelper(DateTime startDate)
        {
            Update(startDate, startDate.AddDays(2));
        }

        public FSCalendarHelper(DateTime startDate, DateTime endDate)
        {
            Update(startDate, endDate);
        }

        public FSCalendarHelper(CalendarViewMode viewMode, DateTime startDate)
        {
            Update(viewMode, startDate);
        }

        public void UpdateStartDate(int calendarAdd)
        {
            StartDate = StartDate.AddMonths(calendarAdd);
            //if (CurrentMode == CalendarViewMode.Month)
            //{
            //    StartDate = StartDate.AddMonths(calendarAdd);
            //}
            //else if (CurrentMode == CalendarViewMode.Day)
            //{
            //    StartDate = StartDate.AddDays(calendarAdd);
            //}
            //else
            //{
            //    StartDate = StartDate.AddDays(7 * calendarAdd);
            //}
            Update(CurrentMode, StartDate);
        }

        public void Update(CalendarViewMode viewMode, DateTime startDate)
        {
            CurrentMode = viewMode;
            StartDate = startDate.Year <= 1900 ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) : new DateTime(startDate.Year, startDate.Month, startDate.Day);

            if (CurrentMode == CalendarViewMode.Month)
            {
                StartDate = StartDate.AddDays(-StartDate.Day + 1);
                EndDate = StartDate.AddMonths(1).AddDays(-1);
                return;
            }

            if (CurrentMode == CalendarViewMode.Day)
            {
                EndDate = StartDate;
                return;
            }

            int offset = StartDate.DayOfWeek - FirstDayOfWeek;
            StartDate = StartDate.AddDays(-offset);
            EndDate = StartDate.AddDays((CurrentMode == CalendarViewMode.Week ? 6 : 4));
        }

        public void Update(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate.Year <= 1900 ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) : new DateTime(startDate.Year, startDate.Month, startDate.Day);
            EndDate = endDate.Year <= 1900 ? StartDate.AddDays(2) : new DateTime(endDate.Year, endDate.Month, endDate.Day);

            if (StartDate.Date.Equals(EndDate.Date))
            {
                Update(CurrentMode, StartDate);
                return;
            }

            TimeSpan span = EndDate.Subtract(startDate);
            if (span.Days < 3)
            {
                CurrentMode = CalendarViewMode.Day;
                return;
            }
            if (span.Days > 7)
            {
                CurrentMode = CalendarViewMode.Month;
                return;
            }
            if (StartDate.DayOfWeek == DayOfWeek.Saturday || StartDate.DayOfWeek == DayOfWeek.Sunday ||
                EndDate.DayOfWeek == DayOfWeek.Saturday || EndDate.DayOfWeek == DayOfWeek.Sunday)
            {
                CurrentMode = CalendarViewMode.Week;
                StartDate = SetDateToSpecificDay(StartDate, FirstDayOfWeek);
                EndDate = StartDate.AddDays(6);
            }
            else
            {
                CurrentMode = CalendarViewMode.WorkWeek;
                StartDate = SetDateToSpecificDay(StartDate, FirstDayOfWorkWeek);
                EndDate = StartDate.AddDays(4);
            }
        }

        public DateTime SetDateToSpecificDay(DateTime currDate, DayOfWeek toWeekDay)
        {
            int offset = currDate.DayOfWeek - toWeekDay;
            return currDate.AddDays(-offset);
        }
    }
}
