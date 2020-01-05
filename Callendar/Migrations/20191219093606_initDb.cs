using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Callendar.Migrations
{
    public partial class initDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Absences",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    IsWork = table.Column<bool>(),
                    SalaryPercent = table.Column<double>(),
                    RepresentingColor = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Absences", x => x.Id); });

            migrationBuilder.CreateTable(
                "Permissions",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Permissions", x => x.Id); });

            migrationBuilder.CreateTable(
                "TaskCategories",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ScorePoints = table.Column<int>()
                },
                constraints: table => { table.PrimaryKey("PK_TaskCategories", x => x.Id); });

            migrationBuilder.CreateTable(
                "Teams",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Teams", x => x.Id); });

            migrationBuilder.CreateTable(
                "Users",
                table => new
                {
                    Id = table.Column<Guid>(),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Points = table.Column<int>(),
                    VacationDaysLeft = table.Column<int>(),
                    PhotoPath = table.Column<string>(nullable: true),
                    PositionId = table.Column<int>(),
                    TeamId = table.Column<int>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        "FK_Users_Permissions_PositionId",
                        x => x.PositionId,
                        "Permissions",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Users_Teams_TeamId",
                        x => x.TeamId,
                        "Teams",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "TakenAbsences",
                table => new
                {
                    AbsenceId = table.Column<int>(),
                    UserId = table.Column<Guid>(),
                    Id = table.Column<int>(),
                    DaysCount = table.Column<int>(),
                    IsAccepted = table.Column<bool>(),
                    StartDate = table.Column<DateTime>(),
                    EndDate = table.Column<DateTime>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TakenAbsences", x => new {x.UserId, x.AbsenceId});
                    table.ForeignKey(
                        "FK_TakenAbsences_Absences_AbsenceId",
                        x => x.AbsenceId,
                        "Absences",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_TakenAbsences_Users_UserId",
                        x => x.UserId,
                        "Users",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Tasks",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    IsClosed = table.Column<bool>(),
                    TaskCategoryId = table.Column<int>(),
                    UserId = table.Column<Guid>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        "FK_Tasks_TaskCategories_TaskCategoryId",
                        x => x.TaskCategoryId,
                        "TaskCategories",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Tasks_Users_UserId",
                        x => x.UserId,
                        "Users",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_TakenAbsences_AbsenceId",
                "TakenAbsences",
                "AbsenceId");

            migrationBuilder.CreateIndex(
                "IX_Tasks_TaskCategoryId",
                "Tasks",
                "TaskCategoryId");

            migrationBuilder.CreateIndex(
                "IX_Tasks_UserId",
                "Tasks",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_Users_PositionId",
                "Users",
                "PositionId");

            migrationBuilder.CreateIndex(
                "IX_Users_TeamId",
                "Users",
                "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "TakenAbsences");

            migrationBuilder.DropTable(
                "Tasks");

            migrationBuilder.DropTable(
                "Absences");

            migrationBuilder.DropTable(
                "TaskCategories");

            migrationBuilder.DropTable(
                "Users");

            migrationBuilder.DropTable(
                "Permissions");

            migrationBuilder.DropTable(
                "Teams");
        }
    }
}