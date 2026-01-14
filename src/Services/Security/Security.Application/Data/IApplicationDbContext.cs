using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Security.Domain.Models;

namespace Security.Application.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Person> Persons { get; }
        DbSet<User> Users { get; }
        DbSet<Role> Roles { get; }
        DbSet<Profile> Profiles { get; }

        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
