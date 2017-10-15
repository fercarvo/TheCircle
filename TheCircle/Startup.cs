using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TheCircle.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;

namespace TheCircle
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("TheCircle")));

            //Previene el uso de rutas que no usen HTTPS
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });


            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //Redireccion permanente al puerto seguro HTTPS
            app.UseRewriter(
                new RewriteOptions().AddRedirectToHttps(301, 4430)
            );

            env.EnvironmentName = EnvironmentName.Production;
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            } else {
                app.UseExceptionHandler("/error");
            }

            app.Use(async (context, next) => {
                context.Response.Headers.Append("X-Rights", "All rights reserved to Children International");
                context.Response.Headers.Append("X-Development-By", "Edgar Carvajal efcarvaj@espol.edu.ec");
                await next();
            });
            

            app.UseStaticFiles(
                new StaticFileOptions() {
                    OnPrepareResponse = ctx =>
                    {
                        //ctx.Context.Response.Headers.Append("Cache-Control", $"private,max-age={5}"); //Cache development
                        ctx.Context.Response.Headers.Append("Cache-Control", $"private,max-age={60*60*24*15}"); // 15 dias Cache produccion
                        ctx.Context.Response.Headers.Append("X-TheCircle", "Static Files");
                    }
                }
            );

            app.UseMvc();
        }
    }
}
