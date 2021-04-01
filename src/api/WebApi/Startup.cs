using Logic.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WebApi.Utils;
using Extensions;
using Cache.Redis;
using Cache;
using System.Collections.Generic;
using Logic.Students.Dtos;

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
            services.AddControllers();

            var databaseSettings = Configuration.BindSection<DatabaseSettings>("DatabaseSettings");
            services.RegisterStudentHandlers(databaseSettings);

            services.RegisterDispatcher();

            services.AddTransient<ExceptionHandlerMiddleware>();

            var registerSettings = Configuration.BindSection<RedisSettings>("RedisSettings");
            services.RegisterRedis(registerSettings);
            services.AddSingleton<ICacheService<IReadOnlyList<StudentDto>>, RedisCache<IReadOnlyList<StudentDto>>>(); //TODO: create autoregister

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCustomExceptionHandler();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
