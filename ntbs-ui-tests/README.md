# NTBS UI Tests

UI tests using Selenium for the ntbs-service project.

## First time setup

The following steps are PowerShell specific, but it should be straightforward to modify them for Bash:

1. Make sure you have a Java runtime (it doesn't need to be a JDK)
2. `cd` into the ntbs-ui-tests directory
3. Run `npm ci` to get the `selenium-standalone` dependency
4. Run `.\node_modules\.bin\selenium-standalone.ps1 install` to install `selenium-standalone`.
   This will also run every time you run `.\test.ps1` or `.\start_server.ps1`

## Running the tests

The tests require a Selenium server running in the background, accessible at `localhost:4444`.
This controls the (usually headless) web browser used to perform the tests.

To run the UI tests in PowerShell, run the script `.\test.ps1` This will setup and teardown a Selenium
server during each run.

Alternatively, start it running in the background and leave it running all day. It shouldn't use too
many resources when it is not being used, and it will allow you to run the tests from inside your IDE
(making use of a debugger, breakpoints, etc.) Run the script `.\start_selenium.ps1` to do this, stop
it with `Ctrl+C` when you are done.

## Troubleshooting

### Is there a way of seeing what Selenium "sees" during a test?
Yes! Change the `IsHeadless` setting to `false` in `Hooks\TestSettings.cs` to see the browser Selenium
is using and watch it interact with it.

### I'm getting certificate warnings from Chrome and it is stopping the tests from reaching the website
To check if this is an issue for you, run the tests not in headless mode (see previous question). This
will manifest as all tests failing to find their first element in headless mode.

To fix this: trust your local IIS Express certificate. A guide to doing this can be found
[here](https://blogs.iis.net/robert_mcmurray/how-to-trust-the-iis-express-self-signed-certificate),
using resolution number 2.
