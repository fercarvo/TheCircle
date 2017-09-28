using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace TheCircle
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel(options =>
                {
                    options.UseHttps("Development/localhost.pfx", "DevelopmentSSLCI"); //cambiar a 192-168-16-60.pfx para probar en otras maquinas
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .UseUrls("https://*:4430", "http://*:5000")
                .Build();

            host.Run();
        }
    }
}
