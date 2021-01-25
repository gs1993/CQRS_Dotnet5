using System.Linq;
using Logic.Models;
using Logic.Utils;

namespace Logic.Repositories
{
    public sealed class CourseRepository
    {
        private readonly UnitOfWork _unitOfWork;

        public CourseRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Course GetByName(string name)
        {
            return _unitOfWork.Query<Course>()
                .SingleOrDefault(x => x.Name == name);
        }
    }
}
