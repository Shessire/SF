using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SF.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleGroupRoles",
                columns: table => new
                {
                    RoleGroupId = table.Column<int>(type: "int", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleGroupRoles", x => new { x.RoleGroupId, x.RoleName });
                    table.ForeignKey(
                        name: "FK_RoleGroupRoles_RoleGroups_RoleGroupId",
                        column: x => x.RoleGroupId,
                        principalTable: "RoleGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleGroupRoles");

            migrationBuilder.DropTable(
                name: "RoleGroups");
        }
    }
}
