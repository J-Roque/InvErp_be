using Messaging.Application.RemoteServices.Modules.Event.Models;

namespace Messaging.Application.RemoteServices.Modules.Event.Interfaces;

public interface IEventRemoteService
{
     Task<SaveEventResult> SaveEvent(SaveEventBody body);
     
}