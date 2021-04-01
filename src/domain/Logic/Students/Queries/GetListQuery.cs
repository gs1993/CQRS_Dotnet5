using System.Collections.Generic;
using System.Linq;
using Logic.Utils;
using System.Data.SqlClient;
using Dapper;
using System.Threading.Tasks;
using System;
using Logic.Students.Dtos;
using Logic.Utils.Shared;
using Logic.Utils.Decorators.Query;

namespace Logic.Students.Queries
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

        [Cache(slidingExpirationInMinutes: 1, absoluteExpirationInMinutes: 5)]
        internal sealed class GetListQueryHandler : IQueryHandler<GetListQuery, IReadOnlyList<StudentDto>>
        {
            private readonly string _connectionString;

            public GetListQueryHandler(DatabaseSettings databaseSettings)
            {
                _connectionString = databaseSettings?.QueriesConnectionString ?? throw new ArgumentException(nameof(databaseSettings));
            }

            public async Task<IReadOnlyList<StudentDto>> Handle(GetListQuery query)
            {
                string sql = @"
                    SELECT [Id], [Name], [Email], [Course1], [Course1Grade], [Course1Credits], [Course2], [Course2Grade], [Course2Credits]
                    FROM [CqrsInPractice].[dbo].[StudentsView]
                    WHERE (Course1 = @Course OR Course2 = @Course OR @Course IS NULL)
                    ORDER BY Id ASC";

                using var connection = new SqlConnection(_connectionString);
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
