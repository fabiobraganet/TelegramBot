using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ESB.Data.Migrations.Bots
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    MessageInId = table.Column<Guid>(nullable: false),
                    AccountId = table.Column<string>(nullable: true),
                    Messenger = table.Column<string>(nullable: true),
                    Order = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Moment = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.MessageInId);
                });

            migrationBuilder.CreateTable(
                name: "MessageOut",
                columns: table => new
                {
                    MessageOutId = table.Column<Guid>(nullable: false),
                    AccountId = table.Column<string>(nullable: true),
                    Messenger = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    MessageInId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageOut", x => x.MessageOutId);
                    table.ForeignKey(
                        name: "FK_MessageOut_Message_MessageInId",
                        column: x => x.MessageInId,
                        principalTable: "Message",
                        principalColumn: "MessageInId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageOut_MessageInId",
                table: "MessageOut",
                column: "MessageInId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageOut");

            migrationBuilder.DropTable(
                name: "Message");
        }
    }
}
