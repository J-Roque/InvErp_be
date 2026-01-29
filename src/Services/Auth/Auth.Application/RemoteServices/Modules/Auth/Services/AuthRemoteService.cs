using System.Text;
using System.Text.Json;
using Auth.Application.Configuration;
using Auth.Application.RemoteServices.Modules.Auth.Interfaces;
using Auth.Application.RemoteServices.Modules.Auth.Models;
using Auth.Application.RemoteServices.Shared;
using Microsoft.Extensions.Logging;

namespace Auth.Application.RemoteServices.Modules.Auth.Services;

public class AuthRemoteService(ILogger<AuthRemoteService> logger, AppConstants appConstants)
    : BaseRemoteService<AuthRemoteService>(logger), IAuthRemoteService
{
    public string ServiceName => "Auth";

    public Task<JwtVerifierResult> JwtVerifierAsync(string jwt)
    {
        var defaultErrorResult = new JwtVerifierResult
        {
            IsValid = false,
            Message = "Sucedió un error inesperado al validar el token",
            Email = "",
            UserId = -1,
            Profile = null,
            Roles = [],
            Ip = null,
            TokenId = -1
        };

        return TryCallAsync<JwtVerifierResult>(async () =>
        {
            using var client = new HttpClient();
            var url = appConstants.Backend.Auth + "/ms/auth/verify-jwt";
            var body = new { Jwt = jwt };
            var json = JsonSerializer.Serialize(body);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, stringContent);

            if (!response.IsSuccessStatusCode)
            {
                defaultErrorResult.Message = "Error al validar el token";
                return defaultErrorResult;
            }

            var respContent = await response.Content.ReadAsStringAsync();
            var jsonResult = JsonSerializer.Deserialize<JwtVerifierResult>(respContent);

            if (jsonResult == null)
            {
                defaultErrorResult.Message = "Error al deserializar la respuesta";
                return defaultErrorResult;
            }

            return jsonResult;
        }, nameof(JwtVerifierAsync), defaultErrorResult);
    }
}