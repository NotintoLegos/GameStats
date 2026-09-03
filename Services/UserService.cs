using System.Security.Cryptography;

public class UserService
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 600_000;

    private readonly List<UserProfile> _users = new();
    public UserProfile CreateUser(string id, string name, string email, string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        var user = new UserProfile
        {
            UniqueID = Guid.NewGuid().ToString(),
            UserName = name,
            Email = email,
            PasswordHash = HashPassword(password)
        };

        _users.Add(user);
        return user;
    }

    public UserProfile? GetUserByID(string id)
    {
        return _users.FirstOrDefault(u => u.UniqueID == id);
    }

    public bool VerifyPassword(string id, string password)
    {
        var user = GetUserByID(id);
        return user is not null && VerifyPasswordHash(password, user.PasswordHash);
    }

    private static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSize);

        return $"PBKDF2-SHA256${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    private static bool VerifyPasswordHash(string password, string storedHash)
    {
        var parts = storedHash.Split('$');
        if (parts.Length != 4 || parts[0] != "PBKDF2-SHA256" || !int.TryParse(parts[1], out var iterations))
        {
            return false;
        }

        try
        {
            var salt = Convert.FromBase64String(parts[2]);
            var expectedHash = Convert.FromBase64String(parts[3]);
            var actualHash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                expectedHash.Length);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}