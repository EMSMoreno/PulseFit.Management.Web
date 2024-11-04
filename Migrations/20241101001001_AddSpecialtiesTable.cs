using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulseFit.Management.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddSpecialtiesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Specialty",
                table: "PersonalTrainers");

            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonalTrainerSpecialties",
                columns: table => new
                {
                    PersonalTrainersId = table.Column<int>(type: "int", nullable: false),
                    SpecialtiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalTrainerSpecialties", x => new { x.PersonalTrainersId, x.SpecialtiesId });
                    table.ForeignKey(
                        name: "FK_PersonalTrainerSpecialties_PersonalTrainers_PersonalTrainersId",
                        column: x => x.PersonalTrainersId,
                        principalTable: "PersonalTrainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonalTrainerSpecialties_Specialties_SpecialtiesId",
                        column: x => x.SpecialtiesId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalTrainerSpecialties_SpecialtiesId",
                table: "PersonalTrainerSpecialties",
                column: "SpecialtiesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonalTrainerSpecialties");

            migrationBuilder.DropTable(
                name: "Specialties");

            migrationBuilder.AddColumn<string>(
                name: "Specialty",
                table: "PersonalTrainers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
