using System;
using System.Collections.Generic;

namespace Phoenix.Core.Repositories
{
    public interface IHolidayRepository
    {
        Dictionary<DateTime, String> Get();
        void Add(DateTime holidayDate, String holidayDescription);
        bool Delete(DateTime holidayDate);
        void Update(DateTime holidayDate, String holidayDescription);
    }
}