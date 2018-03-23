# Setting Up the DevChatterBot
More details coming.

## Configuration
There are some configuration options you'll need to set up using information you set up in Twitch accounts. Read the [Configuration](Configuration.md) section for more info.

## Building Local Database
We are using SQL LocalDb for the database for the project, which means you'll need to check the connection string in the `appsettings.json` file and confirm that it will work on your machine.

[Read about EF Core Migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)

### Be sure to install the EF Core Tools
Using NuGet's Package Manager Console, run the following command:

#### PowerShell
```
Install-Package Microsoft.EntityFrameworkCore.Tools
```

If everything worked correctly, you should be able to run this command:

#### PowerShell
```
Get-Help about_EntityFrameworkCore
```

### Applying the latest schema changes to the datbase
To apply the migration to the database and create the schema, do the following:

#### PowerShell
```
Update-Database
```

#### Console
```
dotnet ef database update
```

### Tips

 - You can check in the SQL Server Object Explorer in Visual Studio to see your localdb.
 - The migration will pull the connection string from the `appsettings.json` file that gets copied on build.
 - I've found that the best place to run these commands is in the Package Manager Console. Find it in Visual Studio udner `Tools` -> `NuGet Package Manager` -> `Package Manager Console`.
