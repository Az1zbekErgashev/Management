using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Entities.Attachment;
using ProjectManagement.Domain.Entities.Country;
using ProjectManagement.Domain.Entities.Logs;
using ProjectManagement.Domain.Entities.MultilingualText;
using ProjectManagement.Domain.Entities.Requests;
using ProjectManagement.Domain.Entities.User;
namespace ProjectManagement.Infrastructure.Contexts
{
    public class ProjectManagementDB : DbContext
    {
        public ProjectManagementDB(DbContextOptions<ProjectManagementDB> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Logs> Logs { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Country> Countrys { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestStatus> RequestStatuses { get; set; }
        public DbSet<MultilingualText> MultilingualText { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Logs>()
                .HasOne(l => l.User)
                .WithMany(u => u.Logs)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
               .HasOne(u => u.Country)
               .WithMany()
               .HasForeignKey(u => u.CountryId);

            modelBuilder.Entity<User>()
               .HasOne(u => u.Image)
               .WithMany()
               .HasForeignKey(u => u.ImageId);

            modelBuilder.Entity<Request>()
                .HasOne(tm => tm.RequestStatus)
                .WithMany()
                .HasForeignKey(tm => tm.RequestStatusId);

            modelBuilder.Entity<Request>()
                .HasOne(tm => tm.File)
                .WithMany()
                .HasForeignKey(tm => tm.FileId);

            modelBuilder.Entity<Comments>()
                .HasOne(tm => tm.Request)
                .WithMany(x => x.Comments)
                .HasForeignKey(l => l.RequestId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<RequestHistory>()
                .HasOne(tm => tm.Request)
                .WithMany(x => x.History)
                .HasForeignKey(l => l.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RequestHistory>()
                .HasOne(tm => tm.User)
                .WithMany()
                .HasForeignKey(l => l.UserId);

            var user = new User
            {
                Id = 1,
                Name = "Admin",
                Email = "admin@gmail.com",
                Password = "web123$",
                PhoneNumber = "998881422030",
                Surname = "System",
                CreatedAt = DateTime.UtcNow,
                IndividualRole = Domain.Enum.Role.SuperAdmin,
                CountryId = 1,
                DateOfBirth = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc)
            };

            modelBuilder.Entity<User>().HasData(
               user
            );

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Name);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email);
            modelBuilder.Entity<Logs>()
                .HasIndex(l => l.UserId);
           

            modelBuilder.Entity<Country>().HasData(
               new Country() { Id = 1, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Afghanistan", IsDeleted = 0 },
               new Country() { Id = 2, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Åland Islands", IsDeleted = 0 },
               new Country() { Id = 3, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Albania", IsDeleted = 0 },
               new Country() { Id = 4, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Algeria", IsDeleted = 0 },
               new Country() { Id = 5, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "American Samoa", IsDeleted = 0 },
               new Country() { Id = 6, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Andorra", IsDeleted = 0 },
               new Country() { Id = 7, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Angola", IsDeleted = 0 },
               new Country() { Id = 8, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Anguilla", IsDeleted = 0 },
               new Country() { Id = 9, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Antarctica", IsDeleted = 0 },
               new Country() { Id = 10, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Antigua and Barbuda", IsDeleted = 0 },
               new Country() { Id = 11, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Argentina", IsDeleted = 0 },
               new Country() { Id = 12, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Armenia", IsDeleted = 0 },
               new Country() { Id = 13, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Aruba", IsDeleted = 0 },
               new Country() { Id = 14, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Australia", IsDeleted = 0 },
               new Country() { Id = 15, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Austria", IsDeleted = 0 },
               new Country() { Id = 16, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Azerbaijan", IsDeleted = 0 },
               new Country() { Id = 17, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Bahamas", IsDeleted = 0 },
               new Country() { Id = 18, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Bahrain", IsDeleted = 0 },
               new Country() { Id = 19, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Bangladesh", IsDeleted = 0 },
               new Country() { Id = 20, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Barbados", IsDeleted = 0 },
               new Country() { Id = 21, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Belarus", IsDeleted = 0 },
               new Country() { Id = 22, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Belgium", IsDeleted = 0 },
               new Country() { Id = 23, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Belize", IsDeleted = 0 },
               new Country() { Id = 24, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Benin", IsDeleted = 0 },
               new Country() { Id = 25, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Bermuda", IsDeleted = 0 },
               new Country() { Id = 26, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Bhutan", IsDeleted = 0 },
               new Country() { Id = 27, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Bolivia", IsDeleted = 0 },
               new Country() { Id = 28, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Bonaire, Sint Eustatius and Saba", IsDeleted = 0 },
               new Country() { Id = 29, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Bosnia and Herzegovina", IsDeleted = 0 },
               new Country() { Id = 30, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Botswana", IsDeleted = 0 },
               new Country() { Id = 31, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Bouvet Island", IsDeleted = 0 },
               new Country() { Id = 32, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Brazil", IsDeleted = 0 },
               new Country() { Id = 33, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "British Indian Ocean Territory", IsDeleted = 0 },
               new Country() { Id = 34, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Brunei Darussalam", IsDeleted = 0 },
               new Country() { Id = 35, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Bulgaria", IsDeleted = 0 },
               new Country() { Id = 36, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Burkina Faso", IsDeleted = 0 },
               new Country() { Id = 37, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Burundi", IsDeleted = 0 },
               new Country() { Id = 38, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Cambodia", IsDeleted = 0 },
               new Country() { Id = 39, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Cameroon", IsDeleted = 0 },
               new Country() { Id = 40, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Canada", IsDeleted = 0 },
               new Country() { Id = 41, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Cape Verde", IsDeleted = 0 },
               new Country() { Id = 42, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Cayman Islands", IsDeleted = 0 },
               new Country() { Id = 43, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Central African Republic", IsDeleted = 0 },
               new Country() { Id = 44, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Chad", IsDeleted = 0 },
               new Country() { Id = 45, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Chile", IsDeleted = 0 },
               new Country() { Id = 46, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "China", IsDeleted = 0 },
               new Country() { Id = 47, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Christmas Island", IsDeleted = 0 },
               new Country() { Id = 48, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Cocos (Keeling) Islands", IsDeleted = 0 },
               new Country() { Id = 49, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Colombia", IsDeleted = 0 },
               new Country() { Id = 50, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Comoros", IsDeleted = 0 },
               new Country() { Id = 51, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Congo, Republic of the (Brazzaville)", IsDeleted = 0 },
               new Country() { Id = 52, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Congo, the Democratic Republic of the (Kinshasa)", IsDeleted = 0 },
               new Country() { Id = 53, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Cook Islands", IsDeleted = 0 },
               new Country() { Id = 54, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Costa Rica", IsDeleted = 0 },
               new Country() { Id = 55, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Côte d'Ivoire, Republic of", IsDeleted = 0 },
               new Country() { Id = 56, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Croatia", IsDeleted = 0 },
               new Country() { Id = 57, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Cuba", IsDeleted = 0 },
               new Country() { Id = 58, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Curaçao", IsDeleted = 0 },
               new Country() { Id = 59, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Cyprus", IsDeleted = 0 },
               new Country() { Id = 60, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Czech Republic", IsDeleted = 0 },
               new Country() { Id = 61, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Denmark", IsDeleted = 0 },
               new Country() { Id = 62, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Djibouti", IsDeleted = 0 },
               new Country() { Id = 63, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Dominica", IsDeleted = 0 },
               new Country() { Id = 64, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Dominican Republic", IsDeleted = 0 },
               new Country() { Id = 65, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Ecuador", IsDeleted = 0 },
               new Country() { Id = 66, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Egypt", IsDeleted = 0 },
               new Country() { Id = 67, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "El Salvador", IsDeleted = 0 },
               new Country() { Id = 68, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Equatorial Guinea", IsDeleted = 0 },
               new Country() { Id = 69, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Eritrea", IsDeleted = 0 },
               new Country() { Id = 70, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Estonia", IsDeleted = 0 },
               new Country() { Id = 71, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Ethiopia", IsDeleted = 0 },
               new Country() { Id = 72, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Falkland Islands (Islas Malvinas)", IsDeleted = 0 },
               new Country() { Id = 73, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Faroe Islands", IsDeleted = 0 },
               new Country() { Id = 74, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Fiji", IsDeleted = 0 },
               new Country() { Id = 75, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Finland", IsDeleted = 0 },
               new Country() { Id = 76, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "France", IsDeleted = 0 },
               new Country() { Id = 77, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "French Guiana", IsDeleted = 0 },
               new Country() { Id = 78, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "French Polynesia", IsDeleted = 0 },
               new Country() { Id = 79, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "French Southern and Antarctic Lands", IsDeleted = 0 },
               new Country() { Id = 80, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Gabon", IsDeleted = 0 },
               new Country() { Id = 81, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Gambia, The", IsDeleted = 0 },
               new Country() { Id = 82, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Georgia", IsDeleted = 0 },
               new Country() { Id = 83, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Germany", IsDeleted = 0 },
               new Country() { Id = 84, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Ghana", IsDeleted = 0 },
               new Country() { Id = 85, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Gibraltar", IsDeleted = 0 },
               new Country() { Id = 86, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Greece", IsDeleted = 0 },
               new Country() { Id = 87, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Greenland", IsDeleted = 0 },
               new Country() { Id = 88, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Grenada", IsDeleted = 0 },
               new Country() { Id = 89, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Guadeloupe", IsDeleted = 0 },
               new Country() { Id = 90, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Guam", IsDeleted = 0 },
               new Country() { Id = 91, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Guatemala", IsDeleted = 0 },
               new Country() { Id = 92, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Guernsey", IsDeleted = 0 },
               new Country() { Id = 93, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Guinea", IsDeleted = 0 },
               new Country() { Id = 94, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Guinea-Bissau", IsDeleted = 0 },
               new Country() { Id = 95, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Guyana", IsDeleted = 0 },
               new Country() { Id = 96, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Haiti", IsDeleted = 0 },
               new Country() { Id = 97, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Heard Island and McDonald Islands", IsDeleted = 0 },
               new Country() { Id = 98, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Holy See (Vatican City)", IsDeleted = 0 },
               new Country() { Id = 99, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Honduras", IsDeleted = 0 },
               new Country() { Id = 100, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Hong Kong", IsDeleted = 0 },
               new Country() { Id = 101, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Hungary", IsDeleted = 0 },
               new Country() { Id = 102, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Iceland", IsDeleted = 0 },
               new Country() { Id = 103, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "India", IsDeleted = 0 },
               new Country() { Id = 104, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Indonesia", IsDeleted = 0 },
               new Country() { Id = 105, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Iran, Islamic Republic of", IsDeleted = 0 },
               new Country() { Id = 106, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Iraq", IsDeleted = 0 },
               new Country() { Id = 107, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Ireland", IsDeleted = 0 },
               new Country() { Id = 108, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Isle of Man", IsDeleted = 0 },
               new Country() { Id = 109, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Israel", IsDeleted = 0 },
               new Country() { Id = 110, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Italy", IsDeleted = 0 },
               new Country() { Id = 111, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Jamaica", IsDeleted = 0 },
               new Country() { Id = 112, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Japan", IsDeleted = 0 },
               new Country() { Id = 113, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Jersey", IsDeleted = 0 },
               new Country() { Id = 114, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Jordan", IsDeleted = 0 },
               new Country() { Id = 115, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Kazakhstan", IsDeleted = 0 },
               new Country() { Id = 116, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Kenya", IsDeleted = 0 },
               new Country() { Id = 117, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Kiribati", IsDeleted = 0 },
               new Country() { Id = 118, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Korea, Democratic People's Republic of", IsDeleted = 0 },
               new Country() { Id = 119, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Korea, Republic of", IsDeleted = 0 },
               new Country() { Id = 120, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Kosovo", IsDeleted = 0 },
               new Country() { Id = 121, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Kuwait", IsDeleted = 0 },
               new Country() { Id = 122, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Kyrgyzstan", IsDeleted = 0 },
               new Country() { Id = 123, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Laos", IsDeleted = 0 },
               new Country() { Id = 124, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Latvia", IsDeleted = 0 },
               new Country() { Id = 125, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Lebanon", IsDeleted = 0 },
               new Country() { Id = 126, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Lesotho", IsDeleted = 0 },
               new Country() { Id = 127, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Liberia", IsDeleted = 0 },
               new Country() { Id = 128, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Libya", IsDeleted = 0 },
               new Country() { Id = 129, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Liechtenstein", IsDeleted = 0 },
               new Country() { Id = 130, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Lithuania", IsDeleted = 0 },
               new Country() { Id = 131, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Luxembourg", IsDeleted = 0 },
               new Country() { Id = 132, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Macao", IsDeleted = 0 },
               new Country() { Id = 133, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Macedonia, Republic of", IsDeleted = 0 },
               new Country() { Id = 134, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Madagascar", IsDeleted = 0 },
               new Country() { Id = 135, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Malawi", IsDeleted = 0 },
               new Country() { Id = 136, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Malaysia", IsDeleted = 0 },
               new Country() { Id = 137, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Maldives", IsDeleted = 0 },
               new Country() { Id = 138, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Mali", IsDeleted = 0 },
               new Country() { Id = 139, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Malta", IsDeleted = 0 },
               new Country() { Id = 140, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Marshall Islands", IsDeleted = 0 },
               new Country() { Id = 141, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Martinique", IsDeleted = 0 },
               new Country() { Id = 142, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Mauritania", IsDeleted = 0 },
               new Country() { Id = 143, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Mauritius", IsDeleted = 0 },
               new Country() { Id = 144, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Mayotte", IsDeleted = 0 },
               new Country() { Id = 145, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Mexico", IsDeleted = 0 },
               new Country() { Id = 146, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Micronesia, Federated States of", IsDeleted = 0 },
               new Country() { Id = 147, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Moldova", IsDeleted = 0 },
               new Country() { Id = 148, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Monaco", IsDeleted = 0 },
               new Country() { Id = 149, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Mongolia", IsDeleted = 0 },
               new Country() { Id = 150, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Montenegro", IsDeleted = 0 },
               new Country() { Id = 151, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Montserrat", IsDeleted = 0 },
               new Country() { Id = 152, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Morocco", IsDeleted = 0 },
               new Country() { Id = 153, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Mozambique", IsDeleted = 0 },
               new Country() { Id = 154, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Myanmar", IsDeleted = 0 },
               new Country() { Id = 155, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Namibia", IsDeleted = 0 },
               new Country() { Id = 156, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Nauru", IsDeleted = 0 },
               new Country() { Id = 157, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Nepal", IsDeleted = 0 },
               new Country() { Id = 158, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Netherlands", IsDeleted = 0 },
               new Country() { Id = 159, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "New Caledonia", IsDeleted = 0 },
               new Country() { Id = 160, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "New Zealand", IsDeleted = 0 },
               new Country() { Id = 161, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Nicaragua", IsDeleted = 0 },
               new Country() { Id = 162, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Niger", IsDeleted = 0 },
               new Country() { Id = 163, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Nigeria", IsDeleted = 0 },
               new Country() { Id = 164, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Niue", IsDeleted = 0 },
               new Country() { Id = 165, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Norfolk Island", IsDeleted = 0 },
               new Country() { Id = 166, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Northern Mariana Islands", IsDeleted = 0 },
               new Country() { Id = 167, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Norway", IsDeleted = 0 },
               new Country() { Id = 168, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Oman", IsDeleted = 0 },
               new Country() { Id = 169, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Pakistan", IsDeleted = 0 },
               new Country() { Id = 170, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Palau", IsDeleted = 0 },
               new Country() { Id = 171, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Palestine, State of", IsDeleted = 0 },
               new Country() { Id = 172, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Panama", IsDeleted = 0 },
               new Country() { Id = 173, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Papua New Guinea", IsDeleted = 0 },
               new Country() { Id = 174, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Paraguay", IsDeleted = 0 },
               new Country() { Id = 175, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Peru", IsDeleted = 0 },
               new Country() { Id = 176, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Philippines", IsDeleted = 0 },
               new Country() { Id = 177, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Pitcairn", IsDeleted = 0 },
               new Country() { Id = 178, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Poland", IsDeleted = 0 },
               new Country() { Id = 179, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Portugal", IsDeleted = 0 },
               new Country() { Id = 180, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Puerto Rico", IsDeleted = 0 },
               new Country() { Id = 181, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Qatar", IsDeleted = 0 },
               new Country() { Id = 182, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Réunion", IsDeleted = 0 },
               new Country() { Id = 183, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Romania", IsDeleted = 0 },
               new Country() { Id = 184, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Russian Federation", IsDeleted = 0 },
               new Country() { Id = 185, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Rwanda", IsDeleted = 0 },
               new Country() { Id = 186, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Saint Barthélemy", IsDeleted = 0 },
               new Country() { Id = 187, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Saint Helena, Ascension and Tristan da Cunha", IsDeleted = 0 },
               new Country() { Id = 188, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Saint Kitts and Nevis", IsDeleted = 0 },
               new Country() { Id = 189, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Saint Lucia", IsDeleted = 0 },
               new Country() { Id = 190, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Saint Martin", IsDeleted = 0 },
               new Country() { Id = 191, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Saint Pierre and Miquelon", IsDeleted = 0 },
               new Country() { Id = 192, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Saint Vincent and the Grenadines", IsDeleted = 0 },
               new Country() { Id = 193, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Samoa", IsDeleted = 0 },
               new Country() { Id = 194, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "San Marino", IsDeleted = 0 },
               new Country() { Id = 195, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Sao Tome and Principe", IsDeleted = 0 },
               new Country() { Id = 196, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Saudi Arabia", IsDeleted = 0 },
               new Country() { Id = 197, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Senegal", IsDeleted = 0 },
               new Country() { Id = 198, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Serbia", IsDeleted = 0 },
               new Country() { Id = 199, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Seychelles", IsDeleted = 0 },
               new Country() { Id = 200, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Sierra Leone", IsDeleted = 0 },
               new Country() { Id = 201, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Singapore", IsDeleted = 0 },
               new Country() { Id = 202, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Sint Maarten (Dutch part)", IsDeleted = 0 },
               new Country() { Id = 203, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Slovakia", IsDeleted = 0 },
               new Country() { Id = 204, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Slovenia", IsDeleted = 0 },
               new Country() { Id = 205, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Solomon Islands", IsDeleted = 0 },
               new Country() { Id = 206, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Somalia", IsDeleted = 0 },
               new Country() { Id = 207, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "South Africa", IsDeleted = 0 },
               new Country() { Id = 208, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "South Georgia and South Sandwich Islands", IsDeleted = 0 },
               new Country() { Id = 209, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "South Sudan", IsDeleted = 0 },
               new Country() { Id = 210, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Spain", IsDeleted = 0 },
               new Country() { Id = 211, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Sri Lanka", IsDeleted = 0 },
               new Country() { Id = 212, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Sudan", IsDeleted = 0 },
               new Country() { Id = 213, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Suriname", IsDeleted = 0 },
               new Country() { Id = 214, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Eswatini", IsDeleted = 0 },
               new Country() { Id = 215, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Sweden", IsDeleted = 0 },
               new Country() { Id = 216, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Switzerland", IsDeleted = 0 },
               new Country() { Id = 217, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Syrian Arab Republic", IsDeleted = 0 },
               new Country() { Id = 218, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Taiwan", IsDeleted = 0 },
               new Country() { Id = 219, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Tajikistan", IsDeleted = 0 },
               new Country() { Id = 220, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Tanzania, United Republic of", IsDeleted = 0 },
               new Country() { Id = 221, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Thailand", IsDeleted = 0 },
               new Country() { Id = 222, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Timor-Leste", IsDeleted = 0 },
               new Country() { Id = 223, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Togo", IsDeleted = 0 },
               new Country() { Id = 224, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Tokelau", IsDeleted = 0 },
               new Country() { Id = 225, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Tonga", IsDeleted = 0 },
               new Country() { Id = 226, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Trinidad and Tobago", IsDeleted = 0 },
               new Country() { Id = 227, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Tunisia", IsDeleted = 0 },
               new Country() { Id = 228, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Turkey", IsDeleted = 0 },
               new Country() { Id = 229, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Turkmenistan", IsDeleted = 0 },
               new Country() { Id = 230, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Turks and Caicos Islands", IsDeleted = 0 },
               new Country() { Id = 231, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Tuvalu", IsDeleted = 0 },
               new Country() { Id = 232, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Uganda", IsDeleted = 0 },
               new Country() { Id = 233, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Ukraine", IsDeleted = 0 },
               new Country() { Id = 234, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "United Arab Emirates", IsDeleted = 0 },
               new Country() { Id = 235, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "United Kingdom", IsDeleted = 0 },
               new Country() { Id = 236, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "United States", IsDeleted = 0 },
               new Country() { Id = 237, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "United States Minor Outlying Islands", IsDeleted = 0 },
               new Country() { Id = 238, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Uruguay", IsDeleted = 0 },
               new Country() { Id = 239, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Uzbekistan", IsDeleted = 0 },
               new Country() { Id = 240, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Vanuatu", IsDeleted = 0 },
               new Country() { Id = 241, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Venezuela, Bolivarian Republic of", IsDeleted = 0 },
               new Country() { Id = 242, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Vietnam", IsDeleted = 0 },
               new Country() { Id = 243, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Virgin Islands, British", IsDeleted = 0 },
               new Country() { Id = 244, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Virgin Islands, U.S.", IsDeleted = 0 },
               new Country() { Id = 245, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Wallis and Futuna", IsDeleted = 0 },
               new Country() { Id = 246, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Western Sahara", IsDeleted = 0 },
               new Country() { Id = 247, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Yemen", IsDeleted = 0 },
               new Country() { Id = 248, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Zambia", IsDeleted = 0 },
               new Country() { Id = 249, CreatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), UpdatedAt = new DateTime(2023, 11, 23, 16, 13, 56, 461, DateTimeKind.Utc), Name = "Zimbabwe", IsDeleted = 0 }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
