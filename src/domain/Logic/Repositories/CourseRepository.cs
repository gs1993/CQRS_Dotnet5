using System.Linq;
using Logic.Models;
using Logic.Utils;

namespace Logic.Repositories
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Course GetByName(string name);
    }

    public sealed class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(EfDbContext context) : base(context)
        {
        }

        public Course GetByName(string name)
        {
            return _context.Set<Course>()
                .SingleOrDefault(x => x.Name == name);
        }
    }
}
