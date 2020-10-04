using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NW.Repository;
using NW.Discord;

namespace NW
{
    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IRepository, MongoDBRepository>();
            services.AddSingleton<IDiscord, DiscordClient>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ApplicationServices.GetService<IDiscord>().Login();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseTokenAuthentication();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
