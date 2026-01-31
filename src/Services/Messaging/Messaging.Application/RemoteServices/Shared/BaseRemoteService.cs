namespace Messaging.Application.RemoteServices.Shared;

public abstract class BaseRemoteService<T>(ILogger<T> logger)
{
    protected async Task<TResult> TryCallAsync<TResult>(Func<Task<TResult>> operation, string operationName, TResult fallback)
    {
        try
        {
            return await operation();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[{Service}] Error in {Operation}", typeof(T).Name, operationName);
            return fallback;
        }
    }
}