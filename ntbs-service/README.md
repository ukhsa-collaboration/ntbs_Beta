# NTBS web server
The NTBS web server is the main interaction point with the NTBS system for most users. It provides an interface to create and update TB Notification records.

## Development setup (Z2H)
Make sure you have the following tools installed locally:
  * SQL Server 2016 (developer mode)
    * During installation make sure you install DB Engine.
    * Once installed, use your preferred connection method to create a `ntbsDev` database in the default instance.
  * [.NET cli tools](https://dotnet.microsoft.com/download)
  * IDE of choice (eg [Visual Studio Code](https://code.visualstudio.com/download) with [C# plugin](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp))

In this dirctory, run:
- `dotnet restore` and `npm install` to pull in dependencies
- `dotnet run` to launch the webserver locally
- `dotnet watch run` to launch the webserver locally with hot-reloading enabled

The frontend assets are compiled through Webpack, with hot-reload enabled automatically in development mode.

### Debugging
VS Code should pick up the debugging configuration automtically. Open the debugging panel and launch the debug configuration available. Note that this launches the web app itself, so there's no need to run `dotnet` commands directly - but it is *not* compatible with hot relaoding!

### Dev-mode secrets
Use [dotnet secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windowsgit) for configuring environment variables that should be kept secret (and therefore not checked into the repo) or specific to your machine:
`dotnet user-secrets set "<envVariable>" "<value>"`

For example, to override the main connection database string:
`dotnet user-secrets set "ConnectionStrings:ntbsContext" "my-alternative-connection-string"`

When first setting up, you'll need to set secrets for the following variables. You can find values for these in the project's shared password management:
* `ConnectionStrings:ets`
* `ConnectionStrings:ltbr`
* `ConnectionStrings:annualDataset`

## Testing
We are using the [xunit](https://xunit.net/) frameworkTo run unit tests. To run tests, cd into the ntbs-service-tests project and run "dotnet test".

## Database migrations
To minimize friction in development and deployments, the app uses Entity Framework migrations.
The simplest way to make a schema change is to make the appropriate changes in the model (e.g. [Notification](Models/Notification.cs) and the [database context](Models/NtbsContext.cs) and run
```dotnet ef migrations add <NameOfMigration>```
This will create a migration file, that can be edited further to match needs. It will be run at application startup, or can be envoked by hand using
```dotnet ef database update```

[See more information on migration](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)