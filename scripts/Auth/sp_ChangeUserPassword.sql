-- =============================================
-- Author:      Auth Service
-- Create date: 2026-01-13
-- Description: Cambia la contraseña de un usuario y marca el token como usado
-- Returns:     1 = Success, -1 = User not found, -2 = Invalid token
-- =============================================
CREATE PROCEDURE sp_ChangeUserPassword
    @UserId BIGINT,
    @NewPassword NVARCHAR(255),
    @Salt NVARCHAR(255),
    @TokenCode NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY
        -- Actualizar contraseña del usuario
        UPDATE Users
        SET Password = @NewPassword,
            Salt = @Salt
        WHERE Id = @UserId;

        IF @@ROWCOUNT = 0
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('No se encontró el usuario especificado.', 16, 1);
            RETURN -1;
        END

        -- Marcar token como usado e inactivo
        UPDATE Tokens
        SET LastUsedAt = GETDATE(),
            IsActive = 0,
            LastModified = GETDATE(),
            LastModifiedBy = @UserId
        WHERE Code = @TokenCode;

        IF @@ROWCOUNT = 0
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('El token especificado no es válido.', 16, 1);
            RETURN -2;
        END

        COMMIT TRANSACTION;

        RETURN 1;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN 0;
    END CATCH
END
GO
