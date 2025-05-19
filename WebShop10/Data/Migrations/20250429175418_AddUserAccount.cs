using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace WebShop10.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAccount : Migration
    {
        const string USER_GUID = "76f3624d-fc24-4f04-95ce-9c2aa794285b";
        const string ROLE_GUID = "52a6f6fd-cffd-4d11-8a46-6ef4499e954e";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            var passwordHash = hasher.HashPassword(null, "Pass12345.");

            StringBuilder sqlUser = new StringBuilder();

            sqlUser.AppendLine("INSERT INTO AspNetUsers(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, Address, FirstName, LastName)");
            sqlUser.AppendLine("VALUES (");
            sqlUser.AppendLine($"'{USER_GUID}'");           // Id
            sqlUser.AppendLine(", 'user@user.com'");        // UserName
            sqlUser.AppendLine(", 'USER@USER.COM'");        // NormalizedUserName
            sqlUser.AppendLine(", 'user@user.com'");        // Email
            sqlUser.AppendLine(", 'USER@USER.COM'");        // NormalizedEmail
            sqlUser.AppendLine(", 1");                      // EmailConfirmed
            sqlUser.AppendLine($", '{passwordHash}'");      // PasswordHash
            sqlUser.AppendLine(", NEWID()");                // SecurityStamp
            sqlUser.AppendLine(", NEWID()");                // ConcurrencyStamp
            sqlUser.AppendLine(", NULL");                   // PhoneNumber
            sqlUser.AppendLine(", 0");                      // PhoneNumberConfirmed
            sqlUser.AppendLine(", 0");                      // TwoFactorEnabled
            sqlUser.AppendLine(", NULL");                   // LockoutEnd
            sqlUser.AppendLine(", 1");                      // LockoutEnabled
            sqlUser.AppendLine(", 0");                      // AccessFailedCount
            sqlUser.AppendLine(", ''");                     // Address
            sqlUser.AppendLine(", 'User'");                 // FirstName 
            sqlUser.AppendLine(", ''");                     // LastName
            sqlUser.AppendLine(")");

            migrationBuilder.Sql(sqlUser.ToString());

            migrationBuilder.Sql($"INSERT INTO AspNetRoles(Id, Name, NormalizedName) VALUES ('{ROLE_GUID}', 'User', 'USER')");

            migrationBuilder.Sql($"INSERT INTO AspNetUserRoles(UserId, RoleId) VALUES ('{USER_GUID}', '{ROLE_GUID}')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM AspNetUserRoles WHERE UserId = '{USER_GUID}' AND RoleId = '{ROLE_GUID}'");

            migrationBuilder.Sql($"DELETE FROM AspNetRoles WHERE Id = '{ROLE_GUID}'");

            migrationBuilder.Sql($"DELETE FROM AspNetUsers WHERE Id = '{USER_GUID}'");
        }
    }
}
