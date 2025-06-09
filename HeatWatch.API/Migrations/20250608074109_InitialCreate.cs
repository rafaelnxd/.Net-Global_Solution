using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeatWatch.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Regioes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Latitude = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    Longitude = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    Descricao = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Area = table.Column<double>(type: "BINARY_DOUBLE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regioes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventosCalor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DataFim = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Intensidade = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    RegiaoId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventosCalor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventosCalor_Regioes_RegiaoId",
                        column: x => x.RegiaoId,
                        principalTable: "Regioes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrosTemperatura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    RegiaoId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DataRegistro = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    TemperaturaCelsius = table.Column<double>(type: "BINARY_DOUBLE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosTemperatura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrosTemperatura_Regioes_RegiaoId",
                        column: x => x.RegiaoId,
                        principalTable: "Regioes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alertas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Mensagem = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DataEmissao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Severidade = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    EventoCalorId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alertas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alertas_EventosCalor_EventoCalorId",
                        column: x => x.EventoCalorId,
                        principalTable: "EventosCalor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_EventoCalorId",
                table: "Alertas",
                column: "EventoCalorId");

            migrationBuilder.CreateIndex(
                name: "IX_EventosCalor_RegiaoId",
                table: "EventosCalor",
                column: "RegiaoId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosTemperatura_RegiaoId",
                table: "RegistrosTemperatura",
                column: "RegiaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alertas");

            migrationBuilder.DropTable(
                name: "RegistrosTemperatura");

            migrationBuilder.DropTable(
                name: "EventosCalor");

            migrationBuilder.DropTable(
                name: "Regioes");
        }
    }
}
