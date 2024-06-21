using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;

namespace Gymphony.Application.Common.Identity.Services;

public interface IVerificationTokenGeneratorService
{
    VerificationToken GenerateVerificationToken(User user, VerificationType type);
}