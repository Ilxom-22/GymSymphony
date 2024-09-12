using Gymphony.Application.Common.Identity.Services;
using System.Text;

namespace Gymphony.Infrastructure.Common.Identity.Services;

public class PasswordGeneratorService : IPasswordGeneratorService
{
    private static readonly string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
    private static readonly string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static readonly string Digits = "0123456789";
    private static readonly string SpecialCharacters = "!@#$%^&*()_+[]{}|;:,.<>?";

    public string GeneratePassword(int length, bool includeUppercase = true, bool includeLowercase = true, bool includeDigits = true, bool includeSpecialCharacters = true)
    {
        if (length <= 0)
            throw new ArgumentException("Password length must be greater than 0");

        StringBuilder characterSet = new StringBuilder();
        if (includeLowercase) characterSet.Append(LowercaseLetters);
        if (includeUppercase) characterSet.Append(UppercaseLetters);
        if (includeDigits) characterSet.Append(Digits);
        if (includeSpecialCharacters) characterSet.Append(SpecialCharacters);

        if (characterSet.Length == 0)
            throw new InvalidOperationException("No character sets selected for password generation");

        return GenerateRandomPassword(characterSet.ToString(), length);
    }

    private string GenerateRandomPassword(string characterSet, int length)
    {
        StringBuilder password = new StringBuilder();
        Random random = new Random();

        for (int i = 0; i < length; i++)
        {
            int index = random.Next(0, characterSet.Length);
            password.Append(characterSet[index]);
        }

        return password.ToString();
    }
}
