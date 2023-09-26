﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Core.Entities;
using System;

namespace SocialMedia.Infrastructure.Data
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comentario");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("IdComentario").ValueGeneratedNever();

            builder.Property(e => e.PostId)
                .HasColumnName("IdPublicacion").ValueGeneratedNever();

            builder.Property(e => e.UserId)
                .HasColumnName("IdUsuario").ValueGeneratedNever();

            builder.Property(e => e.IsActive)
                .HasColumnName("Activo").ValueGeneratedNever();

            builder.Property(e => e.Description)
                .HasColumnName("Descripcion")
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false);

            builder.Property(e => e.Date)
                .HasColumnName("Fecha")
                .HasColumnType("datetime");

            builder.HasOne(d => d.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comentario_Publicacion");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Comment)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comentario_Usuario");

            //builder.HasData(
            //    new Comment()
            //    {
            //        Id = Guid.NewGuid(),
            //        Description = "Beatae doloremque error maxime dicta placeat numquam voluptatem sed laborum. ",
            //        Date = System.DateTime.Now,
            //        IsActive = true,
            //        PostId = 1,
            //        UserId = 1
            //    },
            //     new Comment()
            //     {
            //         Id = Guid.NewGuid(),
            //         Description = "Beatae doloremque error maxime dicta placeat numquam voluptatem sed laborum. ",
            //         Date = System.DateTime.Now,
            //         IsActive = true,
            //         PostId = 1,
            //         UserId = 1
            //     }
            //    );
        }
    }
}
