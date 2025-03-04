using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Employee.Infra.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SecurityKeys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KeyId = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Use = table.Column<string>(type: "text", nullable: true),
                    Parameters = table.Column<string>(type: "text", nullable: true),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    RevokedReason = table.Column<string>(type: "text", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Surname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Document = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ImmediateSupervisor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PositionId = table.Column<int>(type: "integer", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_Position_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Position",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Phone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    AreaCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Number = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Phone_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_PositionId",
                table: "Employee",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Phone_EmployeeId",
                table: "Phone",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_User_EmployeeId",
                table: "User",
                column: "EmployeeId");

            migrationBuilder.InsertData(
                table: "Position",
                columns: ["Id", "Name", "Description", "Active"],
                values: new object[,]
                {
                    { 1, "Gestor", "Coordena e lidera times", true },
                    { 2, "Desenvolvedor", "Desenvolve softwares", true },
                    { 3, "Analista de RH", "Gerir e desenvolver os colaboradores de uma empresa", true }
                });

            var employeeId = Guid.NewGuid();
            migrationBuilder.InsertData(
                table: "Employee",
                columns: ["Id", "Name", "Surname", "Email", "Document", "ImmediateSupervisor", "PositionId", "BirthDate", "Active"],
                values:
                [
                    employeeId, "John", "Doe", "john.doe@example.com", "123456789", null, 1, new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc), true
                ]);

            migrationBuilder.InsertData(
                table: "User",
                columns: ["Id", "Role", "Username", "EmailConfirmed", "EmployeeId", "Password", "Active"], // Password = Teste@1234
                values:
                [
                    Guid.NewGuid(), 1, "john.doe@example.com", true, employeeId, "y4rYMzvZKybF3EYhmDC5GPg6GOm4ouaTH+LSJaOukBKQx8ISQqBerPUnFtBoMxLqyBaY58zZ91GIVH5+LdMzkg==;JMHPtcbEa02lvu+gUePmYkD0vZCkUMRZip+EwZrka6AsE/yhVkfR5ou4dI3P5K07zxjkqNQygYHTfFQqkUL+Hg==", true
                ]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Phone");

            migrationBuilder.DropTable(
                name: "SecurityKeys");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Position");
        }
    }
}
