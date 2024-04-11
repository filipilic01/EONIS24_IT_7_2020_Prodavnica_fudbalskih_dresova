using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    AdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdminUserName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AdminFirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AdminLastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AdminPassword = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    AdminEmail = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AdminPhoneNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AdminAddress = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.AdminId);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerUserName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CustomerFirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CustomerLastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CustomerPassword = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    CustomerEmail = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CustomerPhoneNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CustomerAddress = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Jerseys",
                columns: table => new
                {
                    JerseyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Team = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Season = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Competition = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TeamUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jerseys", x => x.JerseyId);
                    table.CheckConstraint("CHK_Status", "Status IN ('na stanju', 'nedostupno')");
                    table.ForeignKey(
                        name: "FK_Jerseys_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "AdminId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderTotalAmount = table.Column<double>(type: "float", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JerseySizes",
                columns: table => new
                {
                    JerseySizeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JerseySizeValue = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    JerseyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JerseySizes", x => x.JerseySizeId);
                    table.ForeignKey(
                        name: "FK_JerseySizes_Jerseys_JerseyId",
                        column: x => x.JerseyId,
                        principalTable: "Jerseys",
                        principalColumn: "JerseyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemNumber = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JerseySizeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_JerseySizes_JerseySizeId",
                        column: x => x.JerseySizeId,
                        principalTable: "JerseySizes",
                        principalColumn: "JerseySizeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_AdminEmail",
                table: "Admins",
                column: "AdminEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Admins_AdminUserName",
                table: "Admins",
                column: "AdminUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerEmail",
                table: "Customers",
                column: "CustomerEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerUserName",
                table: "Customers",
                column: "CustomerUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jerseys_AdminId",
                table: "Jerseys",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_JerseySizes_JerseyId",
                table: "JerseySizes",
                column: "JerseyId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_JerseySizeId",
                table: "OrderItems",
                column: "JerseySizeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "JerseySizes");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Jerseys");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Admins");
        }
    }
}
