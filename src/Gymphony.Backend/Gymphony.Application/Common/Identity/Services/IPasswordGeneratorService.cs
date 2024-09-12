namespace Gymphony.Application.Common.Identity.Services;

public interface IPasswordGeneratorService
{
    string GeneratePassword(int length, bool includeUppercase = true, bool includeLowercase = true, bool includeDigits = true, bool includeSpecialCharacters = true);
}
