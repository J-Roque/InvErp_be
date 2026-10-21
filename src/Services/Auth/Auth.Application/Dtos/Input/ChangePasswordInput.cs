namespace Auth.Application.Dtos.Input;

public record ChangePasswordInput(
    string NewPassword,
    string ResetJwt
);
