using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TravelGod.ru.Models;
using TravelGod.ru.Services;

namespace TravelGod.ru
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddDbContext<ApplicationContext>(x =>
            {
                var connStr = Configuration.GetConnectionString("DefaultConnection");
                x.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
                x.LogTo(Console.WriteLine, LogLevel.Information);
            });
            services.AddTransient<UserService>();
            services.AddTransient<TripService>();
            services.AddTransient<SessionService>();
            services.AddTransient<FileService>();
            services.AddTransient<ChatService>();
            services.AddTransient<CommentService>();
            services.AddTransient<MessageService>();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<AuthenticationMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }
    }
}