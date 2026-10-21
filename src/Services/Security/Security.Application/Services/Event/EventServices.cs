using System.Text.Json;
using Security.Application.Configuration;
using Security.Application.Wrappers.Event;
using Utilities.Protos.SaveEvent;

namespace Security.Application.Services.Event;
public class EventServices(AppConstants appConstants)
    : IEventService
{
    public async Task<bool> SaveEvent(
        string activityDescription,
        long eventTypeId,
        object data,
        string? ipAddress,
        string moduleName,
        long userId,
        long? tokenId
    )
    {
        try
        {
            var dataStr = JsonSerializer.Serialize(data);
            var clientWrapper = new SaveEventWrapper(appConstants.Backend.Event);
            var model = new SaveEventRequest()
            {
                ActivityDescription = activityDescription,
                EventTypeId = eventTypeId,
                Data = dataStr,
                IpAddress = ipAddress,
                ModuleName = moduleName,
                UserId = userId,
                TokenId = tokenId ?? 0
            };

            var response = await clientWrapper.SaveEvent(model);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}