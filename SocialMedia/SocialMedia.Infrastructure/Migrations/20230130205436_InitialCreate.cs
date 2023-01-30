using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialMedia.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Seguridad",
                columns: table => new
                {
                    IdSeguridad = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Usuario = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    NombreUsuario = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    Contrasena = table.Column<string>(unicode: false, maxLength: 200, nullable: false),
                    Rol = table.Column<string>(maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seguridad", x => x.IdSeguridad);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(nullable: false),
                    Nombres = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Apellidos = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(unicode: false, maxLength: 30, nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "date", nullable: false),
                    Telefono = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    Activo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Publicacion",
                columns: table => new
                {
                    IdPublicacion = table.Column<int>(nullable: false),
                    IdUsuario = table.Column<int>(nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false),
                    Descripcion = table.Column<string>(unicode: false, maxLength: 1000, nullable: false),
                    Imagen = table.Column<string>(unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publicacion", x => x.IdPublicacion);
                    table.ForeignKey(
                        name: "FK_Publicacion_Usuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comentario",
                columns: table => new
                {
                    IdComentario = table.Column<int>(nullable: false),
                    IdPublicacion = table.Column<int>(nullable: false),
                    IdUsuario = table.Column<int>(nullable: false),
                    Descripcion = table.Column<string>(unicode: false, maxLength: 500, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false),
                    Activo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentario", x => x.IdComentario);
                    table.ForeignKey(
                        name: "FK_Comentario_Publicacion",
                        column: x => x.IdPublicacion,
                        principalTable: "Publicacion",
                        principalColumn: "IdPublicacion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comentario_Usuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Seguridad",
                columns: new[] { "IdSeguridad", "Contrasena", "Rol", "Usuario", "NombreUsuario" },
                values: new object[] { 1, "10000.mmlVX3xzYuLQromOzqELBQ==.JIwrJbVGsgYiTMjqWqcvulmXk8Fv6c7hxbl8mEqixTI=", "Administrator", "Oscar", "Admin" });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "IdUsuario", "FechaNacimiento", "Email", "Activo", "Apellidos", "Nombres", "Telefono" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "oruedar@yopmail.com", true, "Rueda", "Oscar", null });

            migrationBuilder.InsertData(
                table: "Publicacion",
                columns: new[] { "IdPublicacion", "Fecha", "Descripcion", "Imagen", "IdUsuario" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 1, 30, 15, 54, 36, 631, DateTimeKind.Local).AddTicks(6436), "Beatae doloremque error maxime dicta placeat numquam voluptatem sed laborum. ", null, 1 },
                    { 2, new DateTime(2023, 1, 30, 15, 54, 36, 631, DateTimeKind.Local).AddTicks(6761), "Beatae doloremque error maxime dicta placeat numquam voluptatem sed laborum. ", null, 1 },
                    { 3, new DateTime(2023, 1, 30, 15, 54, 36, 631, DateTimeKind.Local).AddTicks(6771), "Beatae doloremque error maxime dicta placeat numquam voluptatem sed laborum. ", null, 1 },
                    { 4, new DateTime(2023, 1, 30, 15, 54, 36, 631, DateTimeKind.Local).AddTicks(6775), "Beatae doloremque error maxime dicta placeat numquam voluptatem sed laborum. ", null, 1 }
                });

            migrationBuilder.InsertData(
                table: "Comentario",
                columns: new[] { "IdComentario", "Fecha", "Descripcion", "Activo", "IdPublicacion", "IdUsuario" },
                values: new object[] { 1, new DateTime(2023, 1, 30, 15, 54, 36, 628, DateTimeKind.Local).AddTicks(4255), "Beatae doloremque error maxime dicta placeat numquam voluptatem sed laborum. ", true, 1, 1 });

            migrationBuilder.InsertData(
                table: "Comentario",
                columns: new[] { "IdComentario", "Fecha", "Descripcion", "Activo", "IdPublicacion", "IdUsuario" },
                values: new object[] { 2, new DateTime(2023, 1, 30, 15, 54, 36, 628, DateTimeKind.Local).AddTicks(9582), "Beatae doloremque error maxime dicta placeat numquam voluptatem sed laborum. ", true, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Comentario_IdPublicacion",
                table: "Comentario",
                column: "IdPublicacion");

            migrationBuilder.CreateIndex(
                name: "IX_Comentario_IdUsuario",
                table: "Comentario",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Publicacion_IdUsuario",
                table: "Publicacion",
                column: "IdUsuario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comentario");

            migrationBuilder.DropTable(
                name: "Seguridad");

            migrationBuilder.DropTable(
                name: "Publicacion");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
