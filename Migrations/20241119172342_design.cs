using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulseFit.Management.Web.Migrations
{
    /// <inheritdoc />
    public partial class design : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_AspNetUsers_UserId1",
                table: "Alerts");

            migrationBuilder.DropIndex(
                name: "IX_Alerts_UserId1",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Alerts");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Alerts",
                newName: "EmployeeId");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Alerts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_EmployeeId",
                table: "Alerts",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_Employees_EmployeeId",
                table: "Alerts",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_Employees_EmployeeId",
                table: "Alerts");

            migrationBuilder.DropIndex(
                name: "IX_Alerts_EmployeeId",
                table: "Alerts");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Alerts",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Alerts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Alerts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_UserId1",
                table: "Alerts",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_AspNetUsers_UserId1",
                table: "Alerts",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
