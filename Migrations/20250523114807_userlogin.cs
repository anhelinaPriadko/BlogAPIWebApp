using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogAPIWebApp.Migrations
{
    /// <inheritdoc />
    public partial class userlogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Login",
                table: "BlogUsers",
                newName: "UserLogin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserLogin",
                table: "BlogUsers",
                newName: "Login");
        }
    }
}
