using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpendingTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSetting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false),
                    DefaultValueAsString = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSetting", x => x.Id);
                });
            
            migrationBuilder.Sql($@"
insert into ""UserSetting"" (""Id"", ""Key"", ""DefaultValueAsString"", ""IsDeleted"")
values (
        '32cf39c9-6577-44a5-bf1e-f7b1066cf804',
        'ViewCurrencyId',
        (SELECT ""Id"" FROM public.""Currency"" t WHERE ""Code"" = 'RUB' LIMIT 1),
        false
	)
");

            migrationBuilder.CreateTable(
                name: "UserSettingValue",
                columns: table => new
                {
                    SettingId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ValueAsString = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettingValue", x => new { x.SettingId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserSettingValue_UserSetting_SettingId",
                        column: x => x.SettingId,
                        principalTable: "UserSetting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSettingValue");

            migrationBuilder.DropTable(
                name: "UserSetting");
        }
    }
}
