using Microsoft.EntityFrameworkCore;
using Project.Data.DTO;
using Project.Data.Models;

namespace Project.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<LocalUser> LocalUsers { get; set; }
        public DbSet<UserProfileDTO> UserProfiles { get; set; }

    }

}
