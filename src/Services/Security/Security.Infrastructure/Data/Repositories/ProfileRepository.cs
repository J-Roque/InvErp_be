using Dapper;
using Security.Application.Dtos.Results;
using Security.Application.Interfaces.Persistance;
using Security.Domain.Parameters.Profile;
using Security.Infrastructure.Consrants;
using System.Data;

namespace Security.Infrastructure.Data.Repositories
{
    public sealed class ProfileRepository : GenericRepository<Profile>, IProfileRepository
    {
        public ProfileRepository(DapperDataContext dapperDataContext) : base(dapperDataContext)
        {
        }

        public async Task<ProfilePaginationResult> GetProfilesWithFiltersAndPagination(
            ProfilePaginationParameter queryParameters)
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
                StoredProcedures.GetProfilesWithFiltersAndPagination,
                parameters,
                commandType: CommandType.StoredProcedure);

            var data = await result.ReadAsync<ProfilePaginationItem>();
            var totalCount = await result.ReadSingleAsync<long>();

            return new ProfilePaginationResult(
                Data: data,
                TotalCount: totalCount,
                Page: queryParameters.Page,
                PageSize: queryParameters.PageSize
            );
        }

        public Task<ProfileInfo?> GetProfileInfoById(long profileId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ProfileId", profileId, DbType.Int64);

            using var connection = _dapperDataContext.Connection;

            if (connection == null)
            {
                throw new Exception("No existe conexion con la base de datos");
            }

            using var result = connection.QueryMultiple(
                StoredProcedures.GetProfileInfoById,
                parameters,
                commandType: CommandType.StoredProcedure);

            var data = result.Read<ProfileInfoSp>().FirstOrDefault();

            if (data == null)
            {
                return Task.FromResult<ProfileInfo?>(null);
            }

            var profileInfo = new ProfileInfo(data);

            return Task.FromResult<ProfileInfo?>(profileInfo);
        }
    }
}
