using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulseFit.Management.Web.Migrations
{
    /// <inheritdoc />
    public partial class ModifyEquipments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_Gyms_GymId",
                table: "Equipments");

            migrationBuilder.DropIndex(
                name: "IX_Equipments_GymId",
                table: "Equipments");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkoutImageId",
                table: "Workouts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentImageId",
                table: "Equipments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GymName",
                table: "Equipments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EquipmentImageId",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "GymName",
                table: "Equipments");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkoutImageId",
                table: "Workouts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_GymId",
                table: "Equipments",
                column: "GymId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipments_Gyms_GymId",
                table: "Equipments",
                column: "GymId",
                principalTable: "Gyms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
