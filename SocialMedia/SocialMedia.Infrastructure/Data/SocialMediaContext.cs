using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SocialMedia.Core.Entities;
using SocialMedia.Infrastructure.Options;
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
        public virtual DbSet<PasswordRecovery> PasswordRecovery { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<RolModule> RolModule { get; set; }
        public virtual DbSet<UserInRoles> UserInRoles { get; set; }
        public virtual DbSet<Security> Security { get; set; }
        public virtual DbSet<UserLogin> UserLogin { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=localhost;Initial Catalog=SocialMedia;Persist Security Info=False;User ID=sa;Password=Pass@Word;Connection Timeout=30;TrustServerCertificate=True");
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("host=localhost;port=5432;database=SocialMedia;username=opinor_crm;password=opinor_crm");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Read all configuration classes from Data folder.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
