using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using Phoenix.Core.Repositories;
using Phoenix.Core.Templates;

namespace Phoenix.Core.Models
{
    public class SchedulerDistributionList
    {
        public Dictionary<string, SchedulerDistributionItem> Items { get; private set; }
        
        public SchedulerDistributionList()
        {
            Items = new Dictionary<string, SchedulerDistributionItem>();
        }

        public string Add(ISchedulerReport report, ISchedulerSubscription subscription)
        {
            var subscriptionParams =
                new SchedulerSubscriptionParameterRepository().GetParametersStringForSubscription(
                    subscription.SubscriptionId);
            var reportKey = string.Format("{0}{1}{2}", report.ReportId.ToString("00000"), FSFormat.BasicDateTime(subscription.StartTime), subscriptionParams);
            if (!Items.ContainsKey(reportKey))
            {
                Items.Add(reportKey, new SchedulerDistributionItem(report));
            }
            Items[reportKey].RecipientEmailAddresses.Add(subscription.PersonEmailAddress);

            return reportKey;
        }

        public void SendEmail()
        {
            var fromEmailAddress = FSConfig.AppSettings("SchedulerFromAddress");
            var fromAlias = FSConfig.AppSettings("SchedulerAlias");
            var currentDate = FSFormat.BasicDate(DateTime.Now);

            foreach (var report in Items.Values)
            {
                if (!report.ReportComplete) 
                    continue;

                var emailSubject = string.Format("{0} {1}", report.ReportName, currentDate);
                var reportName = report.ReportName;
                if (!reportName.Contains("Report"))
                    reportName += " Report";
                var template = CoreTemplates.ReportTemplate;
                template = template.Replace(@"||Report||", reportName);
                template = template.Replace(@"||Summary||",
                    report.Summary.Length > 0 ? string.Format("<br/><br/>Report Summary<br/>{0}<br/><br/", report.Summary) : "");
                var email = new FSEmail(emailSubject, MailPriority.Normal, template);
                email.Attachments = Path.Combine(report.SystemAppDataPath, report.Filename);
                email.SendInlineImage(report.RecipientEmailAddresses, fromEmailAddress, fromAlias);
            }
        }
    }
}
