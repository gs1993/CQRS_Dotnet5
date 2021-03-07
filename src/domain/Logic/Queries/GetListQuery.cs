using System.Collections.Generic;
using System.Linq;
using Logic.Models;
using Logic.Utils;
using System.Data.SqlClient;
using Dapper;
using System.Threading.Tasks;
using Dto.Student;

namespace Logic.AppServices
{
    public sealed class GetListQuery : IQuery<IReadOnlyList<StudentDto>>
    {
        public string EnrolledIn { get; }
        public int? NumberOfCourses { get; }

        public GetListQuery(string enrolledIn, int? numberOfCourses)
        {
            EnrolledIn = enrolledIn;
            NumberOfCourses = numberOfCourses;
        }

        internal sealed class GetListQueryHandler : IQueryHandler<GetListQuery, IReadOnlyList<StudentDto>>
        {
            private readonly QueriesConnectionString _connectionString;

            public GetListQueryHandler(QueriesConnectionString connectionString)
            {
                _connectionString = connectionString;
            }

            public async Task<IReadOnlyList<StudentDto>> Handle(GetListQuery query)
            {
                string sql = @"
                    SELECT [Id], [Name], [Email], [Course1], [Course1Grade], [Course1Credits], [Course2], [Course2Grade], [Course2Credits]
                    FROM [CqrsInPractice].[dbo].[StudentsView]
                    WHERE (Course1 = @Course OR Course2 = @Course OR @Course IS NULL)
                    ORDER BY Id ASC";

                using (SqlConnection connection = new SqlConnection(_connectionString.Value))
                {
                    var students = await connection
                        .QueryAsync<StudentDto>(sql, new
                        {
                            Course = query.EnrolledIn,
                            Number = query.NumberOfCourses
                        });

                    return students.ToList();
                }
            }
        }
    }
}
