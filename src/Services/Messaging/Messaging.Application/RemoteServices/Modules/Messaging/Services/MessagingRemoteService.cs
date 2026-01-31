using System.Text;
using System.Text.Json;
using Messaging.Application.Configuraction;
using Messaging.Application.Configuration;
using Messaging.Application.RemoteServices.Modules.Messaging.Interfaces;
using Messaging.Application.RemoteServices.Modules.Messaging.Models;
using Messaging.Application.RemoteServices.Shared;

namespace Messaging.Application.RemoteServices.Modules.Messaging.Services;

public class MessagingRemoteService(ILogger<MessagingRemoteService> logger, AppConstants appConstants)
    : BaseRemoteService<MessagingRemoteService>(logger), IMessagingRemoteService
{
    public Task<SaveEmailToSendResult> SaveEmailToSend(EmailToSendBody data)
    {
        var defaultErrorResult = new SaveEmailToSendResult() { Id = -1};
        
        return TryCallAsync<SaveEmailToSendResult>(async () =>
        {
            using var client = new HttpClient();
            var url = appConstants.Backend.Messaging + "/ms/emails-to-send/save";
            var body = new { Email = data };
            var json = JsonSerializer.Serialize(body);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, stringContent);

            if (!response.IsSuccessStatusCode)
            {
                return defaultErrorResult;
            }

            var respContent = await response.Content.ReadAsStringAsync();
            var jsonResult = JsonSerializer.Deserialize<SaveEmailToSendResult>(respContent);

            return jsonResult ?? defaultErrorResult;
            
        }, nameof(SaveEmailToSend), defaultErrorResult);
    }

    public Task<SendEmailResult> SendEmail(EmailToSendBody data)
    {
        var defaultErrorResult = new SendEmailResult() { IsSuccess = false };
        
        return TryCallAsync<SendEmailResult>(async () =>
        {
            using var client = new HttpClient();
            var url = appConstants.Backend.Messaging + "/ms/emails-to-send/send";
            var body = new { Email = data };
            var json = JsonSerializer.Serialize(body);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, stringContent);

            if (!response.IsSuccessStatusCode)
            {
                return defaultErrorResult;
            }

            var respContent = await response.Content.ReadAsStringAsync();
            var jsonResult = JsonSerializer.Deserialize<SendEmailResult>(respContent);

            return jsonResult ?? defaultErrorResult;
            
        }, nameof(SendEmail), defaultErrorResult);
    }
}