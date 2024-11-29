using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SF.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAddressModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Addresses",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Addresses",
                newName: "Country");

            migrationBuilder.AddColumn<string>(
                name: "AddressOpt",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressPri",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TaxBranchCode",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressOpt",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "AddressPri",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "TaxBranchCode",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Addresses",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Addresses",
                newName: "City");
        }
    }
}
