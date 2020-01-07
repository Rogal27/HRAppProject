using Microsoft.EntityFrameworkCore.Migrations;

namespace HRWebApplication.Migrations
{
    public partial class CascadeDeleteForJobOffersApplications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_JobOffer",
                table: "Applications");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_JobOffer",
                table: "Applications",
                column: "JobOfferID",
                principalTable: "JobOffers",
                principalColumn: "JobOfferID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_JobOffer",
                table: "Applications");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_JobOffer",
                table: "Applications",
                column: "JobOfferID",
                principalTable: "JobOffers",
                principalColumn: "JobOfferID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
