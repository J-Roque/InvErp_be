-- =====================================================
-- INSERTS PARA NAVIGATIONITEMS
-- =====================================================
-- Estructura: 
-- Id, Identifier, Module, Name, Route, Icon, ParentId, IsActive, OrderIndex, Level, ShowInMenu, NavigationItemId, CreatedAt, CreatedBy, LastModified, LastModifiedBy
-- =====================================================

-- MENU PRINCIPAL - LEVEL 0 (Items principales sin ParentId)

-- 1. Dashboard
INSERT INTO NavigationItem (Identifier, Module, Name, Route, Icon, ParentId, IsActive, OrderIndex, Level, ShowInMenu, CreatedAt, CreatedBy)
VALUES ('dashboard', 'Core', 'Dashboard', '/dashboard', 'dashboard', NULL, 1, 1, 0, 1, GETDATE(), 1);

-- 2. Administración
INSERT INTO NavigationItem (Identifier, Module, Name, Route, Icon, ParentId, IsActive, OrderIndex, Level, ShowInMenu, CreatedAt, CreatedBy)
VALUES ('admin', 'Security', 'Administración', '/admin', 'settings', NULL, 1, 2, 0, 1, GETDATE(), 1);

-- 3. Gestión de Usuarios
INSERT INTO NavigationItem (Identifier, Module, Name, Route, Icon, ParentId, IsActive, OrderIndex, Level, ShowInMenu, CreatedAt, CreatedBy)
VALUES ('users', 'Security', 'Usuarios', '/users', 'people', NULL, 1, 3, 0, 1, GETDATE(), 1);

-- 4. Gestión de Roles
INSERT INTO NavigationItem (Identifier, Module, Name, Route, Icon, ParentId, IsActive, OrderIndex, Level, ShowInMenu, CreatedAt, CreatedBy)
VALUES ('roles', 'Security', 'Roles', '/roles', 'security', NULL, 1, 4, 0, 1, GETDATE(), 1);

-- 5. Perfiles
INSERT INTO NavigationItem (Identifier, Module, Name, Route, Icon, ParentId, IsActive, OrderIndex, Level, ShowInMenu, CreatedAt, CreatedBy)
VALUES ('profiles', 'Security', 'Perfiles', '/profiles', 'assignment_ind', NULL, 1, 5, 0, 1, GETDATE(), 1);

-- 6. Configuración
INSERT INTO NavigationItem (Identifier, Module, Name, Route, Icon, ParentId, IsActive, OrderIndex, Level, ShowInMenu, CreatedAt, CreatedBy)
VALUES ('settings', 'Core', 'Configuración', '/settings', 'tune', NULL, 1, 6, 0, 1, GETDATE(), 1);

-- 7. Reportes
INSERT INTO NavigationItem (Identifier, Module, Name, Route, Icon, ParentId, IsActive, OrderIndex, Level, ShowInMenu, CreatedAt, CreatedBy)
VALUES ('reports', 'Core', 'Reportes', '/reports', 'assessment', NULL, 1, 7, 0, 1, GETDATE(), 1);

-- =====================================================
-- SUBMENUS - LEVEL 1 (Items hijos de otros items)
-- =====================================================

-- Submenu de Administración
-- Nota: Aquí debes reemplazar el ParentId (2) con el ID real generado para 'admin'

-- 2.1 Gestión de Permisos
INSERT INTO NavigationItem (Identifier, Module, Name, Route, Icon, ParentId, IsActive, OrderIndex, Level, ShowInMenu, CreatedAt, CreatedBy)
VALUES ('permissions', 'Security', 'Permisos', '/admin/permissions', 'lock', 2, 1, 1, 1, 1, GETDATE(), 1);

-- 2.2 Auditoría
INSERT INTO NavigationItem (Identifier, Module, Name, Route, Icon, ParentId, IsActive, OrderIndex, Level, ShowInMenu, CreatedAt, CreatedBy)
VALUES ('audit', 'Security', 'Auditoría', '/admin/audit', 'history', 2, 1, 2, 1, 1, GETDATE(), 1);

-- 2.3 Logs del Sistema
INSERT INTO NavigationItem (Identifier, Module, Name, Route, Icon, ParentId, IsActive, OrderIndex, Level, ShowInMenu, CreatedAt, CreatedBy)
VALUES ('system_logs', 'Core', 'Logs del Sistema', '/admin/system-logs', 'description', 2, 1, 3, 1, 1, GETDATE(), 1);

-- Submenu de Configuración
-- Nota: Reemplaza ParentId (6) con el ID real generado para 'settings'

-- 6.1 Parámetros Generales
INSERT INTO NavigationItem (Identifier, Module, Name, Route, Icon, ParentId, IsActive, OrderIndex, Level, ShowInMenu, CreatedAt, CreatedBy)
VALUES ('general_params', 'Core', 'Parámetros Generales', '/settings/general', 'build', 6, 1, 1, 1, 1, GETDATE(), 1);

-- 6.2 Configuración de Email
INSERT INTO NavigationItem (Identifier, Module, Name, Route, Icon, ParentId, IsActive, OrderIndex, Level, ShowInMenu, CreatedAt, CreatedBy)
VALUES ('email_config', 'Messaging', 'Configuración de Email', '/settings/email', 'email', 6, 1, 2, 1, 1, GETDATE(), 1);

-- =====================================================
-- NOTAS IMPORTANTES:
-- =====================================================
-- 1. Los ParentId en los SUBMENUS deben coincidir con los IDs reales generados.
--    Por ejemplo, si 'admin' recibe ID 10, todos los subitems deben tener ParentId = 10
--
-- 2. Ejecuta este script en partes:
--    - Primero los items LEVEL 0 (sin submenu)
--    - Luego obtén los IDs generados
--    - Finalmente ajusta los ParentId de LEVEL 1 y ejecuta esos inserts
--
-- 3. Para ver los IDs generados, ejecuta:
--    SELECT Id, Identifier, Name, Level, ParentId FROM NavigationItem ORDER BY OrderIndex;
--
-- 4. Estructura recomendada con IDs (ejemplo):
--    ID 1: Dashboard (Level 0)
--    ID 2: Administración (Level 0)
--    ID 3: Usuarios (Level 0)
--    ID 4: Roles (Level 0)
--    ID 5: Perfiles (Level 0)
--    ID 6: Configuración (Level 0)
--    ID 7: Reportes (Level 0)
--    ID 8: Permisos (Level 1, ParentId = 2)
--    ID 9: Auditoría (Level 1, ParentId = 2)
--    ID 10: Logs del Sistema (Level 1, ParentId = 2)
--    ID 11: Parámetros Generales (Level 1, ParentId = 6)
--    ID 12: Configuración de Email (Level 1, ParentId = 6)
--
-- =====================================================
-- SCRIPT FINAL COMPLETO CON IDs CORRECTOS (después de obtener los IDs reales):
-- =====================================================

-- Si quieres agregar más items, sigue este patrón:
-- INSERT INTO NavigationItem (Identifier, Module, Name, Route, Icon, ParentId, IsActive, OrderIndex, Level, ShowInMenu, CreatedAt, CreatedBy)
-- VALUES ('unique_identifier', 'ModuleName', 'Display Name', '/route/path', 'icon_name', NULL_or_PARENTID, 1, ORDER, LEVEL, 1, GETDATE(), 1);
