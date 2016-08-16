using System;

namespace Phoenix.Core.Models
{
    public class Person : IPerson
    {
        public int ID { get; set; }
        public string First { get; set; }
        public string Second { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateOfBirth { get; set; }
        
        public string FullName {
            get { return string.Format("{0} {1}", First, Second); }
        }

        public string FormattedDateOfBirth {
            get { return DateOfBirth.ToString("dd/MM/yyyy"); }
        }

        public Person(int id, string first, string second, string emailAddress)
        {
            ID = id;
            First = first;
            Second = second;
            EmailAddress = emailAddress;
        }

        public Person(int id, string first, string second, string emailAddress, DateTime dateOfBirth)
        {
            ID = id;
            First = first;
            Second = second;
            EmailAddress = emailAddress;
            DateOfBirth = dateOfBirth;
        }
    }
}
