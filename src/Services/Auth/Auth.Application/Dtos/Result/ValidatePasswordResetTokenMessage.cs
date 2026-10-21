namespace Auth.Application.Dtos.Result;

public record ValidatePasswordResetTokenMessage(
    bool IsValid,
    string Message
);
