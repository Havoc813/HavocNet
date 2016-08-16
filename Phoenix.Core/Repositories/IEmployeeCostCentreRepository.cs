using System.Collections.Generic;
using Phoenix.Core.Models;

namespace Phoenix.Core.Repositories
{
    public interface IEmployeeCostCentreRepository
    {
        IEnumerable<IEmployeeCostCentre> Get();
        IEnumerable<IEmployeeCostCentre> Get(string division);
        IEmployeeCostCentre Add(IEmployeeCostCentre employeeCostCentre);
        bool Delete(int id);
        bool Update(IEmployeeCostCentre employeeCostCentre);
    }
}