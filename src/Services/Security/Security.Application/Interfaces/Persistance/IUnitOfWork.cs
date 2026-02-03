namespace Security.Application.Interfaces.Persistance
{
    public interface IUnitOfWork
    {
        public IUserRepository Users { get; }
        public IProfileRepository Profiles { get; }
        void BeginTransaction();
        void Commit();
        void CommitAndCloseConnection();
        void Rollback();
    }
}
