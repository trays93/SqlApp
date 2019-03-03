using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SQLApp.Models
{
    public class DatabaseHelper
    {
        private static SqlConnection sqlConnection;

        public static List<string> GetDatabaseNames(string machineName, string userName, string password)
        {
            string connectionString = $"Server={machineName};Database=master;User Id={userName};Password={password};";
            List<string> databases = new List<string>();

            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                SqlCommand command = new SqlCommand("EXEC sp_helpdb", sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                databases = new List<string>();
                while (reader.Read())
                {
                    string databaseName = reader.GetString(0);
                    databases.Add(databaseName);
                }
                reader.Close();

                sqlConnection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            
            return databases;
        }
    }
}
