namespace FinanceTrackerDotNET.Services;

using FinanceTrackerDotNET.Models;
using System.Text.Json;

public static class UsersService
{
    #region User Data Management
    private static void SaveAll(List<User> users)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string appUsersFilePath = Utils.GetAppUsersFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(users);
        File.WriteAllText(appUsersFilePath, json);
    }

    public static List<User> GetAll()
    {
        string appUsersFilePath = Utils.GetAppUsersFilePath();
        if (!File.Exists(appUsersFilePath))
        {
            return new List<User>();
        }

        var json = File.ReadAllText(appUsersFilePath);
        return JsonSerializer.Deserialize<List<User>>(json);
    }
    #endregion

    #region User Operations
    public static List<User> Create(Guid userId, string username, string password, string currency)
    {
        List<User> users = GetAll();
        bool usernameExists = users.Any(x => x.UserName == username);

        if (usernameExists)
        {
            throw new Exception("Username already exists.");
        }

        users.Add(
            new User
            {
                UserName = username,
                userPasswordHash = Utils.HashSecret(password),
                preCurrency = currency
            }
        );
        SaveAll(users);
        return users;
    }

    public static User GetById(Guid id)
    {
        List<User> users = GetAll();
        return users.FirstOrDefault(x => x.Id == id);
    }

    public static User Login(string username, string password)
    {
        const string loginErrorMessage = "Invalid username or password.";
        List<User> users = GetAll();
        User user = users.FirstOrDefault(x => x.UserName == username);

        if (user == null)
        {
            throw new Exception(loginErrorMessage);
        }

        bool passwordIsValid = Utils.VerifyHash(password, user.userPasswordHash);

        if (!passwordIsValid)
        {
            throw new Exception(loginErrorMessage);
        }

        return user;
    }
    #endregion
}