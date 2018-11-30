using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebNeuralNets.Models.DB.Migrations
{
    public partial class TranslationPrimareKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TranslationValues",
                table: "TranslationValues");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TranslationValues");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "TranslationValues",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TranslationValues",
                table: "TranslationValues",
                columns: new[] { "LanguageCode", "Key" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TranslationValues",
                table: "TranslationValues");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "TranslationValues",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TranslationValues",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TranslationValues",
                table: "TranslationValues",
                column: "Id");
        }
    }
}
