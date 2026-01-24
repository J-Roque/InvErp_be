namespace Security.Application.Dtos.Input;

public record ChangeOwnPasswordInput(
    string OldPassword,
    string NewPassword
);

