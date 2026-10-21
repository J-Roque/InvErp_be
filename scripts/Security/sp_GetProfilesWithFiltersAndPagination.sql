-- =============================================
-- Author:      Security Service
-- Description: Lista perfiles con filtros (JSON: { "SearchTerm": "", "IsActive": true })
--              y paginación. Devuelve dos resultados: los datos y el total.
-- =============================================
CREATE OR ALTER PROCEDURE sp_GetProfilesWithFiltersAndPagination
    @Page INT = 1,
    @PageSize INT = 10,
    @IgnorePagination BIT = 0,
    @Filters NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SearchTerm NVARCHAR(255) = NULL,
            @IsActive BIT = NULL;

    IF @Filters IS NOT NULL AND ISJSON(@Filters) = 1
    BEGIN
        SELECT
            @SearchTerm = JSON_VALUE(@Filters, '$.SearchTerm'),
            @IsActive = TRY_CAST(JSON_VALUE(@Filters, '$.IsActive') AS BIT);
    END

    CREATE TABLE #Profiles (
        Id BIGINT PRIMARY KEY,
        Name NVARCHAR(255),
        IsActive BIT,
        ImageAttachmentId BIGINT,
        ImageAttachmentUrl NVARCHAR(255),
        CreatedAt DATETIME2(7)
    );

    INSERT INTO #Profiles (Id, Name, IsActive, ImageAttachmentId, ImageAttachmentUrl, CreatedAt)
    SELECT
        p.Id,
        p.Name,
        p.IsActive,
        p.ImageAttachmentId,
        NULL,
        p.CreatedAt
    FROM Profiles p
    WHERE
        (@SearchTerm IS NULL OR p.Name LIKE '%' + @SearchTerm + '%') AND
        (@IsActive IS NULL OR p.IsActive = @IsActive);

    DECLARE @TotalProfiles BIGINT;
    SELECT @TotalProfiles = COUNT(*) FROM #Profiles;

    IF @IgnorePagination = 1
    BEGIN
        SELECT * FROM #Profiles ORDER BY CreatedAt;
    END
    ELSE
    BEGIN
        SELECT *
        FROM #Profiles
        ORDER BY CreatedAt
        OFFSET (@Page - 1) * @PageSize ROWS
        FETCH NEXT @PageSize ROWS ONLY;
    END

    SELECT @TotalProfiles AS TotalCount;

    DROP TABLE #Profiles;
END
GO
