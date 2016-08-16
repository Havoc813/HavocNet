using System.Collections;
using System.Data;
using System.Linq;
using Phoenix.Core.Logging;

namespace Phoenix.Core
{
    public class FSScript
    {
        private readonly FSServer _aServer;
        private readonly string _fileName;
        public readonly ArrayList Variables;
        private string _command = "";

        public FSScript(FSServer aServer, string fileName, ArrayList variables, string command = "")
        {
            _aServer = aServer;
            _fileName = fileName;
            Variables = variables;
            _command = command;
        }

        public DataTable Run()
        {
            _aServer.Open();

            var strSQL = "DECLARE @strSQL VARCHAR(1024) SET @strSQL = '" + _command + " " + this._fileName + "";

            strSQL = Variables.Cast<string>().Aggregate(strSQL, (current, s) => current + (" " + s + ""));

            strSQL += "' EXEC master..xp_cmdshell @strSQL";

            var aLog = new WebLog();
            aLog.WriteToFile("Scripts.log", strSQL);

            var results = _aServer.ExecuteReaderAsDataTable(strSQL);

            _aServer.Close();

            return results;
        }

        public string RunAsString()
        {
            var result = "";
            foreach (DataRow o in this.Run().Rows)
            {
                result += (o[0] + "<br />");
            }
            return result;
        }
    }
}
