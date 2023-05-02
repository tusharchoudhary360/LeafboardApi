# Leafboard Api
## Tools 
1. Visual Studio
2. EntityFramework
3. Jwt token
4. Smtp-client 
5. SQLite as Database

```Without Identity and ``` 

## Pre-installed Packages  
1. AspNetCore.OpenApi
2. Swashbuckle.ASPNetCore

## Installed Packages 
1. Microsoft.AspNetCore.Authentication.JwtBearer
2. Microsoft.EntityFrameworkCore
3. Microsoft.EntityFrameworkCore.Sqlite
4. Microsoft.EntityFrameworkCore.

## appsettings.json
```json 
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
```

## Changes (Optional)
1. Default Connection String in `appsettings.json` 
2. Jwtconfig in `appsettings.json`
3.  In `Models/Url.cs` change url

## Changes (Required)
1. EmailConfiguration in `appsettings.json`
2. Add `Uploads` folder in your project
3. following Commands 
```
Add-migration migration_name
update-database
```


