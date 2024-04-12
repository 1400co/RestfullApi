using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMedia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    AuditLogId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TableName = table.Column<string>(type: "text", nullable: true),
                    ActionType = table.Column<string>(type: "text", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    KeyValues = table.Column<string>(type: "text", nullable: true),
                    OldValues = table.Column<string>(type: "text", nullable: true),
                    NewValues = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.AuditLogId);
                });

            migrationBuilder.CreateTable(
                name: "Modulos",
                columns: table => new
                {
                    IdModulo = table.Column<Guid>(type: "uuid", nullable: false),
                    NombreModulo = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Responsable = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modulos", x => x.IdModulo);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRol = table.Column<Guid>(type: "uuid", nullable: false),
                    RolName = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Responsable = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRol);
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
                    Subscription = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Responsable = table.Column<string>(type: "text", nullable: true)
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
                    Created = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Edited = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Listed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Printed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IdRol = table.Column<Guid>(type: "uuid", nullable: false),
                    ModuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Responsable = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolModule", x => x.IdRolModule);
                    table.ForeignKey(
                        name: "FK_ROL_ROL_MODULE_CUSTOM",
                        column: x => x.IdRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol");
                    table.ForeignKey(
                        name: "FK_RolModule_Modulos_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modulos",
                        principalColumn: "IdModulo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PasswordRecovery",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PasswordRecoveryToken = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Responsable = table.Column<string>(type: "text", nullable: true)
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
                    Description = table.Column<string>(type: "text", nullable: true),
                    Image = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Responsable = table.Column<string>(type: "text", nullable: true)
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
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Responsable = table.Column<string>(type: "text", nullable: true)
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
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Responsable = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInRoles", x => x.IdUserInRoles);
                    table.ForeignKey(
                        name: "FK_UserInRoles_Roles_IdRol",
                        column: x => x.IdRol,
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
                name: "UserLogin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    User = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Responsable = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLogin_Usuario_UserId",
                        column: x => x.UserId,
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
                    IdUsuario = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Responsable = table.Column<string>(type: "text", nullable: true)
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
                table: "Modulos",
                columns: new[] { "IdModulo", "CreatedAt", "NombreModulo", "Responsable" },
                values: new object[,]
                {
                    { new Guid("1885be8d-aa27-4221-abdf-7affc845c63a"), new DateTime(2024, 4, 12, 16, 30, 43, 171, DateTimeKind.Utc).AddTicks(9066), "Permisos", "System" },
                    { new Guid("1885be8d-aa27-4221-abdf-7affc845c63b"), new DateTime(2024, 4, 12, 16, 30, 43, 171, DateTimeKind.Utc).AddTicks(9068), "Home", "System" },
                    { new Guid("24c51d74-d6f3-4409-a2b4-fccc8a4193b4"), new DateTime(2024, 4, 12, 16, 30, 43, 171, DateTimeKind.Utc).AddTicks(9070), "Modules", "System" },
                    { new Guid("642812ea-344e-4008-b4b6-4f74fba9b091"), new DateTime(2024, 4, 12, 16, 30, 43, 171, DateTimeKind.Utc).AddTicks(9064), "Roles", "System" },
                    { new Guid("88b9cb17-dc3d-47e4-b60e-0bc75de3cae1"), new DateTime(2024, 4, 12, 16, 30, 43, 171, DateTimeKind.Utc).AddTicks(9056), "Usuarios", "System" },
                    { new Guid("a23e6ada-2bb4-44b5-b7ad-69581f198e1c"), new DateTime(2024, 4, 12, 16, 30, 43, 171, DateTimeKind.Utc).AddTicks(9072), "Demo", "System" },
                    { new Guid("d9e00690-bef3-483c-8275-49624fdeca2b"), new DateTime(2024, 4, 12, 16, 30, 43, 171, DateTimeKind.Utc).AddTicks(9060), "Credenciales", "System" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "IdRol", "CreatedAt", "Responsable", "RolName" },
                values: new object[,]
                {
                    { new Guid("3a9a7ce2-9a5c-4aff-a47a-c5fdfcd955ae"), new DateTime(2024, 4, 12, 16, 30, 43, 170, DateTimeKind.Utc).AddTicks(3962), "System", "Super Administrator" },
                    { new Guid("7c2e1e9b-410b-4a6b-b9ae-8b078422eb2d"), new DateTime(2024, 4, 12, 16, 30, 43, 170, DateTimeKind.Utc).AddTicks(3967), "System", "Administrator" }
                });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "IdUsuario", "BornDate", "CreatedAt", "Email", "FullName", "IsActive", "Phone", "Responsable", "Subscription" },
                values: new object[] { new Guid("53aeeca4-a5b1-4751-abcb-3207a01b97dc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 12, 16, 30, 43, 171, DateTimeKind.Utc).AddTicks(2985), "oruedar@yopmail.com", "Oscar", true, null, "System", 0 });

            migrationBuilder.InsertData(
                table: "Seguridad",
                columns: new[] { "IdSeguridad", "CreatedAt", "Password", "RefreshToken", "RefreshTokenExpiryTime", "Responsable", "Rol", "UserId", "UserName" },
                values: new object[] { new Guid("d6a320bc-7134-4871-a01b-935075d2cadd"), new DateTime(2024, 4, 12, 16, 30, 43, 171, DateTimeKind.Utc).AddTicks(1477), "10000.mmlVX3xzYuLQromOzqELBQ==.JIwrJbVGsgYiTMjqWqcvulmXk8Fv6c7hxbl8mEqixTI=", null, null, "System", "Administrator", new Guid("53aeeca4-a5b1-4751-abcb-3207a01b97dc"), "admin" });

            migrationBuilder.InsertData(
                table: "UserInRoles",
                columns: new[] { "IdUserInRoles", "CreatedAt", "Responsable", "IdRol", "IdUser" },
                values: new object[] { new Guid("19ba277a-e977-4e10-b743-f5148bf9487d"), new DateTime(2024, 4, 12, 16, 30, 43, 171, DateTimeKind.Utc).AddTicks(7225), "System", new Guid("3a9a7ce2-9a5c-4aff-a47a-c5fdfcd955ae"), new Guid("53aeeca4-a5b1-4751-abcb-3207a01b97dc") });

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
                name: "IX_RolModule_ModuleId",
                table: "RolModule",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Seguridad_UserId",
                table: "Seguridad",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInRoles_IdRol",
                table: "UserInRoles",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_UserInRoles_IdUser",
                table: "UserInRoles",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_UserId",
                table: "UserLogin",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

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
                name: "Modulos");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
