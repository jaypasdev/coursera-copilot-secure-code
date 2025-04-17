using SafeVault.Models;

public interface IDatabase
{
    bool AddUser(string username, string email);
    User GetUserById(int userId);
    User GetUserByEmail(string email);
}
