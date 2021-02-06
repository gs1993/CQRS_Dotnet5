using Logic.AppServices;
using Logic.Commands;
using Logic.Dtos;
using Logic.Models;
using Logic.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using static Logic.AppServices.GetListQuery;
using static Logic.Commands.DisenrollCommand;
using static Logic.Commands.EditPersonalInfoCommand;
using static Logic.Commands.EnrollCommand;
using static Logic.Commands.RegisterCommand;
using static Logic.Commands.TransferCommand;
using static Logic.Commands.UnregisterCommand;

namespace Logic.Utils
{
    public static class Extensions
    {
        public static void RegisterStudentHandlers(this IServiceCollection services, string commandConnectionString, string queryConnectionString)
        {
            services.AddSingleton(new CommandsConnectionString(commandConnectionString));
            services.AddSingleton(new QueriesConnectionString(queryConnectionString));

            services.AddDbContext<EfDbContext>(options =>
            {
                options.UseSqlServer(commandConnectionString);
            });

            services.AddScoped<Dispatcher>();

            services.AddScoped<IGenericRepository<Student>, GenericRepository<Student>>();
            services.AddScoped<IGenericRepository<Course>, GenericRepository<Course>>();
            services.AddScoped<ICourseRepository, CourseRepository>();

            services.AddScoped<ICommandHandlerExecutor, CommandHandlerExecutor>();

            services.AddScoped<ICommandHandler<DisenrollCommand>, DisenrollCommandHandler>();
            services.AddScoped<ICommandHandler<UnregisterCommand>, UnregisterCommandHandler>();
            services.AddScoped<ICommandHandler<EnrollCommand>, EnrollCommandHandler>();
            services.AddScoped<ICommandHandler<TransferCommand>, TransferCommandHandler>();
            services.AddScoped<ICommandHandler<RegisterCommand>, RegisterCommandHandler>();
            services.AddScoped<ICommandHandler<EditPersonalInfoCommand>, EditPersonalInfoCommandHandler>();

            services.AddScoped<IQueryHandler<GetListQuery, IReadOnlyList<StudentDto>>, GetListQueryHandler>();
        }
    }
}
