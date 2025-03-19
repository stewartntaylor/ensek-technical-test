using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ensek.WebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMeterReadingsAccountsFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_MeterReading_Account_AccountId",
                table: "MeterReading",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeterReading_Account_AccountId",
                table: "MeterReading");
        }
    }
}
