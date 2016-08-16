using System.Collections.Generic;
using System.Data;
using System.Linq;
using Phoenix.Core.Models;
using Phoenix.Core.Servers;

namespace Phoenix.Core.Repositories
{
    public class EmployeeCostCentreRepository : IEmployeeCostCentreRepository
    {
        private readonly CoreServer _aServer;
        private const string StrSQL = @"SELECT 
                      ID,
                      Division,
                      CostCenter
                      FROM 
                      dbo.tblEntities ";

        public EmployeeCostCentreRepository()
        {
            _aServer = new CoreServer();
        }

        public IEnumerable<IEmployeeCostCentre> Get()
        {
            _aServer.Open();

            var employeeCostCentres = new Dictionary<int, IEmployeeCostCentre>();

            var aReader = _aServer.ExecuteReader(StrSQL);

            using (aReader)
            {
                while (aReader.Read())
                    employeeCostCentres.Add((int)aReader["ID"], BuildEmployeeCostCentreFromReader(aReader));
            }

            _aServer.Close();

            return employeeCostCentres.Values.OrderBy(employeeCostCentre => employeeCostCentre.Division).ThenBy(employeeCostCentre => employeeCostCentre.CostCentre);
        }

        public IEnumerable<IEmployeeCostCentre> Get(string division)
        {
            _aServer.Open();
            _aServer.SQLParams.Add(division);

            var employeeCostCentres = new Dictionary<int, IEmployeeCostCentre>();

            var aReader = _aServer.ExecuteReader(StrSQL + "WHERE Division = @Param0 ");

            using (aReader)
            {
                while (aReader.Read())
                    employeeCostCentres.Add((int)aReader["ID"], BuildEmployeeCostCentreFromReader(aReader));
            }

            _aServer.Close();

            return employeeCostCentres.Values.OrderBy(employeeCostCentre => employeeCostCentre.CostCentre);
        }

        public IEmployeeCostCentre Add(IEmployeeCostCentre employeeCostCentre)
        {
            return employeeCostCentre;
        }

        public bool Delete(int id)
        {
            return true;
        }

        public bool Update(IEmployeeCostCentre employeeCostCentre)
        {
            return true;
        }

        private static IEmployeeCostCentre BuildEmployeeCostCentreFromReader(IDataReader aReader)
        {
            return new EmployeeCostCentre(
                (int)aReader["ID"],
                (string)aReader["Division"],
                (string)aReader["CostCenter"]
                );
        }
    }
}
