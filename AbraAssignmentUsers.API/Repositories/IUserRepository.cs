using AbraAssignmentUsers.API.Models.Domain;

namespace AbraAssignmentUsers.API.Repositories
{
    public interface IUserRepository
    {

        Task<List<User>> GetAllAsync();
        Task<User> AddUserAsync(User user);
        Task<User> GetUserAsync(Guid id);
        Task<User> UpdateUserAsync(Guid id, User user);
    }
}
