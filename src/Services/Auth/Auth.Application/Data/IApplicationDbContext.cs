using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Auth.Application.Data;

public interface IApplicationDbContext
{
    DbSet<Token> Tokens { get; }
    DbSet<AuthLog> AuthLogs { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DatabaseFacade Database { get; }
}
