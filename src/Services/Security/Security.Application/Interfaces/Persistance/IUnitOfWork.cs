namespace Security.Application.Interfaces.Persistance
{
    public interface IUnitOfWork
    {
        public IUserRepository Users { get; }
        void BeginTransaction();
        void Commit();
        void CommitAndCloseConnection();
        void Rollback();
    }
}
