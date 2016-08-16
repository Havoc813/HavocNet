using System;
using System.Collections.Generic;

namespace Summits.Models
{
    public class Trip
    {
        private readonly int _id;
        private readonly string _name;
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public List<Mountain> Mountains = new List<Mountain>();

        public Trip(
            int id,
            string name,
            DateTime startDate,
            DateTime endDate
            )
        {
            _id = id;
            _name = name;
            _startDate = startDate;
            _endDate = endDate;
        }

        public int ID
        {
            get { return _id; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string StartDate
        {
            get { return _startDate.ToString("dd-MM-yyyy"); }
        }

        public string EndDate
        {
            get { return _endDate.ToString("dd-MM-yyyy"); }
        }
    }
}
