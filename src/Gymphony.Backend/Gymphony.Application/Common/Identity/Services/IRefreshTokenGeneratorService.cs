using Gymphony.Domain.Entities;

namespace Gymphony.Application.Common.Identity.Services;

public interface IRefreshTokenGeneratorService
{
    RefreshToken GenerateRefreshToken(User user);
}