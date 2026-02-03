using Security.Application.Dtos.Results;
using Security.Domain.Parameters.Profile;

namespace Security.Application.Interfaces.Persistance;

public interface IProfileRepository
{
    public Task<ProfilePaginationResult> GetProfilesWithFiltersAndPagination(ProfilePaginationParameter queryParameters);
    public Task<ProfileInfo?> GetProfileInfoById(long id);
}