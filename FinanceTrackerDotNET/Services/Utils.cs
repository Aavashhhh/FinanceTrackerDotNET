namespace FinanceTrackerDotNET.Services;

using System.Security.Cryptography;

public static class Utils
{
    private const char SegmentDelimiter = ':';

    #region Hashing and Verification
    public static string HashSecret(string input)
    {
        int saltSize = 16;
        int iterations = 100_000;
        int keySize = 32;
        HashAlgorithmName algorithm = HashAlgorithmName.SHA256;
        byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, algorithm, keySize);

        return string.Join(
            SegmentDelimiter,
            Convert.ToHexString(hash),
            Convert.ToHexString(salt),
            iterations,
            algorithm
        );
    }

    public static bool VerifyHash(string input, string hashString)
    {
        string[] segments = hashString.Split(SegmentDelimiter);
        byte[] hash = Convert.FromHexString(segments[0]);
        byte[] salt = Convert.FromHexString(segments[1]);
        int iterations = int.Parse(segments[2]);
        HashAlgorithmName algorithm = new(segments[3]);
        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
            input,
            salt,
            iterations,
            algorithm,
            hash.Length
        );

        return CryptographicOperations.FixedTimeEquals(inputHash, hash);
    }
    #endregion

    #region File Path Management
    public static string GetAppDirectoryPath()
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "FinanceTrackerDOTNET"
        );
    }

    public static string GetAppUsersFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(), "users.json");
    }

    public static string GetTodosFilePath(Guid userId)
    {
        return Path.Combine(GetAppDirectoryPath(), $"{userId}_todos.json");
    }

    public static string GetExpenseFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(), "expense.json");
    }

    public static string GetIncomeFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(), "income.json");
    }

    public static string GetCategoryFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(), "categories.json");
    }

    public static string GetDebtFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(), "debt.json");
    }
    #endregion
}