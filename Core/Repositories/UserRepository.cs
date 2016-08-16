using System.Collections.Generic;
using System.Data.Common;
using Phoenix.Core;
using Phoenix.Core.Logging;

namespace Core.Repositories
{
    public class UserRepository
    {
        private readonly FSServer _aServer;
        
        public UserRepository(FSServer aServer)
        {
            _aServer = aServer;
        }

        public bool TryGet(string username, string password, out User aUser)
        {
            _aServer.Open();
            _aServer.SQLParams.Add(username);
            _aServer.SQLParams.Add(Crypto.Encrypt(password));

            var aReader = _aServer.ExecuteReader("SELECT * FROM dbo.tblUsers WHERE Username = @Param0 AND Password = @Param1");

            if (aReader.Read())
            {
                aUser = GetFromReader(aReader);

                aReader.Close();

                _aServer.SQLParams.Clear();
                _aServer.SQLParams.Add(aUser.ID);

                LoadAccess(aUser);

                _aServer.Close();
            }
            else
            {
                aReader.Close();

                aUser = null;    
            }

            _aServer.Close();

            return (aUser != null);
        }

        public User Get(int userID)
        {
            User aUser = null;

            if (userID == 0) return GetPublic();

            _aServer.Open();
            _aServer.SQLParams.Add(userID);

            var aReader = _aServer.ExecuteReader("SELECT * FROM dbo.tblUsers WHERE ID = @Param0");

            if (aReader.Read())
            {
                aUser = GetFromReader(aReader);

                aReader.Close();

                LoadAccess(aUser);
            }
            aReader.Close();

            _aServer.Close();

            return aUser;
        }

        public User GetPublic()
        {
            var aUser = new User(
                    0,
                    "public",
                    "",
                    "Stranger",
                    "",
                    "public@wreakhavoc.co.uk",
                    true,
                    false,
                    "Home.aspx",
                    false,
                    true
                );

            _aServer.Open();
            _aServer.SQLParams.Add(0);

            LoadAccess(aUser);

            _aServer.Close();

            return aUser;
        }

        public Dictionary<int, User> GetAll()
        {
            var allUsers = new Dictionary<int, User>();

            _aServer.Open();

            var aReader = _aServer.ExecuteReader("SELECT * FROM dbo.tblUsers");

            while (aReader.Read())
            {
                var aUser = GetFromReader(aReader);

                allUsers.Add(int.Parse(aReader["ID"].ToString()), aUser);
            }
            aReader.Close();
            _aServer.Close();

            return allUsers;
        }

        public void Save(string username, string password, string firstName, string surname, string enabled, string admin, string emailAddress, User creator)
        {
            _aServer.Open();

            _aServer.SQLParams.Add(username);
            _aServer.SQLParams.Add(Crypto.Encrypt(password));
            _aServer.SQLParams.Add(firstName);
            _aServer.SQLParams.Add(surname);
            _aServer.SQLParams.Add(admin == "on" ? 1 : 0);
            _aServer.SQLParams.Add(enabled == "on" ? 1 : 0);
            _aServer.SQLParams.Add(emailAddress);

            const string strSQL = "INSERT INTO dbo.tblUsers (Username, Password, FirstName, Surname, Admin, Enabled, EmailAddress) " +
                                  "SELECT " +
                                  "@Param0 AS Username, " +
                                  "@Param1 AS Password, " +
                                  "@Param2 AS FirstName, " +
                                  "@Param3 AS Surname, " +
                                  "@Param4 AS Admin, " +
                                  "@Param5 AS Enabled, " +
                                  "@Param6 AS EmailAddress, " +
                                  "'Home.aspx' AS HomePage, " +
                                  "0 AS ChangePassword, " +
                                  "0 AS AllowEmail ";

            _aServer.ExecuteNonQuery(strSQL);

            _aServer.Close();

            User aUser;
            this.TryGet(username, password, out aUser);

            var aAudit = new FSAudit(_aServer, creator.Username, "Creating User", aUser.Publish());
            aAudit.Create();

            //TODO: Welcome Email


        }

        public void Delete(int userID, User deleter)
        {
            var aUser = this.Get(userID);

            _aServer.Open();
            _aServer.SQLParams.Add(userID);

            _aServer.ExecuteNonQuery("DELETE FROM dbo.tblUsers WHERE ID = @Param0");

            _aServer.Close();

            var aAudit = new FSAudit(_aServer, deleter.Username, "Deleting User", aUser.Publish());
            aAudit.Create();
        }

