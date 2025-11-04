using Haviliar.Domain.Networks.Entities;
using Haviliar.Domain.OperationCenters.Entities;
using Haviliar.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace Haviliar.Infra.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<OperationCenter> OperationCenters { get; set; }
        public DbSet<UserOperationCenter> UserOperationCenters { get; set; }
        public DbSet<Network> Networks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
