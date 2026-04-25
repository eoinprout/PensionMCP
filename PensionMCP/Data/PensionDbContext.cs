using Microsoft.EntityFrameworkCore;
using PensionMCP.Models;

namespace PensionMCP.Data
{
    public class PensionDbContext : DbContext
    {
        public PensionDbContext(DbContextOptions<PensionDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients => Set<Client>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Seed data to help with demo and testing.
            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    Id = 1,
                    Name = "James Kirk",
                    DateOfBirth = new DateOnly(1974, 12, 12),
                    NetRelevantIncome = 44000m,
                    CurrentPensionPotValue = 100000m,
                    CurrentMonthlyPensionContribution = 1000m,
                    CurrentMonthlyEmployersContribution = 1000m
                },
                new Client
                {
                    Id = 2,
                    Name = "Nyota Uhura",
                    DateOfBirth = new DateOnly(1982, 7, 25),
                    NetRelevantIncome = 45000m,
                    CurrentPensionPotValue = -1m
                },
                new Client
                {
                    Id = 3,
                    Name = "Leonard McCoy",
                    DateOfBirth = new DateOnly(1990, 11, 1),
                    NetRelevantIncome = 120000m,
                    CurrentPensionPotValue = -1m
                }
            );
        }
    }
}