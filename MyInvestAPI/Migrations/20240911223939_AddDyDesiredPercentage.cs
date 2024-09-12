using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyInvestAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddDyDesiredPercentage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "DYDesiredPercentage",
                table: "Actives",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DYDesiredPercentage",
                table: "Actives");
        }
    }
}
