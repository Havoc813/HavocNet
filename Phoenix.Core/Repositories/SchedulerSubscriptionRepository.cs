using System;
using System.Collections.Generic;
using System.Data;
using Phoenix.Core.Models;
using Phoenix.Core.Servers;

namespace Phoenix.Core.Repositories
{
    public class SchedulerSubscriptionRepository
    {
        private readonly CoreServer _server;
        private const string SubscriptionSql = @"Select ss.SubscriptionID, 
                        ss.ReportID, 
                        ss.PersonID, 
                        p.EmailAddress, 
                        ss.StartTime, 
                        ss.Frequency 
                        FROM dbo.SchedulerSubscriptions ss 
                        INNER JOIN dbo.tblPerson p ON p.ID = ss.PersonID ";

        public SchedulerSubscriptionRepository()
        {
            _server = new CoreServer();
        }

        public ISchedulerSubscription GetOne(int id)
        {
            _server.Open();
            _server.SQLParams.Add(id);

            var aReader = _server.ExecuteReader(SubscriptionSql + " WHERE SubscriptionID = @Param0");
            ISchedulerSubscription subscription;

            using (aReader)
            {
                aReader.Read();

                subscription = BuildSubscriptionFromReader(aReader);
            }

            _server.Close();

            return subscription;
        }

        public List<ISchedulerSubscription> Get(int reportId)
        {
            _server.Open();
            _server.SQLParams.Add(reportId);

            var subscriptions = new List<ISchedulerSubscription>();

            var aReader = _server.ExecuteReader(SubscriptionSql + " WHERE ReportID = @Param0");

            using (aReader)
            {
                while (aReader.Read())
                    subscriptions.Add(BuildSubscriptionFromReader(aReader));
            }

            _server.Close();

            return subscriptions;
        }

        public void UpdateStartTime(ISchedulerSubscription subscription)
        {
            _server.Open();
            const string sql = "Update SchedulerSubscriptions Set StartTime=@Param1 Where SubscriptionID=@Param0";
            _server.SQLParams.Add(subscription.SubscriptionId);
            _server.SQLParams.Add(subscription.StartTime);
            _server.ExecuteNonQuery(sql);
            _server.Close();
        }

        private static ISchedulerSubscription BuildSubscriptionFromReader(IDataReader aReader)
        {
            return new SchedulerSubscription(
                (int)aReader["SubscriptionID"],
                (int)aReader["PersonID"],
                (string)aReader["EmailAddress"],
                (DateTime)aReader["StartTime"],
                (string)aReader["Frequency"]
                );
        }

        public ISchedulerSubscription Add(string reportID, string personID, string startTime, string frequency)
        {
            _server.Open();

            const string strSQL = @"INSERT INTO dbo.SchedulerSubscriptions(ReportID, PersonID, StartTime, Frequency)
                SELECT
                @Param0 AS ReportID, 
                @Param1 AS PersonID,
                @Param2 AS StartTime, 
                @Param3 AS Frequency

                SELECT MAX(SubscriptionID) FROM dbo.SchedulerSubscriptions";

            _server.SQLParams.Add(reportID);
            _server.SQLParams.Add(personID);
            _server.SQLParams.Add(DateTime.Parse(startTime));
            _server.SQLParams.Add(frequency);

            var subscriptionID = int.Parse(_server.ExecuteScalar(strSQL).ToString());

            _server.Close();

            return this.GetOne(subscriptionID);
        }

        public ISchedulerSubscription Delete(int subscriptionID)
        {
            var subscription = GetOne(subscriptionID);

            const string strSQL = @"DELETE FROM dbo.SchedulerSubscriptions WHERE SubscriptionID = @Param0";

            _server.Open();

            _server.SQLParams.Add(subscriptionID);

            _server.ExecuteNonQuery(strSQL);

            _server.Close();

            return subscription;
        }

        public ISchedulerSubscription Update(int subscriptionID, string reportID, string personID, string startTime, string frequency)
        {
            _server.Open();

            _server.SQLParams.Add(subscriptionID);

            const string strSQL = @"UPDATE dbo.SchedulerSubscriptions 
                SET 
                ReportID = @Param1, 
                PersonID = @Param2, 
                StartTime = @Param3, 
                Frequency = @Param4 
                WHERE 
                SubscriptionID = @Param0";

            _server.SQLParams.Add(reportID);
            _server.SQLParams.Add(personID);
            _server.SQLParams.Add(startTime);
            _server.SQLParams.Add(frequency);

            _server.ExecuteNonQuery(strSQL);

            _server.Close();

            return this.GetOne(subscriptionID);
        }
    }
}
