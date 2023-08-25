using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using RecruitmentApp.BAL.ServicesAbstractions;
using RecruitmentApp.BAL.ServicesImplementation;
using RecruitmentApp.BAL.Validators;
using RecruitmentApp.Entities;
using RecruitmentApp.Repositories.Concretions;
using RecruitmentApp.Repositories.IRepositories;
using RecruitmentApp.Repositories.UnitOfWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace RecruitmentApp.Startup
{
    public class Startup
    {
        public static ServiceProvider Services { get; private set; } = null!;
        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            //var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<ICandidateRepository, CandidateRepository>();
            serviceCollection.AddScoped<IOpenRoleRepository, OpenRoleRepository>();
            serviceCollection.AddScoped<IRecruiterRepository, RecruiterRepository>();
            serviceCollection.AddScoped<IRecruitmentProcessRepository, RecruitmentProcessRepository>();
            serviceCollection.AddScoped<ICandidateService, CandidateService>();
            serviceCollection.AddScoped<IOpenRoleService, OpenRoleService>();
            serviceCollection.AddScoped<IRecruiterService, RecruiterService>();
            serviceCollection.AddScoped<IRecruitmentProcessService, RecruitmentProcessService>();
            serviceCollection.AddScoped<CandidateValidator>();
            serviceCollection.AddScoped<OpenRoleValidator>();
            serviceCollection.AddScoped<RecruiterValidator>();
            serviceCollection.AddScoped<RecruitmentContext>();
            serviceCollection.AddScoped<RecruitmentProcessValidator>();
            serviceCollection.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Debug);
                loggingBuilder.AddNLog();
            });
            serviceCollection.AddControllers();
            serviceCollection.AddAuthorization();
            serviceCollection.AddSwaggerGen();
            serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //Services = serviceCollection.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // In Development, use the Developer Exception Page
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // In Staging/Production, route exceptions to /error
                app.UseExceptionHandler("/error");
            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger for RecruitmentAppAPI v1");
                c.RoutePrefix = string.Empty;
                c.DocumentTitle = "RecruitmentAPI";
            });
        }
    }
}
