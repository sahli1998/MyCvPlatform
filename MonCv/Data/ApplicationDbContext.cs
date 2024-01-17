using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MonCv.Model;

namespace MonCv.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skills>()
                .HasMany(p => p.Projects)
                .WithMany(t => t.SkillsUsed)
                .UsingEntity<project_skill>(
                p => { p.HasOne(p => p.Skills).WithMany(p => p.project_Skills).HasForeignKey(p => p.SkillId);
                    p.HasOne(p => p.Project).WithMany(p => p.project_Skills).HasForeignKey(p => p.ProjectId);
                    p.HasKey(p => new {p.ProjectId,p.SkillId});
                    p.Property(p => p.create_date).HasDefaultValueSql("GetDate()");

                    }
                );
            modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserRole<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserToken<string>>().HasNoKey();

            modelBuilder.Entity<IdentityUserRole<string>>(b =>
            {
                b.HasKey(ur => new { ur.UserId, ur.RoleId });
                // Autres configurations si nécessaires
            });

        }
        public DbSet<Skills> Skills { get; set; }
        public DbSet<SkillCategory> SkillCategories { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<project_skill> project_skill { get; set; }
        
    }
}
