using Messaging.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Messaging.Application.Data;

public interface IApplicationDbContext
{
    DbSet<EmailToSend> EmailsToSend { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}