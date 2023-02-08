using AbraAssignmentUsers.API.Data;
using AbraAssignmentUsers.API.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AbraAssignmentUsers.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AbraUsersDbContext abraUsersDbContext;
        private readonly string randomUserUrl;
        private readonly HttpClient _client;
        public UserRepository(AbraUsersDbContext abraUsersDbContext, HttpClient client)
        {
            this.abraUsersDbContext = abraUsersDbContext;
            this.randomUserUrl = "https://randomuser.me/api";
            this._client = client;
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

        public async Task<List<string>> GetListOfMalis()
        {
            List<string> emails = new List<string>();

                HttpResponseMessage response = await _client.GetAsync($"{randomUserUrl}/?results=30");
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                string responseBody = await response.Content.ReadAsStringAsync();

                dynamic users = JsonConvert.DeserializeObject(responseBody);

                foreach (var user in users.results)
                {
                    emails.Add((string)user.email);
                }
          

            return emails;
        }

        public async Task<Models.DTO.OldestUser> GetTheOldestUser()
        {
            Models.DTO.OldestUser oldestUser = new Models.DTO.OldestUser();
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"{randomUserUrl}/?results=100");
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                string responseBody = await response.Content.ReadAsStringAsync();

                dynamic users = JsonConvert.DeserializeObject(responseBody);
                int oldestAge = 0;
                string oldestName = string.Empty;
                foreach (var user in users.results)
                {
                    int age = user.dob.age;
                    if (age > oldestAge)
                    {
                        oldestAge = age;
                        string firstName = user.name.first;
                        string lastName = user.name.last;
                        oldestName = firstName + " " + lastName;
                    }
                }

                oldestUser.Name = oldestName;
                oldestUser.Age = oldestAge;
            }
            catch (Exception)
            {
                return null;
            }

            return oldestUser;
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            return abraUsersDbContext.Users.FirstOrDefault(x => x.Id == id);
        }

        public async Task<string> GetUsersData(string gender)
        {
            HttpResponseMessage response;
            try
            {
                response = await _client.GetAsync($@"{randomUserUrl}/?results=10&gender={gender}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException)
            {
                return null;
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        public async Task<string> MostPupalarCountry()
        {
            int numUsers = 5000;
            HttpResponseMessage response = await _client.GetAsync($"{randomUserUrl}/?results={numUsers}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            dynamic users = JsonConvert.DeserializeObject(responseBody);
            Dictionary<string, int> countryCounts = new Dictionary<string, int>();
            foreach (var user in users.results)
            {
                string country = user.location.country;
                if (countryCounts.ContainsKey(country))
                {
                    countryCounts[country]++;
                }
                else
                {
                    countryCounts[country] = 1;
                }
            }

            string mostPopularCountry = countryCounts.OrderByDescending(kvp => kvp.Value).First().Key;
            return mostPopularCountry;  
        }

        public async Task<User> UpdateUserAsync(Guid id, User user)
        {
            var existingUser = await abraUsersDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            
            if (existingUser == null) { return null; }

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
