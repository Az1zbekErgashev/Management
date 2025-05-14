# 📦 CRM Backend API

This is the backend part of the CRM system built with ASP.NET Core, using Entity Framework Core and JWT-based authentication.

---

## ⚙️ Setup Instructions

Follow the steps below to configure and run the project locally.

---

### 1. 🔧 Create Configuration Files

In the root of the project, create two files:

- `appsettings.json`
- `appsettings.Development.json`

Paste the following content and update it with your values:

```bash
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=your_db;Username=your_user;Password=your_password"
  },
  "JwtSettings": {
    "Secret": "your_super_secret_jwt_key",
    "Issuer": "YourApp",
    "Audience": "YourAppAudience",
    "ExpirationMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
2. 🔐 Configure JWT
Make sure you provide a secure and long secret key in JwtSettings:Secret. This key will be used to sign the JWT tokens.

3. 🗄 Configure the Database
Update the connection string in DefaultConnection to match your PostgreSQL (or other DB) settings.

Example for PostgreSQL:

"Host=localhost;Port=5432;Database=crm_db;Username=postgres;Password=your_password"

4. 🧱 Apply Migrations
Run the following command to apply EF Core migrations and create the database schema:

```bash
dotnet ef database update
```

If EF tools are not installed, add them with:

```bash 
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

🚀 Run the Project
You can run the application either via command line or Visual Studio.

Option 1: Using CLI
```bash
dotnet run
```

Option 2: Using Visual Studio
Open the .sln file in Visual Studio

Click Start or press F5

🔌 API URL
By default, the API will be available at:
```
https://localhost:7234/api/swagger
```
```
http://localhost:7235/api/swagger
```
🛠 Tech Stack
ASP.NET Core 7 / 8

Entity Framework Core

PostgreSQL

JWT Authentication

RESTful API

👨‍💻 Developer
Wisestone Dev Team
GitHub: https://github.com/wisestone-dev
