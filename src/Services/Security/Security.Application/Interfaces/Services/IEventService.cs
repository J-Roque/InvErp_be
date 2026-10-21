namespace Security.Application.Interfaces.Services;

public interface IEventService: ISingletonService
{
    public Task<bool> SaveEvent(
        string activityDescription,
        long eventTypeId,
        object data,
        string? ipAddress,
        string moduleName,
        long userId,
        long? tokenId
    );
}