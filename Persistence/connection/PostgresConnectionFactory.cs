using Npgsql;
using System.Data;


namespace Persistence.connection
{
    public class PostgresConnectionFactory : ConnectionFactory
    {
        public override IDbConnection CreateConnection()
        {
            string connectionStr = "Server=127.0.0.1;Port=5432;Database=MPP;User Id=postgres;Password=admin";

            return new NpgsqlConnection(connectionStr);

        }
    }
}
