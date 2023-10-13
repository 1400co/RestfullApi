using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialMedia.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRol = table.Column<Guid>(nullable: false),
                    RolName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "Roles1",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RolName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles1", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    BornDate = table.Column<DateTime>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Subscription = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<Guid>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    BornDate = table.Column<DateTime>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Subscription = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "RolModule",
                columns: table => new
                {
                    IdRolModule = table.Column<Guid>(nullable: false),
                    Module = table.Column<string>(nullable: true),
                    Created = table.Column<bool>(nullable: false, defaultValue: false),
                    Edited = table.Column<bool>(nullable: false, defaultValue: false),
                    Listed = table.Column<bool>(nullable: false, defaultValue: false),
                    Deleted = table.Column<bool>(nullable: false, defaultValue: false),
                    Printed = table.Column<bool>(nullable: false, defaultValue: false),
                    IdRol = table.Column<Guid>(nullable: false)
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
                name: "RolModule1",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Module = table.Column<string>(nullable: true),
                    Created = table.Column<bool>(nullable: false),
                    Edited = table.Column<bool>(nullable: false),
                    Listed = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Printed = table.Column<bool>(nullable: false),
                    IdRol = table.Column<Guid>(nullable: false),
                    RolId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolModule1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolModule1_Roles1_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles1",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PasswordRecovery",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PasswordRecoveryToken = table.Column<Guid>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordRecovery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordRecovery_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInRoles1",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false),
                    RolId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInRoles1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInRoles1_Roles1_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles1",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserInRoles1_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PasswordRecoveryDto",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PasswordRecoveryToken = table.Column<Guid>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordRecoveryDto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordRecoveryDto_Usuario_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Publicacion",
                columns: table => new
                {
                    IdPublicacion = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    UserDtoId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publicacion", x => x.IdPublicacion);
                    table.ForeignKey(
                        name: "FK_Publicacion_Usuario_UserDtoId",
                        column: x => x.UserDtoId,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Publicacion_Usuario",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Seguridad",
                columns: table => new
                {
                    IdSeguridad = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(nullable: true),
                    Rol = table.Column<string>(maxLength: 15, nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    UserDtoId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seguridad", x => x.IdSeguridad);
                    table.ForeignKey(
                        name: "FK_Seguridad_Usuario_UserDtoId",
                        column: x => x.UserDtoId,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Seguridad_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInRoles",
                columns: table => new
                {
                    IdUserInRoles = table.Column<Guid>(nullable: false),
                    IdUser = table.Column<Guid>(nullable: false),
                    IdRol = table.Column<Guid>(nullable: false),
                    RolId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInRoles", x => x.IdUserInRoles);
                    table.ForeignKey(
                        name: "FK_UserInRoles_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Restrict);
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
                    IdComentario = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IdPublicacion = table.Column<Guid>(nullable: false),
                    IdUsuario = table.Column<Guid>(nullable: false)
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
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    PostId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Publicacion_PostId",
                        column: x => x.PostId,
                        principalTable: "Publicacion",
                        principalColumn: "IdPublicacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Usuario_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Seguridad",
                columns: new[] { "IdSeguridad", "Password", "RefreshToken", "RefreshTokenExpiryTime", "Rol", "UserDtoId", "UserId", "UserName" },
                values: new object[] { new Guid("31ecea6f-9790-4de4-820d-17ed7b940e5f"), "10000.mmlVX3xzYuLQromOzqELBQ==.JIwrJbVGsgYiTMjqWqcvulmXk8Fv6c7hxbl8mEqixTI=", null, null, "Administrator", null, new Guid("53aeeca4-a5b1-4751-abcb-3207a01b97dc"), "Admin" });

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
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordRecovery_UserId",
                table: "PasswordRecovery",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordRecoveryDto_UserId",
                table: "PasswordRecoveryDto",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Publicacion_UserDtoId",
                table: "Publicacion",
                column: "UserDtoId");

            migrationBuilder.CreateIndex(
                name: "IX_Publicacion_UserId",
                table: "Publicacion",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RolModule_IdRol",
                table: "RolModule",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_RolModule1_RolId",
                table: "RolModule1",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Seguridad_UserDtoId",
                table: "Seguridad",
                column: "UserDtoId");

            migrationBuilder.CreateIndex(
                name: "IX_Seguridad_UserId",
                table: "Seguridad",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInRoles_RolId",
                table: "UserInRoles",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInRoles_IdUser",
                table: "UserInRoles",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_UserInRoles1_RolId",
                table: "UserInRoles1",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInRoles1_UserId",
                table: "UserInRoles1",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comentario");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "PasswordRecovery");

            migrationBuilder.DropTable(
                name: "PasswordRecoveryDto");

            migrationBuilder.DropTable(
                name: "RolModule");

            migrationBuilder.DropTable(
                name: "RolModule1");

            migrationBuilder.DropTable(
                name: "Seguridad");

            migrationBuilder.DropTable(
                name: "UserInRoles");

            migrationBuilder.DropTable(
                name: "UserInRoles1");

            migrationBuilder.DropTable(
                name: "Publicacion");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Roles1");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
