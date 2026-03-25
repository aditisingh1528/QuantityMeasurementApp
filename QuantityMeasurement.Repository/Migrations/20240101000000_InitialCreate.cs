using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuantityMeasurementRepository.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuantityMeasurements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    this_value = table.Column<double>(type: "float", nullable: false),
                    this_unit = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    this_measurement_type = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    that_value = table.Column<double>(type: "float", nullable: false),
                    that_unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    that_measurement_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    result_value = table.Column<double>(type: "float", nullable: false),
                    result_unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    result_measurement_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    operation = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    result_string = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_error = table.Column<bool>(type: "bit", nullable: false),
                    error_message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false,
                        defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false,
                        defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuantityMeasurements", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_created_at",
                table: "QuantityMeasurements",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "idx_measurement_type",
                table: "QuantityMeasurements",
                column: "this_measurement_type");

            migrationBuilder.CreateIndex(
                name: "idx_operation",
                table: "QuantityMeasurements",
                column: "operation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "QuantityMeasurements");
        }
    }
}
