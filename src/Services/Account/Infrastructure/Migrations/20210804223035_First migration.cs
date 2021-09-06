﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class Firstmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerSnapshots",
                columns: table => new
                {
                    AggregateVersion = table.Column<int>(type: "int", nullable: false),
                    AggregateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AggregateName = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    AggregateState = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSnapshots", x => new { x.AggregateVersion, x.AggregateId });
                });

            migrationBuilder.CreateTable(
                name: "CustomerStoreEvents",
                columns: table => new
                {
                    Version = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AggregateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AggregateName = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    DomainEventName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    DomainEvent = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerStoreEvents", x => x.Version);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerSnapshots");

            migrationBuilder.DropTable(
                name: "CustomerStoreEvents");
        }
    }
}
