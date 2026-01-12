
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


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

    }
}
