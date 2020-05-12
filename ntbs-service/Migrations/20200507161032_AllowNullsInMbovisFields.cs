using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AllowNullsInMbovisFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MBovisAnimalExposure_Country_CountryId",
                table: "MBovisAnimalExposure");

            migrationBuilder.DropForeignKey(
                name: "FK_MBovisOccupationExposures_Country_CountryId",
                table: "MBovisOccupationExposures");

            migrationBuilder.DropForeignKey(
                name: "FK_MBovisUnpasteurisedMilkConsumption_Country_CountryId",
                table: "MBovisUnpasteurisedMilkConsumption");

            migrationBuilder.AlterColumn<int>(
                name: "YearOfConsumption",
                table: "MBovisUnpasteurisedMilkConsumption",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "MilkProductType",
                table: "MBovisUnpasteurisedMilkConsumption",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "MBovisUnpasteurisedMilkConsumption",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "ConsumptionFrequency",
                table: "MBovisUnpasteurisedMilkConsumption",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<int>(
                name: "YearOfExposure",
                table: "MBovisOccupationExposures",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "OccupationSetting",
                table: "MBovisOccupationExposures",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<int>(
                name: "OccupationDuration",
                table: "MBovisOccupationExposures",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "MBovisOccupationExposures",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "YearOfExposure",
                table: "MBovisAnimalExposure",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ExposureDuration",
                table: "MBovisAnimalExposure",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "MBovisAnimalExposure",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "AnimalType",
                table: "MBovisAnimalExposure",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "AnimalTbStatus",
                table: "MBovisAnimalExposure",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Animal",
                table: "MBovisAnimalExposure",
                maxLength: 35,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 35);

            migrationBuilder.AddForeignKey(
                name: "FK_MBovisAnimalExposure_Country_CountryId",
                table: "MBovisAnimalExposure",
                column: "CountryId",
                principalSchema: "ReferenceData",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MBovisOccupationExposures_Country_CountryId",
                table: "MBovisOccupationExposures",
                column: "CountryId",
                principalSchema: "ReferenceData",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MBovisUnpasteurisedMilkConsumption_Country_CountryId",
                table: "MBovisUnpasteurisedMilkConsumption",
                column: "CountryId",
                principalSchema: "ReferenceData",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MBovisAnimalExposure_Country_CountryId",
                table: "MBovisAnimalExposure");

            migrationBuilder.DropForeignKey(
                name: "FK_MBovisOccupationExposures_Country_CountryId",
                table: "MBovisOccupationExposures");

            migrationBuilder.DropForeignKey(
                name: "FK_MBovisUnpasteurisedMilkConsumption_Country_CountryId",
                table: "MBovisUnpasteurisedMilkConsumption");

            migrationBuilder.AlterColumn<int>(
                name: "YearOfConsumption",
                table: "MBovisUnpasteurisedMilkConsumption",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MilkProductType",
                table: "MBovisUnpasteurisedMilkConsumption",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "MBovisUnpasteurisedMilkConsumption",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConsumptionFrequency",
                table: "MBovisUnpasteurisedMilkConsumption",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "YearOfExposure",
                table: "MBovisOccupationExposures",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OccupationSetting",
                table: "MBovisOccupationExposures",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OccupationDuration",
                table: "MBovisOccupationExposures",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "MBovisOccupationExposures",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "YearOfExposure",
                table: "MBovisAnimalExposure",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ExposureDuration",
                table: "MBovisAnimalExposure",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "MBovisAnimalExposure",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AnimalType",
                table: "MBovisAnimalExposure",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AnimalTbStatus",
                table: "MBovisAnimalExposure",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Animal",
                table: "MBovisAnimalExposure",
                maxLength: 35,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 35,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MBovisAnimalExposure_Country_CountryId",
                table: "MBovisAnimalExposure",
                column: "CountryId",
                principalSchema: "ReferenceData",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MBovisOccupationExposures_Country_CountryId",
                table: "MBovisOccupationExposures",
                column: "CountryId",
                principalSchema: "ReferenceData",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MBovisUnpasteurisedMilkConsumption_Country_CountryId",
                table: "MBovisUnpasteurisedMilkConsumption",
                column: "CountryId",
                principalSchema: "ReferenceData",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
