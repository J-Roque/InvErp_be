CREATE OR ALTER PROCEDURE sp_GetUserInfoById
    @UserId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.Id,
        p.FirstName,
        p.LastName,
        p.DocumentTypeId,
        p.DocumentNumber,
        p.Email,
        u.Username,
        u.IdentityStatusId,
        CASE u.IdentityStatusId
            WHEN 1 THEN 'Activo'
            WHEN 2 THEN 'Inactivo'
            WHEN 3 THEN 'Bloqueado'
            ELSE 'Desconocido'
        END AS IdentityStatus,
        u.ImageAttachmentId,
        NULL AS ImageAttachmentUrl,
        u.ProfileId,
        pr.Name AS ProfileName,
        (SELECT '[' + STRING_AGG(CAST(ur.RoleId AS VARCHAR), ',') + ']'
         FROM UserRoles ur WHERE ur.UserId = u.Id) AS RoleIds,
        u.CreatedAt,
        u.CreatedBy,
        cb.Username AS CreatedByName,
        u.LastModified,
        u.LastModifiedBy,
        mb.Username AS LastModifiedByName
    FROM Users u
    INNER JOIN Persons p ON u.PersonId = p.Id
    LEFT JOIN Profiles pr ON u.ProfileId = pr.Id
    LEFT JOIN Users cb ON u.CreatedBy = cb.Id
    LEFT JOIN Users mb ON u.LastModifiedBy = mb.Id
    WHERE u.Id = @UserId;
END;
GO
