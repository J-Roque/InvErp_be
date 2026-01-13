using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Auth.Application.Data;

namespace Auth.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Token> Tokens => Set<Token>();
    public DbSet<AuthLog> AuthLogs => Set<AuthLog>();

    public DatabaseFacade Database => base.Database;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
