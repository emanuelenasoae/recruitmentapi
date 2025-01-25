using RecruitmentAppAPI.Startup;

namespace RecruitmentAppAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();
            DIContainer.SetupDi(builder);

            var app = builder.Build();

            if (builder.Environment.IsDevelopment())
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

            app.Run();
        }
    }
}