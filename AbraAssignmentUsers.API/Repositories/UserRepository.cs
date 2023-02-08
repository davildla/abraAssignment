using AbraAssignmentUsers.API.Data;
using AbraAssignmentUsers.API.Models.Domain;

namespace AbraAssignmentUsers.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AbraUsersDbContext abraUsersDbContext;

        public UserRepository(AbraUsersDbContext abraUsersDbContext)
        {
            this.abraUsersDbContext = abraUsersDbContext;
        }
        public async Task<User> AddUserAsync(User user)
        {
            user.Id = Guid.NewGuid();
            await abraUsersDbContext.AddAsync(user);
            await abraUsersDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await abraUsersDbContext.Users.ToListAsync();
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            return abraUsersDbContext.Users.FirstOrDefault(x => x.Id == id);
        }

        public async Task<User> UpdateUserAsync(Guid id, User user)
        {
            var existingUser = await abraUsersDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (existingUser != null) { return null; }

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Gender = user.Gender;
            existingUser.Phone = user.Phone;
            existingUser.Country = user.Country;

            await abraUsersDbContext.SaveChangesAsync();

            return existingUser;    
        }
    }
}
