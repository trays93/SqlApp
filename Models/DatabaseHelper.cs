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

            //using (sqlConnection = new SqlConnection(connectionString))
            //{
            //    using (SqlCommand command = new SqlCommand("EXEC sp_helpdb", sqlConnection))
            //    {
            //        using (SqlDataReader reader = command.ExecuteReader())
            //        {
            //            databases = new List<string>();
            //            while (reader.Read())
            //            {
            //                string databaseName = reader.GetString(0);
            //                databases.Add(databaseName);
            //            }
            //        }
            //    }
            //}
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

        public static List<string> GetColumnNames(User user, string sqlQuery, string database)
        {
            List<string> columnNames = new List<string>();
            string connectionString = $"Server={user.MachineName};Database={database};User Id={user.UserName};Password={user.Password};";

            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                SqlCommand command = new SqlCommand(sqlQuery, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                int fieldCount = reader.FieldCount;
                for (int i = 0; i < fieldCount; i++)
                {
                    columnNames.Add(reader.GetName(i));
                }
                reader.Close();

                sqlConnection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }

            return columnNames;
        }

        public static List<Dictionary<string, string>> Query(User user, string sqlQuery, string database)
        {
            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
            string connectionString = $"Server={user.MachineName};Database={database};User Id={user.UserName};Password={user.Password};";

            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                SqlCommand command = new SqlCommand(sqlQuery, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int fieldCount = reader.FieldCount;
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    for (int i = 0; i < fieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        string columnValue = reader.GetValue(i).ToString();
                        dict.Add(columnName, columnValue);
                    }
                    data.Add(dict);
                }
                reader.Close();

                sqlConnection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }

            return data;
        }

        //public static List<Database> MakeDatabaseTree(User user)
        //{
        //    List<Database> databases = new List<Database>();

        //    SqlCommand command = new SqlCommand("EXEC sp_helpdb", sqlConnection);
        //    SqlDataReader reader = command.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        string dbName = reader.GetString(0);
        //        List<Table> tables = GetTableNames(user, dbName);
        //        databases.Add(new Database() { Name = dbName, Tables = GetTableNames(user, dbName)});
        //    }
        //    reader.Close();

        //    return databases;
        //}

        public static List<string> GetTables(User user, string databaseName)
        {
            List<string> tables = new List<string>();

            string connectionString = $"Server={user.MachineName};Database={databaseName};User Id={user.UserName};Password={user.Password};";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand($"SELECT Name from Sysobjects where xtype = 'u'", connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string tableName = reader.GetString(0);
                tables.Add(tableName);
            }
            reader.Close();
            connection.Close();

            return tables;
        }
    }

    public class Database
    {
        public string Name { get; set; }
        public List<Table> Tables { get; set; }
    }

    public class Table
    {
        public string Name { get; set; }
        public List<string> ColumnNames { get; set; }
    }
}
