using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace Phoenix.Core
{
    public class FSOracleServer : IDisposable
    {
        private readonly OleDbConnection _dbConn;
        private readonly OleDbCommand _dbComm;

        public readonly ArrayList OracleParams = new ArrayList();

        public FSOracleServer(string strConnName)
        {
            _dbConn = new OleDbConnection(FSConfig.ConnectionString(strConnName));
            _dbComm = new OleDbCommand("", _dbConn) { CommandTimeout = 0 };
        }

        public void Open()
        {
            OracleParams.Clear();

            _dbConn.Open();
        }

        public void Close()
        {
            OracleParams.Clear();

            _dbConn.Close();
        }

        private void PrepCommand(string strSQL, ArrayList arr)
        {
            _dbComm.CommandText = "ALTER SESSION SET NLS_LANGUAGE = 'AMERICAN' NLS_TERRITORY = 'AMERICA' ";
            _dbComm.ExecuteNonQuery();

            _dbComm.CommandText = strSQL;
            _dbComm.Parameters.Clear();

            for (var i = 0; i < arr.Count; i++)
            {
                _dbComm.Parameters.AddWithValue("PARAM_" + i, arr[i]);
            }
        }

        public DbDataReader ExecuteReader(string strSQL)
        {
            PrepCommand(strSQL, OracleParams);

            return _dbComm.ExecuteReader();
        }

        public int ExecuteNonQuery(string strSQL)
        {
            PrepCommand(strSQL, OracleParams);

            return _dbComm.ExecuteNonQuery();
        }

        public object ExecuteScalar(string strSQL)
        {
            PrepCommand(strSQL, OracleParams);

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
            PrepCommand(strSQL, OracleParams);

            var dt = new DataTable("aTable");
            dt.Load(_dbComm.ExecuteReader());

            return dt;
        }

        public ArrayList ExecuteReaderAsArrayList(string strSQL)
        {
            var aList = new ArrayList();

            PrepCommand(strSQL, OracleParams);

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

            PrepCommand(strSQL, OracleParams);

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

        public string ServerName
        {
            get { return _dbConn.DataSource; }
        }
    }
}