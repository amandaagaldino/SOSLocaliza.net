using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sprint1.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_SOS_USUARIO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NomeCompleto = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Senha = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "date", nullable: false),
                    Cpf = table.Column<string>(type: "NVARCHAR2(11)", maxLength: 11, nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "date", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "date", nullable: true),
                    Ativo = table.Column<int>(type: "NUMBER(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_SOS_USUARIO", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_SOS_USUARIO_Cpf",
                table: "T_SOS_USUARIO",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_SOS_USUARIO_Email",
                table: "T_SOS_USUARIO",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_SOS_USUARIO");
        }
    }
}
