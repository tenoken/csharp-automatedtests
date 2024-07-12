using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace TestAppAPIIntegrationTests.Database.SqlServer
{
    internal class DataContext
    {
        private string connectionString;

        public DataContext()
        {
            // TODO: Should take from config file
            this.connectionString = "";
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
    }
}
