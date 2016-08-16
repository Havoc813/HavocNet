using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using Phoenix.Core.Logging;

namespace Phoenix.Core
{
    public class MAXL : Page
    {
        private readonly string _fileName;
        private DataTable _results;
        private readonly FSServer _aServer;

        public readonly ArrayList Variables = new ArrayList();
        public ScriptResult Result = ScriptResult.Success;

        public enum ScriptResult
        {
            Success = 0,
            Failure = 1,
            Warning = 2
        }

        public MAXL(string scriptName, FSServer aServer)
        {
            this._fileName = scriptName;
            this._aServer = aServer;
        }

        public string Run()
        {
            var aScript = new FSScript(_aServer, this._fileName, Variables, "startMAXL");
            this._results = aScript.Run();

            if (InterpretResults() != ScriptResult.Success)
            {
                var err = new FSError(_aServer);
                err.LogInTable(HttpContext.Current.User.Identity.Name, "FSM", HttpContext.Current.Request.Url.ToString(), new Exception(DisplayResults()));
            }

            return "<p>" + DisplayResults() + "</p>";
        }

        public void Audit(string strResult)
        {
            var aAudit = new FSAudit(_aServer, HttpContext.Current.User.Identity.Name, this._fileName + " " + strResult, this.DisplayVariables());
            aAudit.Create();
        }

        private string DisplayVariables()
        {
            return this.Variables.Cast<string>().Aggregate("", (current, aStr) => current + aStr);
        }

        private ScriptResult InterpretResults()
        {
            var i = 0;

            while (i < this._results.Rows.Count)
            {
                var aRow = this._results.Rows[i][0].ToString();

                if (aRow != "")
                {
                    if (aRow.IndexOf("ERROR - ", StringComparison.Ordinal) >= 0 | aRow.StartsWith("essmsh error: "))
                    {
                        Result = ScriptResult.Failure;

                        break;
                    }
                    if (aRow.IndexOf("WARNING - ", StringComparison.Ordinal) >= 0)
                    {
                        Result = ScriptResult.Warning;
                    }
                }
                i = i + 1;
            }

            switch (Result)
            {
                case ScriptResult.Failure:
                    this.Audit("Failed");
                    break;
                case ScriptResult.Warning:
                    this.Audit("Warning");
                    break;
                default:
                    this.Audit("Succeeded");
                    break;
            }

            return Result;
        }

        public string DisplayResults()
        {
            var returnStr = "";
            var i = 0;
            var newBlock = 0;
            var hasError = false;
            var hasWarn = false;

            if (this._results.Rows[i][0].ToString().StartsWith("essmsh error: "))
            {
                while (i < this._results.Rows.Count)
                {
                    returnStr += this._results.Rows[i][0] + "<br />";
                }
            }
            else
            {
                while (i < this._results.Rows.Count)
                {
                    var aRow = this._results.Rows[i][0].ToString();

                    if (aRow == "")
                    {
                        i += 1;
                        continue;
                    }

                    if (aRow.StartsWith("MAXL> "))
                    {
                        if (hasError)
                        {
                            returnStr += "<span style='color:red'>ERROR</span><br />";
                        }
                        else if (hasWarn)
                        {
                            returnStr += "<span style='color:orange'>WARNING</span><br />";
                        }
                        else
                        {
                            if (newBlock == 1)
                                returnStr += "<span style='color:green'>SUCCESS</span><br />";
                        }

                        if (aRow.IndexOf("login", StringComparison.Ordinal) >= 0)
                        {
                            returnStr += "<br />MAXL> login<br />";
                        }
                        else
                        {
                            returnStr += "<br />" + aRow + "<br />";
                        }
                        newBlock = 1;
                        hasError = false;
                        hasWarn = false;
                    }
                    else if (aRow.StartsWith("   ERROR - "))
                    {
                        returnStr += aRow + "<br />";

                        hasError = true;
                        newBlock = 1;
                    }
                    else if (aRow.StartsWith(" WARNING - "))
                    {
                        returnStr += aRow + "<br />";

                        hasWarn = true;
                        newBlock = 1;
                    }
                    i = i + 1;
                }
            }

            return returnStr;
        }
    }
}
