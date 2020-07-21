using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FailureMicroservice.Web.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Failure",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Body = table.Column<string>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Failure", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Failure");
        }
    }
}
