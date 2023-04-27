# Leafboard Api

## Pre-installed Packages  
1. AspNetCore.OpenApi
2. Swashbuckle.ASPNetCore

## Installed Packages 
1. Microsoft.AspNetCore.Authentication.JwtBearer
2. Microsoft.EntityFrameworkCore
3. Microsoft.EntityFrameworkCore.Sqlite
4. Microsoft.EntityFrameworkCore.

## appsettings.json
'''json 
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "DataSource=app.db;Cache=Shared"
  },
  "EmailConfiguration": {
    "From": "youremail@gmail.com",
    "SmtpServer": "smtp.gmail.com",
    "Port": 587,
    "Username": "your@gmail.com",
    "Password": "yourpassword"
  },
  "JwtConfig": {
    "ValidAudience": "https://localhost:7282/",
    "ValidIssuer": "https://localhost:7282/",
    "Secret": "E7kNGoOiDm55KRBylFjdNteQQyNyHdgTjFDuQQNu"
  }
}
'''

## Here you can change your
1.Default Connection String 
2.Jwtconfig
3.EmailConfiguration
4. In `Models/Url.cs` change url