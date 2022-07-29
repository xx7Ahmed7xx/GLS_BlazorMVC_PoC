using GLS_BlazorMVC_PoC.Models;
using Microsoft.EntityFrameworkCore;

namespace GLS_BlazorMVC_PoC.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "Test123",
                    HashedPassword = Helpers.Hasher.HashPassword("123gg%^"),
                    Email = "test@test.com"
                });
        }

        public DbSet<Password> Passwords { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
