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

        public static List<string> GetColumns(User user, string databaseName, string tableName)
        {
            List<string> columns = new List<string>();

            string connectionString = $"Server={user.MachineName};Database={databaseName};User Id={user.UserName};Password={user.Password};";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'", connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string columnName = reader.GetString(0);
                columns.Add(columnName);
            }
            reader.Close();
            connection.Close();

            return columns;
        }

        public static List<string> GetColumnNames(User user, string sqlQuery, string database)
        {
            List<string> columnNames = new List<string>();
            string connectionString = $"Server={user.MachineName};Database={database};User Id={user.UserName};Password={user.Password}";

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
    }
}
