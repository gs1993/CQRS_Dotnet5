using Logic.Students.Models;
using Microsoft.EntityFrameworkCore;

namespace Logic.Utils
{
    public class EfStudentDbContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Disenrollment> Disenrollments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Student> Students { get; set; }


        public EfStudentDbContext(DbContextOptions<EfStudentDbContext> contextOptions) : base(contextOptions) { }
    }
}
