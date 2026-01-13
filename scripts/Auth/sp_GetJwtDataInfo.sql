-- =============================================
-- Author:      Auth Service
-- Create date: 2026-01-13
-- Description: Obtiene información del token JWT y usuario para validación
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
