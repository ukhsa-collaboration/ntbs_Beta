# NTBS Load Testing

## Initial setup

Before you run the NTBS load tests, you must first do the following:

1. Download Gatling Open Source from [https://gatling.io/open-source/start-testing/](https://gatling.io/open-source/start-testing/).
1. Unzip the bundle and copy the `bin`, `lib` and `results` directories into the root directory of the load testing project.

## Running the tests

In order to the run tests, simply run the `bin\gatling.bat` script.

## Using the Gatling recorder

Gatling provides a "record" feature, which is a useful tool in developing new scenarios.
The idea is that Gatling will follow an active web session and auto-generate the code for the requests it sees.
There are two modes: one uses an HTTP Proxy (we haven't managed to get this to work yet) and the other using HAR files.

To generate a HAR file:
* In Chrome, open dev tools and go to the network tab.
* Clear the current list of network requests, if necessary.
* Perform the actions that you wish to record. (e.g. Go to a page -> edit a value -> click save).
* In dev tools, right-click and select "Save all as HAR with content".

Then, to import this as a new scenario in Gatling:
* Run the `bin\recorder.bat` script.
* Set the recorder mode to "HAR converter".
* Choose the HAR file you just created.
* Update the class name to something appropriate.
* Click "Start!".

This will generate code to replay the exact same requests that you made in the browser.
However, you will need to manually convert this into a series of steps in our scenario builder framework.
