using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDiplomDelivery.Migrations
{
    public partial class aaaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_AspNetUsers_UserId",
                table: "Delivery");

            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_Deliveryman_DeliverymanId",
                table: "Delivery");

            migrationBuilder.DropIndex(
                name: "IX_Delivery_UserId",
                table: "Delivery");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Delivery");

            migrationBuilder.RenameColumn(
                name: "DeliverymanId",
                table: "Delivery",
                newName: "Deliverymanid");

            migrationBuilder.RenameIndex(
                name: "IX_Delivery_DeliverymanId",
                table: "Delivery",
                newName: "IX_Delivery_Deliverymanid");

            migrationBuilder.AlterColumn<string>(
                name: "Deliverymanid",
                table: "Delivery",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Deliverymanid1",
                table: "Delivery",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_Deliverymanid1",
                table: "Delivery",
                column: "Deliverymanid1");

            migrationBuilder.AddForeignKey(
                name: "FK_Delivery_AspNetUsers_Deliverymanid",
                table: "Delivery",
                column: "Deliverymanid",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Delivery_Deliveryman_Deliverymanid1",
                table: "Delivery",
                column: "Deliverymanid1",
                principalTable: "Deliveryman",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_AspNetUsers_Deliverymanid",
                table: "Delivery");

            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_Deliveryman_Deliverymanid1",
                table: "Delivery");

            migrationBuilder.DropIndex(
                name: "IX_Delivery_Deliverymanid1",
                table: "Delivery");

            migrationBuilder.DropColumn(
                name: "Deliverymanid1",
                table: "Delivery");

            migrationBuilder.RenameColumn(
                name: "Deliverymanid",
                table: "Delivery",
                newName: "DeliverymanId");

            migrationBuilder.RenameIndex(
                name: "IX_Delivery_Deliverymanid",
                table: "Delivery",
                newName: "IX_Delivery_DeliverymanId");

            migrationBuilder.AlterColumn<int>(
                name: "DeliverymanId",
                table: "Delivery",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Delivery",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_UserId",
                table: "Delivery",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Delivery_AspNetUsers_UserId",
                table: "Delivery",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Delivery_Deliveryman_DeliverymanId",
                table: "Delivery",
                column: "DeliverymanId",
                principalTable: "Deliveryman",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
