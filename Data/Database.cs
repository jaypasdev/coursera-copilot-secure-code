using Microsoft.Data.SqlClient;
using SafeVault.Models;

namespace SafeVault.Data
{
    public class Database : IDatabase
    {
        private readonly string _connectionString;

        public Database(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool AddUser(string username, string email)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Users (Username, Email) VALUES (@Username, @Email)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Email", email);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
                return false;
            }
        }

        public User GetUserById(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT UserID, Username, Email FROM Users WHERE UserID = @UserID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    UserID = reader.GetInt32(0),
                                    Username = reader.GetString(1),
                                    Email = reader.GetString(2)
                                };
                            }
                            return null; // No user found
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user by ID: {ex.Message}");
                return null;
            }
        }

        public User GetUserByEmail(string email)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT UserID, Username, Email FROM Users WHERE Email = @Email";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    UserID = reader.GetInt32(0),
                                    Username = reader.GetString(1),
                                    Email = reader.GetString(2)
                                };
                            }
                            return null; // No user found
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user by email: {ex.Message}");
                return null;
            }
        }
    }

}
