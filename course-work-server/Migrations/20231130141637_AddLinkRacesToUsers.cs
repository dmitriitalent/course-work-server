using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace course_work_server.Migrations
{
    /// <inheritdoc />
    public partial class AddLinkRacesToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RaceUser",
                columns: table => new
                {
                    RacesId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceUser", x => new { x.RacesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_RaceUser_Races_RacesId",
                        column: x => x.RacesId,
                        principalTable: "Races",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaceUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RaceUser_UsersId",
                table: "RaceUser",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RaceUser");
        }
    }
}
