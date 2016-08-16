using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class SQLHelper
    {
        private readonly ArrayList _sqlParams;
        public readonly IList<string> Selects = new List<string>();
        public readonly IList<string> Groups = new List<string>();
        public readonly IList<string> Havings = new List<string>();
        public readonly IList<string> Filters = new List<string>();
        public readonly IList<string> Orders = new List<string>();
        public readonly Dictionary<int, string> OptionalFilters = new Dictionary<int, string>();
        public readonly Dictionary<string, List<string>> InFilters = new Dictionary<string, List<string>>();

        public string From { get; set; }

        public SQLHelper(ArrayList SQLParams)
        {
            _sqlParams = SQLParams;
        }

        public string MakeSQL()
        {
            var strSQL = AddSelects();

            strSQL += " FROM " + From;

            strSQL += AddFilters();

            strSQL += AddGroups();

            strSQL += AddOrders();

            return strSQL;
        }

        private string AddSelects()
        {
            return Selects.Any() ? "SELECT " + String.Join(", ", Selects) : "SELECT *";
        }

        private string AddFilters()
        {
            var actualFilters = Filters
                .Concat(InFilters.Values.Select(inList => String.Join(", ", inList)))
                .Concat(OptionalFilters
                           .Where(filter => _sqlParams[filter.Key].ToString() != "All")
                           .Select(filter => filter.Value))
                .ToList();

            return actualFilters.Any() ? " WHERE " + String.Join(" AND ", actualFilters) : "";
        }

        private string AddGroups()
        {
            return Groups.Any() ? " GROUP BY " + String.Join(", ", Groups) : "";
        }

        private string AddHaving()
        {
            return Havings.Any() ? "HAVING " + String.Join(" AND ", Havings) : "";
        }

        private string AddOrders()
        {
            return Orders.Any() ? " ORDER BY " + String.Join(", ", Orders) : "";;
        }
    }
}
