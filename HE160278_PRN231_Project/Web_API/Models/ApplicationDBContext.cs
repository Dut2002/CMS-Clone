using Microsoft.EntityFrameworkCore;

namespace Web_API.Models
{
    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options){ }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseManager> CourseManagers { get; set; }
        public DbSet<ContentCourse> ContentCourses { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DownloadFile> DownloadFiles { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseManager>()
                .HasOne(cm => cm.User)
                .WithMany(u => u.CourseManagers)
                .HasForeignKey(cm => cm.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.CourseManagers)
                .WithOne(cm => cm.Course)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.ContentCourses)
                .WithOne(cc => cc.Course)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ContentCourse>()
               .HasMany(cc => cc.Documents)
               .WithOne(d => d.ContentCourse)
               .HasForeignKey(d => d.ContentId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ContentCourse>()
                .HasMany(cc => cc.DownloadFiles)
                .WithOne(df => df.ContentCourse)
                .HasForeignKey(df => df.ContentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ContentCourse>()
                .HasMany(cc => cc.Assignments)
                .WithOne(a => a.ContentCourse)
                .HasForeignKey(a => a.ContentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ContentCourse>()
                .HasMany(cc => cc.Links)
                .WithOne(l => l.ContentCourse)
                .HasForeignKey(l => l.ContentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
