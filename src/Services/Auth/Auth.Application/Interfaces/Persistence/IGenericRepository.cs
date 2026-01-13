using Auth.Domain.Abstractions;

namespace Auth.Application.Interfaces.Persistence;

public interface IGenericRepository<T>
    where T: IEntity
{

}
