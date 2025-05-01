using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalWalletAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSenderAndReceiverToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_WalletId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "WalletId",
                table: "Transactions",
                newName: "SenderWalletId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_WalletId",
                table: "Transactions",
                newName: "IX_Transactions_SenderWalletId");

            migrationBuilder.AddColumn<Guid>(
                name: "ReceiverWalletId",
                table: "Transactions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ReceiverWalletId",
                table: "Transactions",
                column: "ReceiverWalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_ReceiverWalletId",
                table: "Transactions",
                column: "ReceiverWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_SenderWalletId",
                table: "Transactions",
                column: "SenderWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_ReceiverWalletId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_SenderWalletId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ReceiverWalletId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ReceiverWalletId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "SenderWalletId",
                table: "Transactions",
                newName: "WalletId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_SenderWalletId",
                table: "Transactions",
                newName: "IX_Transactions_WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_WalletId",
                table: "Transactions",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
