using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddSessionStateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // https://www.c-sharpcorner.com/article/configure-sql-server-session-state-in-asp-net-core/
            // This sql script is an alternative to 'dotnet sql-cache create'
            migrationBuilder.Sql(@"
                CREATE TABLE [dbo].[SessionState](
                    [Id] [nvarchar](449) NOT NULL,
                    [Value] [varbinary](max) NOT NULL,
                    [ExpiresAtTime] [datetimeoffset](7) NOT NULL,
                    [SlidingExpirationInSeconds] [bigint] NULL,
                    [AbsoluteExpiration] [datetimeoffset](7) NULL,
                 CONSTRAINT [pk_Id] PRIMARY KEY CLUSTERED 
                (
                    [Id] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
                       IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, 
                       ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                 
                CREATE NONCLUSTERED INDEX [Index_ExpiresAtTime] ON [dbo].[SessionState]
                (
                    [ExpiresAtTime] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
                       SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, 
                       ONLINE = OFF, ALLOW_ROW_LOCKS = ON, 
                       ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("SessionState");
        }
    }
}
