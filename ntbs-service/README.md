# NTBS web server
The NTBS web server is the main interaction point with the NTBS system for most users. It provides an interface to create and update TB Notification records.

## Development setup (Z2H)
Make sure you have the following tools isntalled locally:
  * SQL Server 2016 (developer mode)
    * During installation make sure you install DB Engine.
    * Once installed, use your preferred connection method to create a `ntbsDev` databse in the default instance.
  * [.NET cli tools](https://dotnet.microsoft.com/download)
  * IDE of choice (eg [Visual Studio Code](https://code.visualstudio.com/download) with [C# plugin](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp))

In this dirctory, run:
- `dotnet restore` to pull in dependencies
- `dotnet run` to launch the webserver locally
- `dotnet watch run` to launch the webserver locally with hot-reloading enabled

The frontend assets are compiled through Webpack, with hot-reload enabled automatically in development mode.

### Debugging
VS Code should pick up the debugging configuration automtically. Open the debugging panel and launch the debug configuration available. Note that this launches the web app itself, so there's no need to run `dotnet` commands directly - but it is *not* compatible with hot relaoding!

## Testing
We are using the [xunit](https://xunit.net/) frameworkTo run unit tests. To run tests, cd into the ntbs-service-tests project and run "dotnet test".