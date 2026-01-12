using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Security.Domain.Models;

namespace Security.Application.Data
{
    public interface IApplicationDbContext
    {
        //public DbSet<Person> Persons { get; }
        DbSet<User> Users { get; }

    }
}
