using Microsoft.EntityFrameworkCore.Migrations;
using WebNeuralNets.Models.Enums;

namespace WebNeuralNets.Models.DB.Migrations
{
    public partial class NotMappedProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Delta",
                table: "Neurons");

            migrationBuilder.DropColumn(
                name: "Delta",
                table: "Dendrites");

            migrationBuilder.Sql($@"INSERT INTO [TranslationValues] ([Key],[Value],[LanguageCode])
		                                VALUES ('VALIDATION_FIELDREQUIRED', 'This field is required', {(int)LanguageCode.ENG})");
            migrationBuilder.Sql($@"INSERT INTO [TranslationValues] ([Key],[Value],[LanguageCode])
		                                VALUES ('VALIDATION_FIELDREQUIRED', 'To pole jest wymagane', {(int)LanguageCode.PL})");
            migrationBuilder.Sql($@"INSERT INTO [TranslationValues] ([Key],[Value],[LanguageCode])
		                                VALUES ('VALIDATION_INVALIDTRAININGDATA', 'Invalid training data', {(int)LanguageCode.ENG})");
            migrationBuilder.Sql($@"INSERT INTO [TranslationValues] ([Key],[Value],[LanguageCode])
		                                VALUES ('VALIDATION_INVALIDTRAININGDATA', 'Błędne dane uczące', {(int)LanguageCode.PL})");
            migrationBuilder.Sql($@"INSERT INTO [TranslationValues] ([Key],[Value],[LanguageCode])
		                                VALUES ('GENERIC_ERROR', 'Oops, something went wrong :(', {(int)LanguageCode.ENG})");
            migrationBuilder.Sql($@"INSERT INTO [TranslationValues] ([Key],[Value],[LanguageCode])
		                                VALUES ('GENERIC_ERROR', 'Upss, coś poszło nie tak :(', {(int)LanguageCode.PL})");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Delta",
                table: "Neurons",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Delta",
                table: "Dendrites",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
