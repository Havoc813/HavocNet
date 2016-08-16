using System;
using System.Collections;
using System.Data;
using System.Web;
using Phoenix.Core.Logging;

namespace Phoenix.Core
{
    public class FSSSIS
    {
        private readonly bool _debug;
        private readonly WebLog _debugLog = new WebLog();

        private readonly string _packageName;
        private readonly string _packageServer;
        private readonly string _packageAction;
        private readonly ArrayList _variableNames;
        private readonly ArrayList _variableValues;
        private DataTable _results;
        private readonly FSServer _aServer;

        public SSISPackageResult Result = SSISPackageResult.Failure;

        public enum SSISPackageResult
        {
            Success = 0,
            Failure = 1,
            PackageNotFound = 2,
            ServerNotFound = 3
        }

        public FSSSIS(FSServer aServer, string packageServer, string packageName, ArrayList variableNames, ArrayList variableValues, string action)
        {
            this._aServer = aServer;
            this._packageName = packageName;
            this._packageServer = packageServer;
            this._variableNames = variableNames;
            this._variableValues = variableValues;
            this._packageAction = action;

            this._debug = (FSConfig.AppSettings("debug") == "true");
        }

        public void Run()
        {
            _aServer.Open();
            this._aServer.SQLParams = _variableValues;

            var strSQL = @"DECLARE @strSQL VARCHAR(1024) " +
                         @"SET @strSQL = 'C:\""Program Files (x86)""\""Microsoft SQL Server""\100\DTS\Binn\DTEXEC.exe " +
                         @"/SQL \" + this._packageName + " " +
                         @"/SERVER " + this._packageServer + " " +
                         @"/MAXCONCURRENT -1 /CHECKPOINTING OFF  ";

            var i = 0;

            foreach (string vbl in this._variableNames)
            {
                strSQL += @"/SET \Package.Variables[User::" + vbl + "].Value;\"' + @Param" + Convert.ToString(i) + " + '\" ";

                i = i + 1;
            }
            strSQL += "' EXEC master..xp_cmdshell @strSQL";

            if (_debug) _debugLog.WriteToFile("SSIS.txt", "Name: " + _packageName);
            if (_debug) _debugLog.WriteToFile("SSIS.txt", "Server: " + _packageServer);
            if (_debug) _debugLog.WriteArrayToFile("SSIS.txt", _variableNames);
            if (_debug) _debugLog.WriteArrayToFile("SSIS.txt", _variableValues);
            if (_debug) _debugLog.WriteToFile("SSIS.txt", "Query: " + strSQL);

            _results = _aServer.ExecuteReaderAsDataTable(strSQL);

            if (InterpretResults() != SSISPackageResult.Success)
            {
                var aError = new FSError(_aServer);
                aError.LogInTable(HttpContext.Current.User.Identity.Name, "FSM", HttpContext.Current.Request.Url.ToString(), new Exception(GetErrorMessage()));
            }

            _aServer.Close();
        }

        private void Audit(string strResult)
        {
            var data = "";

            for (var i = 0; i <= this._variableNames.Count - 1; i++)
            {
                var vblName = this._variableNames[i].ToString().Replace("gstr", "").Replace("gint", "");
                var vblVal = this._variableValues[i].ToString();

                if (vblName.ToLower().Contains("password"))
                {
                    vblVal = "**********";
                }

                data += vblName + ": " + vblVal + ", ";
            }

            var aAudit = new FSAudit(_aServer, HttpContext.Current.User.Identity.Name, this._packageAction + " " + strResult, data.Remove(data.Length - 2));
            aAudit.Create();
        }

        private SSISPackageResult InterpretResults()
        {
            var i = 0;

            while (i < this._results.Rows.Count)
            {
                var aRow = this._results.Rows[i][0].ToString();

                if (_debug) _debugLog.WriteToFile("SSIS.txt", aRow);

                if (aRow != "")
                {
                    if (aRow == "DTExec: The package execution returned DTSER_SUCCESS (0).")
                    {
                        Result = SSISPackageResult.Success;
                        this.Audit("Succeeded");
                    }
                    else if (aRow.Contains("Could not load package"))
                    {
                        if (aRow.Contains("0xC0014062"))
                        {
                            Result = SSISPackageResult.ServerNotFound;
                            this.Audit("Failed (Server Not Found)");
                        }
                        else
                        {
                            Result = SSISPackageResult.PackageNotFound;
                            this.Audit("Failed (Package Not Found)");
                        }
                    }
                    else if (aRow.Contains("Could not set \\Package.Variables"))
                    {
                        this.Audit("Problem Setting Variable - Package Out Of Date");
                    }
                    else if (aRow == "DTExec: The package execution returned DTSER_FAILURE (1).")
                    {
                        this.Audit("Failed");
                    }
                }
                i = i + 1;
            }

            return Result;
        }

        public string GetErrorMessage()
        {
            var returnStr = "";
            var i = 0;

            switch (Result)
            {
                case SSISPackageResult.Success:
                    break;
                case SSISPackageResult.PackageNotFound:
                    returnStr = "Could Not Find Package " + this._packageName + " on server " + this._packageServer;
                    break;
                case SSISPackageResult.ServerNotFound:
                    returnStr = "Could Not Find Server " + this._packageServer;
                    break;
                case SSISPackageResult.Failure:
                    while (i < this._results.Rows.Count)
                    {
                        var aRow = this._results.Rows[i][0].ToString();

                        if (aRow != "")
                        {
                            if ((aRow.IndexOf("Error:", StringComparison.Ordinal) >= 0 & aRow.IndexOf("End Error", StringComparison.Ordinal) < 0) | aRow.IndexOf("error ", StringComparison.Ordinal) >= 0)
                            {
                                //Navigate to the description
                                returnStr += aRow + "<br />";

                                while (aRow.IndexOf("Description", StringComparison.Ordinal) == -1)
                                {
                                    i = i + 1;
                                    aRow = this._results.Rows[i][0].ToString();
                                }

                                returnStr += aRow + "<br />";
                            }
                        }
                        i = i + 1;
                    }
                    break;
            }

            return returnStr;
        }
    }
}
