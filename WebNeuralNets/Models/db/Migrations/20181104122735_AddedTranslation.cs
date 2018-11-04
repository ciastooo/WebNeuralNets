using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebNeuralNets.Models.db.Migrations
{
    public partial class AddedTranslation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "LanguageCode",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TranslationValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    LanguageCode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslationValues", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TranslationValues");

            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }
    }
}
