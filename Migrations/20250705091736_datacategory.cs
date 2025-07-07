using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_ticket.Migrations
{
    /// <inheritdoc />
    public partial class datacategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("insert into Categories (Name) values ('Comedy'); insert into Categories (Name) values ('Action'); insert into Categories (Name) values ('Romantic'); insert into Categories (Name) values ('Cartoon'); insert into Categories (Name) values ('dramatic');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Truncate Table Categories");
        }
    }
}
