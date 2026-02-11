using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZetaSaasHRMSBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "Menu",
            columns: new[] { "MenuName", "ParentMenuId", "RouteUrl", "Icon", "DisplayOrder" },
            values: new object[,]
            {
                  {
                    "User Profile",
                    "0",
                    "",
                    "fa-solid fa-address-card",
                    0.00
                  },
                  {
                    "My Profile",
                    "1",
                    "profile",
                    "<fa-solid fa-layer-group menu-icon",
                    0.10
                  },
                  {
                    "User Management",
                    "0",
                    "",
                    "fa-regular fa-user",
                    1.00
                  },
                  {
                    "User",
                    "3",
                    "user-list",
                    "fa-solid fa-layer-group menu-icon",
                    1.10
                  },
                  {
                    "Role",
                    "3",
                    "role-list",
                    "fa-solid fa-layer-group menu-icon",
                    1.20
                  },
                  {
                    "Employee Management",
                    "0",
                    "",
                    "fa-utility-fill fa-semibold fa-airplay",
                    2.00
                  },
                  {
                    "Employee",
                    "6",
                    "employee-list",
                    "fa-solid fa-layer-group menu-icon",
                    2.10
                  }

            }
          );


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
