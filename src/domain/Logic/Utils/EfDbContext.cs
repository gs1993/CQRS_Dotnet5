using Logic.Models;
using Microsoft.EntityFrameworkCore;

namespace Logic.Utils
{
    public class EfDbContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Disenrollment> Disenrollments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Student> Students { get; set; }


        public EfDbContext(DbContextOptions<EfDbContext> contextOptions) : base(contextOptions) { }
    }
}
