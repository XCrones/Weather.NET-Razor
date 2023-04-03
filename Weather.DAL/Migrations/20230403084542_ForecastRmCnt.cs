using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weather.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ForecastRmCnt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cnt",
                table: "Forecasts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "cnt",
                table: "Forecasts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
