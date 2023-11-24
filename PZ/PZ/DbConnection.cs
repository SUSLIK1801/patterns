using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ
{
    public class DbConnection
    {
        private MySqlConnection connection;
        private static DbConnection instance;

        private DbConnection()
        {
            string connectionString = "server=localhost;port=3306;username=root;password=7788354Dima;database=confectionery_store;";
            connection = new MySqlConnection(connectionString);
        }

        public static DbConnection GetInstance()
        {
            if (instance == null)
            {
                instance = new DbConnection();
            }
            return instance;
        }

        public MySqlConnection GetConnection()
        {
            return connection;
        }
    }
}
