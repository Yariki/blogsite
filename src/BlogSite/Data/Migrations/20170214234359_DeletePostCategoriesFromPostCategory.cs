using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogSite.Data.Migrations
{
    public partial class DeletePostCategoriesFromPostCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostCategories_PostCategories_PostCategoryID",
                table: "PostCategories");

            migrationBuilder.DropIndex(
                name: "IX_PostCategories_PostCategoryID",
                table: "PostCategories");

            migrationBuilder.DropColumn(
                name: "PostCategoryID",
                table: "PostCategories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostCategoryID",
                table: "PostCategories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostCategories_PostCategoryID",
                table: "PostCategories",
                column: "PostCategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategories_PostCategories_PostCategoryID",
                table: "PostCategories",
                column: "PostCategoryID",
                principalTable: "PostCategories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
