using System.Text.RegularExpressions;

namespace SafeVault.Models
{
    public static class InputValidation
    {
        public static bool IsValid(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            // Check for unsafe characters (SQL injection or XSS)
            string unsafePattern = "[';<>]|--"; // Disallow unsafe characters and SQL injection patterns
            if (Regex.IsMatch(input, unsafePattern))
            {
                return false;
            }

            // Input is valid
            return true;
        }
    }
}
