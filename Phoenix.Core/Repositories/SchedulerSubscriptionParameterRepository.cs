using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Phoenix.Core.Models;
using Phoenix.Core.Servers;

namespace Phoenix.Core.Repositories
{
    public class SchedulerSubscriptionParameterRepository
    {
        private readonly CoreServer _server;
        private const string SubscriptionParamSql = @"Select ParamID,
                        ParamName,
                        ParamType,
                        ParamValue
                        FROM 
                        dbo.SchedulerSubsParams ";

        public SchedulerSubscriptionParameterRepository()
        {
            _server = new CoreServer();
        }

        public IEnumerable<ISchedulerSubscriptionParameter> Get(int subscriptionId)
        {
            _server.Open();
            _server.SQLParams.Add(subscriptionId);

            var subscriptionParams = new List<ISchedulerSubscriptionParameter>();

            var aReader = _server.ExecuteReader(SubscriptionParamSql + " WHERE SubscriptionID = @Param0");

            using (aReader)
            {
                while (aReader.Read())
                    subscriptionParams.Add(BuildSubscriptionParameterFromReader(subscriptionId, aReader));
            }

            _server.Close();

            return subscriptionParams.OrderBy(key => key.ParameterName);
        }

        public void Add(ISchedulerSubscriptionParameter subscriptionParam)
        {
        }

        public bool Delete(ISchedulerSubscriptionParameter subscriptionParam)
        {
            return false;
        }

        public void Update(ISchedulerSubscriptionParameter subscriptionParam)
        {
        }

        private static ISchedulerSubscriptionParameter BuildSubscriptionParameterFromReader(int subscriptionId, IDataReader aReader)
        {
            return new SchedulerSubscriptionParameter(
                subscriptionId,
                (int)aReader["ParamID"],
                (string)aReader["ParamName"],
                (string)aReader["ParamType"],
                (string)aReader["ParamValue"]
                );
        }

        public ArrayList GetParametersForSubscription(int subscriptionId)
        {
            var subscriptionParams = new ArrayList();
            foreach (var subParam in Get(subscriptionId))
            {
                subscriptionParams.Add(subParam.GetTypeCastedValue());
            }

            return subscriptionParams;
        }

        public string GetParametersStringForSubscription(int subscriptionId)
        {
            var subscriptionValues = new StringBuilder("");
            foreach (var subParam in Get(subscriptionId))
            {
                subscriptionValues.Append(string.Format("{0}:{1}|", subParam.ParameterName, subParam.ParameterValue));
            }

            return subscriptionValues.ToString();
        }
    }
}
