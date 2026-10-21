-- =============================================
-- Author:      Security Service
-- Description: Obtiene la información de un perfil, incluidos los Ids de
--              sus items de navegación activos (JSON: [1,2,3])
-- =============================================
CREATE OR ALTER PROCEDURE sp_GetProfileInfoById
@ProfileId BIGINT
AS
BEGIN
  SET NOCOUNT ON;

  SELECT
    pro.Id,
    pro.Name,
    pro.IsActive,
    pro.ImageAttachmentId,
    CAST(NULL AS NVARCHAR(255)) AS ImageAttachmentUrl,
    ISNULL(
      (
        SELECT '[' + STRING_AGG(CAST(pni.NavigationItemId AS VARCHAR(20)), ',') + ']'
        FROM ProfileNavigationItem pni
        INNER JOIN NavigationItems ni
          ON ni.Id = pni.NavigationItemId
        WHERE pni.ProfileId = pro.Id
          AND ni.IsActive = 1
      ),
      '[]'
    ) AS NavigationItemIds,

    pro.CreatedAt,
    pro.CreatedBy,
    LTRIM(RTRIM(CONCAT(COALESCE(pcb.FirstName, ''), ' ', COALESCE(pcb.LastName, '')))) AS CreatedByName,
    pro.LastModified,
    pro.LastModifiedBy,
    LTRIM(RTRIM(CONCAT(COALESCE(plm.FirstName, ''), ' ', COALESCE(plm.LastName, '')))) AS LastModifiedByName

  FROM Profiles pro
  LEFT JOIN Users ucb
    ON pro.CreatedBy = ucb.Id
  LEFT JOIN Persons pcb
    ON ucb.PersonId = pcb.Id
  LEFT JOIN Users ulm
    ON pro.LastModifiedBy = ulm.Id
  LEFT JOIN Persons plm
    ON ulm.PersonId = plm.Id
  WHERE pro.Id = @ProfileId
END
GO
