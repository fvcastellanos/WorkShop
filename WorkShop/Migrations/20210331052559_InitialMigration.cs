using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkShop.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "workshop");

            migrationBuilder.CreateTable(
                name: "discount_type",
                schema: "workshop",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(50)", nullable: false),
                    name = table.Column<string>(type: "varchar(150)", nullable: false),
                    description = table.Column<string>(type: "varchar(300)", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    active = table.Column<int>(nullable: false, defaultValue: 1),
                    tenant = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discount_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "operation_type",
                schema: "workshop",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(50)", nullable: false),
                    name = table.Column<string>(type: "varchar(150)", nullable: false),
                    description = table.Column<string>(type: "varchar(300)", nullable: true),
                    inbound = table.Column<int>(nullable: false, defaultValueSql: "0"),
                    created = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    active = table.Column<int>(nullable: false, defaultValue: 1),
                    tenant = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operation_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                schema: "workshop",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(50)", nullable: false),
                    code = table.Column<string>(type: "varchar(50)", nullable: false),
                    name = table.Column<string>(type: "varchar(150)", nullable: false),
                    description = table.Column<string>(type: "varchar(300)", nullable: true),
                    minimal_amount = table.Column<int>(nullable: false, defaultValue: 0),
                    sale_price = table.Column<double>(nullable: false),
                    created = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    active = table.Column<int>(nullable: false, defaultValue: 1),
                    tenant = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "provider",
                schema: "workshop",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(50)", nullable: false),
                    code = table.Column<string>(type: "varchar(50)", nullable: false),
                    name = table.Column<string>(type: "varchar(150)", nullable: false),
                    contact = table.Column<string>(type: "varchar(150)", nullable: true),
                    tax_id = table.Column<string>(type: "varchar(50)", nullable: true),
                    description = table.Column<string>(type: "varchar(300)", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    active = table.Column<int>(nullable: false, defaultValue: 1),
                    tenant = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provider", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "provider_invoice",
                schema: "workshop",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(50)", nullable: false),
                    provider_id = table.Column<string>(nullable: true),
                    suffix = table.Column<string>(type: "varchar(30)", nullable: false),
                    number = table.Column<string>(type: "varchar(100)", nullable: false),
                    image_url = table.Column<string>(type: "varchar(250)", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    tenant = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provider_invoice", x => x.id);
                    table.ForeignKey(
                        name: "FK_provider_invoice_provider_provider_id",
                        column: x => x.provider_id,
                        principalSchema: "workshop",
                        principalTable: "provider",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "inventory",
                schema: "workshop",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(50)", nullable: false),
                    product_id = table.Column<string>(nullable: true),
                    operation_type_id = table.Column<string>(nullable: true),
                    provider_invoice_id = table.Column<string>(nullable: true),
                    amount = table.Column<double>(nullable: false, defaultValue: 1.0),
                    unit_price = table.Column<double>(nullable: false),
                    discount_type_id = table.Column<string>(nullable: true),
                    discount_value = table.Column<double>(nullable: false),
                    total = table.Column<double>(nullable: false),
                    created = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    tenant = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory", x => x.id);
                    table.ForeignKey(
                        name: "FK_inventory_discount_type_discount_type_id",
                        column: x => x.discount_type_id,
                        principalSchema: "workshop",
                        principalTable: "discount_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_inventory_operation_type_operation_type_id",
                        column: x => x.operation_type_id,
                        principalSchema: "workshop",
                        principalTable: "operation_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_inventory_product_product_id",
                        column: x => x.product_id,
                        principalSchema: "workshop",
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_inventory_provider_invoice_provider_invoice_id",
                        column: x => x.provider_invoice_id,
                        principalSchema: "workshop",
                        principalTable: "provider_invoice",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "idx_discount_type_created",
                schema: "workshop",
                table: "discount_type",
                column: "created");

            migrationBuilder.CreateIndex(
                name: "idx_discount_type_tenant",
                schema: "workshop",
                table: "discount_type",
                column: "tenant");

            migrationBuilder.CreateIndex(
                name: "idx_discount_type_updated",
                schema: "workshop",
                table: "discount_type",
                column: "updated");

            migrationBuilder.CreateIndex(
                name: "idx_inventory_created",
                schema: "workshop",
                table: "inventory",
                column: "created");

            migrationBuilder.CreateIndex(
                name: "idx_inventory_tenant",
                schema: "workshop",
                table: "inventory",
                column: "tenant");

            migrationBuilder.CreateIndex(
                name: "idx_inventory_updated",
                schema: "workshop",
                table: "inventory",
                column: "updated");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_discount_type_id",
                schema: "workshop",
                table: "inventory",
                column: "discount_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_operation_type_id",
                schema: "workshop",
                table: "inventory",
                column: "operation_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_product_id",
                schema: "workshop",
                table: "inventory",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_provider_invoice_id",
                schema: "workshop",
                table: "inventory",
                column: "provider_invoice_id");

            migrationBuilder.CreateIndex(
                name: "idx_operation_type_created",
                schema: "workshop",
                table: "operation_type",
                column: "created");

            migrationBuilder.CreateIndex(
                name: "idx_operation_type_inbound",
                schema: "workshop",
                table: "operation_type",
                column: "inbound");

            migrationBuilder.CreateIndex(
                name: "idx_operation_type_tenant",
                schema: "workshop",
                table: "operation_type",
                column: "tenant");

            migrationBuilder.CreateIndex(
                name: "idx_operation_type_updated",
                schema: "workshop",
                table: "operation_type",
                column: "updated");

            migrationBuilder.CreateIndex(
                name: "uq_product_code",
                schema: "workshop",
                table: "product",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_product_created",
                schema: "workshop",
                table: "product",
                column: "created");

            migrationBuilder.CreateIndex(
                name: "idx_product_tenant",
                schema: "workshop",
                table: "product",
                column: "tenant");

            migrationBuilder.CreateIndex(
                name: "idx_product_updated",
                schema: "workshop",
                table: "product",
                column: "updated");

            migrationBuilder.CreateIndex(
                name: "uq_provider_code",
                schema: "workshop",
                table: "provider",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_provider_created",
                schema: "workshop",
                table: "provider",
                column: "created");

            migrationBuilder.CreateIndex(
                name: "idx_provider_tax_id",
                schema: "workshop",
                table: "provider",
                column: "tax_id");

            migrationBuilder.CreateIndex(
                name: "idx_provider_tenant",
                schema: "workshop",
                table: "provider",
                column: "tenant");

            migrationBuilder.CreateIndex(
                name: "idx_provider_updated",
                schema: "workshop",
                table: "provider",
                column: "updated");

            migrationBuilder.CreateIndex(
                name: "idx_provider_invoice_created",
                schema: "workshop",
                table: "provider_invoice",
                column: "created");

            migrationBuilder.CreateIndex(
                name: "idx_provider_invoice_tenant",
                schema: "workshop",
                table: "provider_invoice",
                column: "tenant");

            migrationBuilder.CreateIndex(
                name: "idx_provider_invoice_updated",
                schema: "workshop",
                table: "provider_invoice",
                column: "updated");

            migrationBuilder.CreateIndex(
                name: "IX_provider_invoice_provider_id",
                schema: "workshop",
                table: "provider_invoice",
                column: "provider_id");

            migrationBuilder.CreateIndex(
                name: "idx_provider_invoice_number",
                schema: "workshop",
                table: "provider_invoice",
                columns: new[] { "suffix", "number" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inventory",
                schema: "workshop");

            migrationBuilder.DropTable(
                name: "discount_type",
                schema: "workshop");

            migrationBuilder.DropTable(
                name: "operation_type",
                schema: "workshop");

            migrationBuilder.DropTable(
                name: "product",
                schema: "workshop");

            migrationBuilder.DropTable(
                name: "provider_invoice",
                schema: "workshop");

            migrationBuilder.DropTable(
                name: "provider",
                schema: "workshop");
        }
    }
}
