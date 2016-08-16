using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Phoenix.Core.Servers;
using Phoenix.Core.Models;

namespace Phoenix.Core.Repositories
{
    public class DatabaseDistributionListRepository : IDatabaseDistributionListRepository
    {
        private readonly CoreServer _aServer;
        private ArrayList _params;
        private const string StrSQL = @"SELECT 
                      dl.DLID,
                      dl.DLName,
                      dl.DLDescription,
                      ISNULL(STUFF((SELECT ';' + ISNULL(p.EmailAddress,'')
                      FROM DistributionListMembers dlm  
                      INNER JOIN tblPerson p ON p.ID = dlm.PersonID
                      WHERE dlm.DLID = dl.DLID
                      FOR XML PATH('')),1,1,''),'')
                      + ';'
                      + ISNULL(STUFF((SELECT ';' + ISNULL(dlo.Email,'')
                      FROM DistributionListOther dlo
                      WHERE dlo.DLID = dl.DLID
                      FOR XML PATH('')),1,1,''),'') 
                      AS EmailAddress
                      FROM DistributionList dl ";

        public DatabaseDistributionListRepository()
        {
            _aServer = new CoreServer();
            _params = new ArrayList();
        }

        public IDistributionList GetOneByID(int id)
        {
            return BuildOneDistributionList(id.ToString(CultureInfo.InvariantCulture), StrSQL + "WHERE dl.DLID = @Param0 ");
        }

        public IDistributionList GetOneByName(string name)
        {
            return BuildOneDistributionList(name, StrSQL + "WHERE dl.DLName = @Param0 ");
        }

        private IDistributionList BuildOneDistributionList(string param, string sql)
        {
            _aServer.Open();
            _aServer.SQLParams.Add(param);

            var aReader = _aServer.ExecuteReader(sql);
            IDistributionList aDistributionList;

            using (aReader)
            {
                aDistributionList = aReader.Read() ? BuildDistributionListFromReader(aReader) : BuildBlankDistributionList();
            }

            _aServer.Close();

            return aDistributionList;
        }

        public IEnumerable<IDistributionList> Get()
        {
            _aServer.Open();

            var distributionLists = new Dictionary<int, IDistributionList>();
            var aReader = _aServer.ExecuteReader(StrSQL, _params);
            using (aReader)
            {
                while (aReader.Read())
                    distributionLists.Add((int)aReader["DLID"], BuildDistributionListFromReader(aReader));
            }

            _aServer.Close();

            return distributionLists.Values.OrderBy(distlist => distlist.Name);
        }


        public IDistributionList Add(IDistributionList distributionList)
        {
            return distributionList;
        }

        public bool Delete(int id)
        {
            return true;
        }

        public bool Update(IDistributionList distributionList)
        {
            return true;
        }

        private static IDistributionList BuildDistributionListFromReader(IDataReader aReader)
        {
            return new DistributionList((int) aReader["DLID"], (string) aReader["DLName"],
                (string) aReader["DLDescription"], (string) aReader["EmailAddress"], ';');
        }

        private static IDistributionList BuildBlankDistributionList()
        {
            return new DistributionList(
                 -1,
                 "Invalid",
                 "Invalid"
             );
        }
    }
}