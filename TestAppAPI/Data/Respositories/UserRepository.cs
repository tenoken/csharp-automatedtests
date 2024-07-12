
using TestAppAPI.Data.Respositories.Interfaces;
using TestAppAPI.Models;

namespace TestAppAPI.Data.Respositories
{
    public class UserRepository : DataContext, IUserRepository
    {

        public Task AddUser(User user)
        {
            return Task.Run(() =>
            {
                Execute("INSERT INTO Users (Name) VALUES (@Name)", user);
            });
        }

        public Task DeleteUser(int id, User user)
        {
            return Task.Run(() =>
            {
                if (user.Id != id)
                    throw new ArgumentException("Id and user are different.");

                Execute("DELETE FROM Users WHERE Id = @Id", new { Id = id });
            });
        }

        public Task<List<User>> GetAllUsers()
        {
            return Task.Run(() =>
            {
                var users = Query<DataObjects.User>("SELECT * FROM Users");
                var list = new List<User>();
                foreach (var user in users)
                {
                    list.Add(new User() { Name = user.Name, Id = user.Id });
                }
                return list;
            });
        }

        public Task<User> GetById(int id)
        {
            return Task.Run(() =>
            {
                var user = Query<DataObjects.User>("SELECT TOP 1 * FROM Users WHERE Id = @Id", new { Id = id }).First();
                return new User() { Name = user.Name, Id = user.Id };
            });
        }

        public Task<User> GetByName(string name)
        {
            return Task.Run(() =>
            {
                var user = Query<DataObjects.User>("SELECT TOP 1 * FROM Users WHERE Name LIKE @Name + '%'", new { Name = name }).First();
                return new User() { Name = user.Name, Id = user.Id };
            });
        }

        public Task UpdateUser(int id, User user)
        {
            return Task.Run(() =>
            {
                if (user.Id != id)
                    throw new ArgumentException("Id and user are different.");

                var dbUser = Query<DataObjects.User>("SELECT TOP 1 * FROM Users WHERE Id = @Id", new { Id = id }).First();
                dbUser.Name = user.Name;
                Execute("UPDATE Users SET Name = @Name WHERE Id = @Id", user);
                return;
            });
        }
    }
}
