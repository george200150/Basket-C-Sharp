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
        public override IDbConnection createConnection()
        {
            //MySql Connection
            String connectionString = "Database=mpp;" +
                                        "Data Source=localhost;" +
                                        "User id=test;" +
                                        "Password=passtest;";
            return new NpgsqlConnection(connectionString);


        }
    }
}
