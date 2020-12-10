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
                values: new object[] { new Guid("04cbfe94-9a64-473c-b984-2e0d4de82042"), 7.5 });

            migrationBuilder.InsertData(
                table: "TaskSource",
                columns: new[] { "Guid", "CodeName", "DisplayName" },
                values: new object[,]
                {
                    { new Guid("0ea3fc61-cb91-4d99-96ba-02bfdc21daa5"), "Girl friend", "Girl friend" },
                    { new Guid("9c44500e-c1a3-4e9f-92be-4838721deaea"), "Mother", "Mother" },
                    { new Guid("7f17e9b1-a6dd-45a5-a415-271252e46d1b"), "Boss", "Boss" },
                    { new Guid("970dd577-c8fc-4925-9197-2aa7c0637b5e"), "Father", "Father" }
                });

            migrationBuilder.InsertData(
                table: "TaskType",
                columns: new[] { "Guid", "CodeName", "DisplayName" },
                values: new object[,]
                {
                    { new Guid("05f9eaad-59e3-4558-80c3-00752a0fb809"), "Wash Floors", "Wash Floors" },
                    { new Guid("97ea913a-a096-4ea6-8b44-a52bb56a20ee"), "Play Baseball", "Play Baseball" },
                    { new Guid("2d31d15c-4b05-4399-a934-6408f5a4a092"), "Shopping", "Shopping" },
                    { new Guid("72c52a8b-7f53-404f-a3de-3803f41ec2fb"), "Coding", "Coding" },
                    { new Guid("e9b0d908-80ac-44c6-b8d3-93f2f88b54e7"), "Play PC Game", "Play PC Game" },
                    { new Guid("aaf76239-c469-4826-aa6f-ef328cf3a0bc"), "Reading", "Reading" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccountStatus", "Email", "Guid", "Name", "PasswordHash" },
                values: new object[,]
                {
                    { 4, 3, "test2@auth.com", new Guid("aede61c4-5476-47f0-b642-79f46197ae63"), "Test2", "$MYHASH$V1$10000$7nZEfAf98N3l2N5CFgMC56m3o3z0uFSsmK1CBU+nwsK8vqlH" },
                    { 3, 0, "test@auth.com", new Guid("0377a7c4-8d91-4b78-9436-3adf8e9687c7"), "Test", "$MYHASH$V1$10000$KVyXX69Ud4VKailitOg3Gq4SdMFzIuLxGuv3O5tRNcfu3g0C" },
                    { 2, 1, "user@auth.com", new Guid("b5fda9b8-98e1-43ff-abca-a32639244df1"), "User", "$MYHASH$V1$10000$mBhmoyztuhebEYYoGcpx2MAo/5dlIR6pAb3zA3+6BKVAJoyR" },
                    { 1, 1, "admin@auth.com", new Guid("bf33fbaa-38b8-4055-a9c5-9ef6ce547f66"), "Admin", "$MYHASH$V1$10000$O1gJccN+bi4FDasghs0d9t7E7wYyPzoCUVMu3AsZqLCkSPgJ" }
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
