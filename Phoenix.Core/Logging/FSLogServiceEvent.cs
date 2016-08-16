using System;
using System.IO;
using Phoenix.Core.Email;
using Phoenix.Core.Enums;

namespace Phoenix.Core.Logging
{
    public static class FSLogServiceEvent
    {
        public static void LogInFile(string fileName, string path, string message)
        {
            FSLog.WriteToFile(Path.Combine(path, fileName), message);
        }

        public static void SendEmail(string service, string serviceName, string servicePath, ServiceState state)
        {
            var subject = string.Format("Service {0} {1}", service, Enum.GetName(typeof(ServiceState),state));
            var messageBody = EmailBody.GetServiceBody(service, serviceName, servicePath, state);
            var email = new FSEmail(subject, System.Net.Mail.MailPriority.Normal, messageBody);
            email.Send(FSConfig.AppSettings("FSHelpdesk"));
        }
    }
}
