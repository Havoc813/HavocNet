using System;
using System.Collections;
using System.Diagnostics;

namespace Phoenix.Core.Logging
{
    public class FSAudit
    {
        private readonly string _action;
        private readonly string _data;
        private readonly string _username;
        private readonly string _system;
        protected readonly FSServer aServer;

        private bool _autoClose;

        public string Username
        {
            get { return _username; }
        }

        public string Action
        {
            get { return _action; }
        }

        public string AuditData
        {
            get { return _data; }
        }

        public FSAudit(FSServer aServer, string username, string auditAction, string auditData)
        {
            this.aServer = aServer;
            _data = auditData;
            _action = auditAction;
            _username = username;
        }

        public void Create()
        {
            var arr = new ArrayList();

            if (!aServer.IsOpen())
            {
                aServer.Open();
                _autoClose = true;
            }

            try
            {
                arr.Add(Username);
                arr.Add(Action);
                arr.Add(AuditData);

                aServer.ExecuteNonQuery("INSERT INTO dbo.Audits (Username, Timestamp, Action, Data) VALUES (@Param0, getDate(), @Param1, @Param2)",arr);
            }
            catch (Exception ex)
            {
                var aError = new FSError(aServer);

                FSError.LogInFile(Username, "/FSAudit.vb", ex, "FSAudit_Err.txt");
                aError.LogInTable(Username, this._system, "/FSAudit.vb", ex);
            }
            finally
            {
                LogInFile();
            }

            if (_autoClose)
            {
                aServer.Close();
            }
        }

        public void LogInFile()
        {
            var arr = new ArrayList
                {
                    "**********************************************************************",
                    "Date:    " + DateTime.Now.ToString("dd/MM/yyyy hh:mm"),
                    "User:    " + Username,
                    "Action   " + _action,
                    "Data:    " + _data
                };

            FSLog.WriteArrayToFile("FSAudits.txt", arr);
        }

        public void LogInEventLog(string auditDesc)
        {
            const string eventLogName = "Phoenix Log";
            var aLog = new EventLog();

            if (!EventLog.SourceExists(eventLogName))
            {
                EventLog.CreateEventSource(eventLogName, eventLogName);
            }

            aLog.Source = eventLogName;
            aLog.WriteEntry(auditDesc, EventLogEntryType.Error);
        }
    }
}
