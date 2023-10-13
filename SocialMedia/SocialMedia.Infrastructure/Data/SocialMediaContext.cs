using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using System.Reflection;

namespace SocialMedia.Infrastructure.Data
{
    public partial class SocialMediaContext : DbContext
    {
        public SocialMediaContext()
        {
        }

        public SocialMediaContext(DbContextOptions<SocialMediaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Server=localhost;Initial Catalog=EcomKit; Persist Security Info=False;User ID=sa;Password=Pass@Word;Connection Timeout=60;");
                optionsBuilder.UseSqlServer("Server=localhost;Initial Catalog=EcomKit;Persist Security Info=False;User ID=sa;Password=Pass@Word;Connection Timeout=30;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Read all configuration classes from Data folder.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
