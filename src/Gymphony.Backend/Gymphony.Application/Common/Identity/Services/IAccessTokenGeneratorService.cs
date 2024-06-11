using Gymphony.Domain.Entities;

namespace Gymphony.Application.Common.Identity.Services;

public interface IAccessTokenGeneratorService
{
    AccessToken GetAccessToken(User user);
}