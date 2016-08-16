using System.Collections.Generic;

namespace Phoenix.Core.Models
{
    public interface IDistributionList
    {
        int ID { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        List<string> MemberEmailAddresses { get; set; }
    }
}