using dotNetRPG.Models;
using Microsoft.EntityFrameworkCore;

namespace dotNetRPG.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Skill> Skills { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData(
                    new Skill { Id = 1, Damage = 30, Name = "Fireball"},
                    new Skill { Id = 2, Damage = 20, Name = "Frenzy" },
                    new Skill { Id = 3, Damage = 50, Name = "Blizzard" }
                );
        }
    }
}
