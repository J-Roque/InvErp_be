-- =============================================
-- Author:      Auth Service
-- Create date: 2026-01-13
-- Description: Busca un usuario por username o email para login
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
