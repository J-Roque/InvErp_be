using Security.Application.RemoteServices.Modules.Event.Models;

namespace Security.Application.RemoteServices.Modules.Event.Interfaces;

public interface IEventRemoteService
{
     Task<SaveEventResult> SaveEvent(SaveEventBody body);
     
}