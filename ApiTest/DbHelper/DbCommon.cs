using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using System.Configuration;

namespace ApiTest.DbHelper
{
    class DbCommon
    {
        private static DbProviderFactory _factory;
        private static string _connectionString;

        public DbCommon()
        {
            switch (ConfigurationManager.AppSettings.Get("DbType"))
            {
                case "MSSQL":
                    _factory = SqlClientFactory.Instance;
                    _connectionString = GetMssqlConnectionString();
                    break;
                case "PostgreSQL":
                    _factory = NpgsqlFactory.Instance;
                    _connectionString = GetPostgreSqlConnectionString();
                    break;
                default:
                    Assert.Fail("Неизвестный провайдер БД " + ConfigurationManager.AppSettings.Get("DbType"));
                    break;
            }
        }

        /// <summary>
        /// Создает открытое соединение
        /// </summary>
        public IDbConnection CreateConnection()
        {
            IDbConnection cn = _factory.CreateConnection();
            cn.ConnectionString = _connectionString;
            cn.Open();
            return cn;
        }

        /// <summary>
        /// Устанавливает значение параметра в БД.
        /// </summary>
        /// <param name="paramName">Имя параметра.</param>
        /// <param name="paramValue">Значение параметра, которое необходимо установить.</param>
        public void SetParameter(string paramName, string paramValue)
        {
            using (IDbConnection sqlConnect = CreateConnection())
            using (IDbCommand sqlCommand = sqlConnect.CreateCommand())
            {
                sqlCommand.CommandText = String.Format(@"UPDATE dbo.ldoption SET ""Value""='{0}' WHERE ""Name""='{1}'",
                    paramValue, paramName);
                sqlCommand.ExecuteNonQuery();
            }
        }

        private static string GetMssqlConnectionString()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = ConfigurationManager.AppSettings.Get("DbServerMS"),
                InitialCatalog = ConfigurationManager.AppSettings.Get("DbNameMS"),
                UserID = ConfigurationManager.AppSettings.Get("DbLoginMS"),
                Password = ConfigurationManager.AppSettings.Get("DbPasswordMS"),
                Pooling = true
            };

            return builder.ConnectionString;
        }

        private static string GetPostgreSqlConnectionString()
        {
            var pgBuilder = new NpgsqlConnectionStringBuilder
            {
                Host = ConfigurationManager.AppSettings.Get("DbServerPG"),
                Database = ConfigurationManager.AppSettings.Get("DbNamePG"),
                Username = ConfigurationManager.AppSettings.Get("DbLoginPG"),
                Password = ConfigurationManager.AppSettings.Get("DbPasswordPG"),
                Pooling = false
            };
            return pgBuilder.ConnectionString;
        }
    }
}
