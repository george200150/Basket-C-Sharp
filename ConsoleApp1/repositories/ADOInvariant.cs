using Basket.connectionUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.repositories
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
