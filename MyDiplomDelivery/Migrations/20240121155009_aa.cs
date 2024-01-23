using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDiplomDelivery.Migrations
{
    public partial class aa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDetail_DeliveryId",
                table: "DeliveryDetail",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDetail_OrderId",
                table: "DeliveryDetail",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_DeliverymanId",
                table: "Delivery",
                column: "DeliverymanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Delivery_Deliveryman_DeliverymanId",
                table: "Delivery",
                column: "DeliverymanId",
                principalTable: "Deliveryman",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryDetail_Delivery_DeliveryId",
                table: "DeliveryDetail",
                column: "DeliveryId",
                principalTable: "Delivery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryDetail_Order_OrderId",
                table: "DeliveryDetail",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_Deliveryman_DeliverymanId",
                table: "Delivery");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryDetail_Delivery_DeliveryId",
                table: "DeliveryDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryDetail_Order_OrderId",
                table: "DeliveryDetail");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryDetail_DeliveryId",
                table: "DeliveryDetail");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryDetail_OrderId",
                table: "DeliveryDetail");

            migrationBuilder.DropIndex(
                name: "IX_Delivery_DeliverymanId",
                table: "Delivery");
        }
    }
}
