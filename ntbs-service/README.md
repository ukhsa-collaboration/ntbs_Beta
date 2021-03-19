# NTBS web server

The NTBS web server is the main interaction point with the NTBS system for most users. It provides an interface to create and update TB Notification records.

## Development setup (Z2H)

Make sure you have the following tools installed locally:

- SQL Server 2016 (developer mode)
  - During installation make sure you install DB Engine.
  - Once installed, use your preferred connection method to create a `ntbsDev` database in the default instance.
- [.NET cli tools](https://dotnet.microsoft.com/download) - the codebase should be compatible with the newest SDK tools
- [.NET SDK 5.0](https://dotnet.microsoft.com/download/dotnet/5.0)
  - once `dotnet` is installed, run `dotnet tool install --global dotnet-ef` to install the `dotnet ef` tools package
- IDE of choice (eg. [Rider](https://www.jetbrains.com/rider/) or [Visual Studio Code](https://code.visualstudio.com/download) with [C# plugin](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp))
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/?view=azure-cli-latest)

In this directory, run:

- `git submodule update --init --recursive` to recursively initialise and update submodules, if this was not done as part of the cloning process
- `dotnet restore` and `npm install` to pull in dependencies
- `npm run build` to compile frontend assets through webpack
- `dotnet run` to launch the webserver locally
- `dotnet watch run` to launch the webserver locally with hot-reloading of non-webpack-managed changes enabled

Make sure to get the dev secrets from Azure (see [dev mode secrets](#dev-mode-secrets)) to connect to Azure dbs.

Hot module reloading was supported until the .NET 5 upgrade (NTBS-2074, #1022) but was removed as part of NTBS-1768.
This was because in .NET 5 `UseWebpackDevMiddleware` was removed, see [here](https://github.com/dotnet/AspNetCore/issues/12890)
for a discussion of this change.

### Debugging

VS Code should pick up the debugging configuration automatically. Open the debugging panel and launch the debug configuration available. Note that this launches the web app itself, so there's no need to run `dotnet` commands directly - but it is _not_ compatible with hot reloading!

### Dev resources
#### Frontend guides and libraries
We are following the [NHS Digital Service Manual](https://beta.nhs.uk/service-manual/) as the baseline for the project's styling and frontend, supplemented by [GOV.UK Service Manual](). We are making using of the NHS [implementation of the components in Razor](https://github.com/nhsuk/frontend-dotnetcore/) (note, that the useful bits of documentation can be found on READMEs of individual directories, e.g. [layout](https://github.com/nhsuk/frontend-dotnetcore/tree/master/src/NHSUKFrontEndLibraryTagHelpers/NHSUK.FrontEndLibrary.TagHelpers/Tags/Layout)).

### Dev-mode secrets

Use [dotnet secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windowsgit)
for configuring environment variables that should be kept secret (and therefore not checked into the repo) or specific to your machine:
`dotnet user-secrets set "<envVariable>" "<value>"`

For example, to override the main connection database string:
`dotnet user-secrets set "ConnectionStrings:ntbsContext" "my-alternative-connection-string"`

A master copy of local secrets is stored in Azure Key Vault. These can be set up in bulk using the Azure CLI.

*Running this will be necessary to connect the local app to the azure data migration db.*

```PowerShell
# Use `az login` to authenticate first if necessary
# Use `az account set -s 6850ca99-e7c3-4686-9208-25575cef522a` to change to phe-ntbs azure subscription

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

See [ntbs-ui-tests/README.md](../ntbs-ui-tests/README.md) for information on the UI tests.

### Accessing app logs whilst under integration tests
The integration tests by default don't collect logs from the system under test (ie the whole app).
However, when Debugging, it will add the logs to the Debug console.
You can also make the tests log to a file by un-commenting the appropriate lines in #
[NtbsWebApplicationFactory](/ntbs-integration-tests/NtbsWebApplicationFactory.cs).

## Database migrations

See [the dedicated migrations readme.](Migrations/README.md) for information on migrations.

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
- `az aks get-credentials -g NTBS_Development -n ntbs-envs` to add appropriate credentials to your `~/.kube/config` file

## Images registry
We're using ACR to store docker images. When logged in to Azure, run this command to see the username-password for registry user.
`az acr credential show -n ntbsContainerRegistry`
Login into docker with above credentials
` docker login -u [Username] -p [Password] [SERVER]`

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

## Azure Active Directory Setup
The system can be switched to use Microsoft Azure Active Directory for authentication and authorisation.
To switch on set the AzureADOptions.Enabled app setting to "true".

### Azure AD Application Configuration
NTBS will require an Azure AD Application to be configured for Azure AD Auth.
The configuration of the Azure AD Application should be as follows:

Environment Name:
This will be used in the name of the application and the application id uri.
For non-prod environments it should be used so dev,int,test etc.
Live it should be left out.

- Name:
  - NTBS Login ([Environment Name])
  - e.g. Environment Name = Dev/Int/Test if Live then leave blank.
- Application ID Uri:
  - https://[domain.com]/ntbs-[environmentname]
  - e.g. https://aptemus.com/ntbs-int or https://phe.gov.uk/ntbs (for live)

Authentication:
- Redirect URIs: https://[homepage]/Index e.g. https://ntbs-int.61b431d7ea3041e89733.uksouth.aksapp.io//Index
- Implicit Grant: Enable ID token
- Supported account type: Accounts in this organisation directory only (Single Tenant)
- Allow Public Flows: false

Certificates and Secrets
- Create a Client Secret for two years

Token Configuration
- Add Groups Claim
- Choose Security and Groups assigned to application
- Tokens, ID and Access Token choose to use the Group Id, Emit Group as Role Claims

Api Permissions
- Microsoft Graph
- Application permission - Group.Read.All and User.Read.All and GroupMember.Read.All (Enable Admin consent)
- Delegate permission - profile and User.Read

The User.Read.All and GroupMember.Read.All are require by the user sync job for Azure AD.

### Permissions
The Azure AD Application requires the following permissions:
- Groups.Read.All (Application Permission)
- profile (Delegated Permission)
- User.Read (Delegated Permission)

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
