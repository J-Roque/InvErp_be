
using Messaging.Domain.Abstractions;
using Messaging.Domain.Models;

namespace Messaging.Domain.Events;

public record EmailToSendUpdatedEvent(EmailToSend EmailToSend) : IDomainEvent;
