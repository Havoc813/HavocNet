using System;
using System.Collections.Generic;
using System.Text;
using Phoenix.Core.Enums;
using Phoenix.Core.Templates;

namespace Phoenix.Core.Email
{
    public static class EmailBody
    {
        public static string GetMessageBody(string leaveTemplate, Dictionary<string, string> replacingValues)
        {
            var messageBody = new StringBuilder(leaveTemplate);
            foreach (var keyPair in replacingValues)
                messageBody.Replace(keyPair.Key, keyPair.Value);

            return messageBody.ToString();
        }

        public static string GetErrorBody(string user, string system, string component, Exception err)
        {
            var replacementValues = new Dictionary<string, string>
                {
                    {"||System||", system},
                    {"||DateTime||", FSFormat.BasicDateTime(DateTime.Now)},
                    {"||User||", user},
                    {"||Component||", component},
                    {"||Type||", err.GetType().FullName},
                    {"||Source||", String.IsNullOrEmpty(err.Source) ? "" : err.Source},
                    {"||Message||", err.Message},
                    {"||StackTrace||", String.IsNullOrEmpty(err.StackTrace) ? "" : err.StackTrace}
                };

            return GetMessageBody(CoreTemplates.ErrorTemplate, replacementValues);
        }

        public static string GetServiceBody(string service, string serviceName, string servicePath, ServiceState state)
        {
            var replacementValues = new Dictionary<string, string>
                {
                    {"||Service||", service},
                    {"||DateTime||", FSFormat.BasicDateTime(DateTime.Now)},
                    {"||ServiceName||", serviceName},
                    {"||ServicePath||", servicePath},
                    {"||Status||", Enum.GetName(typeof(ServiceState),state)}
                };

            return GetMessageBody(CoreTemplates.ServiceTemplate, replacementValues);
        }

        public static string GetWelcomeBody(string user, string system, string systemLink)
        {
            var replacementValues = new Dictionary<string, string>
                {
                    {"||User||", user},
                    {"||System||", system},
                    {"||SystemLink||", systemLink},
                };

            return GetMessageBody(CoreTemplates.WelcomeTemplate, replacementValues);
        }

        public static string GetNoAccessBody(string user, string system, string systemLink)
        {
            var replacementValues = new Dictionary<string, string>
                {
                    {"||User||", user},
                    {"||System||", system},
                    {"||SystemLink||", systemLink},
                };

            return GetMessageBody(CoreTemplates.NoAccessTemplate, replacementValues);
        }
    }
}
