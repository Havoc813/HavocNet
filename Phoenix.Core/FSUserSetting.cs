namespace Phoenix.Core
{
    public static class FSUserSetting
    {
        public static string GetSetting(FSUser aUser, string settingName)
        {
            var aServer = aUser.UserServer;
            if (!aServer.IsOpen()) aServer.Open();
            aServer.SQLParams.Add(aUser.ID);
            aServer.SQLParams.Add(settingName);

            const string strSQL = @"DECLARE @x VARCHAR(50) 
                    SELECT @x=SettingValue FROM dbo.UserSettings WHERE UserID = @Param0 AND SettingName = 'BU'
                    SELECT isnull(@x, 'Not Set')";

            var settingValue = aServer.ExecuteScalar(strSQL).ToString();

            aServer.Close();

            return settingValue;
        }

        public static void SetSetting(FSUser aUser, string settingName, string settingValue)
        {
            var aServer = aUser.UserServer;
            if (!aServer.IsOpen()) aServer.Open();
            aServer.SQLParams.Add(aUser.ID);
            aServer.SQLParams.Add(settingName);
            aServer.SQLParams.Add(settingValue);

            var strSQL = @"IF EXISTS(SELECT * FROM dbo.UserSettings WHERE UserID = @Param0 AND SettingName = @Param1)
                            BEGIN
                                UPDATE dbo.UserSettings 
                                SET SettingValue = @Param2 
                                WHERE UserID = @Param0 AND SettingName = @Param1
                            END
                            ELSE
                            BEGIN
                                INSERT INTO dbo.UserSettings(UserID, SettingName, SettingValue) 
                                SELECT @Param0 AS UserID, @Param1 AS SettingName, @Param2 AS SettingValue
                            END";

            aServer.ExecuteNonQuery(strSQL);

            aServer.Close();
        }
    }
}
