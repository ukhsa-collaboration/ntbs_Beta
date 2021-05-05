using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class DrugResistanceProfileNotNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"UPDATE DrugResistanceProfile
                  SET Species = COALESCE(Species, 'No result'),
                      DrugResistanceProfileString = COALESCE(DrugResistanceProfileString, 'No result');");

            migrationBuilder.AlterColumn<string>(
                name: "Species",
                table: "DrugResistanceProfile",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "No result",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true,
                oldDefaultValue: "No result");

            migrationBuilder.AlterColumn<string>(
                name: "DrugResistanceProfileString",
                table: "DrugResistanceProfile",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "No result",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true,
                oldDefaultValue: "No result");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Species",
                table: "DrugResistanceProfile",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                defaultValue: "No result",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValue: "No result");

            migrationBuilder.AlterColumn<string>(
                name: "DrugResistanceProfileString",
                table: "DrugResistanceProfile",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                defaultValue: "No result",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValue: "No result");
        }
    }
}
