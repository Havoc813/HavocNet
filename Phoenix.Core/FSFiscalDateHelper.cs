using System;

namespace Phoenix.Core
{
    public class FSFiscalDateHelper
    {
        public int FiscalPeriod { get; private set; }
        public int FiscalYear { get; private set; }

        public FSFiscalDateHelper(int month, int year)
        {
            Initialise(DateTime.Parse("01/" + month + "/" + year));
        }

        public FSFiscalDateHelper(string month, int year)
        {
            Initialise(DateTime.Parse("01/" + month + "/" + year));
        }

        public FSFiscalDateHelper(DateTime date)
        {
            Initialise(date);
        }

        private void Initialise(DateTime aDate)
        {
            aDate = aDate.AddMonths(6);
            FiscalPeriod = aDate.Month;
            FiscalYear = aDate.Year;
        }

        public static int GetFiscalYearFromCalendar(string month, int year)
        {
            return DateTime.Parse("01/" + month + "/" + year).AddMonths(6).Year;
        }

        public static int GetCalendarYearFromFiscal(string month, int year)
        {
            return DateTime.Parse("01/" + month + "/" + year).AddMonths(-6).Year;
        }

        public static int GetFiscalMonthFromCalendar(string month, int year)
        {
            return DateTime.Parse("01/" + month + "/" + year).AddMonths(6).Month;
        }

        public static string GetFiscalMonthNameFromCalendar(string month, int year)
        {
            return DateTime.Parse("01/" + month + "/" + year).AddMonths(6).ToString("MMM");
        }

        public static int GetCalendarMonthFromFiscal(string month, int year)
        {
            return DateTime.Parse("01/" + month + "/" + year).AddMonths(-6).Month;
        }

        public static string GetCalendarMonthNameFromFiscal(string month, int year)
        {
            return DateTime.Parse("01/" + month + "/" + year).AddMonths(-6).ToString("MMM");
        }

        public string FiscalDescription()
        {
            return (FiscalYear - 1).ToString("").Substring(2) + "/" + FiscalYear.ToString("00").Substring(2);
        }
    }
}
