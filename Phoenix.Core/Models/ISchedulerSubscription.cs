using System;

namespace Phoenix.Core.Models
{
    public interface ISchedulerSubscription
    {
        int SubscriptionId { get; set; }
        int PersonId { get; set; }
        string PersonEmailAddress { get; set; }
        DateTime StartTime { get; set; }
        string Frequency { get; set; }
        void UpdateStartTimeFromNow();
        void UpdateStartTimeFromOriginal();
        string Publish();
    }
}