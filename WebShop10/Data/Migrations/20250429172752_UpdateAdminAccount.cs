using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace WebShop10.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminAccount : Migration
    {
        const string ADMIN_USER_GUID = "c7525ba2-655b-4d26-920e-f90cfddf40ea";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            StringBuilder sqlUser = new StringBuilder();

            sqlUser.AppendLine("UPDATE AspNetUsers");
            sqlUser.AppendLine("SET SecurityStamp = NEWID(),");
            sqlUser.AppendLine("ConcurrencyStamp = NEWID(),");
            sqlUser.AppendLine("LockoutEnabled = 1");
            sqlUser.AppendLine($"WHERE Id = '{ADMIN_USER_GUID}'");

            migrationBuilder.Sql(sqlUser.ToString());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
