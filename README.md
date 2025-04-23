# Security and authentication showcase project for Coursera
Capstone project of module 4 from the Security and Authentication course from Microsoft

## Features
- Go to  `http://localhost:<port>/Home/WebForm` to have a login form. The backend enables login validating against SQL injections and XSS (Activity: writing secure code using copilot)
- Use requests.http to test User and Admin registration and login returning JWT tokens
- Use requests.http to test the protected endpoints using Bearer tokens coming from the above logins

## Requirements
- .NET 9.0 or later
- Visual Studio Code or any compatible code editor

## Usage
1. Clone the repository.
2. Run the application using `dotnet run .\SecurityAndAuthentication.sln`.
3. Run the unit tests using `dotnet test .\SecurityAndAuthentication.sln`.

## Vulnerabilities identified by copilot during development
1. Input Validation missing: Validate email and password inputs against  SQL injection, XSS.
2. Password Security: Enforce password complexity rules.
3. Role Management: Avoid default roles and require explicit role assignments.
4. JWT Security: Use a strong secret key and validate token expiration.
5. Access Control: Add explicit role checks for sensitive endpoints.
7. Logging: Log failed login attempts for monitoring.
8. Token Lifetime: Reduce token lifetime and implement refresh tokens.
9. HTTPS: Enforce HTTPS for secure communication.
10. Use LINQ instead of prepared statements
