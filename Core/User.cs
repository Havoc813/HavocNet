using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Core
{
    public class User
    {
        private readonly int _id;
        private readonly string _username;
        private readonly string _password;
        private readonly string _firstName;
        private readonly string _surname;
        private readonly string _emailAddress;
        private readonly bool _enabled;
        private readonly bool _admin;
        private readonly string _homePage;
        private readonly bool _changePassword;
        private readonly bool _allowEmail;
        private readonly int _version;

        public readonly Dictionary<string, int> PageAccess = new Dictionary<string, int>();
        
        public User(
            int id, 
            string username, 
            string password, 
            string firstName, 
            string surname, 
            string emailAddress, 
            bool enabled, 
            bool admin, 
            string homePage, 
            bool changePassword,
            bool allowEmail,
            int version
            )
        {
            _id = id;
            _username = username;

            _firstName = firstName;
            _surname = surname;
            _emailAddress = emailAddress;
            _enabled = enabled;
            _admin = admin;
            _homePage = homePage;
            _changePassword = changePassword;
            _allowEmail = allowEmail;
            _password = password;
            _version = version;
        }

        public string Username
        {
            get { return _username; }
        }

        public string FirstName
        {
            get { return _firstName; }
        }

        public string Surname
        {
            get { return _surname; }
        }

        public string EmailAddress
        {
            get { return _emailAddress; }
        }

        public bool Enabled
        {
            get { return _enabled; }
        }

        public bool Admin
        {
            get { return _admin; }
        }

        public int ID
        {
            get { return _id; }
        }

        public string FullName
        {
            get { return _firstName + " " + _surname; }
        }

        public string HomePage
        {
            get { return _homePage; }
        }

        public bool MustChangePassword
        {
            get { return _changePassword; }
        }

        public bool AllowEmail
        {
            get { return _allowEmail; }
        }

        public int Version
        {
            get { return _version; }
        }

        public int GetAcccess(string pageIdentifier)
        {
            if (Admin) return 2;
            if (PageAccess.ContainsKey(pageIdentifier)) return PageAccess[pageIdentifier];
            return 0;
        }

        public string Publish()
        {
            return "ID=" + this.ID + ", " +
                   "Username=" + this.Username + ", " +
                   "FirstName=" + this.FirstName + ", " +
                   "Surname=" + this.Surname + ", " +
                   "Email=" + this.EmailAddress + ", " +
                   "Enabled=" + this.Enabled + ", " +
                   "Admin=" + this.Admin;
        }
    }
}
