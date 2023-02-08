using AbraAssignmentUsers.API.Models.Domain;
using System.Collections.Generic;

namespace AbraAssignmentUsers.API.Data
{
    public class AbraUsersDbContext : DbContext
    {
        public AbraUsersDbContext(DbContextOptions<AbraUsersDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }


    }
}
