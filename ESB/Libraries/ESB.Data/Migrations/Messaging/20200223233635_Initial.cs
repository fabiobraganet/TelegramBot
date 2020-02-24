using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ESB.Data.Migrations.Messaging
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AccountId = table.Column<string>(nullable: true),
                    Messenger = table.Column<string>(nullable: true),
                    Order = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Moment = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");
        }
    }
}
