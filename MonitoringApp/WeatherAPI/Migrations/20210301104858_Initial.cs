﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherAPI.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherForecast",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlaceName = table.Column<string>(type: "TEXT", nullable: true),
                    DailyTemperature = table.Column<sbyte>(type: "INTEGER", nullable: false),
                    MaxWeeklyTemperature = table.Column<sbyte>(type: "INTEGER", nullable: false),
                    MinWeeklyTemperature = table.Column<sbyte>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherForecast", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherForecast");
        }
    }
}
