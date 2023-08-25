using RecruitmentApp.Startup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace RecruitmentAppAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            CreateHostBuilder().Build().Run();
        }

        public static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder().ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
        }
    }
}