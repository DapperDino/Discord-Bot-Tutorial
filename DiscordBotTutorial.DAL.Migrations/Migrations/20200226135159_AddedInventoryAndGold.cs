using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscordBotTutorial.DAL.Migrations.Migrations
{
    public partial class AddedInventoryAndGold : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Gold",
                table: "Profiles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Items",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProfileItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileItem_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileItem_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileItem_ItemId",
                table: "ProfileItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileItem_ProfileId",
                table: "ProfileItem",
                column: "ProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileItem");

            migrationBuilder.DropColumn(
                name: "Gold",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Items");
        }
    }
}
