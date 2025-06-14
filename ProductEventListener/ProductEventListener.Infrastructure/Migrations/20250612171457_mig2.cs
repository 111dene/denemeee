using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductEventListener.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReadAt",
                table: "ProductEventLogs",
                newName: "OccurredOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OccurredOn",
                table: "ProductEventLogs",
                newName: "ReadAt");
        }
    }
}
