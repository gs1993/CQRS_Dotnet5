using Logic.Models;
using Logic.Repositories;
using Logic.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var config = new Config(3); // Deserialize from appsettings.json
            services.AddSingleton(config);

            services.AddSingleton(new CommandsConnectionString(Configuration["ConnectionString"]));
            services.AddSingleton(new QueriesConnectionString(Configuration["QueriesConnectionString"]));

            services.AddTransient<IGenericRepository<Student>, GenericRepository<Student>>();
            services.AddTransient<IGenericRepository<Course>, GenericRepository<Course>>();
            services.AddTransient<ICourseRepository, CourseRepository>();

            services.AddSingleton<Dispatcher>();
            services.AddHandlers();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.UseMiddleware<ExceptionHandler>();
        }
    }
}
