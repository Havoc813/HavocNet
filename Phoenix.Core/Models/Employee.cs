using System;

namespace Phoenix.Core.Models
{
    public class Employee : IEmployee
    {
        public int ID { get; set; }
        public string ADUsername { get; set; }
        public string First { get; set; }
        public string Second { get; set; }
        public string EmailAddress { get; set; }
        public int Number { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Division { get; set; }
        public string CostCenter { get; set; }
        public string ContractType { get; set; }
        public DateTime JoinDate { get; set; }
        public IEmployee Supervisor { get; set; }

        public string FullName {
            get { return First + " " + Second; }
            //get { return string.Format("{0} {1}", First, Second); }
        }

        public string FormattedDateOfBirth {
            get { return DateOfBirth.ToString("dd/MM/yyyy"); }
        }

        public Employee(int id, string first, string second, string emailAddress)
        {
            ID = id;
            First = first;
            Second = second;
            EmailAddress = emailAddress;
        }

        public Employee(int id, string adUsername, string first, string second, string emailAddress, int number, DateTime dob, string division, string costCenter, string contractType, DateTime joinDate, IEmployee supervisor)
        {
            ID = id;
            ADUsername = adUsername;
            First = first;
            Second = second;
            EmailAddress = emailAddress;
            Number = number;
            DateOfBirth = dob;
            Division = division;
            CostCenter = costCenter;
            ContractType = contractType;
            JoinDate = joinDate;
            Supervisor = supervisor;
        }
    }
}