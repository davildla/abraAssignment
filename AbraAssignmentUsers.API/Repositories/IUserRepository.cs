using AbraAssignmentUsers.API.Models.Domain;

namespace AbraAssignmentUsers.API.Repositories
{
    public interface IUserRepository
    {

        Task<List<User>> GetAllAsync();
        Task<User> AddUserAsync(User user);
        Task<User> GetUserAsync(Guid id);
        Task<User> UpdateUserAsync(Guid id, User user);

        Task<string> GetUsersData(string gender);
        Task<string> MostPupalarCountry();
        Task<Models.DTO.OldestUser> GetTheOldestUser();
        Task<List<string>> GetListOfMalis();
    }
}
