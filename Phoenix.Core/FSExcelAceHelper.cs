using System;
using System.Data;
using System.Data.OleDb;

namespace Phoenix.Core
{
    public class FSExcelAceHelper
    {
        public Exception Error;

        public DataTable LoadXLS(string strFile, String sheetName, string nameCheck = "")
        {
            var dt = new DataTable(sheetName);
            var strConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", strFile);
            var aConn = new OleDbConnection(strConnectionString);

            try
            {
                aConn.Open();

                var aAdapter = new OleDbDataAdapter();
                var i = 1;

                aAdapter.SelectCommand = new OleDbCommand("SELECT * FROM [" + sheetName + "$A" + (i++) + ":Z]", aConn);
                aAdapter.Fill(dt);

                if (nameCheck != "")
                {
                    while (!dt.Columns.Contains(nameCheck))
                    {
                        dt.Reset();

                        aAdapter.SelectCommand = new OleDbCommand("SELECT * FROM [" + sheetName + "$A" + (i++) + ":Z]", aConn);
                        aAdapter.Fill(dt);
                    }
                }
            }
            catch (Exception e)
            {
                this.Error = e;
            }
            finally
            {
                aConn.Close();
            }

            return dt;
        }

        public bool HasError()
        {
            return Error != null;
        }
    }
}
