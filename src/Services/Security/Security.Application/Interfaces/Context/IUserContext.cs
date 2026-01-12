using Common.Interfaces.Dtos;
using Security.Application.Dtos.General;

namespace Security.Application.Interfaces.Context;

public interface IUserContext: IUserContextDto
{
    public UserContextDto ToDto();

}