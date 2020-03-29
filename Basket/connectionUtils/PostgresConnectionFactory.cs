using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.connectionUtils
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
