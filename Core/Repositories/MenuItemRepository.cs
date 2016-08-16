using System.Collections.Generic;
using Phoenix.Core;

namespace Core.Repositories
{
    public class MenuItemRepository
    {
        private readonly User _aUser;
        private readonly FSServer _aServer;

        public MenuItemRepository(FSServer aServer, User aUser)
        {
            _aUser = aUser;
            _aServer = aServer;
        }

        public string GetPageName(string subMenuPage, string subMenuParams)
        {
            const string strSQL = @"SELECT MenuPageName, 
                    MenuPageParams 
                    FROM 
                    dbo.tblMenuItems 
                    WHERE 
                    MenuItemID = (
                    SELECT MenuItemID FROM dbo.tblSubMenuItems 
                    WHERE 
                    SubMenuPageName = @Param0 AND SubMenuPageParams = @Param1
                    )";

            var pageName = "";
            _aServer.Open();
            _aServer.SQLParams.Add(subMenuPage);
            _aServer.SQLParams.Add(subMenuParams);

            var aReader = _aServer.ExecuteReader(strSQL);
            if (aReader.HasRows)
            {
                aReader.Read();

                pageName += aReader["MenuPageName"] + "_";
                pageName += aReader["MenuPageParams"].ToString();
            }
            aReader.Close();

            _aServer.Close();

            return pageName;
        }

        public Dictionary<int, MenuItem> GetMenuItems(string navMenu, string menuPage, string menuParams)
        {
            var aDictionary = new Dictionary<int, MenuItem>();

            const string strSQL = @"SELECT MenuItemID AS MenuID,
                    MenuItemName AS MenuName, 
                    MenuItemDescription AS MenuDescription, 
                    MenuPageName AS MenuPage, 
                    MenuPageParams AS MenuPageParams 
                    FROM dbo.tblMenuItems a 
                    WHERE NavPageName = @Param1 ORDER BY Ordering";

            _aServer.Open();
            _aServer.SQLParams.Add(_aUser.ID);
            _aServer.SQLParams.Add(navMenu);

            var aReader = _aServer.ExecuteReader(strSQL);
            while (aReader.Read())
            {
                var selected = (aReader["MenuPage"].ToString() == menuPage && aReader["MenuPageParams"].ToString() == menuParams);

                aDictionary.Add(int.Parse(aReader["MenuID"].ToString()), new MenuItem(navMenu, aReader["MenuName"].ToString(), aReader["MenuPage"].ToString(), aReader["MenuDescription"].ToString(), aReader["MenuPageParams"].ToString(), selected));
            }
            aReader.Close();

            _aServer.Close();

            return aDictionary;
        }

        public Dictionary<int, MenuItem> GetTypeMenus(FSServer aServer, string navMenu, string menuPage, string menuParams, string menuPageDescription)
        {
            var aDictionary = new Dictionary<int, MenuItem>();

            const string strSQL = @"SELECT a.ActivityID, ActivityName 
                FROM dbo.Activities a
                INNER JOIN dbo.UserActivities b ON a.ActivityID = b.ActivityID AND UserID = @Param0
                WHERE Active = 1
                ORDER BY Ordering";

            aServer.Open();
            aServer.SQLParams.Add(_aUser.ID);

            aDictionary.Add(0, new MenuItem(navMenu, "Summary", menuPage, menuPageDescription + " for all activity", "ActivityID=0", menuParams == "ActivityID=0"));

            var aReader = aServer.ExecuteReader(strSQL);
            while (aReader.Read())
            {
                var selected = ("ActivityID=" + aReader["ActivityID"] == menuParams);

                aDictionary.Add(
                    int.Parse(aReader["ActivityID"].ToString()),
                    new MenuItem(navMenu, aReader["ActivityName"].ToString(), menuPage, menuPageDescription + " for " + aReader["ActivityName"], "ActivityID=" + aReader["ActivityID"], selected));
            }
            aReader.Close();

            aServer.Close();

            return aDictionary;
        }

        public Dictionary<int, SubMenuItem> GetSubMenuItems(string menuPage, string menuParams, string subMenuPage, string subMenuParams)
        {
            var aDictionary = new Dictionary<int, SubMenuItem>();

            _aServer.Open();
            _aServer.SQLParams.Add(_aUser.ID);
            _aServer.SQLParams.Add(menuPage);
            _aServer.SQLParams.Add(menuParams);

            const string strSQL = @"SELECT 
                    SubMenuItemID AS MenuID, 
                    SubMenuItemName AS MenuName, 
                    SubMenuPageName AS MenuPageName, 
                    SubMenuPageParams AS MenuPageParams 
                    FROM 
                    dbo.tblSubMenuItems a 
                    INNER JOIN dbo.tblMenuItems b ON a.MenuItemID = b.MenuItemID 
                    WHERE MenuPageName = @Param1 AND MenuPageParams = @Param2 ORDER BY a.Ordering";

            var aReader = _aServer.ExecuteReader(strSQL);
            if (aReader.HasRows)
            {
                while (aReader.Read())
                {
                    var menuPageName = aReader["MenuPageName"].ToString();
                    var selected = (menuPageName == subMenuPage && aReader["MenuPageParams"].ToString() == subMenuParams);

                    aDictionary.Add(int.Parse(aReader["MenuID"].ToString()), new SubMenuItem(
                        menuPage,
                        menuParams, 
                        aReader["MenuName"].ToString(), 
                        aReader["MenuPageName"].ToString(), 
                        aReader["MenuPageParams"].ToString(), selected));
                }
            }
            aReader.Close();

            _aServer.Close();

            return aDictionary;
        }

        public void Update(int userID, string pageIdentifier, string pageAccess)
        {
            _aServer.Open();

            _aServer.SQLParams.Clear();
            _aServer.SQLParams.Add(userID);
            _aServer.SQLParams.Add(pageIdentifier);
            _aServer.SQLParams.Add(pageAccess);

            _aServer.ExecuteNonQuery(MakeSQL(pageAccess));

            _aServer.Close();
        }

        private string MakeSQL(string strAccess)
        {
            if (int.Parse(strAccess) == 0)
            {
                return "DELETE FROM dbo.tblUserAccess WHERE UserID = @Param0 AND PageIdentifier = @Param1";
            }

            return @"IF EXISTS (SELECT * FROM dbo.tblUserAccess WHERE UserID = @Param0 AND PageIdentifier = @Param1) 
                BEGIN 
                UPDATE dbo.tblUserAccess SET PageAccess = @Param2 WHERE UserID = @Param0 AND PageIdentifier = @Param1 
                END 
                ELSE 
                BEGIN 
                INSERT INTO dbo.tblUserAccess (UserID, PageIdentifier, PageAccess) SELECT @Param0 AS UserID, @Param1 AS PageIdentifier, @Param2 AS PageAccess 
                END ";
        }
    }
}