        public void Update(int userID, string username, string password, string firstName, string surname, string enabled, string admin, string emailAddress, User updater)
        {
            _aServer.Open();

            _aServer.SQLParams.Add(userID);
            _aServer.SQLParams.Add(username);
            _aServer.SQLParams.Add(Crypto.Encrypt(password));
            _aServer.SQLParams.Add(firstName);
            _aServer.SQLParams.Add(surname);
            _aServer.SQLParams.Add(admin == "on" ? 1 : 0);
            _aServer.SQLParams.Add(enabled == "on" ? 1 : 0);
            _aServer.SQLParams.Add(emailAddress);

            var strSQL = "UPDATE dbo.tblUsers SET Username = @Param1, ";

            if(password != "") strSQL += "Password = @Param2, "; 
            
            strSQL += "FirstName = @Param3, " +
                    "Surname = @Param4, " +
                    "Admin = @Param5, " +
                    "Enabled = @Param6, " +
                    "EmailAddress = @Param7 " +
                    "WHERE " +
                    "ID = @Param0";

            _aServer.ExecuteNonQuery(strSQL);

            _aServer.Close();
            
            var aUser = this.Get(userID);

            var aAudit = new FSAudit(_aServer, updater.Username, "Updating User", aUser.Publish());
            aAudit.Create();
        }

        public bool TryResetPassword(string username, string emailAddress, out string newPassword)
        {            
            _aServer.Open();

            _aServer.SQLParams.Add(username);
            _aServer.SQLParams.Add(emailAddress);

            if (_aServer.ExecuteScalar("SELECT Username FROM dbo.tblUsers WHERE Username = @Param0 AND EmailAddress = @Param1").ToString() == username)
            {
                newPassword = Crypto.RandomPassword(10);

                _aServer.SQLParams.Add(Crypto.Encrypt(newPassword));

                _aServer.ExecuteNonQuery("UPDATE dbo.tblUsers SET Password = @Param2, ChangePassword = 1 WHERE Username = @Param0 AND EmailAddress = @Param1");

                _aServer.Close();

                return true;
            }
            newPassword = "";
            _aServer.Close();

            return false;
        }

        public void ResetPassword(int userID, string password)
        {
            _aServer.Open();

            _aServer.SQLParams.Add(userID);
            _aServer.SQLParams.Add(Crypto.Encrypt(password));

            _aServer.ExecuteNonQuery("UPDATE dbo.tblUsers SET Password = @Param1, ChangePassword = 0 WHERE ID = @Param0");
            
            _aServer.Close();
        }

        private void LoadAccess(User aUser)
        {
            var aReader = _aServer.ExecuteReader("SELECT * FROM dbo.tblUserAccess WHERE UserID = @Param0");

            while (aReader.Read())
            {
                aUser.PageAccess.Add(aReader["PageIdentifier"].ToString(), int.Parse(aReader["PageAccess"].ToString()));
            }

            aReader.Close();

            if (aUser.ID == 0) return;
            aUser.PageAccess.Add("Preferences.aspx_", 2);
            aUser.PageAccess.Add("Home.aspx_", 2);
        }

        private User GetFromReader(DbDataReader aReader)
        {
            return new User(
                int.Parse(aReader["ID"].ToString()),
                aReader["Username"].ToString(),
                aReader["Password"].ToString(),
                aReader["FirstName"].ToString(),
                aReader["Surname"].ToString(),
                aReader["EmailAddress"].ToString(),
                int.Parse(aReader["Enabled"].ToString()) == 1,
                int.Parse(aReader["Admin"].ToString()) == 1,
                aReader["HomePage"].ToString(),
                int.Parse(aReader["ChangePassword"].ToString()) == 1,
                int.Parse(aReader["AllowEmail"].ToString()) == 1
                );
        }

        public User UpdatePreferences(int userID, string firstName, string surname, string emailAddress, string homePage, int newsLetter)
        {
            _aServer.Open();

            _aServer.SQLParams.Add(userID);
            _aServer.SQLParams.Add(firstName);
            _aServer.SQLParams.Add(surname);
            _aServer.SQLParams.Add(emailAddress);
            _aServer.SQLParams.Add(homePage);
            _aServer.SQLParams.Add(newsLetter);

            _aServer.ExecuteNonQuery("UPDATE dbo.tblUsers SET FirstName = @Param1, Surname = @Param2, EmailAddress = @Param3, HomePage = @Param4, AllowEmail = @Param5 WHERE ID = @Param0");

            _aServer.Close();

            return this.Get(userID);
        }
    }
}
