using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HFSolutions.TestDotNet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumn_TaskStateId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaskStatusId",
                table: "TaskState",
                newName: "TaskStateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaskStateId",
                table: "TaskState",
                newName: "TaskStatusId");
        }
    }
}
