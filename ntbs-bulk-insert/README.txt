## What this application does

The ntbs-bulk-insert console application is used to generate large numbers of notifications, mostly for testing how well
the system can cope with large amounts of data.

Each notification is given a minimal amount of randomised data:
- Full name
- Notification date between 2014 and 2017   
- Tb service
- Nhs number (starting with 9 so that it is counted as test data by the system)
- Optionally treatment start and completed events can be added to the system *
- 'UniqueBulkInsert' is assigned to the notes field in ClinicalDetails to allow these records to be easily deleted

*Adding these (how to do so is covered later) will stop treatment outcome at 12/24/36 month alerts being generated 

## Targeting different databases

The connection string for the database is pulled from appsettings.development.json in ntbs-service so to target
a different database change the connection string here.

## Running the bulk insert console app

To run the bulk insert console app navigate to the ntbs-bulk-insert app directory and run:
- dotnet run

The optional treatment events can be toggled on and off using the boolean flag "addTreatmentEvents" by 
passing true to GenerateNotifications in Main().

