using Gymphony.Domain.Entities;

namespace Gymphony.Application.Common.Identity.Services;

public interface ITokenGeneratorService
{
    string GenerateToken();
}