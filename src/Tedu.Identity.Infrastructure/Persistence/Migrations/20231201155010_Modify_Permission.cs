using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tedu.Identity.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Modify_Permission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Role_RoleId",
                schema: "Identity",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_RoleId",
                schema: "Identity",
                table: "Permissions");

            migrationBuilder.DeleteData(
                schema: "Identity",
                table: "Role",
                keyColumn: "Id",
                keyValue: "5c5aacfb-335d-4589-8b04-d9ac7437aba6");

            migrationBuilder.DeleteData(
                schema: "Identity",
                table: "Role",
                keyColumn: "Id",
                keyValue: "5ef46701-659c-4582-ac1c-6201a77feadd");

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9ffc82bd-d478-40c1-a535-20e8b122a0d1", "b5ea9e51-fd76-4486-803f-84e0287c3aa5", "Adminstrator", "ADMINSTRATOR" },
                    { "bcc72696-ce79-44ae-8d5f-9c8a8be76f71", "ac84f98d-810d-4239-9699-116956e3154a", "Customer", "CUSTOMER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Identity",
                table: "Role",
                keyColumn: "Id",
                keyValue: "9ffc82bd-d478-40c1-a535-20e8b122a0d1");

            migrationBuilder.DeleteData(
                schema: "Identity",
                table: "Role",
                keyColumn: "Id",
                keyValue: "bcc72696-ce79-44ae-8d5f-9c8a8be76f71");

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5c5aacfb-335d-4589-8b04-d9ac7437aba6", "29099061-e1e9-4195-a5d7-42863a1858a4", "Adminstrator", "ADMINSTRATOR" },
                    { "5ef46701-659c-4582-ac1c-6201a77feadd", "e2dd3575-1777-4808-8007-e8fc2ff15a7d", "Customer", "CUSTOMER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleId",
                schema: "Identity",
                table: "Permissions",
                column: "RoleId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Role_RoleId",
                schema: "Identity",
                table: "Permissions",
                column: "RoleId",
                principalSchema: "Identity",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
