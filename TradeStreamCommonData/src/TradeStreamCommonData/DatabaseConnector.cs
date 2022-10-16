using Microsoft.Data.SqlClient;
using System.Configuration;

namespace TradeStreamCommonData.DatabaseConnector
{
    public sealed class DatabaseConnector
    {
        private static SqlConnection connection;
        static DatabaseConnector instance;

        private DatabaseConnector() { }

        public static DatabaseConnector GetInstance()
        {
            if (instance == null)
            {
                instance = new DatabaseConnector();
                instance.Connect();
            }
            return instance;
        }

        public SqlConnection GetConnection()
        {
            return connection;
        }

        public void Disconnect()
        {
            if (instance == null)
                return;

            connection.Close();
        }

        private bool Connect()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.DataSource = ConfigurationManager.AppSettings["DataSource"];
                builder.UserID = ConfigurationManager.AppSettings["User"];
                builder.Password = ConfigurationManager.AppSettings["Password"];
                builder.InitialCatalog = ConfigurationManager.AppSettings["Catalog"];
                builder.TrustServerCertificate = true;
                builder.Encrypt = false;
                builder.IntegratedSecurity = true;
                builder.MultipleActiveResultSets = true;

                connection = new SqlConnection(builder.ConnectionString);
                connection.Open();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return true;
        }
    }
}