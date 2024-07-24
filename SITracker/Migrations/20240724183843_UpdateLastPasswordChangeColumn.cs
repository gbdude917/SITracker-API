using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SITracker.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLastPasswordChangeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastPasswordChange",
                table: "users",
                newName: "last_password_change");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "last_password_change",
                table: "users",
                newName: "LastPasswordChange");
        }
    }
}
