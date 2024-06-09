using Gymphony.Domain.Entities;

namespace Gymphony.Application.Common.Identity.Services;

public interface IAccessTokenGeneratorService
{
    string GetToken(User user);
}