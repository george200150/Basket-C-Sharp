using Persistence.connection;

using System.Data;


namespace Persistence.repos
{
    public static class ADOInvariant
    {
        private static IDbConnection instance = null;

        public static IDbConnection GetConnection()
        {
            if (instance == null || instance.State == System.Data.ConnectionState.Closed)
            {
                instance = GetNewConnection();
                instance.Open();
            }
            return instance;
        }

        private static IDbConnection GetNewConnection()
        {

            return ConnectionFactory.GetInstance().CreateConnection();


        }
    }
}
