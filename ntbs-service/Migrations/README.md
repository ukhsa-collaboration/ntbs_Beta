# Migrations

## NTBS database migrations

To minimize friction in development and deployments, the app uses Entity Framework migrations.
The simplest way to make a schema change is to make the appropriate changes in the model (e.g. [Notification](Models/Entities/Notification.cs) and the [database context](DataAccess/NtbsContext.cs) and run 
`dotnet ef migrations add <NameOfMigration> --context=NtbsContext`
This will create a migration file, that can be edited further to match needs. It will be run at application startup,
or can be evoked by hand using
`dotnet ef database update`

[See more information on migration](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)

## DACPAC files

Other NTBS repositories such as [ntbs-specimen-matching](https://github.com/publichealthengland/ntbs-specimen-matching) and [ntbs-reporting](https://github.com/publichealthengland/ntbs-reporting) include DACPAC files of the databases they are reliant on.
If you have made changes to the NTBS database then these files need to be updated in the corresponding repository.

While you can create a DACPAC file through SSMS, this can sometimes lead to errors caused by references to other databases. Therefore, it is recommended that you extract the DACPAC with 'Verify extraction' disabled through Visual Studio in the SQL Server Object Explorer. Instructions can be found [here.](https://docs.microsoft.com/en-us/sql/relational-databases/data-tier-applications/extract-a-dac-from-a-database?view=sql-server-ver15#UsingDACExtractWizard) )


