using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StudyTestingEnvironment.Data.Migrations
{
    public partial class UpdatedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<string>(
                name: "Patronymic",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseIpBinding",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "UseIpBinding",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Patronymic",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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
        }
    }
}
