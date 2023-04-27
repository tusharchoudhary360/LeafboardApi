using AuthApi.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Models.Domain
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<Token> Token { get; set; }
        public DbSet<UserImages> UserImages { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Otp> Otp { get; set; }
        public DbSet<AllUsers> AllUsers { get; set; }   
    }
}
