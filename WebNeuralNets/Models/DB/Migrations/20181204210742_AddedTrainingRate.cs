using Microsoft.EntityFrameworkCore.Migrations;

namespace WebNeuralNets.Models.DB.Migrations
{
    public partial class AddedTrainingRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Neurons");

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
        }
    }
}
