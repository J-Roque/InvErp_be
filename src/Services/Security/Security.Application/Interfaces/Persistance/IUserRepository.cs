
using Security.Application.Dtos.Results;
using Security.Domain.Parameters.User;

namespace Security.Application.Interfaces.Persistance
{
    public interface IUserRepository
    {
        public Task<UserPaginationResult> GetUsersWithFiltersAndPagination(UserPaginationParameter queryParameters);
        public Task<UserInfo?> GetUserInfoById(long userId);
    }
}
