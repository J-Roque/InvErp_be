using Messaging.Application.RemoteServices.Modules.Auth.Models;
using Messaging.Application.RemoteServices.Shared;

namespace Messaging.Application.RemoteServices.Modules.Auth.Interfaces;

public interface IAuthRemoteService: IRemoteService
{
    Task<JwtVerifierResult> JwtVerifierAsync(string jwt);
}