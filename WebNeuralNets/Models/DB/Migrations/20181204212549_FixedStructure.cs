using Microsoft.EntityFrameworkCore.Migrations;

namespace WebNeuralNets.Models.DB.Migrations
{
    public partial class FixedStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NeuralNets_AspNetUsers_UserId1",
                table: "NeuralNets");

            migrationBuilder.DropIndex(
                name: "IX_NeuralNets_UserId1",
                table: "NeuralNets");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Neurons");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "NeuralNets");

            migrationBuilder.AddColumn<double>(
                name: "TrainingRate",
                table: "NeuralNets",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrainingRate",
                table: "NeuralNets");

            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "Neurons",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "NeuralNets",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NeuralNets_UserId1",
                table: "NeuralNets",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_NeuralNets_AspNetUsers_UserId1",
                table: "NeuralNets",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
