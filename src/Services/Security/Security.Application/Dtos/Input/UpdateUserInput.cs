using Security.Domain.Enums;

namespace Security.Application.Dtos.Input;

public record UpdateUserInput(
    string FirstName,
    string LastName,
    int DocumentTypeId,
    string DocumentNumber,
    string Email,
    string Username,
    string? Password,
    IdentityStatus IdentityStatusId,
    long? ImageAttachmentId,
    string? ImageAttachmentUrl,
    long ProfileId,
    List<long> RoleIds,
    List<long> ProviderIds,
    List<long> ClientIdS
);
