# NTBS integration tests
Integration tests for the ntbs-service project.

## Razor Page tests
We want to test that we can make http requests to all our page routes and that they return the correct result.
We make use of the AspNetCore.MVC.Testing package which handles the setup of a TestServer for this purpose.

### Test types
There are some basic tests checking for existance of pages in BasicTests (e.g. home/search pages) and BasicEditPageTests.
If you add a new page, ensure to add a test in those.

For the individual edit pages, we should always test:
- An invalid post for draft state. Write one test to cover all the possible model errors
- An invalid post for notified state. Write one test to cover all the possible model errors
- A valid post. After posting, check data persisted through another get request.
- All "get" methods for dynamic validation.