using NLog.Extensions.Logging;
using RecruitmentApp.BAL.ServicesAbstractions;
using RecruitmentApp.BAL.ServicesImplementation;
using RecruitmentApp.BAL.Validators;
using RecruitmentApp.Entities;
using RecruitmentApp.Repositories.Concretions;
using RecruitmentApp.Repositories.IRepositories;
using RecruitmentApp.Repositories.UnitOfWork;
using RecruitmentAppAPI.Controllers.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RecruitmentAppAPI.OptionsSetup;
using RecruitmentAppAPI.Authentication.Abstractions;
using RecruitmentAppAPI.Authentication.Login;
using RecruitmentAppAPI.Authentication.InfrastructureLayer.Authentication;

namespace RecruitmentAppAPI.Startup
{
    public static class DIContainer
    {
        public static void SetupDi(WebApplicationBuilder builder)
        {
            var services = GetServiceCollection();
            foreach (var service in services)
            {
                builder.Services.Add(service);
            }
        }

        public static IServiceCollection GetServiceCollection()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<ICandidateRepository, CandidateRepository>();
            serviceCollection.AddScoped<IOpenRoleRepository, OpenRoleRepository>();
            serviceCollection.AddScoped<IRecruiterRepository, RecruiterRepository>();
            serviceCollection.AddScoped<IRecruitmentProcessRepository, RecruitmentProcessRepository>();
            serviceCollection.AddScoped<ICandidateService, CandidateService>();
            serviceCollection.AddScoped<IOpenRoleService, OpenRoleService>();
            serviceCollection.AddScoped<IRecruiterService, RecruiterService>();
            serviceCollection.AddScoped<IRecruitmentProcessService, RecruitmentProcessService>();
            serviceCollection.AddScoped<IMemberService, MemberService>();
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
            serviceCollection.AddAutoMapper(typeof(MappingProfiles));

            serviceCollection.AddScoped<ILoginHandler, LoginHandler>();
            serviceCollection.AddScoped<IJwtProvider, JwtProvider>();

            serviceCollection.ConfigureOptions<JwtOptionsSetup>();
            serviceCollection.ConfigureOptions<JwtBearerOptionsSetup>();
            serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            return serviceCollection;
        }
    }
}
