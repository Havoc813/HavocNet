using System;
using System.Collections.Generic;

namespace Phoenix.Core.Models
{
    public interface ISchedulerReport
    {
        int ReportId { get; set; }
        Boolean Enabled { get; set; }
        string ReportName { get; set; }
        string ReportDescription { get; set; }
        string ReportPath { get; set; }
        string ReportFilename { get; set; }
        string SystemTag { get; set; }
        string SystemAppDataPath { get; set; }
        List<ISchedulerSubscription> Subscriptions { get; set; }
        string Publish();
    }
}