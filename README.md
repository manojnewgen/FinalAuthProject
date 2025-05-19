# FinalAuthProject

A secure ASP.NET Core project implementing authentication and authorization using JWT tokens, role-based access control, and user management with ASP.NET Identity.

## Introduction

Follow this step-by-step guide to use Microsoft Copilot to set up secure authentication and authorization for your application.

---

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0 or later)
- SQL Server
- Visual Studio or any code editor of your choice

## Project Setup

### 1. Clone the Repository

```bash
git clone <repository-url>
cd FinalAuthProject
```

### 2. Install Required NuGet Packages

Run the following commands to install the necessary NuGet packages:

```bash
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Json
dotnet add package Microsoft.Extensions.Logging
dotnet add package Swashbuckle.AspNetCore
```

### 3. Configure the Database

Update the `appsettings.json` file with your SQL Server connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=FinalAuthProjectDb;Trusted_Connection=True;MultipleActiveResultSets=true"
},
"Jwt": {
  "Secret": "YourSecretKeyHere",
  "Issuer": "YourIssuer",
  "Audience": "YourAudience",
  "AccessTokenExpiryMinutes": "15"
}
```

### 4. Apply Migrations

Run the following commands to create the database and apply migrations:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. Seed Roles and Admin User

The application seeds roles (`Admin`, `User`, `Guest`) and an admin user during startup. Ensure the `Program.cs` file includes the seeding logic.

### 6. Run the Application

Start the application using the following command:

```bash
dotnet run
```

The application will be available at `https://localhost:5001`.

---

## How to Implement Authentication

### 1. Generate Authentication Code

**Step**: In your code editor, type a prompt like:

> Generate a login form with user authentication in ASP.NET.

---

### 2. Add User Registration

**Step**: Use a prompt like:

> Create a user registration function with password hashing.

**Tip**: Ensure Copilot includes hashing techniques (like bcrypt) for password security.

---

### 3. Integrate ASP.NET Identity

**Step**: Type a prompt such as:

> Scaffold ASP.NET Identity for user management.

---

### 4. Set Up Token-Based Authentication

**Step**: Prompt Copilot with:

> Generate code for issuing and validating JWT tokens in ASP.NET Core.

**Tip**: Use JWT for secure API communication.

---

## How to Implement Authorization

### 1. Define User Roles

**Step**: Use a prompt like:

> Create roles for Admin, User, and Guest in the application.

**Tip**: Assign roles that match your application’s requirements.

---

### 2. Configure Role-Based Access Control (RBAC)

**Step**: Prompt Copilot with:

> Write authorization rules for different user roles in ASP.NET Core.

**Example**: Restrict access to admin features based on roles.

---

### 3. Apply Authorization Policies

**Step**: Type a prompt like:

> Add authorization policies to secure specific API endpoints.

---

## API Endpoints

### Authentication

- **Login**: `POST /api/Auth/login`
  - Request Body:
    ```json
    {
      "username": "admin",
      "password": "Admin@123"
    }
    ```
  - Response:
    ```json
    {
      "token": "jwt-token",
      "expiration": "2023-01-01T00:00:00Z"
    }
    ```

- **Register**: `POST /api/Auth/register`
  - Request Body:
    ```json
    {
      "username": "newuser",
      "email": "newuser@example.com",
      "password": "Password@123",
      "firstName": "New",
      "lastName": "User"
    }
    ```

### Role-Based Access

- **Admin Endpoint**: `GET /api/sample/admin` (Requires `Admin` role)
- **User Endpoint**: `GET /api/sample/user` (Requires `User` role)
- **Guest Endpoint**: `GET /api/sample/guest` (Requires `Guest` role)

---

## How to Test and Debug

### 1. Test Authentication

**Step**: Prompt Copilot with:

> Write test cases for user login and registration.

**Tip**: Ensure tests cover valid and invalid user inputs.

---

### 2. Check Authorization Rules

**Step**: Use a prompt like:

> Create tests for verifying role-based access to endpoints.

---

### 3. Debug Security Issues

**Step**: Type a prompt such as:

> Identify and fix security vulnerabilities in authentication and authorization code.

**Tip**: Copilot may suggest improvements or highlight potential issues.

---

## Best Practices for Secure User Access

### 1. Hash and Salt Passwords

**Step**: Use a prompt like:

> Implement secure password hashing and salting in ASP.NET.

---

### 2. Enforce HTTPS

**Step**: Type:

> Ensure the application uses HTTPS for all communications.

---

### 3. Set JWT Expiry Times

**Step**: Use a prompt like:

> Configure short-lived JWT tokens with refresh token support.

---

### 4. Add Input Validation

**Step**: Type:

> Add input validation to prevent injection attacks.

---

### 5. Log Access Events

**Step**: Prompt Copilot with:

> Add logging for user login and access events.

---

## Swagger Documentation

Swagger is enabled for API documentation. Access it at `https://localhost:5001/swagger`.

## HTTPS Enforcement

The application enforces HTTPS for all communications. Ensure you access the application over `https://`.

## Refresh Token Support

The application supports short-lived JWT tokens with refresh token functionality. Use the `/refresh-token` endpoint to renew access tokens.

## Logging

The application logs user login and registration events for monitoring and debugging purposes.

## Testing

### Run Unit Tests

To run the unit tests, execute:

```bash
dotnet test
```

## Additional Information

Refer to the API endpoints, Swagger documentation, and testing instructions in the sections above for more details on using the application.

## Contributing

Feel free to submit issues or pull requests to improve the project.

## License

This project is licensed under the MIT License.
