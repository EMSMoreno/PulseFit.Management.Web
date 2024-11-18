using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulseFit.Management.Web.Migrations
{
    /// <inheritdoc />
    public partial class ModifyWorkoutPlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkoutPlanType",
                table: "WorkoutPlan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkoutPlanId",
                table: "Equipments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_WorkoutPlanId",
                table: "Equipments",
                column: "WorkoutPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipments_WorkoutPlan_WorkoutPlanId",
                table: "Equipments",
                column: "WorkoutPlanId",
                principalTable: "WorkoutPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_WorkoutPlan_WorkoutPlanId",
                table: "Equipments");

            migrationBuilder.DropIndex(
                name: "IX_Equipments_WorkoutPlanId",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "WorkoutPlanType",
                table: "WorkoutPlan");

            migrationBuilder.DropColumn(
                name: "WorkoutPlanId",
                table: "Equipments");
        }
    }
}
