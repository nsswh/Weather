using Microsoft.EntityFrameworkCore.Migrations;

namespace Weather.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Skys",
                columns: table => new
                {
                    SkyId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(nullable: true),
                    Timestamp = table.Column<string>(nullable: true),
                    Day = table.Column<string>(nullable: true),
                    Temp = table.Column<string>(nullable: true),
                    Preciptype = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skys", x => x.SkyId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Skys");
        }
    }
}
