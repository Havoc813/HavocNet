namespace HavocNet.Repositories
{
    public static class RepositoryHelp
    {
        public static string MakeMoveUpSQL(string tableName, string idColumnName = "ID", string idOrderName = "Ordering")
        {
            return "DECLARE @OldOrder VARCHAR(20) " +
                    "DECLARE @NewOrder VARCHAR(20) " +
                    "SET @OldOrder = (SELECT " + idOrderName + " FROM dbo." + tableName + " WHERE " + idColumnName + " = @Param0) " +
                    "SET @NewOrder = (SELECT TOP 1 " + idOrderName + " FROM dbo." + tableName + " WHERE " + idOrderName + " < @OldOrder ORDER BY " + idOrderName + " DESC) " +
                    "UPDATE dbo." + tableName + " SET " + idOrderName + " = @OldOrder WHERE " + idColumnName + " = (SELECT " + idColumnName + " FROM dbo." + tableName + " WHERE " + idOrderName + " = @NewOrder) " +
                    "UPDATE dbo." + tableName + " SET " + idOrderName + " = @NewOrder WHERE " + idColumnName + " = @Param0 ";
        }

        public static string MakeMoveDownSQL(string tableName, string idColumnName = "ID", string idOrderName = "Ordering")
        {
            return "DECLARE @OldOrder VARCHAR(20) " +
                    "DECLARE @NewOrder VARCHAR(20) " +
                    "SET @OldOrder = (SELECT " + idOrderName + " FROM dbo." + tableName + " WHERE " + idColumnName + " = @Param0) " +
                    "SET @NewOrder = (SELECT TOP 1 " + idOrderName + " FROM dbo." + tableName + " WHERE " + idOrderName + " > @OldOrder ORDER BY " + idOrderName + ") " +
                    "UPDATE dbo." + tableName + " SET " + idOrderName + " = @OldOrder WHERE " + idColumnName + " = (SELECT " + idColumnName + " FROM dbo." + tableName + " WHERE " + idOrderName + " = @NewOrder) " +
                    "UPDATE dbo." + tableName + " SET " + idOrderName + " = @NewOrder WHERE " + idColumnName + " = @Param0 ";
        }
    }
}
