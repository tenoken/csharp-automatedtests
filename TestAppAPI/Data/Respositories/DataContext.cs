using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace TestAppAPI.Data.Respositories
{
    public abstract class DataContext : IDisposable
    {
        private string connectionString;

        public DataContext()
        {
            string c = Directory.GetCurrentDirectory();
            IConfigurationRoot configuration =
            new ConfigurationBuilder().SetBasePath(c).AddJsonFile("appsettings.Development.json").Build();
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<T> Query<T>(string sql, object parameters = null)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                return dbConnection.Query<T>(sql, parameters);
            }
        }

        public int Execute(string sql, object parameters = null)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                return dbConnection.Execute(sql, parameters);
            }
        }

        public T QuerySingle<T>(string sql, object parameters = null)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                return dbConnection.QuerySingle<T>(sql, parameters);
            }
        }

        public void ExecuteTransaction(Action<IDbConnection, IDbTransaction> action)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        action(dbConnection, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public void Dispose()
        {

        }
    }
}

