using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulseFit.Management.Web.Migrations
{
    /// <inheritdoc />
    public partial class Subs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NutritionPlans_Subscriptions_SubscriptionId",
                table: "NutritionPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_OnlineClasses_Subscriptions_SubscriptionId",
                table: "OnlineClasses");

            migrationBuilder.DropIndex(
                name: "IX_OnlineClasses_SubscriptionId",
                table: "OnlineClasses");

            migrationBuilder.DropIndex(
                name: "IX_NutritionPlans_SubscriptionId",
                table: "NutritionPlans");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "OnlineClasses");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "NutritionPlans");

            migrationBuilder.AddColumn<bool>(
                name: "IncludeNutritionPlans",
                table: "Subscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IncludeOnlineClasses",
                table: "Subscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncludeNutritionPlans",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IncludeOnlineClasses",
                table: "Subscriptions");

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionId",
                table: "OnlineClasses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionId",
                table: "NutritionPlans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OnlineClasses_SubscriptionId",
                table: "OnlineClasses",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionPlans_SubscriptionId",
                table: "NutritionPlans",
                column: "SubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_NutritionPlans_Subscriptions_SubscriptionId",
                table: "NutritionPlans",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OnlineClasses_Subscriptions_SubscriptionId",
                table: "OnlineClasses",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
