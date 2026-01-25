

using Auth.Application.RemoteServices.Modules.Auth.Models;
using Auth.Application.RemoteServices.Modules.Event.Models;
using Auth.Application.RemoteServices.Shared;

namespace Auth.Application.RemoteServices.Modules.Event.Interfaces;

public interface IEventRemoteService: IRemoteService
{
    Task<JwtVerifierResult> JwtVerifierAsync(string jwt);
}
