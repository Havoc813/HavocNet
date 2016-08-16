using System.Collections.Generic;
using System.Data;
using System.Linq;
using Phoenix.Core.Models;
using Phoenix.Core.Servers;

namespace Phoenix.Core.Repositories
{
    public class SchedulerReportRepository
    {
        private readonly CoreServer _server;
        private const string ReportSql = @"Select sr.ReportID,
                        sr.Enabled,
                        sr.ReportName,
                        sr.ReportDescription,
                        sr.ReportPath,
                        sr.ReportFileName,
                        sy.SystemTag,
                        sy.SystemName,
                        ISNULL(sy.AppDataPath,'') AS AppDataPath
                        FROM 
                        dbo.SchedulerReports sr 
                        INNER JOIN dbo.Systems sy on sy.SystemTag = sr.SystemTag ";

        public SchedulerReportRepository()
        {
            _server = new CoreServer();
        }

        public IEnumerable<ISchedulerReport> Get()
        {
            return GetAll("");
        }

        public IEnumerable<ISchedulerReport> GetEnabled()
        {
            return GetAll(" WHERE sr.Enabled = 1");
        }

        private IEnumerable<ISchedulerReport> GetAll(string clause)
        {
            _server.Open();

            var reports = new Dictionary<int, ISchedulerReport>();

            var aReader = _server.ExecuteReader(ReportSql + clause);

            using (aReader)
            {
                while (aReader.Read())
                    reports.Add((int)aReader["ReportID"], BuildReportFromReader(aReader));
            }

            _server.Close();

            return reports.Values.OrderBy(report => report.ReportName);
        }

        public ISchedulerReport GetOne(int reportID)
        {
            _server.Open();
            _server.SQLParams.Add(reportID);

            var aReader = _server.ExecuteReader(ReportSql + " WHERE ReportID = @Param0");
            ISchedulerReport report;

            using (aReader)
            {
                aReader.Read();

                report = BuildReportFromReader(aReader);
            }

            _server.Close();

            return report;
        }

        public ISchedulerReport Add(
            bool enabled,
            string reportName,
            string reportDescription,
            string reportPath,
            string systemTag,
            string reportFilename
            )
        {
            _server.Open();

            const string strSQL = @"INSERT INTO dbo.SchedulerReports(ReportName, ReportDescription, ReportPath, SystemTag, ReportFilename, Enabled)
                        SELECT 
                        @Param0 AS ReportName, 
                        @Param1 AS ReportDescription,
                        @Param2 AS ReportPath,
                        @Param3 AS SystemTag,
                        @Param4 AS ReportFilename, 
                        @Param5 AS Enabled

                        SELECT MAX(ReportID) FROM dbo.SchedulerReports";

            _server.SQLParams.Add(reportName);
            _server.SQLParams.Add(reportDescription);
            _server.SQLParams.Add(reportPath);
            _server.SQLParams.Add(systemTag);
            _server.SQLParams.Add(reportFilename);
            _server.SQLParams.Add(enabled);

            var reportID = int.Parse(_server.ExecuteScalar(strSQL).ToString());

            _server.Close();

            return this.GetOne(reportID);
        }

        public void Delete(int reportID)
        {
            _server.Open();

            _server.SQLParams.Add(reportID);

            _server.ExecuteNonQuery("DELETE FROM dbo.SchedulerReports WHERE ReportID = @Param0");

            _server.Close();
        }

        public ISchedulerReport Update(
            int reportID,
            int enabled,
            string reportName,
            string reportDescription,
            string reportPath,
            string systemTag,
            string reportFilename
            )
        {
            _server.Open();

            const string strSQL = @"UPDATE dbo.SchedulerReports 
                SET 
                Enabled = @Param5, 
                ReportName = @Param0, 
                ReportDescription = @Param1, 
                ReportPath = @Param2, 
                SystemTag = @Param3, 
                ReportFilename = @Param4
                WHERE 
                ReportID = @Param6";

            _server.SQLParams.Add(reportName);
            _server.SQLParams.Add(reportDescription);
            _server.SQLParams.Add(reportPath);
            _server.SQLParams.Add(systemTag);
            _server.SQLParams.Add(reportFilename);
            _server.SQLParams.Add(enabled);
            _server.SQLParams.Add(reportID);

            _server.ExecuteNonQuery(strSQL);

            _server.Close();

            return this.GetOne(reportID);
        }

        private static ISchedulerReport BuildReportFromReader(IDataReader aReader)
        {
            return new SchedulerReport(
                (int)aReader["ReportID"],
                (bool)aReader["Enabled"],
                (string)aReader["ReportName"],
                (string)aReader["ReportDescription"],
                (string)aReader["ReportPath"],
                (string)aReader["ReportFilename"],
                (string)aReader["SystemName"],
                (string)aReader["AppDataPath"],
                new SchedulerSubscriptionRepository().Get((int)aReader["ReportID"])
                );
        }
    }
}
