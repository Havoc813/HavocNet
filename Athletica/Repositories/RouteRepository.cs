namespace Athletica.Repositories
{
    public class RouteRepository
    {
        private readonly AthleticaServer _aServer;

        public RouteRepository(AthleticaServer aServer)
        {
            _aServer = aServer;
        }

        public Route Get(int routeID)
        {
            _aServer.Open();
            
            const string strSQL = @"SELECT 
                Distance, 
                LocationID, 
                LocationName 
                FROM
                dbo.Route a 
                INNER JOIN dbo.Location b ON a.LocationID = b.LocationID 
                WHERE 
                a.RouteID  = @Param0";

            var aReader = _aServer.ExecuteReader(strSQL);

            var aRoute = new Route(
                double.Parse(aReader["Distance"].ToString()),
                int.Parse(aReader["LocationID"].ToString()),
                aReader["LocationName"].ToString()
                );
            
            _aServer.Close();

            return aRoute;
        }
    }
}
