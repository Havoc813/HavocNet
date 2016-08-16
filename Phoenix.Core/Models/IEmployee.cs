using System;

namespace Phoenix.Core.Models
{
    public interface IEmployee
    {
        //Values

        //Functions
        int ID { get; set; }
        string ADUsername { get; set; }
        string First { get; set; }
        string Second { get; set; }
        string EmailAddress { get; set; }
        int Number { get; set; }
        DateTime DateOfBirth { get; set; }
        string Division { get; set; }
        string CostCenter { get; set; }
        string ContractType { get; set; }
        DateTime JoinDate { get; set; }
        IEmployee Supervisor { get; set; }

        string FullName { get; //get { return string.Format("{0} {1}", First, Second); }
        }

        string FormattedDateOfBirth { get; }
    }
}