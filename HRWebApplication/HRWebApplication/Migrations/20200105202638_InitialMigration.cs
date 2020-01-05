using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRWebApplication.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationStatus",
                columns: table => new
                {
                    ApplicationStatusID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationStatus", x => x.ApplicationStatusID);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    AttachmentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Path = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.AttachmentID);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompanyID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompanyName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyID);
                });

            migrationBuilder.CreateTable(
                name: "CV",
                columns: table => new
                {
                    CVID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Path = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CV", x => x.CVID);
                });

            migrationBuilder.CreateTable(
                name: "JobOfferStatus",
                columns: table => new
                {
                    JobOfferStatusID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<string>(unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOfferStatus", x => x.JobOfferStatusID);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserRoleID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Role = table.Column<string>(unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.UserRoleID);
                });

            migrationBuilder.CreateTable(
                name: "JobOffers",
                columns: table => new
                {
                    JobOfferID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    JobTitle = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    SalaryFrom = table.Column<int>(nullable: true),
                    SalaryTo = table.Column<int>(nullable: true),
                    Location = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "datetime", nullable: true),
                    JobOfferStatusID = table.Column<int>(nullable: false),
                    CompanyID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOffers", x => x.JobOfferID);
                    table.ForeignKey(
                        name: "FK_JobOffers_Companies",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobOffers_JobOfferStatus",
                        column: x => x.JobOfferStatusID,
                        principalTable: "JobOfferStatus",
                        principalColumn: "JobOfferStatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    LastName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    UserRoleID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_UserRoles",
                        column: x => x.UserRoleID,
                        principalTable: "UserRoles",
                        principalColumn: "UserRoleID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    ApplicationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    JobOfferID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    LastName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Phone = table.Column<decimal>(type: "decimal(18, 0)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CVID = table.Column<int>(nullable: true),
                    ApplicationStatusID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.ApplicationID);
                    table.ForeignKey(
                        name: "FK_Applications_ApplicationStatus",
                        column: x => x.ApplicationStatusID,
                        principalTable: "ApplicationStatus",
                        principalColumn: "ApplicationStatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_CV",
                        column: x => x.CVID,
                        principalTable: "CV",
                        principalColumn: "CVID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_JobOffer",
                        column: x => x.JobOfferID,
                        principalTable: "JobOffers",
                        principalColumn: "JobOfferID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_Users",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HRJobOffers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<int>(nullable: false),
                    JobOfferID = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "ApplicationAttachment",
                columns: table => new
                {
                    ApplicationAttachmentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationID = table.Column<int>(nullable: false),
                    AttachmentID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationAttachment", x => x.ApplicationAttachmentID);
                    table.ForeignKey(
                        name: "FK_ApplicationAttachment_Applications",
                        column: x => x.ApplicationID,
                        principalTable: "Applications",
                        principalColumn: "ApplicationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationAttachment_Attachments",
                        column: x => x.AttachmentID,
                        principalTable: "Attachments",
                        principalColumn: "AttachmentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationAttachment_AttachmentID",
                table: "ApplicationAttachment",
                column: "AttachmentID");

            migrationBuilder.CreateIndex(
                name: "idx_application_attachment",
                table: "ApplicationAttachment",
                columns: new[] { "ApplicationID", "AttachmentID" });

            migrationBuilder.CreateIndex(
                name: "idx_status",
                table: "Applications",
                column: "ApplicationStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_CVID",
                table: "Applications",
                column: "CVID");

            migrationBuilder.CreateIndex(
                name: "idx_joboffer",
                table: "Applications",
                column: "JobOfferID");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_UserID",
                table: "Applications",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "idx_joboffer_names",
                table: "Applications",
                columns: new[] { "JobOfferID", "FirstName", "LastName" });

            migrationBuilder.CreateIndex(
                name: "idx_name",
                table: "Companies",
                column: "CompanyName");

            migrationBuilder.CreateIndex(
                name: "IX_HRJobOffers_JobOfferID",
                table: "HRJobOffers",
                column: "JobOfferID");

            migrationBuilder.CreateIndex(
                name: "idx_user_joboffer",
                table: "HRJobOffers",
                columns: new[] { "UserID", "JobOfferID" });

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_CompanyID",
                table: "JobOffers",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "idx_date",
                table: "JobOffers",
                column: "CreationDate");

            migrationBuilder.CreateIndex(
                name: "idx_status",
                table: "JobOffers",
                column: "JobOfferStatusID");

            migrationBuilder.CreateIndex(
                name: "idx_title",
                table: "JobOffers",
                column: "JobTitle");

            migrationBuilder.CreateIndex(
                name: "idx_location",
                table: "JobOffers",
                column: "Location");

            migrationBuilder.CreateIndex(
                name: "idx_expirationdate",
                table: "JobOffers",
                column: "ValidUntil");

            migrationBuilder.CreateIndex(
                name: "idx_email",
                table: "Users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "idx_fname",
                table: "Users",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "idx_lname",
                table: "Users",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserRoleID",
                table: "Users",
                column: "UserRoleID");

            migrationBuilder.CreateIndex(
                name: "idx_names",
                table: "Users",
                columns: new[] { "FirstName", "LastName" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationAttachment");

            migrationBuilder.DropTable(
                name: "HRJobOffers");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "ApplicationStatus");

            migrationBuilder.DropTable(
                name: "CV");

            migrationBuilder.DropTable(
                name: "JobOffers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "JobOfferStatus");

            migrationBuilder.DropTable(
                name: "UserRoles");
        }
    }
}
