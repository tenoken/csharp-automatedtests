using TestAppAPIIntegrationTests.Database.SqlServer.Interfaces;

namespace TestAppAPIIntegrationTests.Database.SqlServer
{
    internal class SqlServer : DataContext, ISqlServer
    {
        public void Execute(string command, object parameters)
        {
            base.Execute(command, parameters);
        }

        IEnumerable<T> ISqlServer.Query<T>(string query, object parameters)
        {
            return base.Query<T>(query, parameters);
        }
    }
}
