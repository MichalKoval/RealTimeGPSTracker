using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Migrations;
using RealtimeGpsTracker.Infrastructure.Data.EntityFramework.Extensions;

namespace RealtimeGpsTracker.Infrastructure.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adding tables to the database (custom extension method for MigrationBuilder)
            migrationBuilder.AddTablesAndIndexes();

            //Adding Sql scripts (custom extension method for MigrationBuilder)
            migrationBuilder.AddSqlScripts();

            //Adding initial data to database (custom extension method for MigrationBuilder)
            migrationBuilder.AddSqlSeedData();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Removing old tables from the database (custom extension method for MigrationBuilder)
            migrationBuilder.RemoveOldTables();
        }
    }
}
