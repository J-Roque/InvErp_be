-- =====================================================
-- SEED DE NAVIGATIONITEMS (menú del sidebar del frontend)
-- =====================================================
-- Idempotente: se puede ejecutar varias veces.
--   1. Inserta o actualiza cada item por su Identifier
--   2. Desactiva los items que ya no están en esta lista
--   3. Asigna todos los items activos al perfil ADMIN
--
-- Icon usa clases de PrimeIcons (las que muestra el frontend).
-- Los items con hijos no navegan (solo despliegan el submenú);
-- su Route es solo el prefijo de las rutas hijas.
-- =====================================================

USE Inventech
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRANSACTION;

DECLARE @Items TABLE (
    Identifier       NVARCHAR(100) NOT NULL PRIMARY KEY,
    ParentIdentifier NVARCHAR(100) NULL,
    Module           NVARCHAR(100) NOT NULL,
    Name             NVARCHAR(200) NOT NULL,
    Route            NVARCHAR(300) NOT NULL,
    Icon             NVARCHAR(100) NOT NULL,
    OrderIndex       INT NOT NULL
);

INSERT INTO @Items (Identifier, ParentIdentifier, Module, Name, Route, Icon, OrderIndex) VALUES
-- Nivel 0
('dashboard',            NULL,           'Core',       'Dashboard',              '/dashboard',                 'pi pi-home',             1),
('warehouse',            NULL,           'Warehouse',  'Almacén',                '/warehouse',                 'pi pi-box',              2),
('purchases',            NULL,           'Purchases',  'Compras',                '/purchases',                 'pi pi-shopping-cart',    3),
('sales',                NULL,           'Sales',      'Ventas',                 '/sales',                     'pi pi-dollar',           4),
('cashier',              NULL,           'Cashier',    'Caja',                   '/cashier',                   'pi pi-wallet',           5),
('layaway',              NULL,           'Layaway',    'Apartados',              '/layaway',                   'pi pi-bookmark',         6),
('inventory',            NULL,           'Inventory',  'Inventario',             '/inventory',                 'pi pi-chart-bar',        7),
('workshop',             NULL,           'Workshop',   'Taller',                 '/workshop',                  'pi pi-wrench',           8),
('user-management',      NULL,           'Security',   'Gestión de Usuarios',    '/admin/users',               'pi pi-users',            9),
('receipts',             NULL,           'Receipts',   'Comprobantes',           '/admin/receipts',            'pi pi-file-pdf',        10),
('settings',             NULL,           'Settings',   'Configuración',          '/admin/settings',            'pi pi-cog',             11),

-- Almacén
('warehouse-products',      'warehouse', 'Warehouse',  'Productos',              '/warehouse/products',        'pi pi-list',             1),
('warehouse-categories',    'warehouse', 'Warehouse',  'Categorías',             '/warehouse/categories',      'pi pi-tags',             2),
('warehouse-brands',        'warehouse', 'Warehouse',  'Marcas',                 '/warehouse/brands',          'pi pi-bookmark',         3),
('warehouse-presentations', 'warehouse', 'Warehouse',  'Presentaciones',         '/warehouse/presentations',   'pi pi-clone',            4),

-- Compras
('purchases-new',        'purchases',    'Purchases',  'Nueva Compra',           '/purchases/new',             'pi pi-plus-circle',      1),
('purchases-history',    'purchases',    'Purchases',  'Historial',              '/purchases/history',         'pi pi-history',          2),
('purchases-suppliers',  'purchases',    'Purchases',  'Proveedores',            '/purchases/suppliers',       'pi pi-users',            3),
('purchases-credit',     'purchases',    'Purchases',  'Compras a Crédito',      '/purchases/credit',          'pi pi-credit-card',      4),

-- Ventas
('sales-new',            'sales',        'Sales',      'Punto de Venta (POS)',   '/sales/new',                 'pi pi-desktop',          1),
('sales-quotes',         'sales',        'Sales',      'Cotizaciones',           '/quotes',                    'pi pi-file-edit',        2),
('sales-history',        'sales',        'Sales',      'Historial de Ventas',    '/sales/history',             'pi pi-history',          3),
('sales-customers',      'sales',        'Sales',      'Clientes',               '/sales/customers',           'pi pi-users',            4),
('sales-credit',         'sales',        'Sales',      'Ventas a Crédito',       '/sales/credit',              'pi pi-credit-card',      5),

-- Caja
('cashier-register',     'cashier',      'Cashier',    'Abrir/Cerrar Caja',      '/cashier/register',          'pi pi-lock-open',        1),
('cashier-movements',    'cashier',      'Cashier',    'Movimientos',            '/cashier/movements',         'pi pi-list',             2),
('cashier-history',      'cashier',      'Cashier',    'Historial',              '/cashier/history',           'pi pi-history',          3),

-- Inventario
('inventory-kardex',     'inventory',    'Inventory',  'Kardex',                 '/inventory/kardex',          'pi pi-table',            1),
('inventory-movements',  'inventory',    'Inventory',  'Movimientos de Stock',   '/inventory/movements',       'pi pi-arrows-h',         2),
('inventory-reports',    'inventory',    'Inventory',  'Reportes',               '/inventory/reports',         'pi pi-chart-line',       3),

