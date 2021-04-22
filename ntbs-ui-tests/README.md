# NTBS UI Tests

UI tests using Selenium for the ntbs-service project.

## First time setup

The following steps are PowerShell specific, but it should be straightforward to modify them for Bash:

1. Make sure you have a Java runtime (it doesn't need to be a JDK)
2. `cd` into the ntbs-ui-tests directory
3. Run `npm ci` to get the `selenium-standalone` dependency
4. Run `.\node_modules\.bin\selenium-standalone.ps1 install` to install `selenium-standalone`.
   This will also run every time you run `.\test.ps1` or `.\start_server.ps1`

### Dev-mode secrets

We use [dotnet secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windowsgit)
for configuring environment variables that should be kept secret (and therefore not checked into the repo).

A master copy of local secrets is stored in Azure Key Vault. These can be set up in bulk using the Azure CLI (in Powershell) as follows:

```PowerShell
# Use `az login` to authenticate first if necessary
# Use `az account set -s 6850ca99-e7c3-4686-9208-25575cef522a` to change to phe-ntbs azure subscription

az keyvault secret show --vault-name "dev-phe-ntbs-secrets" --name "UITestsSecrets" --query value `
 | ConvertFrom-Json `
 | dotnet user-secrets set
```

Secrets are project specific so make sure that you run these commands in the root of the UI test project.

## Running the tests

The tests require a Selenium server running in the background, accessible at `localhost:4444`.
This controls the (usually headless) web browser used to perform the tests.

To run the UI tests in PowerShell, run the script `.\test.ps1` This will setup and teardown a Selenium
server during each run.

Alternatively, start it running in the background and leave it running all day. It shouldn't use too
many resources when it is not being used, and it will allow you to run the tests from inside your IDE
(making use of a debugger, breakpoints, etc.) Run the script `.\start_selenium.ps1` to do this, stop
it with `Ctrl+C` when you are done.

### Environments

The tests require that the web app is already running in an environment.
Four environments are supported by default: `local`, `int`, `test` and `uat`.
You can switch between these environments using the `EnvironmentUnderTest` setting in [testSettings.json](testSettings.json).

The `int`, `test` and `uat` are hosted in Azure, and should always be running.

If you wish to run with the `local` configuration against your local copy of the code, then you first need to run the site locally.
Follow the instructions in the [ntbs-serice README](../ntbs-service/README.md) to do this (in short: run `dotnet run` in the project root).

## Troubleshooting

### Is there a way of seeing what Selenium "sees" during a test?
Yes! Change the `IsHeadless` setting to `false` in [TestSettings.cs](Hooks/TestConfig.cs) to see the browser Selenium
is using and watch it interact with it.

### I'm getting certificate warnings from Chrome and it is stopping the tests from reaching the website
To check if this is an issue for you, run the tests not in headless mode (see previous question). This
will manifest as all tests failing to find their first element in headless mode.

To fix this: trust your local IIS Express certificate. A guide to doing this can be found
[here](https://blogs.iis.net/robert_mcmurray/how-to-trust-the-iis-express-self-signed-certificate),
using resolution number 2.
