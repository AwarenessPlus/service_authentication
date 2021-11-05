using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthenticationService.Migrations
{
    public partial class NewDatabaseMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authentication",
                columns: table => new
                {
                    AuthenticationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authentication", x => x.AuthenticationID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecondName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Medic",
                columns: table => new
                {
                    MedicID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rotation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    AuthenticationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medic", x => x.MedicID);
                    table.ForeignKey(
                        name: "FK_Medic_Authentication_AuthenticationID",
                        column: x => x.AuthenticationID,
                        principalTable: "Authentication",
                        principalColumn: "AuthenticationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Medic_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    PatientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bloodgroup = table.Column<int>(type: "int", nullable: true),
                    Rh = table.Column<int>(type: "int", nullable: true),
                    Sex = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    DocumentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.PatientID);
                    table.ForeignKey(
                        name: "FK_Patient_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Procedure",
                columns: table => new
                {
                    ProcedureID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcedureName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Asa = table.Column<int>(type: "int", nullable: false),
                    MedicID = table.Column<int>(type: "int", nullable: false),
                    _pacientID = table.Column<int>(type: "int", nullable: false),
                    PacientID = table.Column<int>(type: "int", nullable: false),
                    ProcedureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedure", x => x.ProcedureID);
                    table.ForeignKey(
                        name: "FK_Procedure_Patient__pacientID",
                        column: x => x._pacientID,
                        principalTable: "Patient",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VideoFile",
                columns: table => new
                {
                    VideoFileID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcedureID = table.Column<int>(type: "int", nullable: false),
                    Filepath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Filename = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoFile", x => x.VideoFileID);
                    table.ForeignKey(
                        name: "FK_VideoFile_Procedure_ProcedureID",
                        column: x => x.ProcedureID,
                        principalTable: "Procedure",
                        principalColumn: "ProcedureID"
                        );
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medic_AuthenticationID",
                table: "Medic",
                column: "AuthenticationID");

            migrationBuilder.CreateIndex(
                name: "IX_Medic_UserID",
                table: "Medic",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_UserID",
                table: "Patient",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Procedure__pacientID",
                table: "Procedure",
                column: "_pacientID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Medic");

            migrationBuilder.DropTable(
                name: "Procedure");

            migrationBuilder.DropTable(
                name: "VideoFile");

            migrationBuilder.DropTable(
                name: "Authentication");

            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
