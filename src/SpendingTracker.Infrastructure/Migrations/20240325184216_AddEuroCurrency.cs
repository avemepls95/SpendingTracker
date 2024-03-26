using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpendingTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEuroCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
insert into ""Currency"" (""Id"", ""Code"", ""Title"", ""IsDefault"", ""CreatedDate"", ""CreatedBy"", ""CountryIcon"", ""IsDeleted"")
values (
        'd83d851c-00c0-4f1d-be9f-a86624297073',
        'EUR',
        'Евро',
        false,
        now(),
        '{Domain.User.SystemUserId.Value}',
		'🇪🇺',
		false
	)
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
delete
from ""Currency""
where ""Id"" = 'd83d851c-00c0-4f1d-be9f-a86624297073';
");
        }
    }
}
