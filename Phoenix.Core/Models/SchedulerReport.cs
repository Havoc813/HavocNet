using System;
using System.Collections.Generic;
using System.Linq;

namespace Phoenix.Core.Models
{
    public class SchedulerReport : ISchedulerReport
    {
        public int ReportId { get; set; }
        public Boolean Enabled { get; set; }
        public string ReportName { get; set; }
        public string ReportDescription { get; set; }
        public string ReportPath { get; set; }
        public string ReportFilename { get; set; }
        public string SystemTag { get; set; }
        public string SystemAppDataPath { get; set; }
        public List<ISchedulerSubscription> Subscriptions { get; set; }

        public SchedulerReport(int reportId, bool enabled, string reportName, string reportDescr, string reportPath, string fileName,
                               string systemTag, string systemAppDataPath,  List<ISchedulerSubscription> subscriptions )
        {
            ReportId = reportId;
            Enabled = enabled;
            ReportName = reportName;
            ReportDescription = reportDescr;
            ReportPath = reportPath;
            ReportFilename = fileName;
            SystemTag = systemTag;
            SystemAppDataPath = systemAppDataPath;
            Subscriptions = subscriptions;
        }

        public string Publish()
        {
            return @"ID: " + ReportId + @"
                Enabled: " + Enabled + @"
                Name: " + ReportName + @"
                Description: " + ReportDescription + @"
                Path: " + ReportPath + @"
                Filename: " + ReportFilename + @"
                System: " + SystemTag + @"
                Subscriptions: 
                {" + Subscriptions.Aggregate("", (current, sub) => current + sub.Publish()) + @"}";
        }
    }
}
