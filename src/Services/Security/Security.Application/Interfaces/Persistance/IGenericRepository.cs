using Security.Domain.Abstractions;

namespace Security.Application.Interfaces.Persistance
{
    public interface IGenericRepository<T>
        where T: IEntity
    {

    }
}
