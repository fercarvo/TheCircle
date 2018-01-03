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
                .UseKestrel(/*options =>
                {
                    options.UseHttps("Production/thecircle.pfx", "P@s5w0rdTh3C1rCl3");
                }*/)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .UseUrls("http://*:80")
                .Build();

            host.Run();
        }
    }
}
