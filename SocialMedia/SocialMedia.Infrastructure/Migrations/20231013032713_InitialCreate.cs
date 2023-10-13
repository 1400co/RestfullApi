using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialMedia.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coordenada",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    X = table.Column<string>(nullable: true),
                    Y = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordenada", x => x.Id);
                });

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
                name: "UserLogin",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    User = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => x.Id);
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
                name: "CensoArboreo",
                columns: table => new
                {
                    IdCensoArboreo = table.Column<Guid>(nullable: false),
                    AlturaTotal = table.Column<decimal>(nullable: false),
                    AlturaComercial = table.Column<decimal>(nullable: false),
                    DiametroCopa = table.Column<decimal>(nullable: false),
                    FormaCopa = table.Column<string>(nullable: true),
                    DAP = table.Column<decimal>(nullable: false),
                    NumeroFustes = table.Column<decimal>(nullable: false),
                    NombreComun = table.Column<string>(nullable: true),
                    NombreCientifico = table.Column<string>(nullable: true),
                    Orden = table.Column<string>(nullable: true),
                    Familia = table.Column<string>(nullable: true),
                    Genero = table.Column<string>(nullable: true),
                    Especie = table.Column<string>(nullable: true),
                    CoordenadasGeograficasYPlanasId = table.Column<Guid>(nullable: true),
                    AltitudASNM = table.Column<decimal>(nullable: false),
                    EstadoFitosanitario = table.Column<string>(nullable: true),
                    EstadoMadurez = table.Column<string>(nullable: true),
                    TipoIndividuo = table.Column<string>(nullable: true),
                    Barrio = table.Column<string>(nullable: true),
                    Comuna = table.Column<string>(nullable: true),
                    TipoEmplazamiento = table.Column<string>(nullable: true),
                    InfraestructuraAfectada = table.Column<string>(nullable: true),
                    ApendiceCites = table.Column<bool>(nullable: false),
                    CategoriaUicn = table.Column<bool>(nullable: false),
                    CategoriaMinisterioResolucion0192de2014 = table.Column<bool>(nullable: false),
                    EspecieEndemica = table.Column<bool>(nullable: false),
                    Origen = table.Column<string>(nullable: true),
                    Observacion = table.Column<string>(nullable: true),
                    Fecha = table.Column<DateTime>(nullable: false),
                    RecomendacionesParaManejoSilvicultural = table.Column<string>(nullable: true),
                    Ninguna = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CensoArboreo", x => x.IdCensoArboreo);
                    table.ForeignKey(
                        name: "FK_CensoArboreo_Coordenada_CoordenadasGeograficasYPlanasId",
                        column: x => x.CoordenadasGeograficasYPlanasId,
                        principalTable: "Coordenada",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    IdPublicacion = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true)
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
                    IdSeguridad = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(nullable: true),
                    Rol = table.Column<string>(maxLength: 15, nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
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
                name: "RegistroFotografico",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    CensoArboreoId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroFotografico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistroFotografico_CensoArboreo_CensoArboreoId",
                        column: x => x.CensoArboreoId,
                        principalTable: "CensoArboreo",
                        principalColumn: "IdCensoArboreo",
                        onDelete: ReferentialAction.Restrict);
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
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "IdUsuario", "BornDate", "Email", "FullName", "IsActive", "Phone", "Subscription" },
                values: new object[] { new Guid("53aeeca4-a5b1-4751-abcb-3207a01b97dc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "oruedar@yopmail.com", "Oscar", true, null, 0 });

            migrationBuilder.CreateIndex(
                name: "IX_CensoArboreo_CoordenadasGeograficasYPlanasId",
                table: "CensoArboreo",
                column: "CoordenadasGeograficasYPlanasId");

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
                name: "IX_RegistroFotografico_CensoArboreoId",
                table: "RegistroFotografico",
                column: "CensoArboreoId");

            migrationBuilder.CreateIndex(
                name: "IX_RolModule_IdRol",
                table: "RolModule",
                column: "IdRol");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comentario");

            migrationBuilder.DropTable(
                name: "PasswordRecovery");

            migrationBuilder.DropTable(
                name: "RegistroFotografico");

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
                name: "CensoArboreo");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Coordenada");
        }
    }
}
