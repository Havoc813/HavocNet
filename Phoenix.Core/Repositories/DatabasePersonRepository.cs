using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Phoenix.Core.Models;
using Phoenix.Core.Servers;

namespace Phoenix.Core.Repositories
{
    class DatabasePersonRepository
    {
        private readonly CoreServer _aServer;
        private const string StrSQL = @"SELECT 
                      per.ID,
                      per.FirstName,
                      per.Surname,
                      per.EmailAddress,
                      per.DOB AS DateOfBirth
                      FROM dbo.tblPerson per
                      ";

        public DatabasePersonRepository()
        {
            _aServer = new CoreServer();
        }

        public IPerson GetOneByID(int id)
        {
            return BuildOnePerson(id.ToString(CultureInfo.InvariantCulture), StrSQL + "WHERE per.ID = @Param0 ");
        }

        private IPerson BuildOnePerson(string param, string sql)
        {
            _aServer.Open();
            _aServer.SQLParams.Add(param);

            var aReader = _aServer.ExecuteReader(sql);
            IPerson aPerson;

            using (aReader)
            {
                aPerson = aReader.Read() ? BuildPersonFromReader(aReader) : BuildBlankPerson();
            }
            
            _aServer.Close();

            return aPerson;
        }

        private IEnumerable<IPerson> Get()
        {
            _aServer.Open();

            var persons = new Dictionary<int, IPerson>();
            var aReader = _aServer.ExecuteReader(StrSQL);
            using (aReader)
            {
                while (aReader.Read())
                    persons.Add((int)aReader["ID"], BuildPersonFromReader(aReader));
            }

            _aServer.Close();

            return persons.Values.OrderBy(person => person.Second).ThenBy(person => person.First);
        }

        public IPerson Add(IPerson person)
        {
            return person;
        }

        public bool Delete(int id)
        {
            return true;
        }

        public bool Update(IPerson person)
        {
            return true;
        }

        private static IPerson BuildPersonFromReader(IDataReader aReader)
        {
            return new Person(
                (int)aReader["ID"],
                (string)aReader["FirstName"],
                (string)aReader["Surname"],
                (string)aReader["EmailAddress"],
                DateTime.Parse(aReader["DateOfBirth"].ToString())
                );
        }

        private static IPerson BuildBlankPerson()
        {
            return new Person(
                 -1,
                 "Invalid",
                 "Invalid",
                 "Invalid"
             );
        }
    }
}
