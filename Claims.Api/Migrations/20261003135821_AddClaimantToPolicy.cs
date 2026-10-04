using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Claims.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddClaimantToPolicy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClaimantId",
                table: "Policies",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Policies_ClaimantId",
                table: "Policies",
                column: "ClaimantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Policies_Claimants_ClaimantId",
                table: "Policies",
                column: "ClaimantId",
                principalTable: "Claimants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Policies_Claimants_ClaimantId",
                table: "Policies");

            migrationBuilder.DropIndex(
                name: "IX_Policies_ClaimantId",
                table: "Policies");

            migrationBuilder.DropColumn(
                name: "ClaimantId",
                table: "Policies");
        }
    }
}
