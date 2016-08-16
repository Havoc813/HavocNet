using System;
using Phoenix.Core.Enums;

namespace Phoenix.Core.Models
{
    public class SchedulerSubscriptionParameter : ISchedulerSubscriptionParameter
    {
        public int SubscriptionId { get; set; }
        public int ParameterId { get; set; }
        public string ParameterName { get; set; }
        public SchedulerSubscriptionParameterType ParameterType { get; set; }
        public string ParameterValue { get; set; }

        public SchedulerSubscriptionParameter(int subscriptionId, int parameterId, string parameterName, string parameterType, string parameterValue)
        {
            SubscriptionId = subscriptionId;
            ParameterId = parameterId;
            ParameterName = parameterName;
            ParameterType = (SchedulerSubscriptionParameterType)Enum.Parse(typeof(SchedulerSubscriptionParameterType), parameterType);
            ParameterValue = parameterValue;
        }

        public object GetTypeCastedValue()
        {
            if (ParameterType.Equals(SchedulerSubscriptionParameterType.Date))
                return DateTime.Parse(ParameterValue);

            if (ParameterType.Equals(SchedulerSubscriptionParameterType.Decimal))
                return double.Parse(ParameterValue);

            if (ParameterType.Equals(SchedulerSubscriptionParameterType.Integer))
                return int.Parse(ParameterValue);

            return ParameterValue;
        }
    }
}
