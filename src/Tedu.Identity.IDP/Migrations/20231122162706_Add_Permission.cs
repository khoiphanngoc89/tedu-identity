using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tedu.Identity.IDP.Migrations
{
    /// <inheritdoc />
    public partial class Add_Permission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Identity",
                table: "Role",
                keyColumn: "Id",
                keyValue: "2d53cb10-1f52-48c1-9864-a6f5ecbc1600");

            migrationBuilder.DeleteData(
                schema: "Identity",
                table: "Role",
                keyColumn: "Id",
                keyValue: "7311c63b-6cd9-4bfa-9363-532fd18ce1d4");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                schema: "Identity",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Permissions",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Function = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Command = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    RoleId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => new { x.Id, x.Command, x.Function });
                    table.ForeignKey(
                        name: "FK_Permissions_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5dca9d2b-be75-4555-87d9-e39b00a73647", "6bb174d1-9ce9-486c-b578-a00aad7ad20e", "Customer", "CUSTOMER" },
                    { "6075a3dc-d09f-4155-a8fe-d210d16b336a", "2f5264d8-a326-4f6d-bd10-d231b8578d32", "Adminstrator", "ADMINSTRATOR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleId",
                schema: "Identity",
                table: "Permissions",
                column: "RoleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleId_Function_Command",
                schema: "Identity",
                table: "Permissions",
                columns: new[] { "RoleId", "Function", "Command" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Permissions",
                schema: "Identity");

            migrationBuilder.DeleteData(
                schema: "Identity",
                table: "Role",
                keyColumn: "Id",
                keyValue: "5dca9d2b-be75-4555-87d9-e39b00a73647");

            migrationBuilder.DeleteData(
                schema: "Identity",
                table: "Role",
                keyColumn: "Id",
                keyValue: "6075a3dc-d09f-4155-a8fe-d210d16b336a");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                schema: "Identity",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2d53cb10-1f52-48c1-9864-a6f5ecbc1600", "0e2467b6-86fd-4269-b4d9-e6a44bed4b40", "Customer", "CUSTOMER" },
                    { "7311c63b-6cd9-4bfa-9363-532fd18ce1d4", "606c883d-e0b8-445b-8bbe-a630684fccad", "Adminstrator", "ADMINSTRATOR" }
                });
        }
    }
}
