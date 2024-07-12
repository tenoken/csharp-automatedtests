using TestAppAPI.Models;

namespace TestAppAPI.Data.Respositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetById(int id);
        Task<User> GetByName(string name);
        Task AddUser(User user);
        Task UpdateUser(int id, User user);
        Task DeleteUser(int id, User user);
        Task<List<User>> GetAllUsers();
    }
}
