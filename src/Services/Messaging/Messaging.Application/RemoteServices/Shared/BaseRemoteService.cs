using System.Text.Json;

namespace Messaging.Application.RemoteServices.Shared;

public abstract class BaseRemoteService<T>(ILogger<T> logger)
{
    // Las APIs responden en camelCase; sin estas opciones las propiedades quedan en su valor por defecto
    protected static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

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
