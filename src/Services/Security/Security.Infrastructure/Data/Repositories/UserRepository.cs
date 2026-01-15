using Dapper;
using Security.Application.Dtos.Results;
using Security.Application.Interfaces.Persistance;
using Security.Domain.Parameters.User;
using Security.Infrastructure.Consrants;
using System.Data;

namespace Security.Infrastructure.Data.Repositories
{
    public sealed class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DapperDataContext dapperDataContext) : base(dapperDataContext)
        {
        }

        public async Task<UserPaginationResult> GetUsersWithFiltersAndPagination(
            UserPaginationParameter queryParameters)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Page", queryParameters.Page, DbType.Int32);
            parameters.Add("PageSize", queryParameters.PageSize, DbType.Int32);
            parameters.Add("IgnorePagination", queryParameters.IgnorePagination, DbType.Boolean);
            parameters.Add("Filters", queryParameters.Filters, DbType.String);

            using var connection = _dapperDataContext.Connection;

            if (connection == null)
            {
                throw new Exception("No existe conexion con la base de datos");
            }

            await using var result = await connection.QueryMultipleAsync(
                StoredProcedures.GetUsersWithFiltersAndPagination,
                parameters,
                commandType: CommandType.StoredProcedure);

            var data = await result.ReadAsync<UserPaginationItem>();
            var totalCount = await result.ReadSingleAsync<long>();

            return new UserPaginationResult(
                Data: data,
                TotalCount: totalCount,
                Page: queryParameters.Page,
                PageSize: queryParameters.PageSize
            );
        }

        public Task<UserInfo?> GetUserInfoById(long userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int64);

            using var connection = _dapperDataContext.Connection;

            if (connection == null)
            {
                throw new Exception("No existe conexion con la base de datos");
            }

            using var result = connection.QueryMultiple(
                StoredProcedures.GetUserInfoById,
                parameters,
                commandType: CommandType.StoredProcedure);

            var data = result.Read<UserInfoSp>().FirstOrDefault();

            if (data == null)
            {
                return Task.FromResult<UserInfo?>(null);
            }

            var userInfo = new UserInfo(data);

            return Task.FromResult<UserInfo?>(userInfo);
        }
    }
}