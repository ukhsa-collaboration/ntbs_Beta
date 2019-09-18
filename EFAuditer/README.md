# Entity Framework Auditer

This is a standalone .NET Core auditing project, that can be integrated with any application that uses Entity Framework as its ORM. 
It will audit CRUD operations on your data model to a standalone SqlServer database, into a table "AuditLogs" with columns OriginalId, EntityType, EventType (Read/Insert/Update/Delete), AuditDateTime, AuditUser and AuditData, which contains information about the logged entity as json (including the old and new values in case of an update).

This is based on the [Audit.NET library](https://github.com/thepirat000/Audit.NET), in particular its [Entity Framework extension](https://github.com/thepirat000/Audit.NET/blob/master/src/Audit.EntityFramework/README.md).
This enables automated logging of all create, update and delete entity framework operations.

The Audit.NET framework does not log any database reads, so this project has been extended with a CreateReadLog method which needs to be manually called wherever db access should be logged.


## Usage

To use the Auditer in a project, do the following:
- Install the AuditNET EF extension: `dotnet add package Audit.EntityFramework.Core`
- Add a reference to EFAuditer: `dotnet add reference <path to EFAuditer.csproj file>`
- Specify path to auditing database in (TODO NTBS-218: db settings file)
- Ensure DbContext inherits from `Audit.EntityFramework.AuditDbContext` instead of the ordinary DbContext
- Call the method `Auditer.SetupAuditFramework()` from e.g. Configure within Startup.cs (it needs to be called at application startup)
- Whenever db access should be logged, call `Auditer.CreateReadLog(primaryKeyName, primaryKeyId, model)`, where the first two parameters are the primary key name and integer value of the retrieved entity, and model is the entity itself. This will log the state of the entity when it was retrieved.

This is all that is required for logging db operations. Audit.NET can be customised further, e.g. to not include certain models in the auditing - see the [documentation](https://www.amazon.co.uk/Adapter-Aluminum-Compatible-MacBook-Devices-4-1/dp/B07PV66G2S/ref=sr_1_3?keywords=usbc+to+usb+hub&qid=1568809731&s=gateway&sr=8-3) for more details.