-- =============================================
-- Security Service - Install All Stored Procedures
-- =============================================
-- Este script crea todos los stored procedures necesarios para el microservicio Security
-- Ejecutar en la base de datos: Inventech
-- =============================================

USE Inventech
GO

-- Drop procedures if exist
IF OBJECT_ID('dbo.sp_GetUsersWithFiltersAndPagination', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetUsersWithFiltersAndPagination;
GO

IF OBJECT_ID('dbo.sp_GetUserInfoById', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetUserInfoById;
GO

PRINT 'Creando stored procedures para Security Service...'
GO

-- =============================================
-- sp_GetUsersWithFiltersAndPagination
-- =============================================
CREATE PROCEDURE sp_GetUsersWithFiltersAndPagination
    @Page INT = 1,
    @PageSize INT = 10,
    @IgnorePagination BIT = 0,
    @Filters NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SearchTerm NVARCHAR(200) = NULL;

    IF @Filters IS NOT NULL
    BEGIN
        SET @SearchTerm = JSON_VALUE(@Filters, '$.SearchTerm');
    END

    -- Resultado paginado
    SELECT
        u.Id,
        u.Username,
        p.FirstName,
        p.LastName,
        p.Email,
        u.IdentityStatusId,
        CASE u.IdentityStatusId
            WHEN 1 THEN 'Activo'
            WHEN 2 THEN 'Inactivo'
            WHEN 3 THEN 'Bloqueado'
            ELSE 'Desconocido'
        END AS IdentityStatus,
        u.ProfileId,
        pr.Name AS ProfileName,
        u.ImageAttachmentId,
        NULL AS ImageAttachmentUrl,
        u.CreatedAt
    FROM Users u
    INNER JOIN Persons p ON u.PersonId = p.Id
    LEFT JOIN Profiles pr ON u.ProfileId = pr.Id
    WHERE
        (@SearchTerm IS NULL OR
         u.Username LIKE '%' + @SearchTerm + '%' OR
         p.FirstName LIKE '%' + @SearchTerm + '%' OR
         p.LastName LIKE '%' + @SearchTerm + '%' OR
         p.Email LIKE '%' + @SearchTerm + '%')
    ORDER BY u.Id DESC
    OFFSET CASE WHEN @IgnorePagination = 1 THEN 0 ELSE (@Page - 1) * @PageSize END ROWS
    FETCH NEXT CASE WHEN @IgnorePagination = 1 THEN 999999 ELSE @PageSize END ROWS ONLY;

    -- Total count
    SELECT COUNT(*) AS TotalCount
    FROM Users u
    INNER JOIN Persons p ON u.PersonId = p.Id
    WHERE
        (@SearchTerm IS NULL OR
         u.Username LIKE '%' + @SearchTerm + '%' OR
         p.FirstName LIKE '%' + @SearchTerm + '%' OR
         p.LastName LIKE '%' + @SearchTerm + '%' OR
         p.Email LIKE '%' + @SearchTerm + '%');
END;
GO

PRINT 'sp_GetUsersWithFiltersAndPagination creado exitosamente.'
GO

-- =============================================
-- sp_GetUserInfoById
-- =============================================
CREATE PROCEDURE sp_GetUserInfoById
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

PRINT 'sp_GetUserInfoById creado exitosamente.'
GO

PRINT '========================================='
PRINT 'Todos los stored procedures de Security Service han sido creados exitosamente!'
PRINT '========================================='
GO
