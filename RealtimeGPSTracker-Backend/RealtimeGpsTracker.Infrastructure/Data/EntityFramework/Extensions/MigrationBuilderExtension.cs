using Microsoft.EntityFrameworkCore.Migrations;
using RealtimeGpsTracker.Infrastructure.Data.EntityFramework.SqlScripts;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealtimeGpsTracker.Infrastructure.Data.EntityFramework.Extensions
{
    public static class MigrationBuilderExtension
    {
        public static MigrationBuilder AddTablesAndIndexes(this MigrationBuilder migrationBuilder)
        {
            // Adding tables to the database
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GpsDevices",
                columns: table => new
                {
                    GpsDeviceId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Color = table.Column<string>(maxLength: 16, nullable: true),
                    Interval = table.Column<int>(nullable: false),
                    IntervalChanged = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GpsDevices", x => x.GpsDeviceId);
                    table.ForeignKey(
                        name: "FK_GpsDevices_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    TripId = table.Column<string>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    Distance = table.Column<int>(nullable: false),
                    InProgress = table.Column<bool>(nullable: false),
                    GpsDeviceId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.TripId);
                    table.ForeignKey(
                        name: "FK_Trips_GpsDevices_GpsDeviceId",
                        column: x => x.GpsDeviceId,
                        principalTable: "GpsDevices",
                        principalColumn: "GpsDeviceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trips_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GpsCoordinates",
                columns: table => new
                {
                    GpsCoordinateId = table.Column<string>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    Lt = table.Column<string>(maxLength: 64, nullable: true),
                    Lg = table.Column<string>(maxLength: 64, nullable: true),
                    Speed = table.Column<string>(maxLength: 64, nullable: true),
                    TripId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GpsCoordinates", x => x.GpsCoordinateId);
                    table.ForeignKey(
                        name: "FK_GpsCoordinates_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "TripId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GpsCoordinates_TripId",
                table: "GpsCoordinates",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_GpsDevices_UserId",
                table: "GpsDevices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_GpsDeviceId",
                table: "Trips",
                column: "GpsDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_UserId",
                table: "Trips",
                column: "UserId");

            return migrationBuilder;
        }

        public static MigrationBuilder AddSqlScripts(this MigrationBuilder migrationBuilder)
        {
            //Adding data types to the database
            migrationBuilder.Sql(DataType.GpsCoordinatesType);

            //Adding functions to the database
            migrationBuilder.Sql(Function.SplitStringByDelimiter);

            //Adding stored procedures to the database
            migrationBuilder.Sql(StoredProcedure.AspNetUsers_Delete);
            migrationBuilder.Sql(StoredProcedure.CreateCoordinatesTable);
            migrationBuilder.Sql(StoredProcedure.CreateNewCoordinatesTableWithTimestamp);
            migrationBuilder.Sql(StoredProcedure.GpsCoordinates_Delete);
            migrationBuilder.Sql(StoredProcedure.GpsCoordinates_DeleteByTrip);
            migrationBuilder.Sql(StoredProcedure.GpsCoordinates_GetData);
            migrationBuilder.Sql(StoredProcedure.GpsCoordinates_Insert);
            migrationBuilder.Sql(StoredProcedure.GpsDevice_AspNetUserId);
            migrationBuilder.Sql(StoredProcedure.GpsDevice_Exists);
            migrationBuilder.Sql(StoredProcedure.GpsDevices_GetCount);
            migrationBuilder.Sql(StoredProcedure.GpsDevices_Delete);
            migrationBuilder.Sql(StoredProcedure.GpsDevices_RefreshStatus);
            migrationBuilder.Sql(StoredProcedure.Trips_Delete);

            return migrationBuilder;
        }
        public static MigrationBuilder AddSqlSeedData(this MigrationBuilder migrationBuilder)
        {
            //Order of called scripts matter !!!
            
            //Adding users data to the database
            migrationBuilder.Sql(DataSeed.Users);

            //Adding devices data to the database
            migrationBuilder.Sql(DataSeed.Devices);

            //Adding trips data to the database
            migrationBuilder.Sql(DataSeed.Trips);

            //Adding coordinates partitioning history (how tables with coordinates are partitioned) to the database
            migrationBuilder.Sql(DataSeed.CoordinatesPartitioningHistory);

            //Adding coordinates data to the database (Must be done as last one !!!)
            migrationBuilder.Sql(DataSeed.Coordinates);

            return migrationBuilder;
        }

        public static MigrationBuilder RemoveOldTables(this MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "GpsCoordinates");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "GpsDevices");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            return migrationBuilder;
        }
    }
}
