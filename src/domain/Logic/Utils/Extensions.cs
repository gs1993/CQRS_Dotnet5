using Logic.Commands;
using Logic.Models;
using Logic.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

            services.AddSingleton<Dispatcher>();

            services.AddTransient<IGenericRepository<Student>, GenericRepository<Student>>();
            services.AddTransient<IGenericRepository<Course>, GenericRepository<Course>>();
            services.AddTransient<ICourseRepository, CourseRepository>();

            services.AddTransient<ICommandHandlerExecutor, CommandHandlerExecutor>();

            services.AddTransient<ICommandHandler<DisenrollCommand>, DisenrollCommandHandler>();
            services.AddTransient<ICommandHandler<UnregisterCommand>, UnregisterCommandHandler>();
            services.AddTransient<ICommandHandler<EnrollCommand>, EnrollCommandHandler>();
            services.AddTransient<ICommandHandler<TransferCommand>, TransferCommandHandler>();
            services.AddTransient<ICommandHandler<RegisterCommand>, RegisterCommandHandler>();
            services.AddTransient<ICommandHandler<EditPersonalInfoCommand>, EditPersonalInfoCommandHandler>();

            var aaaa = services.BuildServiceProvider().GetServices(typeof(ICommandHandler<ICommand>));
        }
    }
}
