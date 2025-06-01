using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTrail.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecommendedByUserId",
                table: "Recommendation");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ReadLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ReadLog");

            migrationBuilder.AddColumn<Guid>(
                name: "RecommendedByUserId",
                table: "Recommendation",
                type: "uuid",
                nullable: true);
        }
    }
}
