using Auth.Application.Interfaces.Persistence;

namespace Auth.Infrastructure.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T>
    where T : IEntity
{
    protected readonly DapperDataContext _dapperDataContext;

    public GenericRepository(DapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }
}
