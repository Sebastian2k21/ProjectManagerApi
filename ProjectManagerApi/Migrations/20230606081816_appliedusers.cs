using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class appliedusers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectUser",
                columns: table => new
                {
                    ApplicantsId = table.Column<int>(type: "int", nullable: false),
                    AppliedProjectsProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUser", x => new { x.ApplicantsId, x.AppliedProjectsProjectId });
                    table.ForeignKey(
                        name: "FK_ProjectUser_Projects_AppliedProjectsProjectId",
                        column: x => x.AppliedProjectsProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUser_Users_ApplicantsId",
                        column: x => x.ApplicantsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUser_AppliedProjectsProjectId",
                table: "ProjectUser",
                column: "AppliedProjectsProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectUser");
        }
    }
}
