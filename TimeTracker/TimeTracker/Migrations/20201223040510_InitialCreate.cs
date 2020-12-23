using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeTracker.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DayWorkLimitTime",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LimitWorkTime = table.Column<double>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayWorkLimitTime", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "NonWorkDays",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NonWorkDate = table.Column<DateTime>(type: "Date", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonWorkDays", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Period",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "Date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "Date", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Period", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "TaskSource",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CodeName = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: false),
                    DisplayName = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskSource", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "TaskType",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    CodeName = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: false),
                    DisplayName = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskType", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeName = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: false),
                    DisplayName = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "TaskDay",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsLeave = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Date = table.Column<DateTime>(type: "Date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskDay", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_TaskDay_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "MapUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserRolesId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapUserRoles", x => new { x.UserId, x.UserRolesId });
                    table.ForeignKey(
                        name: "FK_MapUserRoles_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MapUserRoles_UserRole_UserRolesId",
                        column: x => x.UserRolesId,
                        principalTable: "UserRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsumeTime = table.Column<double>(type: "float", nullable: false),
                    TaskDayGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Date = table.Column<DateTime>(type: "Date", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    TaskTypeGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaskSourceGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaskName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TaskContent = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Task_TaskDay_TaskDayGuid",
                        column: x => x.TaskDayGuid,
                        principalTable: "TaskDay",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Task_TaskSource_TaskSourceGuid",
                        column: x => x.TaskSourceGuid,
                        principalTable: "TaskSource",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Task_TaskType_TaskTypeGuid",
                        column: x => x.TaskTypeGuid,
                        principalTable: "TaskType",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskTimeRange",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaskDayGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    Date = table.Column<DateTime>(type: "Date", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    TaskTypeGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaskSourceGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaskName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TaskContent = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTimeRange", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_TaskTimeRange_TaskDay_TaskDayGuid",
                        column: x => x.TaskDayGuid,
                        principalTable: "TaskDay",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TaskTimeRange_TaskSource_TaskSourceGuid",
                        column: x => x.TaskSourceGuid,
                        principalTable: "TaskSource",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskTimeRange_TaskType_TaskTypeGuid",
                        column: x => x.TaskTypeGuid,
                        principalTable: "TaskType",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "DayWorkLimitTime",
                columns: new[] { "Guid", "LimitWorkTime" },
                values: new object[] { new Guid("0eff278d-f5d8-48c2-ae77-b1c7831fadcd"), 7.5 });

            migrationBuilder.InsertData(
                table: "Period",
                columns: new[] { "Guid", "EndDate", "Name", "StartDate" },
                values: new object[] { new Guid("b52ae551-5541-4894-b1f3-675d6594a963"), new DateTime(2020, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test1", new DateTime(2020, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "TaskSource",
                columns: new[] { "Guid", "CodeName", "DisplayName", "IsActive" },
                values: new object[,]
                {
                    { new Guid("05d51407-4082-4746-a816-463106f68f1a"), "Boss", "Boss", true },
                    { new Guid("28fce02f-146d-4bc0-8eb4-abffd63f8d12"), "default", "", true },
                    { new Guid("dcfae46b-a036-4b30-aa6d-80e6b1535d15"), "Mother", "Mother", true },
                    { new Guid("e4c9c211-b0b3-437e-9eb8-cf418b54d262"), "Girl friend", "Girl friend", true },
                    { new Guid("6d5f7be6-e44d-4121-be95-d8216700784c"), "Father", "Father", true }
                });

            migrationBuilder.InsertData(
                table: "TaskType",
                columns: new[] { "Guid", "CodeName", "DisplayName", "IsActive" },
                values: new object[,]
                {
                    { new Guid("c74bea15-cadf-4f73-b56d-9af80676b24a"), "Wash Floors", "Wash Floors", true },
                    { new Guid("b44afdaa-4c82-45a8-b0e2-9dc7017973d1"), "Play Baseball", "Play Baseball", true },
                    { new Guid("bc9c35bc-1a57-4422-905a-278c5c96747e"), "Shopping", "Shopping", true },
                    { new Guid("7e8aef7a-3a2d-4023-b330-17b02c6f043f"), "Coding", "Coding", true },
                    { new Guid("bf7a6ca5-8202-4f2c-ba1e-637d07b432c9"), "default", "", true },
                    { new Guid("4f3acf7f-7cc4-4a76-bd07-7bbeabd2882a"), "Play PC Game", "Play PC Game", true },
                    { new Guid("533c120c-a1aa-4182-926f-943fccfc7a15"), "Reading", "Reading", true }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccountStatus", "Email", "Guid", "Name", "PasswordHash" },
                values: new object[,]
                {
                    { 4, 3, "test2@auth.com", new Guid("bec552b9-3d14-43ba-a927-2e3d6a2da1a6"), "Test2", "$MYHASH$V1$10000$Xx63Yky8pmBz3DhPHYuyPV+MY/5sTfaLt8KMHX5gM5RLtI7V" },
                    { 3, 0, "test@auth.com", new Guid("b0823635-86a3-4dd4-afbd-bffb7f69d46d"), "Test", "$MYHASH$V1$10000$zrh46ZLlGqzln9eQ8uOQN7yvUde17uCWc7xKlZ1tVtad3FLc" },
                    { 2, 1, "user@auth.com", new Guid("e14cf273-17a2-44f3-a7f5-0e6d3e7522dc"), "User", "$MYHASH$V1$10000$4iD4Ab5Z+nwCj5QpRoSRQ07iZ/dCZjriZ+TAa1b5fVYA+tyc" },
                    { 1, 1, "admin@auth.com", new Guid("095eaced-2cd8-4498-b061-9922370536eb"), "Admin", "$MYHASH$V1$10000$QNfbryMVH4/NtgniuzQy2hU8EIclMgA3kHT2vAjg9cr4O8Dk" }
                });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "Id", "CodeName", "DisplayName" },
                values: new object[,]
                {
                    { 2, "User", "User" },
                    { 1, "Admin", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "MapUserRoles",
                columns: new[] { "UserId", "UserRolesId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 2 },
                    { 4, 2 }
                });

            migrationBuilder.InsertData(
                table: "TaskDay",
                columns: new[] { "Guid", "Date", "IsLeave", "UserId" },
                values: new object[,]
                {
                    { new Guid("9c903286-0816-4800-a04c-a04774f2e9d9"), new DateTime(2020, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1 },
                    { new Guid("0232dc59-fca7-415c-ab8d-354a3efe39d0"), new DateTime(2020, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1 },
                    { new Guid("f7d1bef4-f5d2-4e19-a22a-bceda34c5201"), new DateTime(2020, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1 },
                    { new Guid("6063bb59-a6c5-4ab5-90ed-b1e47fbe4429"), new DateTime(2020, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1 },
                    { new Guid("69af239c-2be5-4f17-8ac8-54b903b26fcc"), new DateTime(2020, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1 },
                    { new Guid("d118d32e-119c-43aa-8c58-8d538dcb044a"), new DateTime(2020, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1 },
                    { new Guid("3db25699-dbb4-4887-8d78-048edf9699b2"), new DateTime(2020, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2 },
                    { new Guid("e41347d1-4637-4686-98f9-ee24034099ff"), new DateTime(2020, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2 },
                    { new Guid("5ac060a0-f33f-412e-ab3d-237927e953e1"), new DateTime(2020, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2 }
                });

            migrationBuilder.InsertData(
                table: "Task",
                columns: new[] { "Guid", "ConsumeTime", "Date", "DisplayOrder", "TaskContent", "TaskDayGuid", "TaskName", "TaskSourceGuid", "TaskTypeGuid" },
                values: new object[,]
                {
                    { new Guid("c1e81b33-8819-4e12-be9b-8a494748b644"), 4.0, new DateTime(2020, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Architecture design", new Guid("9c903286-0816-4800-a04c-a04774f2e9d9"), "Track Time", new Guid("05d51407-4082-4746-a816-463106f68f1a"), new Guid("7e8aef7a-3a2d-4023-b330-17b02c6f043f") },
                    { new Guid("8c74bd17-2f8d-4c46-be24-be446fbd44d4"), 3.0, new DateTime(2020, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "", new Guid("9c903286-0816-4800-a04c-a04774f2e9d9"), "FEZ", new Guid("28fce02f-146d-4bc0-8eb4-abffd63f8d12"), new Guid("4f3acf7f-7cc4-4a76-bd07-7bbeabd2882a") },
                    { new Guid("a52433ff-268f-41f7-a0bb-2cdb409ab696"), 3.0, new DateTime(2020, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "AAAAA", new Guid("0232dc59-fca7-415c-ab8d-354a3efe39d0"), "", new Guid("28fce02f-146d-4bc0-8eb4-abffd63f8d12"), new Guid("bf7a6ca5-8202-4f2c-ba1e-637d07b432c9") },
                    { new Guid("0db027e5-2cba-478e-91f4-ab62eb3b01e4"), 0.0, new DateTime(2020, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "", new Guid("0232dc59-fca7-415c-ab8d-354a3efe39d0"), "BBBBBB", new Guid("28fce02f-146d-4bc0-8eb4-abffd63f8d12"), new Guid("bf7a6ca5-8202-4f2c-ba1e-637d07b432c9") },
                    { new Guid("a13a6b6a-914e-4605-b13e-e2a97781b26e"), 2.0, new DateTime(2020, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "hooooooooooo", new Guid("f7d1bef4-f5d2-4e19-a22a-bceda34c5201"), "Yeah", new Guid("6d5f7be6-e44d-4121-be95-d8216700784c"), new Guid("b44afdaa-4c82-45a8-b0e2-9dc7017973d1") },
                    { new Guid("5900e184-21e7-417c-8560-ba32c0e60b44"), 4.5, new DateTime(2020, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "", new Guid("f7d1bef4-f5d2-4e19-a22a-bceda34c5201"), "", new Guid("05d51407-4082-4746-a816-463106f68f1a"), new Guid("bf7a6ca5-8202-4f2c-ba1e-637d07b432c9") },
                    { new Guid("649d8897-75a6-42fa-b019-e932a351353f"), 4.0, new DateTime(2020, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "dadadadadada", new Guid("f7d1bef4-f5d2-4e19-a22a-bceda34c5201"), "FEZ", new Guid("6d5f7be6-e44d-4121-be95-d8216700784c"), new Guid("4f3acf7f-7cc4-4a76-bd07-7bbeabd2882a") },
                    { new Guid("90950112-6c55-456d-9bec-fc977f1dbff4"), 1.5, new DateTime(2020, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "cccCcccCcccC", new Guid("f7d1bef4-f5d2-4e19-a22a-bceda34c5201"), "CCCCCCCCCC", new Guid("28fce02f-146d-4bc0-8eb4-abffd63f8d12"), new Guid("bf7a6ca5-8202-4f2c-ba1e-637d07b432c9") },
                    { new Guid("16e9da49-f4ff-497c-a5e5-e98c31e5ca58"), 7.0, new DateTime(2020, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "", new Guid("6063bb59-a6c5-4ab5-90ed-b1e47fbe4429"), "", new Guid("28fce02f-146d-4bc0-8eb4-abffd63f8d12"), new Guid("bf7a6ca5-8202-4f2c-ba1e-637d07b432c9") },
                    { new Guid("24ea4b7f-f8e2-452d-8edf-5d160524d251"), 4.5, new DateTime(2020, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "", new Guid("69af239c-2be5-4f17-8ac8-54b903b26fcc"), "", new Guid("28fce02f-146d-4bc0-8eb4-abffd63f8d12"), new Guid("bf7a6ca5-8202-4f2c-ba1e-637d07b432c9") },
                    { new Guid("86191f6c-e0e5-4d57-a554-5f9531e1df3e"), 3.0, new DateTime(2020, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "", new Guid("3db25699-dbb4-4887-8d78-048edf9699b2"), "", new Guid("28fce02f-146d-4bc0-8eb4-abffd63f8d12"), new Guid("bf7a6ca5-8202-4f2c-ba1e-637d07b432c9") },
                    { new Guid("07bf1e95-8bb9-4561-b240-5d602160cedb"), 2.0, new DateTime(2020, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "", new Guid("e41347d1-4637-4686-98f9-ee24034099ff"), "", new Guid("28fce02f-146d-4bc0-8eb4-abffd63f8d12"), new Guid("bf7a6ca5-8202-4f2c-ba1e-637d07b432c9") },
                    { new Guid("9474ad51-3726-4242-87f7-903550bcbd4a"), 3.0, new DateTime(2020, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "", new Guid("e41347d1-4637-4686-98f9-ee24034099ff"), "", new Guid("28fce02f-146d-4bc0-8eb4-abffd63f8d12"), new Guid("bf7a6ca5-8202-4f2c-ba1e-637d07b432c9") },
                    { new Guid("c362df5f-ab37-4638-b430-3933c84ca33e"), 5.0, new DateTime(2020, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "", new Guid("5ac060a0-f33f-412e-ab3d-237927e953e1"), "", new Guid("28fce02f-146d-4bc0-8eb4-abffd63f8d12"), new Guid("bf7a6ca5-8202-4f2c-ba1e-637d07b432c9") }
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
                name: "IX_MapUserRoles_UserRolesId",
                table: "MapUserRoles",
                column: "UserRolesId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_TaskDayGuid",
                table: "Task",
                column: "TaskDayGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Task_TaskSourceGuid",
                table: "Task",
                column: "TaskSourceGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Task_TaskTypeGuid",
                table: "Task",
                column: "TaskTypeGuid");

            migrationBuilder.CreateIndex(
                name: "IX_TaskDay_UserId",
                table: "TaskDay",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskSource_CodeName",
                table: "TaskSource",
                column: "CodeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskTimeRange_TaskDayGuid",
                table: "TaskTimeRange",
                column: "TaskDayGuid");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTimeRange_TaskSourceGuid",
                table: "TaskTimeRange",
                column: "TaskSourceGuid");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTimeRange_TaskTypeGuid",
                table: "TaskTimeRange",
                column: "TaskTypeGuid");

            migrationBuilder.CreateIndex(
                name: "IX_TaskType_CodeName",
                table: "TaskType",
                column: "CodeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Guid",
                table: "User",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_CodeName",
                table: "UserRole",
                column: "CodeName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "DayWorkLimitTime");

            migrationBuilder.DropTable(
                name: "MapUserRoles");

            migrationBuilder.DropTable(
                name: "NonWorkDays");

            migrationBuilder.DropTable(
                name: "Period");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "TaskTimeRange");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "TaskDay");

            migrationBuilder.DropTable(
                name: "TaskSource");

            migrationBuilder.DropTable(
                name: "TaskType");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
