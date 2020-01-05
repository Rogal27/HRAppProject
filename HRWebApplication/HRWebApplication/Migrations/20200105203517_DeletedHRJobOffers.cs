using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRWebApplication.Migrations
{
    public partial class DeletedHRJobOffers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HRJobOffers");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "JobOffers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_UserId",
                table: "JobOffers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_Users_UserId",
                table: "JobOffers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_Users_UserId",
                table: "JobOffers");

            migrationBuilder.DropIndex(
                name: "IX_JobOffers_UserId",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "JobOffers");

            migrationBuilder.CreateTable(
                name: "HRJobOffers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    JobOfferID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRJobOffers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HRJobOffers_JobOffers",
                        column: x => x.JobOfferID,
                        principalTable: "JobOffers",
                        principalColumn: "JobOfferID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HRJobOffers_Users",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HRJobOffers_JobOfferID",
                table: "HRJobOffers",
                column: "JobOfferID");

            migrationBuilder.CreateIndex(
                name: "idx_user_joboffer",
                table: "HRJobOffers",
                columns: new[] { "UserID", "JobOfferID" });
        }
    }
}
