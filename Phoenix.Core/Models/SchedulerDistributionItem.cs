using System.Collections.Generic;

namespace Phoenix.Core.Models
{
    public class SchedulerDistributionItem
    {
        public string ReportName { get; private set; }
        public string Filename { get; set; }
        public string SystemAppDataPath { get; private set; }
        public List<string> RecipientEmailAddresses { get; set; }
        public bool ReportComplete { get; set; }
        public string Summary { get; set; }

        public SchedulerDistributionItem(ISchedulerReport report)
        {
            ReportName = report.ReportName;
            Filename = report.ReportFilename;
            SystemAppDataPath = report.SystemAppDataPath;
            RecipientEmailAddresses = new List<string>();
        }
    }
}
