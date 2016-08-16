using System.DirectoryServices;

namespace Phoenix.Core
{
    public class FSIdentity : IFSIdentity
    {
        private readonly SearchResult _aResult;
        private readonly string _username;
        private readonly string _domain;
        private readonly bool _deleted;
        private readonly bool _notPresent;

        public FSIdentity(string user)
        {
            if (string.IsNullOrEmpty(user)) return;

            this._domain = FetchDomain(user);
            this._username = FetchUser(user);

            var aSearch = new DirectorySearcher
                {
                    Filter = "(&(sAMAccountName=" + this._username + "))",
                    SearchRoot = new DirectoryEntry(FSConfig.AppSettings("LDAP_" + this._domain.ToUpper()))
                };

            this._aResult = aSearch.FindOne();

            if (_aResult == null)
            {
                this._notPresent = true;
                return;
            }

            if (_aResult.Path.Contains("OU=Deleted Users"))
            {
                this._deleted = true;
                return;
            }
        }

        private static string FetchUser(string user)
        {
            if (user.IndexOf("\\", System.StringComparison.Ordinal) > 0)
                return user.Substring(user.IndexOf("\\", System.StringComparison.Ordinal) + 1);
            return user;
        }

        private static string FetchDomain(string user)
        {
            if (user.IndexOf("\\", System.StringComparison.Ordinal) > 0) 
                return user.Substring(0, user.IndexOf("\\", System.StringComparison.Ordinal));
            return FSConfig.AppSettings("Domain_Default");
        }

        public bool Deleted
        {
            get { return _deleted; }
        }

        public bool NotPresent
        {
            get { return _notPresent; }
        }

        public string Username
        {
            get { return this._domain + "\\" + this._username; }
        }

        public string FirstName
        {
            get { return FetchProperty("givenName", "First Name"); }
        }

        public string Surname
        {
            get { return FetchProperty("sn", "Surname"); }
        }

        public string EmailAddress
        {
            get { return FetchProperty("mail", "Email Address"); }
        }

        public string Department
        {
            get { return FetchProperty("description", "Department"); }
        }

        public string Telephone
        {
            get { return FetchProperty("telephoneNumber", "Telephone Number"); }
        }

        public string DisplayName()
        {
            return string.IsNullOrEmpty(this._username) ? "" : string.IsNullOrEmpty(_aResult.ToString()) ? "User " + this._username : FirstName + " " + Surname;
        }

        public string Path()
        {
            return string.IsNullOrEmpty(_aResult.ToString()) ? "Invalid" : _aResult.Path;
        }

        private string FetchProperty(string aPropertyName, string aPropertyDescription)
        {
            if (_aResult == null) return "Invalid: " + this._username;
            if (_aResult.Properties[aPropertyName].Count == 0) return "No " + aPropertyDescription + " Set";
            return _aResult.Properties[aPropertyName][0].ToString();
        }
    }
}