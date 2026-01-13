namespace Auth.Application.Interfaces.Persistence;

public interface IUnitOfWork
{
    public IAuthRepository Auth { get; }

    void BeginTransaction();
    void Commit();
    void CommitAndCloseConnection();
    void Rollback();
}
