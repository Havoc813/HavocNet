using System.Collections.Generic;
using Phoenix.Core.Models;

namespace Phoenix.Core.Repositories
{
    public interface IEmployeeRepository
    {
        IEmployee GetOneByID(int id);
        IEmployee GetOneByUsername(string username);
        IEmployee GetOneByEmail(string email);
        IEnumerable<IEmployee> Get();
        IEnumerable<IEmployee> Get(int entityID);
        IEnumerable<IEmployee> GetInactive();
        IEmployee Add(IEmployee employee);
        bool Delete(int id);
        bool Update(IEmployee employee);
    }
}