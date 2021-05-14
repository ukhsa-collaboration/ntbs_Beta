## What this application does

The `load-test-data-generation` console application is used to generate large numbers of notifications,
 primarily to populate our load test environment with a quantity of data which is representative of the live system.
However, it can be used to generate test data in any environment.

## Configuring the application

The application has the following config settings, which can be found in [testSettings.json](testSettings.json):
* `ConnectionString` - the connection string of the database to populate.
  By default this is a local database server, but you can use [dotnet user-secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-5.0&tabs=windows) to point at the load test database instead.
* `GenerateUsers` - a boolean. If `true`, running the application will create a user for every active TB Service.
* `NumberOfNotificationsToGenerate` - a nullable integer. This tells the application how many new notifications to create.
  Each notification will be given a random TB Service, so it is important to make sure that there is at least one user for each
  active TB Service before setting this value. (If in doubt, setting `GenerateUsers` to `true` will ensure this is the case).

## Running the application

To run the console app, either debug using Visual Studio, or build the project and run the executable directly.

Unfortunately, using `dotnet run` does not work at present, as the working directory is not the output directory,
meaning that files needed to build the database context are not in the expected location.
