using System;
using System.Collections.Generic;

namespace Phoenix.Core.Models
{
    public interface IHolidays
    {
        Dictionary<DateTime, String> GetHolidays();
        int GetHolidayCount(DateTime dateTestFrom, DateTime dateTestTo);
    }
}