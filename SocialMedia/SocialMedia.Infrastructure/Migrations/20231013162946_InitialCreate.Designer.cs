﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.Infrastructure.Data;

#nullable disable

namespace SocialMedia.Infrastructure.Migrations
{
    [DbContext(typeof(SocialMediaContext))]
    [Migration("20231013162946_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SocialMedia.Core.Entities.ActividadesSilviculturales", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("IdActividadesSilviculturales");

                    b.Property<Guid?>("CensoArboreoId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CensoId")
                        .HasColumnType("uuid");

                    b.Property<bool>("CortesEnfermos")
                        .HasColumnType("boolean");

                    b.Property<bool>("CortesNuevos")
                        .HasColumnType("boolean");

                    b.Property<bool>("CortesViejos")
                        .HasColumnType("boolean");

                    b.Property<bool>("CortoPlazoC")
                        .HasColumnType("boolean");

                    b.Property<string>("EstructurasCercanasTipoEmplazamiento")
                        .HasColumnType("text");

                    b.Property<Guid>("IdCensoArboreo")
                        .HasColumnType("uuid");

                    b.Property<bool>("InmediataI")
                        .HasColumnType("boolean");

                    b.Property<bool>("LargoPlazoL")
                        .HasColumnType("boolean");

                    b.Property<string>("Limpieza")
                        .HasColumnType("text");

                    b.Property<string>("Observacion")
                        .HasColumnType("text");

                    b.Property<bool>("PodaEstabilidadE")
                        .HasColumnType("boolean");

                    b.Property<bool>("PodaMantenimientoM")
                        .HasColumnType("boolean");

                    b.Property<bool>("PodaRaices")
                        .HasColumnType("boolean");

                    b.Property<bool>("PodaRealceR")
                        .HasColumnType("boolean");

                    b.Property<string>("PodaSanitaria")
                        .HasColumnType("text");

                    b.Property<bool>("Trasplante")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("CensoArboreoId");

                    b.HasIndex("CensoId");

                    b.ToTable("ActividadesSilviculturales", (string)null);
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.CensoArboreo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("IdCensoArboreo");

                    b.Property<decimal>("AltitudASNM")
                        .HasColumnType("numeric");

                    b.Property<decimal>("AlturaComercial")
                        .HasColumnType("numeric");

                    b.Property<decimal>("AlturaTotal")
                        .HasColumnType("numeric");

                    b.Property<bool>("ApendiceCites")
                        .HasColumnType("boolean");

                    b.Property<string>("Barrio")
                        .HasColumnType("text");

                    b.Property<bool>("CategoriaMinisterioResolucion0192de2014")
                        .HasColumnType("boolean");

                    b.Property<bool>("CategoriaUicn")
                        .HasColumnType("boolean");

                    b.Property<string>("Comuna")
                        .HasColumnType("text");

                    b.Property<Guid?>("CoordenadasGeograficasYPlanasId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("DAP")
                        .HasColumnType("numeric");

                    b.Property<decimal>("DiametroCopa")
                        .HasColumnType("numeric");

                    b.Property<string>("Especie")
                        .HasColumnType("text");

                    b.Property<bool>("EspecieEndemica")
                        .HasColumnType("boolean");

                    b.Property<string>("EstadoFitosanitario")
                        .HasColumnType("text");

                    b.Property<string>("EstadoMadurez")
                        .HasColumnType("text");

                    b.Property<string>("Familia")
                        .HasColumnType("text");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FormaCopa")
                        .HasColumnType("text");

                    b.Property<string>("Genero")
                        .HasColumnType("text");

                    b.Property<string>("InfraestructuraAfectada")
                        .HasColumnType("text");

                    b.Property<string>("Ninguna")
                        .HasColumnType("text");

                    b.Property<string>("NombreCientifico")
                        .HasColumnType("text");

                    b.Property<string>("NombreComun")
                        .HasColumnType("text");

                    b.Property<decimal>("NumeroFustes")
                        .HasColumnType("numeric");

                    b.Property<string>("Observacion")
                        .HasColumnType("text");

                    b.Property<string>("Orden")
                        .HasColumnType("text");

                    b.Property<string>("Origen")
                        .HasColumnType("text");

                    b.Property<string>("RecomendacionesParaManejoSilvicultural")
                        .HasColumnType("text");

                    b.Property<string>("TipoEmplazamiento")
                        .HasColumnType("text");

                    b.Property<string>("TipoIndividuo")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CoordenadasGeograficasYPlanasId");

                    b.ToTable("CensoArboreo", (string)null);
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("IdComentario");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid")
                        .HasColumnName("IdPublicacion");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("IdUsuario");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Comentario", (string)null);
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.Coordenada", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("X")
                        .HasColumnType("text");

                    b.Property<string>("Y")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Coordenada");
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.EspaciosPotenciales", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("IdEspaciosPotenciales");

                    b.Property<decimal>("AltitudASNM")
                        .HasColumnType("numeric");

                    b.Property<decimal>("AlturasPotencialesArboles")
                        .HasColumnType("numeric");

                    b.Property<decimal>("AreasDisponibles")
                        .HasColumnType("numeric");

                    b.Property<string>("Barrio")
                        .HasColumnType("text");

                    b.Property<decimal>("CaracteristicasEdaficasODelSuelo")
                        .HasColumnType("numeric");

                    b.Property<string>("Comuna")
                        .HasColumnType("text");

                    b.Property<decimal>("CoordenadasGeograficasYPlanas")
                        .HasColumnType("numeric");

                    b.Property<bool>("Cuadrado")
                        .HasColumnType("boolean");

                    b.Property<decimal>("DistanciamientoSiembra")
                        .HasColumnType("numeric");

                    b.Property<string>("EspeciesPotencialesParaSiembraAlMenosTresOpciones")
                        .HasColumnType("text");

                    b.Property<bool>("IndividualOUno")
                        .HasColumnType("boolean");

                    b.Property<string>("InfraestructuraAfectada")
                        .HasColumnType("text");

                    b.Property<string>("PendienteDelTerreno")
                        .HasColumnType("text");

                    b.Property<bool>("Rectangular")
                        .HasColumnType("boolean");

                    b.Property<string>("TipoEmplazamiento")
                        .HasColumnType("text");

                    b.Property<bool>("TresBolillo")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("EspaciosPotenciales", (string)null);
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.PasswordRecovery", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("PasswordRecoveryToken")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("PasswordRecovery");
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("IdPublicacion");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Publicacion", (string)null);
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.RegistroFotografico", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CensoArboreoId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("EspaciosPotencialesId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CensoArboreoId");

                    b.HasIndex("EspaciosPotencialesId");

                    b.ToTable("RegistroFotografico");
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.RolModule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("IdRolModule");

                    b.Property<bool>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("Created");

                    b.Property<bool>("Deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("Deleted");

                    b.Property<bool>("Edited")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("Edited");

                    b.Property<Guid>("IdRol")
                        .HasColumnType("uuid");

                    b.Property<bool>("Listed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("Listed");

                    b.Property<string>("Module")
                        .HasColumnType("text");

                    b.Property<bool>("Printed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("Printed");

                    b.HasKey("Id");

                    b.HasIndex("IdRol");

                    b.ToTable("RolModule", (string)null);
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.Roles", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("IdRol");

                    b.Property<string>("RolName")
                        .HasColumnType("text")
                        .HasColumnName("RolName");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.Security", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("IdSeguridad");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)")
                        .HasColumnName("Rol");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Seguridad", (string)null);
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("IdUsuario");

                    b.Property<DateTime>("BornDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<int>("Subscription")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Usuario", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("53aeeca4-a5b1-4751-abcb-3207a01b97dc"),
                            BornDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "oruedar@yopmail.com",
                            FullName = "Oscar",
                            IsActive = true,
                            Subscription = 0
                        });
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.UserInRoles", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("IdUserInRoles");

                    b.Property<Guid?>("RolId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("IdRol");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("IdUser");

                    b.HasKey("Id");

                    b.HasIndex("RolId");

                    b.HasIndex("UserId");

                    b.ToTable("UserInRoles", (string)null);
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.UserLogin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("User")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("UserLogin");
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.ActividadesSilviculturales", b =>
                {
                    b.HasOne("SocialMedia.Core.Entities.CensoArboreo", "CensoArboreo")
                        .WithMany()
                        .HasForeignKey("CensoArboreoId");

                    b.HasOne("SocialMedia.Core.Entities.CensoArboreo", "Censo")
                        .WithMany()
                        .HasForeignKey("CensoId");

                    b.Navigation("Censo");

                    b.Navigation("CensoArboreo");
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.CensoArboreo", b =>
                {
                    b.HasOne("SocialMedia.Core.Entities.Coordenada", "CoordenadasGeograficasYPlanas")
                        .WithMany()
                        .HasForeignKey("CoordenadasGeograficasYPlanasId");

                    b.Navigation("CoordenadasGeograficasYPlanas");
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.Comment", b =>
                {
                    b.HasOne("SocialMedia.Core.Entities.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .IsRequired()
                        .HasConstraintName("FK_Comentario_Publicacion");

                    b.HasOne("SocialMedia.Core.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_Comentario_Usuario");

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.PasswordRecovery", b =>
                {
                    b.HasOne("SocialMedia.Core.Entities.User", "User")
                        .WithMany("PasswordRecovery")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.Post", b =>
                {
                    b.HasOne("SocialMedia.Core.Entities.User", "User")
                        .WithMany("Post")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_Publicacion_Usuario");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.RegistroFotografico", b =>
                {
                    b.HasOne("SocialMedia.Core.Entities.CensoArboreo", null)
                        .WithMany("Fotos")
                        .HasForeignKey("CensoArboreoId");

                    b.HasOne("SocialMedia.Core.Entities.EspaciosPotenciales", null)
                        .WithMany("Fotos")
                        .HasForeignKey("EspaciosPotencialesId");
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.RolModule", b =>
                {
                    b.HasOne("SocialMedia.Core.Entities.Roles", "Rol")
                        .WithMany("RolModules")
                        .HasForeignKey("IdRol")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_ROL_ROL_MODULE_CUSTOM");

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.Security", b =>
                {
                    b.HasOne("SocialMedia.Core.Entities.User", "User")
                        .WithMany("Security")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.UserInRoles", b =>
                {
                    b.HasOne("SocialMedia.Core.Entities.Roles", "Rol")
                        .WithMany("UserInRoles")
                        .HasForeignKey("RolId");

                    b.HasOne("SocialMedia.Core.Entities.User", "User")
                        .WithMany("UserInRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rol");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.CensoArboreo", b =>
                {
                    b.Navigation("Fotos");
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.EspaciosPotenciales", b =>
                {
                    b.Navigation("Fotos");
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.Post", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.Roles", b =>
                {
                    b.Navigation("RolModules");

                    b.Navigation("UserInRoles");
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("PasswordRecovery");

                    b.Navigation("Post");

                    b.Navigation("Security");

                    b.Navigation("UserInRoles");
                });
#pragma warning restore 612, 618
        }
    }
}