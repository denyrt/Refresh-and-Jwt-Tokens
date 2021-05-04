using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StudyTestingEnvironment.Data.Migrations
{
    public partial class RefreshSessions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2b49ce13-15d5-48df-92b9-77491c930b71"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ce7dda92-6ffb-4c26-9ecb-c3b7d37760df"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("da8c31cb-d5d1-408d-a018-6a8d5d5df93a"));

            migrationBuilder.CreateTable(
                name: "RefreshSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FingerPrint = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ExpiresInUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshSessions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "UserId" },
                values: new object[] { new Guid("69266a23-2322-4c09-84ca-4eb22e804839"), "f642bdbb-4de6-400f-8ed1-01957f267872", "Teacher", "TEACHER", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "UserId" },
                values: new object[] { new Guid("a830f7a2-691b-4cb3-bf6a-163c167e8b8f"), "c3b0b280-f04b-407d-a04f-eb4fb397ef77", "Student", "STUDENT", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "UserId" },
                values: new object[] { new Guid("ce7a2ead-68f1-4270-90f6-88f864dfdeae"), "3bba6c3e-a0d8-4182-9945-7ababe1cca5c", "Moderator", "MODERATOR", null });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshSessions_UserId",
                table: "RefreshSessions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshSessions");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("69266a23-2322-4c09-84ca-4eb22e804839"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a830f7a2-691b-4cb3-bf6a-163c167e8b8f"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ce7a2ead-68f1-4270-90f6-88f864dfdeae"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "UserId" },
                values: new object[] { new Guid("2b49ce13-15d5-48df-92b9-77491c930b71"), "dcb93877-2b01-4f7d-8936-c52c4e5e8684", "Teacher", "TEACHER", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "UserId" },
                values: new object[] { new Guid("da8c31cb-d5d1-408d-a018-6a8d5d5df93a"), "b21fe638-3fda-4779-abc7-1e6491517f0c", "Student", "STUDENT", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "UserId" },
                values: new object[] { new Guid("ce7dda92-6ffb-4c26-9ecb-c3b7d37760df"), "ced93c28-babf-4629-aaca-f739c35bd2b8", "Moderator", "MODERATOR", null });
        }
    }
}
