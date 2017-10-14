namespace Dapper.Repositories.UnitTests.Mocks
{
    using System;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    public class TestDatabase
    {
        private const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog={0};Integrated Security=True;Connect Timeout=30;Encrypt=False";

        public static string GetConnectionString(string name)
        {
            return string.Format(ConnectionString, name);
        }

        public static void Delete(string name)
        {
            try
            {
                var directory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Data");
                var database = string.Concat(name, ".mdf");
                var filepath = Path.Combine(directory, database);
                var logpath = Path.Combine(directory, $"{name}_log.ldf");

                DetachDatabase(name);

                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }

                if (File.Exists(logpath))
                {
                    File.Delete(logpath);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"TestDatabase.Delete({name}) -> {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        public static void Create(string name, bool overwrite = false)
        {
            try
            {
                var directory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Data");
                var database = string.Concat(name, ".mdf");
                var filepath = Path.Combine(directory, database);
                var logpath = Path.Combine(directory, $"{name}_log.ldf");

                Directory.CreateDirectory(directory);

                if (File.Exists(filepath) && overwrite)
                {
                    if (File.Exists(logpath))
                    {
                        File.Delete(logpath);
                    }

                    File.Delete(filepath);

                    CreateDatabase(name, filepath);
                }
                else if (!File.Exists(filepath))
                {
                    CreateDatabase(name, filepath);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"TestDatabase.Create({name}, {overwrite}) -> {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private static void CreateDatabase(string name, string filepath)
        {
            try
            {
                using (var connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        DetachDatabase(name);

                        command.CommandText = $"CREATE DATABASE {name} ON (NAME = N'{name}', FILENAME = N'{filepath}')";
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"TestDatabase.CreateDatabase({name}, {filepath}) -> {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        private static void DetachDatabase(string name)
        {
            try
            {
                using (var connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = $"IF EXISTS (SELECT name FROM sys.databases WHERE name = N'{name}') EXEC sp_detach_db '{name}'";
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"TestDatabase.DetachDatabase({name}) -> {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }
    }
}