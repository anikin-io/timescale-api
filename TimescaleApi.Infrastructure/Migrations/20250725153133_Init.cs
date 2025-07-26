using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimescaleApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "results",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_name = table.Column<string>(type: "text", nullable: false),
                    first_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    time_delta_seconds = table.Column<double>(type: "double precision", nullable: false),
                    avg_execution_time = table.Column<double>(type: "double precision", nullable: false),
                    avg_value = table.Column<double>(type: "double precision", nullable: false),
                    median_value = table.Column<double>(type: "double precision", nullable: false),
                    min_value = table.Column<double>(type: "double precision", nullable: false),
                    max_value = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_results", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "values",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_name = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    execution_time = table.Column<double>(type: "double precision", nullable: false),
                    value = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_values", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "results");

            migrationBuilder.DropTable(
                name: "values");
        }
    }
}
