using System;

namespace Phoenix.Core.Models
{
    public class SchedulerSubscription : ISchedulerSubscription
    {
        public int SubscriptionId { get; set; }
        public int PersonId { get; set; }
        public string PersonEmailAddress { get; set; }
        public DateTime StartTime { get; set; }
        public string Frequency { get; set; }

        public SchedulerSubscription(int subscriptionId, int personId, string email, DateTime startTime, string frequency)
        {
            SubscriptionId = subscriptionId;
            PersonId = personId;
            PersonEmailAddress = email;
            StartTime = startTime;
            Frequency = frequency;
        }

        public void UpdateStartTimeFromNow()
        {
            var currentDate = DateTime.Now;
            currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, currentDate.Minute, 0);
            UpdateStartTime(currentDate);
        }

        public void UpdateStartTimeFromOriginal()
        {
            UpdateStartTime(StartTime);
        }

        private void UpdateStartTime(DateTime newStartTime)
        {
            var freqPeriod = Frequency.Substring(0, 2).ToUpper();
            var freqValue = int.Parse(Frequency.Substring(2));
            
            switch (freqPeriod)
            {
                case "DD":
                    //StartTime = newStartTime.AddDays(freqValue);
                    StartTime = FSDateHelper.GetNextWeekDay(newStartTime.AddDays(freqValue));
                    break;
                case "WW":
                    StartTime = newStartTime.AddDays(freqValue * 7);
                    break;
                case "MM":
                    StartTime = newStartTime.AddMonths(freqValue);
                    break;
                case "YY":
                    StartTime = newStartTime.AddYears(freqValue);
                    break;
                case "HH":
                    //StartTime = newStartTime.AddHours(freqValue);
                    StartTime = FSDateHelper.GetNextWeekDay(newStartTime.AddHours(freqValue));
                    break;
            }
        }

        public string Publish()
        {
            return "Start: " + FSFormat.BasicDateTime(StartTime) + @"
                    Frequency: " + Frequency + @"
                    Person: " + PersonEmailAddress + @"
                    ";
        }
    }
}
