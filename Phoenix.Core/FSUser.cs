using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using Phoenix.Core.Enums;

namespace Phoenix.Core
{
    public class FSUser
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public int Enabled { get; set; }
        public int AccessLevel { get; set; }
        public string EmailAddress { get; set; }

        public readonly Dictionary<string, int> UserAccess = new Dictionary<string, int>();
        public readonly FSServer UserServer;

        public FSUser(FSServer aServer, string username)
        {
            this.Username = username;

            var identity = new FSIdentity(HttpContext.Current.User.Identity.Name);
            this.FirstName = identity.FirstName;
            this.Surname = identity.Surname;
            this.EmailAddress = identity.EmailAddress;

            UserServer = aServer;

            var arr = new ArrayList { username };

            if (!UserServer.IsOpen()) UserServer.Open();

            var aReader = UserServer.ExecuteReader("SELECT UserID, Enabled, AccessLevel FROM dbo.Users WHERE Username = @Param0", arr);
            if (aReader.Read())
            {
                this.ID = Convert.ToInt32(aReader["UserID"]);
                this.Enabled = Convert.ToInt32(aReader["Enabled"]);
                this.AccessLevel = Convert.ToInt32(aReader["AccessLevel"]);
            }
            else
            {
                this.Enabled = 0;
                this.AccessLevel = 0;
            }

            aReader.Close();

            arr.Add(this.ID);

            aReader = UserServer.ExecuteReader("SELECT * FROM dbo.UserAccess WHERE UserID = @Param1", arr);
            while (aReader.Read())
            {
                UserAccess.Add(aReader["PageIdentifier"].ToString(), int.Parse(aReader["PageAccess"].ToString()));
            }
            aReader.Close();

            UserServer.Close();
        }

        public bool Admin
        {
            get { return (this.AccessLevel == 1); }
        }

        public string SafeUsername()
        {
            return Username.Substring(Username.IndexOf("\\", StringComparison.Ordinal) + 1);
        }

        public AccessType GetAcccess(string pageIdentifier)
        {
            if (Admin) return (AccessType)2;
            if (UserAccess.ContainsKey(pageIdentifier)) return (AccessType)UserAccess[pageIdentifier];

            foreach (var page in UserAccess.Keys)
            {
                if (page.StartsWith(pageIdentifier)) return (AccessType)UserAccess[page];
            }

            return 0;
        }

        public string EmailAlias()
        {
            return this.Surname + ", " + this.FirstName;
        }

        public string DisplayName()
        {
            return this.FirstName + " " + this.Surname;
        }
    }
}
