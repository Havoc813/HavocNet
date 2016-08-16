using System;
using System.Collections;
using System.Diagnostics;
using Phoenix.Core.Email;

namespace Phoenix.Core.Logging
{
    public class FSError
    {
        protected readonly FSServer aServer;
        private bool _autoClose;
        private const string ErrorInTableLog = "FSError_Err.txt";

        public FSError(FSServer aServer)
        {
            this.aServer = aServer;
            this._autoClose = false;
        }

        public void LogInTable(string userID, string app, string url, Exception err)
        {
            var @params = new ArrayList();

            if (!aServer.IsOpen())
            {
                aServer.Open();
                _autoClose = true;
            }

            try
            {
                @params.Add(userID);
                @params.Add(app);
                @params.Add(url);
                @params.Add(string.IsNullOrEmpty(err.Source) ? "" : err.Source);
                @params.Add(string.IsNullOrEmpty(err.StackTrace) ? "" : err.StackTrace);
                @params.Add(err.Message);

                const string strSQL = @"INSERT INTO dbo.Errors (Timestamp,UserID,ErrorApplication,ErrorURL,ErrorSource,ErrorStackTrace,ErrorMessage) 
                    SELECT 
                    getDate() AS Timestamp, 
                    @Param0 AS UserID, 
                    @Param1 AS ErrorApplication, 
                    @Param2 AS ErrorURL, 
                    @Param3 AS ErrorSource, 
                    @Param4 AS ErrorStackTrace, 
                    @Param5 AS ErrorMessage ";

                aServer.ExecuteNonQuery(strSQL, @params);
            }
            catch (Exception e)
            {
                LogInFile(userID, url, err, ErrorInTableLog);
                LogInFile(userID, url, e, ErrorInTableLog);
            }

            if (_autoClose) aServer.Close();
        }

        public static void LogInFile(string userID, string url, Exception err, String location)
        {
            var arr = new ArrayList
                {
                    "**********************************************************************",
                    "Date:    " + FSFormat.BasicDateTime(DateTime.Now),
                    "User:    " + userID,
                    "URL:     " + url,
                    string.IsNullOrEmpty(err.Source) ? "Source:  " : "Source:  " + err.Source,
                    "Message: " + err.Message,
                    string.IsNullOrEmpty(err.StackTrace) ? "Stack:  " : "Stack:  " + err.StackTrace
                };

            FSLog.WriteArrayToFile(location, arr);
        }

        public void LogInEventLog(string errDesc)
        {
            const string eventLogName = "FS Log";
            var aLog = new EventLog();

            if (!EventLog.SourceExists(eventLogName))
            {
                EventLog.CreateEventSource(eventLogName, eventLogName);
            }

            aLog.Source = eventLogName;
            aLog.WriteEntry(errDesc, EventLogEntryType.Error);
        }

        public static void EmailFSHelpdesk(string user, string system, string component, Exception err)
        {
            var subject = string.Format("{0} Error Occurred", system);
            var messageBody = EmailBody.GetErrorBody(user, system, component, err);
            var email = new FSEmail(subject, System.Net.Mail.MailPriority.High, messageBody);
            email.Send(FSConfig.AppSettings("FSHelpdesk"));
        }

        public static void EmailITHelpdesk(string user, string system, string component, Exception err)
        {
            var subject = string.Format("{0} Error Occurred", system);
            var messageBody = EmailBody.GetErrorBody(user, system, component, err);
            var email = new FSEmail(subject, System.Net.Mail.MailPriority.High, messageBody);
            email.Send(FSConfig.AppSettings("ITHelpdesk"));
        }
    }
}
