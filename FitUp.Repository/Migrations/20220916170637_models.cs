using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FitUp.Repository.Migrations
{
    public partial class models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Rate",
                table: "Coaches",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "num_raters",
                table: "Coaches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "active",
                table: "Clients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "allowed_times",
                table: "Clients",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "day_last_payment",
                table: "Clients",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "enteries",
                table: "Clients",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "num_raters",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "active",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "allowed_times",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "day_last_payment",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "enteries",
                table: "Clients");

            migrationBuilder.AlterColumn<int>(
                name: "Rate",
                table: "Coaches",
                type: "int",
                nullable: false,
                oldClrType: typeof(float));
        }
    }
}
