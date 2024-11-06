using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulseFit.Management.Web.Migrations
{
    /// <inheritdoc />
    public partial class ModifyWorkoutsAndBookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "InstructorId",
                table: "Workouts",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "GymName",
                table: "Workouts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstructorName",
                table: "Workouts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GymName",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GymName",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "InstructorName",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "GymName",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Bookings");

            migrationBuilder.AlterColumn<string>(
                name: "InstructorId",
                table: "Workouts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
