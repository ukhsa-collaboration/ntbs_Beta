# NTBS web server

The NTBS web server is the main interaction point with the NTBS system for most users. It provides an interface to create and update TB Notification records.

## Development setup (Z2H)

Make sure you have the following tools installed locally:

- SQL Server 2016 (developer mode)
  - During installation make sure you install DB Engine.
  - Once installed, use your preferred connection method to create a `ntbsDev` database in the default instance.
- [.NET cli tools](https://dotnet.microsoft.com/download) - the codebase should be compatible with the newest SDK tools
  - once `dotnet` is installed, run `dotnet tool install --global dotnet-ef` to install the `dotnet ef` tools package
- IDE of choice (eg [Visual Studio Code](https://code.visualstudio.com/download) with [C# plugin](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp))
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/?view=azure-cli-latest)

In this directory, run:

- `git submodule update --init --recursive` to recursively initialise and update submodules, if this was not done as part of the cloning process
- `dotnet restore` and `npm install` to pull in dependencies
- `dotnet run` to launch the webserver locally
- `dotnet watch run` to launch the webserver locally with hot-reloading enabled

The frontend assets are compiled through Webpack, with hot-reload enabled automatically in development mode.

### Debugging

VS Code should pick up the debugging configuration automatically. Open the debugging panel and launch the debug configuration available. Note that this launches the web app itself, so there's no need to run `dotnet` commands directly - but it is _not_ compatible with hot reloading!

### Dev resources
#### Frontend guides and libraries
We are following the [NHS Digital Service Manual](https://beta.nhs.uk/service-manual/) as the baseline for the project's styling and frontend, supplemented by [GOV.UK Service Manual](). We are making using of the NHS [implementation of the components in Razor](https://github.com/nhsuk/frontend-dotnetcore/) (note, that the useful bits of documentation can be found on READMEs of individual directories, e.g. [layout](https://github.com/nhsuk/frontend-dotnetcore/tree/master/src/NHSUKFrontEndLibraryTagHelpers/NHSUK.FrontEndLibrary.TagHelpers/Tags/Layout)).

### Dev-mode secrets

Use [dotnet secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windowsgit) for configuring environment variables that should be kept secret (and therefore not checked into the repo) or specific to your machine:
`dotnet user-secrets set "<envVariable>" "<value>"`

For example, to override the main connection database string:
`dotnet user-secrets set "ConnectionStrings:ntbsContext" "my-alternative-connection-string"`

A master copy of local secrets is stored in Azure Key Vault. These can be set up in bulk using the Azure CLI:

```PowerShell
# Use `az login` to authenticate first if necessary

az keyvault secret show `
 --vault-name "dev-phe-ntbs-secrets" `
 --name "LocalUserSecrets" `
 --query value `
 | ConvertFrom-Json `
 | dotnet user-secrets set
```

Secrets are project specific so run these commands in the root of a project.

## Testing

We are using the [xunit](https://xunit.net/) framework for testing.
To run unit/integration tests, cd into the ntbs-service-unit-tests/ntbs-integration-tests project and run `dotnet test`.

Before first running ui tests locally (the below steps are powershell specific, trivial changes are required for bash):

 - Make sure you have a local install of java (doesn't have to be jdk) 
 - cd into the ntbs-ui-tests directory 
 - run `npm ci` to get the   selenium-standalone dependency 
 - run   `.\node_modules\.bin\selenium-standalone install` to install  selenium-standalone - Flip slashes for bash

To run ui tests, run the script `.\test.ps1` - If you don't have bash configured to be able to run powershell, `./test.sh` is a good alternative.
To see the browser window when running them change the setting in Hooks\TestSettings.cs

### Accessing app logs whilst under integration tests
The integration tests by default don't collect logs from the system under test (ie the whole app).
However, when Debugging, it will add the logs to the Debug console.
You can also make the tests log to a file by un-commenting the appropriate lines in #
[NtbsWebApplicationFactory](/ntbs-integration-tests/NtbsWebApplicationFactory.cs).

## Database migrations

To minimize friction in development and deployments, the app uses Entity Framework migrations.
The simplest way to make a schema change is to make the appropriate changes in the model (e.g. [Notification](Models/Entities/Notification.cs) and the [database context](DataAccess/NtbsContext.cs) and run
`dotnet ef migrations add <NameOfMigration>`
This will create a migration file, that can be edited further to match needs. It will be run at application startup, 
or can be evoked by hand using
`dotnet ef database update`

[See more information on migration](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)

# Jobs
The project includes Hangfire for managing jobs, with a console available to admin users at `/Hangfire`. It's
useful for monitoring and/or triggering:
- user sync
- migration (can't trigger from the hangfire console, follow `/Migration` instead)

# Deployments
The deployments are based on Docker-on-Kubernetes infrastructure.
The development envs are available on Azure, with production environments hosted on PHE's Kubernetes instance.

## First time set up
Ensure you have `azure-cli` installed
- `az login` to log into the azure subscription
- use `az account set -s <Name or Id>` to set a default subscription if there are multiple
- `az aks install-cli` to install `kubectl`
- `az aks get-credentials -g phe-ntbs -n ntbs-envs` to add appropriate credentials to your `~/.kube/config` file

## Images registry
We're using ACR to store docker images. When logged in to Azure, run this command to see the username-password for registry user.
`az acr credential show -n ntbsContainerRegistry`

## Deploying to environments
`master` branch deploys to `int` automatically.

For `test` and `uat` environments, pick the docker image <TAG> of the build from registry and from root directory run:

`.\scripts\release.sh <ENV> <TAG>`

For `live` environment, the process is the same once
[you've connected to Openshift successfully](https://airelogic-nis.atlassian.net/wiki/spaces/R2/pages/163446793/Deployments+on+PHE+infrastructure)

Note - when using two kubectl pods 
[these commands](https://kubernetes.io/docs/reference/kubectl/cheatsheet/#kubectl-context-and-configuration)
will help to keep track of the kubectl contexts


## Running the app in Docker (builds in production mode)
```
docker build -t ntbs .
docker run -it --rm -p 8000:80 --name ntbs ntbs
```
Publishing current image
```
docker build -t ntbs .
docker tag ntbs ntbscontainerregistry.azurecr.io/ntbs-service
docker push ntbscontainerregistry.azurecr.io/ntbs-service
```

## Maintenance
- See current deployments, including build versions:
  - `kubectl get deployment -o wide` - all envs
  - `kubectl get deployment ntbs-<env> -o wide` - specific <env>
- logs: `kubectl logs deployment/ntbs-<env>` - specific <env>
- dashboard - UI access to env health, logs, etc : `az aks browse --resource-group PHE-NTBS --name ntbs-envs`
- adding kubernetes secrets: `kubectl create secret generic <secret> --from-literal=<key>=<value>`
- purging registry - every once in a while the images registry [should be purged](../scripts/purge-images.ps1) so it
 doesn't grow too big. It runs with `--dry-run` by default, 
 make sure to remove the flag once you're happy nothing relevant will be deleted.
- SSL certificates - see [dedicated readme](./deployments/README.md)
