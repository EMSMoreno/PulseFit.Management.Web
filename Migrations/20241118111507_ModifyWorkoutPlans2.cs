using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulseFit.Management.Web.Migrations
{
    /// <inheritdoc />
    public partial class ModifyWorkoutPlans2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_WorkoutPlan_WorkoutPlanId",
                table: "Equipments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutPlan",
                table: "WorkoutPlan");

            migrationBuilder.RenameTable(
                name: "WorkoutPlan",
                newName: "WorkoutPlans");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutPlans",
                table: "WorkoutPlans",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipments_WorkoutPlans_WorkoutPlanId",
                table: "Equipments",
                column: "WorkoutPlanId",
                principalTable: "WorkoutPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_WorkoutPlans_WorkoutPlanId",
                table: "Equipments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutPlans",
                table: "WorkoutPlans");

            migrationBuilder.RenameTable(
                name: "WorkoutPlans",
                newName: "WorkoutPlan");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutPlan",
                table: "WorkoutPlan",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipments_WorkoutPlan_WorkoutPlanId",
                table: "Equipments",
                column: "WorkoutPlanId",
                principalTable: "WorkoutPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
