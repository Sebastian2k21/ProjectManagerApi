using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class addidtoprojectstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectStatuses",
                table: "ProjectStatuses");

            migrationBuilder.AddColumn<int>(
                name: "ProjectStatusId",
                table: "ProjectStatuses",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectStatuses",
                table: "ProjectStatuses",
                column: "ProjectStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStatuses_StatusId",
                table: "ProjectStatuses",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectStatuses",
                table: "ProjectStatuses");

            migrationBuilder.DropIndex(
                name: "IX_ProjectStatuses_StatusId",
                table: "ProjectStatuses");

            migrationBuilder.DropColumn(
                name: "ProjectStatusId",
                table: "ProjectStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectStatuses",
                table: "ProjectStatuses",
                columns: new[] { "StatusId", "ProjectId" });
        }
    }
}
