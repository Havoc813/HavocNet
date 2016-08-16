using System.Collections.Generic;
using Phoenix.Core.Models;

namespace Phoenix.Core.Repositories
{
    public interface IDatabaseDistributionListRepository
    {
        IDistributionList GetOneByID(int id);
        IDistributionList GetOneByName(string name);
        IEnumerable<IDistributionList> Get();
        IDistributionList Add(IDistributionList distributionList);
        bool Delete(int id);
        bool Update(IDistributionList distributionList);
    }
}