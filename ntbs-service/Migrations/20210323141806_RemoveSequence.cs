using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class RemoveSequence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderIndex",
                table: "FrequentlyAskedQuestion",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "NEXT VALUE FOR shared.OrderIndex");
            
            migrationBuilder.DropSequence(
                 name: "OrderIndex",
                 schema: "shared");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "shared");

            migrationBuilder.CreateSequence<int>(
                name: "OrderIndex",
                schema: "shared");

            migrationBuilder.AlterColumn<int>(
                name: "OrderIndex",
                table: "FrequentlyAskedQuestion",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR shared.OrderIndex",
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
