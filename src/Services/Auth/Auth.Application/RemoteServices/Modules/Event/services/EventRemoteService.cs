using System.Text;
using System.Text.Json;
using Auth.Application.Configuration;
using Auth.Application.RemoteServices.Modules.Event.Interfaces;
using Auth.Application.RemoteServices.Modules.Event.Models;
using Auth.Application.RemoteServices.Shared;
using Microsoft.Extensions.Logging;

namespace Auth.Application.RemoteServices.Modules.Event.Services;

public class EventRemoteService(ILogger<EventRemoteService> logger, AppConstants appConstants)
    : BaseRemoteService<EventRemoteService>(logger), IEventRemoteService
{
    public Task<SaveEventResult> SaveEvent(SaveEventBody body)
    {
        var defaultErrorResult = new SaveEventResult() { IsSuccess = false };
        
        return TryCallAsync<SaveEventResult>(async () =>
        {
            using var client = new HttpClient();
            var url = appConstants.Backend.Event + "/ms/events";
            var json = JsonSerializer.Serialize(body);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, stringContent);

            if (!response.IsSuccessStatusCode)
            {
                return defaultErrorResult;
            }

            var respContent = await response.Content.ReadAsStringAsync();
            var jsonResult = JsonSerializer.Deserialize<SaveEventResult>(respContent);

            return jsonResult ?? defaultErrorResult;
            
        }, nameof(SaveEvent), defaultErrorResult);
    }
}