-- Taller
('workshop-orders',      'workshop',     'Workshop',   'Órdenes de Servicio',    '/workshop/orders',           'pi pi-file',             1),
('workshop-new',         'workshop',     'Workshop',   'Nueva Orden',            '/workshop/new',              'pi pi-plus',             2),
('workshop-technicians', 'workshop',     'Workshop',   'Técnicos',               '/workshop/technicians',      'pi pi-user',             3),

-- Gestión de Usuarios
('users',                'user-management', 'Security', 'Usuarios',              '/admin/users/users',         'pi pi-user',             1),
('employees',            'user-management', 'Security', 'Empleados',             '/admin/users/employees',     'pi pi-id-card',          2),
('roles',                'user-management', 'Security', 'Roles',                 '/admin/users/roles',         'pi pi-shield',           3),
('permissions',          'user-management', 'Security', 'Permisos',              '/admin/users/permissions',   'pi pi-lock',             4),

-- Comprobantes
('receipts-types',       'receipts',     'Receipts',   'Tipos de Comprobante',   '/admin/receipts/types',      'pi pi-list',             1),
('receipts-series',      'receipts',     'Receipts',   'Series',                 '/admin/receipts/series',     'pi pi-hashtag',          2),
('receipts-numbering',   'receipts',     'Receipts',   'Numeración',             '/admin/receipts/numbering',  'pi pi-sort-numeric-up',  3),

-- Configuración
('settings-general',     'settings',     'Settings',   'General',                '/admin/settings/general',    'pi pi-sliders-h',        1),
('settings-company',     'settings',     'Settings',   'Empresa',                '/admin/settings/company',    'pi pi-building',         2),
('settings-system',      'settings',     'Settings',   'Sistema',                '/admin/settings/system',     'pi pi-desktop',          3),
('settings-tax',         'settings',     'Settings',   'Impuestos',              '/admin/settings/tax',        'pi pi-percentage',       4);

-- 1a. Items de nivel 0
MERGE NavigationItems AS target
USING (SELECT * FROM @Items WHERE ParentIdentifier IS NULL) AS src
    ON target.Identifier = src.Identifier
WHEN MATCHED THEN UPDATE SET
    Module = src.Module, Name = src.Name, Route = src.Route, Icon = src.Icon,
    ParentId = NULL, Level = 0, OrderIndex = src.OrderIndex,
    IsActive = 1, ShowInMenu = 1, LastModified = SYSUTCDATETIME()
WHEN NOT MATCHED THEN
    INSERT (Identifier, Module, Name, Route, Icon, ParentId, IsActive, OrderIndex, Level, ShowInMenu, CreatedAt)
    VALUES (src.Identifier, src.Module, src.Name, src.Route, src.Icon, NULL, 1, src.OrderIndex, 0, 1, SYSUTCDATETIME());

-- 1b. Items de nivel 1 (el ParentId se resuelve por Identifier)
MERGE NavigationItems AS target
USING (
    SELECT i.*, p.Id AS ParentId
    FROM @Items i
    INNER JOIN NavigationItems p ON p.Identifier = i.ParentIdentifier
) AS src
    ON target.Identifier = src.Identifier
WHEN MATCHED THEN UPDATE SET
    Module = src.Module, Name = src.Name, Route = src.Route, Icon = src.Icon,
    ParentId = src.ParentId, Level = 1, OrderIndex = src.OrderIndex,
    IsActive = 1, ShowInMenu = 1, LastModified = SYSUTCDATETIME()
WHEN NOT MATCHED THEN
    INSERT (Identifier, Module, Name, Route, Icon, ParentId, IsActive, OrderIndex, Level, ShowInMenu, CreatedAt)
    VALUES (src.Identifier, src.Module, src.Name, src.Route, src.Icon, src.ParentId, 1, src.OrderIndex, 1, 1, SYSUTCDATETIME());

-- 2. Items que ya no forman parte del menú
UPDATE NavigationItems
SET IsActive = 0, LastModified = SYSUTCDATETIME()
WHERE IsActive = 1
  AND Identifier NOT IN (SELECT Identifier FROM @Items);

-- 3. El perfil ADMIN ve todo el menú
DECLARE @AdminProfileId BIGINT = (SELECT Id FROM Profiles WHERE Code = 'ADMIN');

IF @AdminProfileId IS NULL
    PRINT 'Aviso: no existe el perfil con Code = ADMIN; no se asignaron items.';
ELSE
    INSERT INTO ProfileNavigationItem (ProfileId, NavigationItemId)
    SELECT @AdminProfileId, ni.Id
    FROM NavigationItems ni
    WHERE ni.IsActive = 1
      AND NOT EXISTS (
          SELECT 1 FROM ProfileNavigationItem pni
          WHERE pni.ProfileId = @AdminProfileId AND pni.NavigationItemId = ni.Id
      );

COMMIT TRANSACTION;

SELECT
    (SELECT COUNT(*) FROM NavigationItems WHERE IsActive = 1) AS ItemsActivos,
    (SELECT COUNT(*) FROM ProfileNavigationItem WHERE ProfileId = @AdminProfileId) AS ItemsDelAdmin;
GO
