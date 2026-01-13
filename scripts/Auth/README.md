# Stored Procedures - Auth Service

Este directorio contiene todos los stored procedures necesarios para el microservicio de Auth.

## Stored Procedures

### 1. sp_SearchUserForLogin
**Descripción:** Busca un usuario por username o email para el proceso de login.

**Parámetros:**
- `@Value` (NVARCHAR(MAX)): Username o email del usuario

**Retorna:**
- UserId, FullName, Username, Password (hash), Salt, Email, Roles (JSON), Profile (JSON)

**Uso:**
```sql
EXEC sp_SearchUserForLogin @Value = 'john.doe@example.com'
```

---

### 2. sp_GetJwtDataInfo
**Descripción:** Obtiene la información del token JWT y del usuario asociado para validación.

**Parámetros:**
- `@Code` (NVARCHAR(MAX)): Código único del token JWT

**Retorna:**
- Token: Id, Type, ExpiresAt, Ip, IsActive
- User: UserId, Email, Roles (JSON), Profile (JSON)

**Uso:**
```sql
EXEC sp_GetJwtDataInfo @Code = 'a1b2c3d4-e5f6-7890-abcd-ef1234567890'
```

---

### 3. sp_ChangeUserPassword
**Descripción:** Cambia la contraseña de un usuario y marca el token de reset como usado.

**Parámetros:**
- `@UserId` (BIGINT): ID del usuario
- `@NewPassword` (NVARCHAR(255)): Nueva contraseña (hash)
- `@Salt` (NVARCHAR(255)): Salt de la nueva contraseña
- `@TokenCode` (NVARCHAR(255)): Código del token de reset

**Retorna:**
- `1`: Éxito
- `-1`: Usuario no encontrado
- `-2`: Token inválido
- `0`: Error inesperado

**Uso:**
```sql
EXEC sp_ChangeUserPassword
    @UserId = 1,
    @NewPassword = 'hashedpassword',
    @Salt = 'salt123',
    @TokenCode = 'token-code-here'
```

---

## Instalación

Para ejecutar todos los stored procedures en orden:

```bash
# En SQL Server Management Studio o Azure Data Studio
# Ejecutar cada archivo en el siguiente orden:
1. sp_SearchUserForLogin.sql
2. sp_GetJwtDataInfo.sql
3. sp_ChangeUserPassword.sql
```

## Dependencias

Estos stored procedures requieren las siguientes tablas:
- `Users` (del microservicio Security)
- `Persons` (del microservicio Security)
- `Roles` (del microservicio Security)
- `UserRoles` (del microservicio Security)
- `Profiles` (del microservicio Security)
- `Tokens` (del microservicio Auth)

## Notas

- Todos los stored procedures están optimizados para usar índices en las tablas.
- Los campos JSON (Roles, Profile) permiten retornar relaciones complejas en una sola consulta.
- El procedimiento `sp_ChangeUserPassword` usa transacciones para garantizar consistencia de datos.
