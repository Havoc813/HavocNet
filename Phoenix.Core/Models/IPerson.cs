using System;

namespace Phoenix.Core.Models
{
    public interface IPerson
    {
        int ID { get; set; }
        string First { get; set; }
        string Second { get; set; }
        string EmailAddress { get; set; }
        DateTime DateOfBirth { get; set; }
        string FullName { get; }
        string FormattedDateOfBirth { get; }
    }
}