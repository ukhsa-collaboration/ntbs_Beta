using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AdjustEthnicityMappingsAndOrderings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 1,
                columns: new[] { "Code", "Label", "Order" },
                values: new object[] { "WW", "White", 1 });

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 4,
                column: "Order",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 5,
                column: "Order",
                value: 11);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 6,
                column: "Order",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 7,
                column: "Order",
                value: 13);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 8,
                column: "Order",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 9,
                column: "Order",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 10,
                column: "Order",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 11,
                column: "Order",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 12,
                column: "Order",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 13,
                column: "Order",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 14,
                column: "Order",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 15,
                column: "Order",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 16,
                column: "Order",
                value: 9);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 1,
                columns: new[] { "Code", "Label", "Order" },
                values: new object[] { "A", "White British", 16 });

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 4,
                column: "Order",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 5,
                column: "Order",
                value: 13);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 6,
                column: "Order",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 7,
                column: "Order",
                value: 9);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 8,
                column: "Order",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 9,
                column: "Order",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 10,
                column: "Order",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 11,
                column: "Order",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 12,
                column: "Order",
                value: 11);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 13,
                column: "Order",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 14,
                column: "Order",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 15,
                column: "Order",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Ethnicity",
                keyColumn: "EthnicityId",
                keyValue: 16,
                column: "Order",
                value: 4);

            migrationBuilder.InsertData(
                table: "Ethnicity",
                columns: new[] { "EthnicityId", "Code", "Label", "Order" },
                values: new object[,]
                {
                    { 2, "B", "White Irish", 17 },
                    { 3, "C", "Any other White background", 3 }
                });
        }
    }
}
