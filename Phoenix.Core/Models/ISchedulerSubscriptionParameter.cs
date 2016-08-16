using Phoenix.Core.Enums;

namespace Phoenix.Core.Models
{
    public interface ISchedulerSubscriptionParameter
    {
        int SubscriptionId { get; set; }
        int ParameterId { get; set; }
        string ParameterName { get; set; }
        SchedulerSubscriptionParameterType ParameterType { get; set; }
        string ParameterValue { get; set; }
        object GetTypeCastedValue();
    }
}