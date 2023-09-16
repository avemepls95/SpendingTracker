using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpendingTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyRateByDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrencyRateByDay",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Base = table.Column<Guid>(type: "uuid", nullable: false),
                    Target = table.Column<Guid>(type: "uuid", nullable: false),
                    Coefficient = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyRateByDay", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyRateByDay");
        }
    }
}
