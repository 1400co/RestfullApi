﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SocialMedia.Infrastructure.Data;

#nullable disable

namespace SocialMedia.Infrastructure.Migrations
{
    [DbContext(typeof(SocialMediaContext))]
    partial class SocialMediaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SocialMedia.Core.CustomEntities.AuditLog", b =>
                {
                    b.Property<int>("AuditLogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AuditLogId"));

                    b.Property<string>("ActionType")
                        .HasColumnType("text");

                    b.Property<string>("KeyValues")
                        .HasColumnType("text");

                    b.Property<string>("NewValues")
                        .HasColumnType("text");

                    b.Property<string>("OldValues")
                        .HasColumnType("text");

                    b.Property<string>("TableName")
                        .HasColumnType("text");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("AuditLogId");

                    b.ToTable("AuditLogs");
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("IdComentario");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid")
                        .HasColumnName("IdPublicacion");

                    b.Property<string>("Responsable")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("IdUsuario");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Comentario", (string)null);
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.PasswordRecovery", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("PasswordRecoveryToken")
                        .HasColumnType("uuid");

                    b.Property<string>("Responsable")
                        .HasColumnType("text");

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

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<string>("Responsable")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Publicacion", (string)null);
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

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

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

                    b.Property<string>("Responsable")
                        .HasColumnType("text");

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

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Responsable")
                        .HasColumnType("text");

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

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Responsable")
                        .HasColumnType("text");

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

                    b.HasData(
                        new
                        {
                            Id = new Guid("7c58cf3c-e0e5-44aa-812c-4fc26ccf16ac"),
                            CreatedAt = new DateTime(2023, 12, 23, 18, 57, 4, 214, DateTimeKind.Utc).AddTicks(9320),
                            Password = "10000.mmlVX3xzYuLQromOzqELBQ==.JIwrJbVGsgYiTMjqWqcvulmXk8Fv6c7hxbl8mEqixTI=",
                            Role = "Administrator",
                            UserId = new Guid("53aeeca4-a5b1-4751-abcb-3207a01b97dc"),
                            UserName = "admin"
                        });
                });

            modelBuilder.Entity("SocialMedia.Core.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("IdUsuario");

                    b.Property<DateTime>("BornDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<string>("Responsable")
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
                            CreatedAt = new DateTime(2023, 12, 23, 18, 57, 4, 215, DateTimeKind.Utc).AddTicks(1080),
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

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Responsable")
                        .HasColumnType("text");

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

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("Responsable")
                        .HasColumnType("text");

                    b.Property<string>("User")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("UserLogin");
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
