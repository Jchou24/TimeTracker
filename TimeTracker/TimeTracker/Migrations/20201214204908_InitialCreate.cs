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
                    DisplayName = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: true)
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
                    DisplayName = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: true)
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
                values: new object[] { new Guid("fb67e5cc-90c1-4394-b579-4b8203be3374"), 7.5 });

            migrationBuilder.InsertData(
                table: "TaskSource",
                columns: new[] { "Guid", "CodeName", "DisplayName" },
                values: new object[,]
                {
                    { new Guid("97af70b3-f5c9-4e9d-964c-3d1c713d5b96"), "Girl friend", "Girl friend" },
                    { new Guid("fbf9bcfd-f443-4d01-8364-6d50337714ab"), "default", "" },
                    { new Guid("8ad3a826-e524-41ca-a305-e4eec449f39c"), "Boss", "Boss" },
                    { new Guid("3d3828da-eb55-4625-88dc-854193f1bf3e"), "Mother", "Mother" },
                    { new Guid("7626436e-596a-42f2-8053-3e9b1c696f23"), "Father", "Father" }
                });

            migrationBuilder.InsertData(
                table: "TaskType",
                columns: new[] { "Guid", "CodeName", "DisplayName" },
                values: new object[,]
                {
                    { new Guid("c8d080e7-4c12-4d68-a2ab-3800db829248"), "Wash Floors", "Wash Floors" },
                    { new Guid("f680f01c-9b4e-4004-8231-3164e9b96d2b"), "Play Baseball", "Play Baseball" },
                    { new Guid("fbbde38a-6f1e-4869-92c7-610e0801193c"), "Shopping", "Shopping" },
                    { new Guid("542c0388-5e13-4036-900a-8e2d335eabb1"), "Coding", "Coding" },
                    { new Guid("d0143f5b-f764-42b8-be69-0991db5e68ea"), "default", "" },
                    { new Guid("fc572033-ca5d-434e-b1b0-cbe569ad774a"), "Play PC Game", "Play PC Game" },
                    { new Guid("42d5aa55-139a-433a-8d58-f034ad8a7e05"), "Reading", "Reading" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccountStatus", "Email", "Guid", "Name", "PasswordHash" },
                values: new object[,]
                {
                    { 4, 3, "test2@auth.com", new Guid("fa4ab26e-9506-4e7d-9c32-6193a1e4a218"), "Test2", "$MYHASH$V1$10000$ZG8AWlu88pNNOy+4wknMosIV9aeAuDitji2BCPmfUNoVmpMK" },
                    { 3, 0, "test@auth.com", new Guid("9b750880-3538-4b39-9d91-d0477c605cab"), "Test", "$MYHASH$V1$10000$KQvY9JgfmUrdjFBAAX8PHSNrYJOxuWiJL1NL6isDkO5UqL0a" },
                    { 2, 1, "user@auth.com", new Guid("dbeed58e-29b2-476f-b22b-00df38516427"), "User", "$MYHASH$V1$10000$dGx1xEJAWbvpyfwpNjMFcQn18GFQYZavCU4e3SFgVA/1Dmk7" },
                    { 1, 1, "admin@auth.com", new Guid("3ebe55e4-236e-4782-8df0-5d32f01582f2"), "Admin", "$MYHASH$V1$10000$oGVbCMGhuybVZjC/4kZyQC2jS2pAP/rlz3iQ+RbL/ZOwp0Av" }
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
