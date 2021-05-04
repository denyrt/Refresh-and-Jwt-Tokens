using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StudyTestingEnvironment.Data.Migrations
{
    public partial class RemoveSessions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshSessions");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0d998ffa-bd3e-4900-98b5-6d64c04b8d08"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7ed42895-6391-4420-8a50-4de4c486b89f"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d24caa67-ef2e-4a33-a739-bde0729d6f49"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "UserId" },
                values: new object[] { new Guid("2909989a-2baa-4546-b1d0-fec5fa4c32af"), "95f99310-7b48-4809-999d-86fe83752d16", "Teacher", "TEACHER", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "UserId" },
                values: new object[] { new Guid("8c34bc0e-cb44-4511-9c5d-6106fea92a4f"), "f705b455-3a90-4cdf-a885-1e9c655dfd8f", "Student", "STUDENT", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "UserId" },
                values: new object[] { new Guid("e76c92cb-f5fe-4be0-9f57-1ee2548e5dad"), "3d74e641-6162-43e5-a107-e0eb43d6b0a8", "Moderator", "MODERATOR", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2909989a-2baa-4546-b1d0-fec5fa4c32af"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8c34bc0e-cb44-4511-9c5d-6106fea92a4f"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e76c92cb-f5fe-4be0-9f57-1ee2548e5dad"));

            migrationBuilder.CreateTable(
                name: "RefreshSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateAtUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresInUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FingerPrint = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                values: new object[] { new Guid("7ed42895-6391-4420-8a50-4de4c486b89f"), "abb82d5f-d3d6-4a70-9d51-c96a5c0c2629", "Teacher", "TEACHER", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "UserId" },
                values: new object[] { new Guid("0d998ffa-bd3e-4900-98b5-6d64c04b8d08"), "8d4d3156-9c81-4f10-9fa6-fea322d5b3c9", "Student", "STUDENT", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "UserId" },
                values: new object[] { new Guid("d24caa67-ef2e-4a33-a739-bde0729d6f49"), "4c1bd182-025b-479b-97fd-0822c91f16ea", "Moderator", "MODERATOR", null });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshSessions_UserId",
                table: "RefreshSessions",
                column: "UserId");
        }
    }
}
