
using Microsoft.EntityFrameworkCore.Infrastructure;
using Security.Application.Data;
using Security.Domain.Models;
using System.Reflection;

namespace Security.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        public DbSet<User> Users => Set<User>();
        public DbSet<Person> Persons => Set<Person>();
        public DbSet<Profile> Profiles => Set<Profile>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<NavigationItem> NavigationItems => Set<NavigationItem>();
        public DbSet<DocumentType> DocumentTypes => Set<DocumentType>();
        public DbSet<SecurityLog> SecurityLogs => Set<SecurityLog>();
        public DatabaseFacade Database => base.Database;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

    }
}
