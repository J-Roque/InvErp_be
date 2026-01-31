namespace Messaging.Application.Dtos.General;

public record EmailToSendDto(
  string Subject,
  string Template,
  string Data,
  string Recipients,
  string? Cc,
  string? Bcc,
  int Priority
);
