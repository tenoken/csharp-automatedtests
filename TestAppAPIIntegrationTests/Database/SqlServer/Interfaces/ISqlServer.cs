namespace TestAppAPIIntegrationTests.Database.SqlServer.Interfaces
{
    public interface ISqlServer
    {
        void Execute(string command, object parameter = null);
        IEnumerable<T> Query<T>(string query, object parameters = null);
    }
}
