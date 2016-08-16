using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Phoenix.Core.Servers;
using Phoenix.Core.Models;

namespace Phoenix.Core.Repositories
{
    public class DatabaseEmployeeRepository : IEmployeeRepository
    {
        private readonly CoreServer _aServer;
        private ArrayList _params;
        private const string StrSQL = @"SELECT 
                      per.ID,
                      emp.Username,
                      per.FirstName,
                      per.Surname,
                      per.EmailAddress,
                      emp.Number,
                      per.DOB AS DateOfBirth,
                      ent.Division,
                      ent.CostCenter,
                      con.ContractType,
                      emp.JoinDate,
                      sup.ID AS SupervisorID,
                      sup.FirstName AS SupervisorFirstName,
                      sup.Surname AS SupervisorSurname,
                      sup.EmailAddress AS SupervisorEmailAddress 
                      FROM 
                      dbo.tblEmployees emp
                      INNER JOIN dbo.tblPerson per on emp.PersonID = per.ID 
                      LEFT OUTER JOIN dbo.tblEntities ent ON emp.EntityID = ent.ID 
                      LEFT OUTER JOIN dbo.tblContractType con ON emp.ContractTypeID = con.ID 
                      LEFT OUTER JOIN dbo.tblPerson sup ON emp.LineManagerID = sup.ID ";

        public DatabaseEmployeeRepository()
        {
            _aServer = new CoreServer();
            _params = new ArrayList();
        }

        public IEmployee GetOneByID(int id)
        {
            return BuildOneEmployee(id.ToString(CultureInfo.InvariantCulture), StrSQL + "WHERE per.ID = @Param0 ");
        }

        public IEmployee GetOneByUsername(string username)
        {
            return BuildOneEmployee(username, StrSQL + "WHERE emp.Username = @Param0 ");
        }

        public IEmployee GetOneByEmail(string email)
        {
            return BuildOneEmployee(email, StrSQL + "WHERE per.EmailAddress = @Param0 ");
        }

        private IEmployee BuildOneEmployee(string param, string sql)
        {
            _aServer.Open();
            _aServer.SQLParams.Add(param);

            var aReader = _aServer.ExecuteReader(sql);
            IEmployee aEmployee;

            using (aReader)
            {
                aEmployee = aReader.Read() ? BuildEmployeeFromReader(aReader) : BuildBlankEmployee();
            }
            
            _aServer.Close();

            return aEmployee;
        }

        public IEnumerable<IEmployee> Get()
        {
            //_aServer.Open();
            
            //var employees = new Dictionary<int, IEmployee>();

            //var aReader = _aServer.ExecuteReader(StrSQL + "WHERE a.LeaveDate IS NULL ");

            //using (aReader)
            //{
            //    while (aReader.Read())
            //        employees.Add((int)aReader["ID"], BuildEmployeeFromReader(aReader));
            //}

            //_aServer.Close();

            //return employees.Values.OrderBy(employee => employee.Second).ThenBy(employee => employee.First);  
            return GetEmployees("WHERE emp.LeaveDate IS NULL ");
        }

        public IEnumerable<IEmployee> Get(int entityID)
        {
            //_aServer.Open();
            //_aServer.SQLParams.Add(entityID);

            //var employees = new Dictionary<int, IEmployee>();

            //var aReader = _aServer.ExecuteReader(StrSQL + "WHERE a.EntityID = @Param0 AND a.LeaveDate IS NULL ");

            //using (aReader)
            //{
            //    while (aReader.Read())
            //        employees.Add((int)aReader["ID"], BuildEmployeeFromReader(aReader));
            //}

            //_aServer.Close();

            //return employees.Values.OrderBy(employee => employee.Second).ThenBy(employee => employee.First);
            _params.Add(entityID);
            return GetEmployees("WHERE emp.EntityID = @Param0 AND emp.LeaveDate IS NULL ");
        }

        public IEnumerable<IEmployee> GetInactive()
        {
            return GetEmployees("WHERE emp.LeaveDate IS NOT NULL ");
        }

        private IEnumerable<IEmployee> GetEmployees(string clause)
        {
            _aServer.Open();
            
            var employees = new Dictionary<int, IEmployee>();
            var aReader = _aServer.ExecuteReader(StrSQL + clause, _params);
            using (aReader)
            {
                while (aReader.Read())
                    employees.Add((int)aReader["ID"], BuildEmployeeFromReader(aReader));
            }

            _aServer.Close();

            return employees.Values.OrderBy(employee => employee.Second).ThenBy(employee => employee.First);
        }

        public IEmployee Add(IEmployee employee)
        {
            return employee;
        }

        public bool Delete(int id)
        {
            return true;
        }

        public bool Update(IEmployee employee)
        {
            return true;
        }

        private static IEmployee BuildEmployeeFromReader(IDataReader aReader)
        {
            IEmployee manager;

            if (Convert.IsDBNull(aReader["SupervisorID"]) )
                manager = BuildBlankEmployee();
            else
                manager = new Employee(
                    (int)aReader["SupervisorID"],
                    (string)aReader["SupervisorFirstName"],
                    (string)aReader["SupervisorSurname"],
                    (string)aReader["SupervisorEmailAddress"]
                    );

            return new Employee(
                (int)aReader["ID"],
                (string)aReader["Username"],
                (string)aReader["FirstName"],
                (string)aReader["Surname"],
                (string)aReader["EmailAddress"],
                (int)aReader["Number"],
                DateTime.Parse(aReader["DateOfBirth"].ToString()),
                (string)aReader["Division"],
                (string)aReader["CostCenter"],
                (string)aReader["ContractType"],
                DateTime.Parse(aReader["JoinDate"].ToString()),
                manager
                );
        }

        private static IEmployee BuildBlankEmployee()
        {
            return new Employee(
                 -1,
                 "Invalid",
                 "Invalid",
                 "Invalid"
             );
        }
    }
}