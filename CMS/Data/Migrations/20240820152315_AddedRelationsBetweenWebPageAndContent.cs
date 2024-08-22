using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMS.Migrations
{
    /// <inheritdoc />
    public partial class AddedRelationsBetweenWebPageAndContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WebPage");

            migrationBuilder.CreateTable(
                name: "Content",
                columns: table => new
                {
                    ContentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentType = table.Column<int>(type: "int", nullable: false),
                    ContentInput = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebPageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Content", x => x.ContentId);
                    table.ForeignKey(
                        name: "FK_Content_WebPage_WebPageId",
                        column: x => x.WebPageId,
                        principalTable: "WebPage",
                        principalColumn: "WebPageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Content_WebPageId",
                table: "Content",
                column: "WebPageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Content");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "WebPage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
