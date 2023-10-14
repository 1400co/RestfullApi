using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRol = table.Column<Guid>(type: "uuid", nullable: false),
                    RolName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    User = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    BornDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Subscription = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "RolModule",
                columns: table => new
                {
                    IdRolModule = table.Column<Guid>(type: "uuid", nullable: false),
                    Module = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Edited = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Listed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Printed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IdRol = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolModule", x => x.IdRolModule);
                    table.ForeignKey(
                        name: "FK_ROL_ROL_MODULE_CUSTOM",
                        column: x => x.IdRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol");
                });

            migrationBuilder.CreateTable(
                name: "PasswordRecovery",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PasswordRecoveryToken = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordRecovery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordRecovery_Usuario_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Publicacion",
                columns: table => new
                {
                    IdPublicacion = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publicacion", x => x.IdPublicacion);
                    table.ForeignKey(
                        name: "FK_Publicacion_Usuario",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Seguridad",
                columns: table => new
                {
                    IdSeguridad = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Rol = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seguridad", x => x.IdSeguridad);
                    table.ForeignKey(
                        name: "FK_Seguridad_Usuario_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInRoles",
                columns: table => new
                {
                    IdUserInRoles = table.Column<Guid>(type: "uuid", nullable: false),
                    IdUser = table.Column<Guid>(type: "uuid", nullable: false),
                    IdRol = table.Column<Guid>(type: "uuid", nullable: false),
                    RolId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInRoles", x => x.IdUserInRoles);
                    table.ForeignKey(
                        name: "FK_UserInRoles_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "IdRol");
                    table.ForeignKey(
                        name: "FK_UserInRoles_Usuario_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comentario",
                columns: table => new
                {
                    IdComentario = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IdPublicacion = table.Column<Guid>(type: "uuid", nullable: false),
                    IdUsuario = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentario", x => x.IdComentario);
                    table.ForeignKey(
                        name: "FK_Comentario_Publicacion",
                        column: x => x.IdPublicacion,
                        principalTable: "Publicacion",
                        principalColumn: "IdPublicacion");
                    table.ForeignKey(
                        name: "FK_Comentario_Usuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "IdUsuario", "BornDate", "Email", "FullName", "IsActive", "Phone", "Subscription" },
                values: new object[] { new Guid("53aeeca4-a5b1-4751-abcb-3207a01b97dc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "oruedar@yopmail.com", "Oscar", true, null, 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Comentario_IdPublicacion",
                table: "Comentario",
                column: "IdPublicacion");

            migrationBuilder.CreateIndex(
                name: "IX_Comentario_IdUsuario",
                table: "Comentario",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordRecovery_UserId",
                table: "PasswordRecovery",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Publicacion_UserId",
                table: "Publicacion",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RolModule_IdRol",
                table: "RolModule",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_Seguridad_UserId",
                table: "Seguridad",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInRoles_IdUser",
                table: "UserInRoles",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_UserInRoles_RolId",
                table: "UserInRoles",
                column: "RolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comentario");

            migrationBuilder.DropTable(
                name: "PasswordRecovery");

            migrationBuilder.DropTable(
                name: "RolModule");

            migrationBuilder.DropTable(
                name: "Seguridad");

            migrationBuilder.DropTable(
                name: "UserInRoles");

            migrationBuilder.DropTable(
                name: "UserLogin");

            migrationBuilder.DropTable(
                name: "Publicacion");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
