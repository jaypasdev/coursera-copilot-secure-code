using System.Text.RegularExpressions;

namespace SafeVault.Models
{
    public static class InputValidation
    {
        public static bool IsValid(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            // Check for SQL injection patterns
            string[] sqlInjectionPatterns = { "'", "--", ";", "/*", "*/", "xp_" };
            if (sqlInjectionPatterns.Any(pattern => input.Contains(pattern))) return false;

            // Check for XSS patterns
            string[] xssPatterns = { "<script>", "</script>", "javascript:" };
            if (xssPatterns.Any(pattern => input.Contains(pattern, StringComparison.OrdinalIgnoreCase))) return false;

            return true;
        }
    }
}
