using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Logic.Students.Commands;
using Logic.Students.Queries;
using Logic.Utils.Shared;
using Logic.Studentss.Commands;
using Logic.Students.Models.Dtos;
using static Logic.Students.Queries.GetListQuery;
using static Logic.Studentss.Commands.DisenrollCommand;
using static Logic.Students.Commands.EditPersonalInfoCommand;
using static Logic.Students.Commands.EnrollCommand;
using static Logic.Students.Commands.RegisterCommand;
using static Logic.Students.Commands.TransferCommand;
using static Logic.Students.Commands.UnregisterCommand;
using TanvirArjel.EFCore.GenericRepository;

namespace Logic.Utils
{
    public static class Extensions
    {
        public static void RegisterDbContext(this IServiceCollection services, DatabaseSettings databaseSettings)
        {
            services.AddSingleton(databaseSettings);

            services.AddDbContext<EfDbContext>(options =>
            {
                options.UseSqlServer(databaseSettings.ConnectionString);
            });

            services.AddGenericRepository<EfDbContext>();
        }

        public static void RegisterStudentHandlers(this IServiceCollection services)
        {
            services.AddScoped<Dispatcher>();

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
