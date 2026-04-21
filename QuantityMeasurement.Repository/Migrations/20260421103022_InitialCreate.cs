using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace QuantityMeasurement.Repository.Migrations
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    this_value = table.Column<double>(type: "double precision", nullable: false),
                    this_unit = table.Column<string>(type: "text", nullable: false),
                    this_measurement_type = table.Column<string>(type: "text", nullable: false),
                    that_value = table.Column<double>(type: "double precision", nullable: false),
                    that_unit = table.Column<string>(type: "text", nullable: true),
                    that_measurement_type = table.Column<string>(type: "text", nullable: true),
                    result_value = table.Column<double>(type: "double precision", nullable: false),
                    result_unit = table.Column<string>(type: "text", nullable: true),
                    result_measurement_type = table.Column<string>(type: "text", nullable: true),
                    operation = table.Column<string>(type: "text", nullable: false),
                    result_string = table.Column<string>(type: "text", nullable: true),
                    is_error = table.Column<bool>(type: "boolean", nullable: false),
                    error_message = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuantityMeasurements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
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
            migrationBuilder.DropTable(
                name: "QuantityMeasurements");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
