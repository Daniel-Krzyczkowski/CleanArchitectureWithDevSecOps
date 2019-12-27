using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Infrastructure.Migrations
{
    public partial class initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LessonTopicCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CategoryName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonTopicCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TutorId = table.Column<Guid>(nullable: false),
                    StudentId = table.Column<Guid>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    AcceptedByTutor = table.Column<bool>(nullable: false),
                    CanceledByTutor = table.Column<bool>(nullable: false),
                    CancelledByStudent = table.Column<bool>(nullable: false),
                    Term = table.Column<DateTimeOffset>(nullable: false),
                    Length = table.Column<double>(nullable: false),
                    TopicCategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lessons_LessonTopicCategories_TopicCategoryId",
                        column: x => x.TopicCategoryId,
                        principalTable: "LessonTopicCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "LessonTopicCategories",
                columns: new[] { "Id", "CategoryName" },
                values: new object[,]
                {
                    { new Guid("fb681c44-a917-456f-8a57-60bda5e33f47"), "Maths" },
                    { new Guid("387ce87f-bc98-478b-b6f5-1152423e560e"), "Physics" },
                    { new Guid("62073d1a-5f7a-454f-9b87-e1af62808cf4"), "Programming" },
                    { new Guid("945fe619-120c-4c05-bd04-a91fe93c769f"), "Biology" },
                    { new Guid("f3d4fbe4-a7be-4983-baf7-9a5f10ac141d"), "History" },
                    { new Guid("4974ef92-03cb-4eff-8e28-16d5450d25d6"), "Mechanics" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_TopicCategoryId",
                table: "Lessons",
                column: "TopicCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "LessonTopicCategories");
        }
    }
}
