using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Logic.Students.Models;
using Logic.Utils;
using Microsoft.EntityFrameworkCore;

namespace Logic.Students.Repositories
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<Maybe<Course>> GetByName(string name);
    }

    public sealed class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(EfDbContext context) : base(context)
        {
        }

        public async Task<Maybe<Course>> GetByName(string name)
        {
            var course = await _context.Set<Course>()
               .FirstOrDefaultAsync(x => x.Name == name);

            return course ?? Maybe<Course>.None;
        }
    }
}
