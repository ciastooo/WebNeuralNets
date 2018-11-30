using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebNeuralNets.Models.DB.Migrations
{
    public partial class NeuralNetStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "NeuralNets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UserId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NeuralNets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NeuralNets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NeuralNets_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Layers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NeuralNetId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Iteration = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Layers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Layers_NeuralNets_NeuralNetId",
                        column: x => x.NeuralNetId,
                        principalTable: "NeuralNets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Neurons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LayerId = table.Column<int>(nullable: false),
                    Value = table.Column<double>(nullable: false),
                    Bias = table.Column<double>(nullable: false),
                    Delta = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Neurons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Neurons_Layers_LayerId",
                        column: x => x.LayerId,
                        principalTable: "Layers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dendrites",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NextNeuronId = table.Column<int>(nullable: false),
                    PreviousNeuronId = table.Column<int>(nullable: false),
                    Weight = table.Column<double>(nullable: false),
                    Delta = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dendrites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dendrites_Neurons_NextNeuronId",
                        column: x => x.NextNeuronId,
                        principalTable: "Neurons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dendrites_Neurons_PreviousNeuronId",
                        column: x => x.PreviousNeuronId,
                        principalTable: "Neurons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dendrites_NextNeuronId",
                table: "Dendrites",
                column: "NextNeuronId");

            migrationBuilder.CreateIndex(
                name: "IX_Dendrites_PreviousNeuronId",
                table: "Dendrites",
                column: "PreviousNeuronId");

            migrationBuilder.CreateIndex(
                name: "IX_Layers_NeuralNetId",
                table: "Layers",
                column: "NeuralNetId");

            migrationBuilder.CreateIndex(
                name: "IX_NeuralNets_UserId",
                table: "NeuralNets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NeuralNets_UserId1",
                table: "NeuralNets",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Neurons_LayerId",
                table: "Neurons",
                column: "LayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dendrites");

            migrationBuilder.DropTable(
                name: "Neurons");

            migrationBuilder.DropTable(
                name: "Layers");

            migrationBuilder.DropTable(
                name: "NeuralNets");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "AspNetUsers",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");
        }
    }
}
