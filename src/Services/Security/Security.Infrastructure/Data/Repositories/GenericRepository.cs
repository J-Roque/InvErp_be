using Security.Application.Interfaces.Persistance;

namespace Security.Infrastructure.Data.Repositories
{
    public class GenericRepository<T>: IGenericRepository<T>
        where T: IEntity
    {
        protected readonly DapperDataContext _dapperDataContext;
        public GenericRepository(DapperDataContext dapperDataContext)
        {
            _dapperDataContext = dapperDataContext;
        }
    }
}
