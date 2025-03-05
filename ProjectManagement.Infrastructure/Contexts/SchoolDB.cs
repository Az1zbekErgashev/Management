using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Entities.Classes;
using ProjectManagement.Domain.Entities.Students;
namespace ProjectManagement.Infrastructure.Contexts
{
    public class ProjectManagementDB : DbContext
    {
        public ProjectManagementDB(DbContextOptions<ProjectManagementDB> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Class)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.ClassId);
        }
    }
}
