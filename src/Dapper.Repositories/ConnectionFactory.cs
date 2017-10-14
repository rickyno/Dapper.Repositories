namespace Dapper.Repositories
{
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Data.SqlClient;
    using Dapper.Repositories.Properties;

    public class ConnectionFactory : IConnectionFactory
    {
        private readonly StringDictionary connectionStrings;
        private string defaultConnectionName;

        public ConnectionFactory()
        {
            this.connectionStrings = new StringDictionary();
        }

        public IDbConnection Create(string name = null)
        {
            if (!string.IsNullOrWhiteSpace(name) && !this.connectionStrings.ContainsKey(name))
            {
                throw new ArgumentException(nameof(name), string.Format(Resources.ConnectionStringNameNotFound, name));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                if (string.IsNullOrWhiteSpace(this.defaultConnectionName))
                {
                    throw new ArgumentNullException(nameof(name), Resources.ConnectionStringNameRequired);
                }

                name = this.defaultConnectionName;
            }

            return new SqlConnection(this.connectionStrings[name]);
        }

        public void Register(string name, string connectionString, bool defaultConnection = false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            this.connectionStrings.Add(name, connectionString);

            if (defaultConnection)
            {
                this.defaultConnectionName = name;
            }
        }
    }
}