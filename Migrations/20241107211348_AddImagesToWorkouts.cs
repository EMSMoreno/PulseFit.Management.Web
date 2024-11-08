using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulseFit.Management.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddImagesToWorkouts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupType",
                table: "Workouts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IndividualType",
                table: "Workouts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkoutImageId",
                table: "Workouts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupType",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "IndividualType",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "WorkoutImageId",
                table: "Workouts");
        }
    }
}
