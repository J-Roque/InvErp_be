using System.Data;
using Auth.Application.Dtos.Result;
using Dapper;
using Auth.Application.Interfaces.Persistence;
using Auth.Infrastructure.Constants;
using Microsoft.Data.SqlClient;


namespace Auth.Infrastructure.Data.Repositories;

public sealed class AuthRepository : GenericRepository<Token>, IAuthRepository
{
    public AuthRepository(DapperDataContext dapperDataContext) : base(dapperDataContext)
    {
    }

    public Task<UserForLogin?> SearchUserForLogin(string value)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Value", value, DbType.String);

        using var connection = _dapperDataContext.Connection;

        if (connection == null)
        {
            throw new Exception("No existe conexión con la base de datos");
        }

        using var result = connection.QueryMultiple(
            StoredProcedures.SearchUserForLogin,
            parameters,
            commandType: CommandType.StoredProcedure);

        var data = result.Read<UserForLoginSp>().FirstOrDefault();

        if (data == null)
        {
            return Task.FromResult<UserForLogin?>(null);
        }

        var user = new UserForLogin(data);

        return Task.FromResult<UserForLogin?>(user);
    }

    public async Task<bool> ChangePassword(long userId, string newPassword, string salt, string tokenCode)
    {
        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId, DbType.Int64);
        parameters.Add("NewPassword", newPassword, DbType.String);
        parameters.Add("Salt", salt, DbType.String);
        parameters.Add("TokenCode", tokenCode, DbType.String);

        parameters.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

        using var connection = _dapperDataContext.Connection;

        if (connection == null)
        {
            throw new Exception("No existe conexión con la base de datos");
        }

        try
        {
            await connection.ExecuteAsync(
                StoredProcedures.ChangeUserPassword,
                parameters,
                commandType: CommandType.StoredProcedure);

            var returnValue = parameters.Get<int>("ReturnValue");

            return returnValue switch
            {
                1 => true,
                -1 => throw new Exception("No se encontró el usuario especificado."),
                -2 => throw new Exception("El token especificado no es válido."),
                _ => throw new Exception($"Error inesperado. Código de error: {returnValue}")
            };
        }
        catch (SqlException ex)
        {
            throw new Exception($"Error en la base de datos: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error inesperado: {ex.Message}", ex);
        }
    }

    public Task<JwtDataInfo?> GetJwtDataInfo(string jwtCode)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Code", jwtCode, DbType.String);

        using var connection = _dapperDataContext.Connection;

        if (connection == null)
        {
            throw new Exception("No existe conexión con la base de datos");
        }

        using var result = connection.QueryMultiple(
            StoredProcedures.GetJwtDataInfo,
            parameters,
            commandType: CommandType.StoredProcedure);

        var data = result.Read<JwtDataInfoSp>().FirstOrDefault();

        if (data == null)
        {
            return Task.FromResult<JwtDataInfo?>(null);
        }

        var resp = new JwtDataInfo(data);

        return Task.FromResult<JwtDataInfo?>(resp);
    }
}
