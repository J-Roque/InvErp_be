-- =============================================
-- Auth Service - Install All Stored Procedures
-- =============================================
-- Este script crea todos los stored procedures necesarios para el microservicio Auth
-- Ejecutar en la base de datos: Inventech
-- =============================================

USE Inventech
GO

-- Drop procedures if exist
IF OBJECT_ID('dbo.sp_SearchUserForLogin', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_SearchUserForLogin;
GO

IF OBJECT_ID('dbo.sp_GetJwtDataInfo', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetJwtDataInfo;
GO

IF OBJECT_ID('dbo.sp_ChangeUserPassword', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ChangeUserPassword;
GO

PRINT 'Creando stored procedures para Auth Service...'
GO

-- =============================================
-- sp_SearchUserForLogin
-- =============================================
CREATE PROCEDURE sp_SearchUserForLogin
@Value NVARCHAR(MAX)
AS
BEGIN
  SELECT TOP 1
		 u.Id UserId,
		 LTRIM(RTRIM(CONCAT(
			COALESCE(pe.FirstName, ''),
			CASE
				WHEN pe.FirstName IS NOT NULL AND pe.LastName IS NOT NULL THEN ' '
				ELSE ''
			END,
			COALESCE(pe.LastName, '')
		))) AS FullName,
		 u.Username,
		 u.Password,
		 u.Salt,
		 pe.Email,
		 ISNULL((
			SELECT
				r.Id,
				r.Name
			FROM UserRoles ur
			INNER JOIN Roles r
			  ON ur.RoleId = r.Id
			WHERE ur.UserId = u.Id
				AND r.IsActive = 1
			FOR JSON PATH
		), '[]') AS Roles,
		ISNULL((
			SELECT pro.Id,
			       pro.Name
			FROM Profiles pro
			WHERE pro.Id = u.ProfileId
				AND pro.IsActive = 1
			FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
			), NULL) AS Profile
  FROM Users u
  INNER JOIN Persons pe
    ON u.PersonId = pe.Id
  WHERE u.Username = @Value OR
		pe.Email = @Value
END
GO

PRINT 'sp_SearchUserForLogin creado exitosamente.'
GO

-- =============================================
-- sp_GetJwtDataInfo
-- =============================================
CREATE PROCEDURE sp_GetJwtDataInfo
@Code NVARCHAR(MAX)
AS
BEGIN
  SELECT TOP 1

		 -- JWT
		 t.Id,
		 t.Type,
		 t.ExpiresAt,
		 t.Ip,
		 t.IsActive,

		 -- USUARIO
		 u.Id UserId,
		 pe.Email,

		 ISNULL((
			SELECT
				r.Id,
				r.Name
			FROM UserRoles ur
			INNER JOIN Roles r
			  ON ur.RoleId = r.Id
			WHERE ur.UserId = u.Id
				AND r.IsActive = 1
			FOR JSON PATH
		), '[]') AS Roles,
		ISNULL((
			SELECT pro.Id,
			       pro.Name
			FROM Profiles pro
			WHERE pro.Id = u.ProfileId
				AND pro.IsActive = 1
			FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
			), NULL) AS Profile
  From Tokens t
  INNER JOIN Users u
    ON t.UserId = u.Id
  INNER JOIN Persons pe
    ON u.PersonId = pe.Id
  WHERE t.Code = @Code
END
GO

PRINT 'sp_GetJwtDataInfo creado exitosamente.'
GO

-- =============================================
-- sp_ChangeUserPassword
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

PRINT 'sp_ChangeUserPassword creado exitosamente.'
GO

PRINT '========================================='
PRINT 'Todos los stored procedures de Auth Service han sido creados exitosamente!'
PRINT '========================================='
GO
