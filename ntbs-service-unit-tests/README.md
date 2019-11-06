# NTBS unit tests
Unit tests for the ntbs-service project.

## Testing

We are using the [xunit](https://xunit.net/) framework for testing.
To run unit/integration tests, cd into the ntbs-service-unit-tests/ntbs-integration-tests project and run "dotnet test".

## Troubleshooting

- Xunit has an issue with providing enums directly as InlineData for theory tests. 
  - It throws an unhelpful exception of: `System.IO.FileNotFoundException : Could not load file or assembly 'Microsoft.Extensions.Configuration.UserSecrets...`
  - The workaround was to cast the enum directly to an int and then cast back to an enum within the test method, as in `NotificationServiceTest.SocialRiskFactorChecklist_AreSetToFalseIfStatusNoOrUnknown`