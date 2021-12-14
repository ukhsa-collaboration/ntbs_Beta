# NTBS Load Testing

## Initial setup

Before you run the NTBS load tests, you must first do the following:

- Install Gradle from [https://gradle.org/install/](https://gradle.org/install/) (follow the "Installing Manually" section).

Additionally, before you use the Gatling recorder, you must:

- Download Gatling Open Source from [https://gatling.io/open-source/start-testing/](https://gatling.io/open-source/start-testing/).
- Unzip the bundle and copy the `bin`, `lib` and `results` directories into the [self-hosted](self-hosted) directory of the load testing project.

## Running the tests

Before you can run tests, you first need to acquire authentication cookies. One way to do this is:

- In a browser, visit the site that you want to run the load tests against and sign-in.
  - It's a good idea to use a national team user for this, so that they have access to all of the records in the environment.
- Open your browser's dev tools, and load any page on the site.
- In the network tab, find the request you just made, and copy the value of the "cookie" header on the request.
- Go to [Config.scala](gradle/src/gatling/scala/Config.scala) and paste the value as the default for the  `COOKIE_HEADER` environment variable (i.e. in place of `<insert cookie here>`).
 
Then to run the tests, simply run the command `bash gradlew gatlingRun-NtbsLoadTest --rerun-tasks` from Git Bash in the [gradle](gradle) directory.

## Using the Gatling recorder

Gatling provides a "record" feature, which is a useful tool in developing new scenarios.
The idea is that Gatling will follow an active web session and auto-generate the code for the requests it sees.
There are two modes: one uses an HTTP Proxy (we haven't managed to get this to work yet) and the other using HAR files.

To generate a HAR file:

- In Chrome, open dev tools and go to the network tab.
- Clear the current list of network requests, if necessary.
- Perform the actions that you wish to record. (e.g. Go to a page -> edit a value -> click save).
- In dev tools, right-click and select "Save all as HAR with content".

Then, to import this as a new scenario in Gatling:

- Run the `recorder.bat` script in the [self-hosted/bin](self-hosted/bin) directory.
- Set the recorder mode to "HAR converter".
- Choose the HAR file you just created.
- Update the class name to something appropriate.
- Update the simulations folder to the appropriate user-files directory.
- Click "Start!".

This will generate code to replay the exact same requests that you made in the browser.
However, you will need to manually convert this into a series of steps in our scenario builder framework, and transfer the class to the [gradle/src/gatling/scala](gradle/src/gatling/scala) directory so that it can be included in the `NtbsLoadTest` simulation.
