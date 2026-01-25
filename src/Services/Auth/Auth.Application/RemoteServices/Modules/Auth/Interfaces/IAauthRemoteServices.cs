using Auth.Application.RemoteServices.Modules.Auth.Models;
using Auth.Application.RemoteServices.Shared;

namespace Auth.Application.RemoteServices.Modules.Auth.Interfaces;

public interface IAuthRemoteService : IRemoteService
{
    Task<JwtVerifierResult> JwtVerifierAsync(string jwt);
}
