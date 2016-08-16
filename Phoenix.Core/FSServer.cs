using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Phoenix.Core
{
    public class FSServer : IDisposable
    {
        private readonly SqlConnection _dbConn;
        private readonly SqlCommand _dbComm;

        public ArrayList SQLParams { get; set; }

        public FSServer(string strConn)
        {
            _dbConn = new SqlConnection(strConn);
            _dbComm = new SqlCommand("", _dbConn) { CommandTimeout = 0 };
            SQLParams = new ArrayList();
        }

        public void Open()
        {
            SQLParams.Clear();

            _dbConn.Open();
        }

        public void Close()
        {
            SQLParams.Clear();
            _dbComm.Parameters.Clear();

            _dbConn.Close();
        }

        private void PrepCommand(string strSQL, ArrayList arr)
        {
            _dbComm.CommandText = strSQL;
            _dbComm.Parameters.Clear();

            for (var i = 0; i < arr.Count; i++)
            {
                _dbComm.Parameters.AddWithValue("@Param" + i, arr[i]);
            }
        }

        public DbDataReader ExecuteReader(string strSQL)
        {
            PrepCommand(strSQL, SQLParams);

            return _dbComm.ExecuteReader();
        }

        public int ExecuteNonQuery(string strSQL)
        {
            PrepCommand(strSQL, SQLParams);

            return _dbComm.ExecuteNonQuery();
        }

        public object ExecuteScalar(string strSQL)
        {
            PrepCommand(strSQL, SQLParams);

            return _dbComm.ExecuteScalar();
        }

        public DbDataReader ExecuteReader(string strSQL, ArrayList arr)
        {
            PrepCommand(strSQL, arr);

            return _dbComm.ExecuteReader();
        }

        public int ExecuteNonQuery(string strSQL, ArrayList arr)
        {
            PrepCommand(strSQL, arr);

            return _dbComm.ExecuteNonQuery();
        }

        public object ExecuteScalar(string strSQL, ArrayList arr)
        {
            PrepCommand(strSQL, arr);

            return _dbComm.ExecuteScalar();
        }

        public DataTable ExecuteReaderAsDataTable(string strSQL)
        {
            PrepCommand(strSQL, SQLParams);

            var dt = new DataTable("aTable");
            dt.Load(_dbComm.ExecuteReader());

            return dt;
        }

        public int BulkLoadDataTable(DataTable dt)
        {
            var s = new SqlBulkCopy(_dbConn) { DestinationTableName = dt.TableName };

            foreach (var aColumn in dt.Columns)
            {
                s.ColumnMappings.Add(aColumn.ToString(), aColumn.ToString());
            }

            s.WriteToServer(dt);

            s.Close();

            return dt.Rows.Count;
        }

        public ArrayList ExecuteReaderAsArrayList(string strSQL)
        {
            var aList = new ArrayList();

            PrepCommand(strSQL, SQLParams);

            var aReader = _dbComm.ExecuteReader();

            while (aReader.Read())
            {
                aList.Add(aReader[0].ToString());
            }
            aReader.Close();

            return aList;
        }

        public Dictionary<int, string> ExecuteReaderAsDictionary(string strSQL)
        {
            var aList = new Dictionary<int, string>();

            PrepCommand(strSQL, SQLParams);

            var aReader = _dbComm.ExecuteReader();

            while (aReader.Read())
            {
                aList.Add(int.Parse(aReader[0].ToString()), aReader[1].ToString());
            }
            aReader.Close();

            return aList;
        }

        public void Dispose()
        {
            Close();

            _dbComm.Dispose();
            _dbConn.Dispose();
        }

        public bool IsOpen()
        {
            return (_dbConn.State == ConnectionState.Open);
        }

        public bool TryOpen()
        {
            try
            {
                Open();
                return IsOpen();
            }
            catch (SqlException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string ServerName
        {
            get { return _dbConn.DataSource; }
        }

        public ArrayList PublishSQLParams()
        {
            var display = new ArrayList();

            for (var i = 0; i < SQLParams.Count; i++)
            {
                display.Add("DECLARE @Param" + i + " VARCHAR(255) SET @Param" + i + " = '" + SQLParams[i] + "' ");
            }

            return display;
        }

        public string PublishSQLParamsAsString()
        {
            var str = "";

            for (var i = 0; i < SQLParams.Count; i++)
            {
                str += "@Param" + i + " = '" + SQLParams[i] + "' ";
            }

            return str;
        }

        public DbDataReader ExecuteReaderWithPaging(int pageID, int pageSize, string sql, string sqlOrderBy)
        {
            if (!sql.Contains("ORDER BY")) throw new Exception("SQL must have order by to be paged");

            PrepCommand(
                buildPagingSQL(
                    sql.Substring(0, sql.IndexOf("ORDER BY", StringComparison.Ordinal)),
                    sqlOrderBy
                ),
                this.SQLParams
            );

            _dbComm.Parameters.AddWithValue("@pageNum", pageID);
            _dbComm.Parameters.AddWithValue("@pageSize", pageSize);

            return _dbComm.ExecuteReader();
        }

        private string buildPagingSQL(string sql, string orderBy)
        {
            return @"SELECT * FROM (
                    SELECT ROW_NUMBER() OVER (ORDER BY " + orderBy + ") AS row_num, * FROM (" +
                   sql +
                   @") AS tempTable1
                    ) AS tempTable2 
                    WHERE 
                    row_num >= ((@pageNum -1) * @pageSize) 
                    AND row_num < ((@pageNum -1) * @pageSize) + @pageSize";
        }
    }
}