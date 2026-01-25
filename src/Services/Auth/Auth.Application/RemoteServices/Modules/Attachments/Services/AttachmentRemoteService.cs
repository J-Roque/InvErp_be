using System.Text;
using System.Text.Json;
using Auth.Application.Configuration;
using Auth.Application.RemoteServices.Modules.Attachments.Interfaces;
using Auth.Application.RemoteServices.Modules.Attachments.Models;
using Auth.Application.RemoteServices.Shared;

namespace Auth.Application.RemoteServices.Modules.Attachments.Services;

public class AttachmentRemoteService(ILogger<AttachmentRemoteService> logger, AppConstants appConstants)
    : BaseRemoteService<AttachmentRemoteService>(logger), IAttachmentRemoteService
{
    public Task<SignFileAndUpdateResult> SignFileAndUpdate(SignFileAndUpdateBody body)
    {
        var defaultErrorResult = new SignFileAndUpdateResult
        {
            IsSuccess = false,
            Url = ""
        };
        
        return TryCallAsync<SignFileAndUpdateResult>(async () =>
        {
            using var client = new HttpClient();
            var url = appConstants.Backend.Attachment + "/ms/attachments/sign-and-update";
            var json = JsonSerializer.Serialize(body);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, stringContent);

            if (!response.IsSuccessStatusCode)
            {
                defaultErrorResult.Url = "";
                return defaultErrorResult;
            }

            var respContent = await response.Content.ReadAsStringAsync();
            var jsonResult = JsonSerializer.Deserialize<SignFileAndUpdateResult>(respContent);

            if (jsonResult == null)
            {
                defaultErrorResult.Url = "";
                return defaultErrorResult;
            }

            return jsonResult;
        }, nameof(SignFileAndUpdate), defaultErrorResult);
        
        
    }
}