using Security.Application.RemoteServices.Modules.Auth.Models;
using Security.Application.RemoteServices.Shared;

namespace Security.Application.RemoteServices.Modules.Auth.Interfaces;

public interface IAuthRemoteService: IRemoteService
{
    Task<JwtVerifierResult> JwtVerifierAsync(string jwt);
}