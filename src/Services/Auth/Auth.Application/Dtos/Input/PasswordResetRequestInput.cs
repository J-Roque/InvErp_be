
namespace Auth.Application.Dtos.Input;

public record PasswordResetRequestInput(
    string Type,
    string Value
);
