using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace WebShop10.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminAccount : Migration
    {
        const string ADMIN_USER_GUID = "c7525ba2-655b-4d26-920e-f90cfddf40ea";
        const string ADMIN_ROLE_GUID = "fd453a7a-e86e-4522-9ac0-878f88aacf7a";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            var passwordHash = hasher.HashPassword(null, "Password12345.");

            StringBuilder sqlUser = new StringBuilder();

            sqlUser.AppendLine("INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, Address, FirstName, LastName)");
            sqlUser.AppendLine("VALUES (");
            sqlUser.AppendLine($"'{ADMIN_USER_GUID}'");     // Id
            sqlUser.AppendLine(", 'admin@admin.com'");      // UserName
            sqlUser.AppendLine(", 'ADMIN@ADMIN.COM'");      // NormalizedUserName
            sqlUser.AppendLine(", 'admin@admin.com'");      // Email
            sqlUser.AppendLine(", 'ADMIN@ADMIN.COM'");      // NormalizedEmail
            sqlUser.AppendLine(", 1");                      // EmailConfirmed
            sqlUser.AppendLine($", '{passwordHash}'");      // PasswordHash
            sqlUser.AppendLine(", NULL");                   // SecurityStamp
            sqlUser.AppendLine(", NULL");                   // ConcurrencyStamp
            sqlUser.AppendLine(", NULL");                   // PhoneNumber
            sqlUser.AppendLine(", 0");                      // PhoneNumberConfirmed
            sqlUser.AppendLine(", 0");                      // TwoFactorEnabled
            sqlUser.AppendLine(", NULL");                   // LockoutEnd
            sqlUser.AppendLine(", 0");                      // LockoutEnabled
            sqlUser.AppendLine(", 0");                      // AccessFailedCount
            sqlUser.AppendLine(", ''");                     // Address
            sqlUser.AppendLine(", 'Admin'");                // FirstName 
            sqlUser.AppendLine(", ''");                     // LastName
            sqlUser.AppendLine(")");

            migrationBuilder.Sql(sqlUser.ToString());

            migrationBuilder.Sql($"INSERT INTO AspNetRoles(Id, Name, NormalizedName) VALUES ('{ADMIN_ROLE_GUID}', 'Admin', 'ADMIN')");

            migrationBuilder.Sql($"INSERT INTO AspNetUserRoles(UserId, RoleId) VALUES ('{ADMIN_USER_GUID}', '{ADMIN_ROLE_GUID}')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM AspNetUserRoles WHERE UserId = '{ADMIN_USER_GUID}' AND RoleId = '{ADMIN_ROLE_GUID}'");

            migrationBuilder.Sql($"DELETE FROM AspNetRoles WHERE Id = '{ADMIN_ROLE_GUID}'");

            migrationBuilder.Sql($"DELETE FROM AspNetUsers WHERE Id = '{ADMIN_USER_GUID}'");
        }
    }
}
