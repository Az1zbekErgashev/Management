using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Entities.Attachment;
using ProjectManagement.Domain.Entities.Certificates;
using ProjectManagement.Domain.Entities.Companies;
using ProjectManagement.Domain.Entities.Logs;
using ProjectManagement.Domain.Entities.Partners;
using ProjectManagement.Domain.Entities.Projects;
using ProjectManagement.Domain.Entities.Task;
using ProjectManagement.Domain.Entities.Teams;
using ProjectManagement.Domain.Entities.User;
namespace ProjectManagement.Infrastructure.Contexts
{
    public class ProjectManagementDB : DbContext
    {
        public ProjectManagementDB(DbContextOptions<ProjectManagementDB> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskReport> TaskReports { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Domain.Entities.Task.Task> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Partners> Partners { get; set; }
        public DbSet<Logs> Logs { get; set; }
        public DbSet<Companies> Companies { get; set; }
        public DbSet<Certificates> Certificates { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<TaskReportPhotos> TaskReportPhotos { get; set; }
        public DbSet<TaskPhotos> TaskPhotos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamMember>()
                .HasKey(tm => new { tm.UserId, tm.TeamId });

            modelBuilder.Entity<Team>()
                .HasOne(t => t.Company)
                .WithMany(c => c.Teams)
                .HasForeignKey(t => t.AssignedCompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Companies)
                .WithMany()
                .HasForeignKey(p => p.AssignedCompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Partner)
                .WithMany()
                .HasForeignKey(p => p.PartnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Certificates)
                .WithOne(c => c.Project)
                .HasForeignKey<Certificates>(c => c.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Certificates>()
                .HasOne(c => c.Project)
                .WithOne(p => p.Certificates)
                .HasForeignKey<Certificates>(c => c.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Logs>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<User>()
                .HasIndex(u => u.Name);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email);
            modelBuilder.Entity<TaskReport>()
                .HasIndex(tr => tr.TaskId);
            modelBuilder.Entity<TaskReport>()
                .HasIndex(tr => tr.UserId);
            modelBuilder.Entity<Team>()
                .HasIndex(t => t.Name);
            modelBuilder.Entity<Team>()
                .HasIndex(t => t.AssignedCompanyId);

            modelBuilder.Entity<TeamMember>()
                .HasIndex(tm => tm.UserId);
            modelBuilder.Entity<TeamMember>()
                .HasIndex(tm => tm.TeamId);
            modelBuilder.Entity<Domain.Entities.Task.Task>()
                .HasIndex(t => t.Status);
            modelBuilder.Entity<Domain.Entities.Task.Task>()
                .HasIndex(t => t.ProjectId);
            modelBuilder.Entity<Project>()
                .HasIndex(p => p.TeamId);
            modelBuilder.Entity<Project>()
                .HasIndex(p => p.AssignedCompanyId);
            modelBuilder.Entity<Project>()
                .HasIndex(p => p.PartnerId);
            modelBuilder.Entity<Partners>()
                .HasIndex(p => p.Name);
            modelBuilder.Entity<Logs>()
                .HasIndex(l => l.UserId);
            modelBuilder.Entity<Certificates>()
                .HasIndex(c => c.ProjectId);
            modelBuilder.Entity<TaskReportPhotos>()
                .HasIndex(trp => trp.TaskReportId);
            modelBuilder.Entity<TaskPhotos>()
                .HasIndex(tp => tp.TaskId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
