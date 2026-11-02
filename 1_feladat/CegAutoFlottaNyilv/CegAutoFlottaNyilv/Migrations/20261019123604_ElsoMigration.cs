using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CegAutoFlottaNyilv.Migrations
{
    /// <inheritdoc />
    public partial class ElsoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ember",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nev = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Osztaly = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ember", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Marka = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Modell = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Rendszam = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Evjarat = table.Column<int>(type: "int", nullable: false),
                    EmberId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auto_Ember_EmberId",
                        column: x => x.EmberId,
                        principalTable: "Ember",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auto_EmberId",
                table: "Auto",
                column: "EmberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auto");

            migrationBuilder.DropTable(
                name: "Ember");
        }
    }
}
