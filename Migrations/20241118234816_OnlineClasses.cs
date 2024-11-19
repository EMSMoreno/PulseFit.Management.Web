using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulseFit.Management.Web.Migrations
{
    /// <inheritdoc />
    public partial class OnlineClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnlineClasses_AspNetUsers_InstructorId",
                table: "OnlineClasses");

            migrationBuilder.DropIndex(
                name: "IX_OnlineClasses_InstructorId",
                table: "OnlineClasses");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "OnlineClasses");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "OnlineClasses");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "OnlineClasses");

            migrationBuilder.AlterColumn<int>(
                name: "Category",
                table: "OnlineClasses",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "WorkoutRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WorkoutId = table.Column<int>(type: "int", nullable: false),
                    RatingValue = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutRatings_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutRatings_UserId1",
                table: "WorkoutRatings",
                column: "UserId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkoutRatings");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "OnlineClasses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "OnlineClasses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InstructorId",
                table: "OnlineClasses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "OnlineClasses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OnlineClasses_InstructorId",
                table: "OnlineClasses",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_OnlineClasses_AspNetUsers_InstructorId",
                table: "OnlineClasses",
                column: "InstructorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
