using System;

namespace Phoenix.Core
{
    public class FSDateDisplay
    {
        private DateTime _dateFrom;
        private DateTime _dateTo;
        public string DateFromDisplay { get; private set; }
        public string DateToDisplay { get; private set; }

        public FSDateDisplay(DateTime dateFrom, DateTime dateTo)
        {
            _dateFrom = dateFrom;
            _dateTo = dateTo;
            FormatDisplay();
        }

        private void FormatDisplay()
        {
            DateFromDisplay = FSFormat.ShortDate(_dateFrom);
            DateToDisplay = FSFormat.ShortDate(_dateTo);
            if (FSDateHelper.IsAfternoon(_dateFrom))
                DateFromDisplay +=  " (pm)";
            if (FSDateHelper.IsMorning(_dateTo))
                DateToDisplay += " (am)";
        }

        public string DateRangeDisplay()
        {
            if (_dateFrom.Date == _dateTo.Date)
                return FSDateHelper.IsAfternoon(_dateFrom) ? DateFromDisplay : DateToDisplay;

            return string.Format("{0} - {1}", DateFromDisplay, DateToDisplay);
        }
    }
}
