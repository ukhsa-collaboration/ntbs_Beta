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

## Running the tests

The tests require a Selenium server running in the background, accessible at `localhost:4444`.
This controls the (usually headless) web browser used to perform the tests.

To run the tests against an environment where two factor authentication is required, you must bypass the usual log in journey (this is because there is no way to log in using two
factor authentication within the tests). To do this you will need to:

- Log in to the site under test using the user account which you want to use in the tests. (You will probably want to use a user with National Team permissions).
- Open your browser's dev tools, and load any page on the site.
- In the network tab, find the request you just made, and copy the value of the "cookie" header on the request.
- Update the following config values:
  - The `AuthenticatedCookieHeader`to the value you copied from the "cookie" header.
  - The `Username` of the `ManuallyLoggedInUser`. This is the username of the user you used to log in above.
- Run the UI tests by running the script `.\test.ps1` in PowerShell.

There are no longer any environments where two factor authentication is not required, but if there are in future, you can run the UI tests with a full login journey by running the script `.\testWithFullLogin.ps1` instead of `.\test.ps1`.
In this scenario, you do not need to any of the other steps above.

### Environments

The tests require that the web app is already running in an environment.
Two environments are supported by default: `local` and `phe-uat`.
You can switch between these environments using the `EnvironmentUnderTest` setting in [testSettings.json](testSettings.json).

The `phe-uat` environment is hosted in the UKHSA OpenShift cluster, and is always running, though it is only accessible from machines on the UKHSA VPN (such as the dev server).

If you wish to run with the `local` configuration against your local copy of the code, then you first need to run the site locally.
Follow the instructions in the [ntbs-service README](../ntbs-service/README.md) to do this (in short: run `dotnet run` in the project root).

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

### I'm getting an error where Chrome driver is at a different version than my Chrome browser
This issue *should not* affect the UI tests running from the github action - this issue was fixed in NTBS-2382.

Locally this is still a possibility. To fix this:
- Go to `About Chrome` in your Chrome's browser settings and then copy the version number. For example let's say your browser is version `98.0.4758.102`
- Add `--drivers.chrome.version=98.0.4758.102` to **both** lines in the [start_selenium](start_selenium.ps1) file.