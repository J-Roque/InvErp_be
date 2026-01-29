using Auth.Application.RemoteServices.Modules.Event.Models;

namespace Auth.Application.RemoteServices.Modules.Event.Interfaces;

public interface IEventRemoteService
{
    Task<SaveEventResult> SaveEvent(SaveEventBody body);

}