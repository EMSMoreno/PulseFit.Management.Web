using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulseFit.Management.Web.Migrations
{
    /// <inheritdoc />
    public partial class WorkoutPlanEquimentList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_WorkoutPlans_WorkoutPlanId",
                table: "Equipments");

            migrationBuilder.DropIndex(
                name: "IX_Equipments_WorkoutPlanId",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "WorkoutPlanId",
                table: "Equipments");

            migrationBuilder.CreateTable(
                name: "WorkoutPlanEquipments",
                columns: table => new
                {
                    EquipmentsId = table.Column<int>(type: "int", nullable: false),
                    WorkoutPlansId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutPlanEquipments", x => new { x.EquipmentsId, x.WorkoutPlansId });
                    table.ForeignKey(
                        name: "FK_WorkoutPlanEquipments_Equipments_EquipmentsId",
                        column: x => x.EquipmentsId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkoutPlanEquipments_WorkoutPlans_WorkoutPlansId",
                        column: x => x.WorkoutPlansId,
                        principalTable: "WorkoutPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlanEquipments_WorkoutPlansId",
                table: "WorkoutPlanEquipments",
                column: "WorkoutPlansId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkoutPlanEquipments");

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
                name: "FK_Equipments_WorkoutPlans_WorkoutPlanId",
                table: "Equipments",
                column: "WorkoutPlanId",
                principalTable: "WorkoutPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
