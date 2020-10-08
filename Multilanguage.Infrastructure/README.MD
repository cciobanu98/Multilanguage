
## Add migrations
To add new migration run folllowing command in Visual Studio Package Manager Console
```powershell
Add-Migration MY_MIGRATION_NAME -StartupProject Multilanguage.Infrastructure -Project Multilanguage.Infrastructure -OutputDir Data\Migrations
```

To add migration from command line run following command from root directory of repository:
```bash
dotnet ef migrations add MY_MIGRATION_NAME --startup-project ./src/Multilanguage.Infrastructure/ --output-dir Data/Migrations --project ./src/Multilanguage.Infrastructure/
```

### Remove migrations
Run folllowing commands in Visual Studio Package Manager Console
```
update-database MIGRATION_BEFORE_MY_MIGRATION_NAME -s Multilanguage.Infrastructure -p Multilanguage.Infrastructure
remove-migration
```

### Update Database
Run `update-database -s Multilanguage.Infrastructure -p Multilanguage.Infrastructure` in Visual Studio Package Manager Console to create Database and apply migrations

Read more on [Entity Framework Core tools reference - Package Manager Console in Visual Studio](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/powershell) 
and [Entity Framework Core tools reference - .NET CLI](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet).

> Connection string is stored in *appsettings.json* file of Multilanguage project. If you need to change connection string DO NOT change it in appsettings.json file. Instead change it in [user secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windows#enable-secret-storage) file.