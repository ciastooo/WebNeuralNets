using Microsoft.EntityFrameworkCore.Migrations;

namespace WebNeuralNets.Models.DB.Migrations
{
    public partial class CascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dendrites_Neurons_NextNeuronId",
                table: "Dendrites");

            migrationBuilder.AddForeignKey(
                name: "FK_Dendrites_Neurons_NextNeuronId",
                table: "Dendrites",
                column: "NextNeuronId",
                principalTable: "Neurons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dendrites_Neurons_NextNeuronId",
                table: "Dendrites");

            migrationBuilder.AddForeignKey(
                name: "FK_Dendrites_Neurons_NextNeuronId",
                table: "Dendrites",
                column: "NextNeuronId",
                principalTable: "Neurons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
