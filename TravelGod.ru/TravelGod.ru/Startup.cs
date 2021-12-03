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
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _environment;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddDbContext<ApplicationContext>(x =>
            {
                string connStr;
                if (_environment.IsDevelopment())
                {
                    connStr = Configuration.GetConnectionString("DefaultConnection");
                    x.LogTo(Console.WriteLine, LogLevel.Information);
                }
                else
                {
                    // Use connection string provided at runtime by Heroku.
                    var connUrl = Environment.GetEnvironmentVariable("CLEARDB_DATABASE_URL");

                    connUrl = connUrl.Replace("mysql://", string.Empty);
                    var userPassSide = connUrl.Split("@")[0];
                    var hostSide = connUrl.Split("@")[1];

                    var connUser = userPassSide.Split(":")[0];
                    var connPass = userPassSide.Split(":")[1];
                    var connHost = hostSide.Split("/")[0];
                    var connDb = hostSide.Split("/")[1].Split("?")[0];

                    connStr = $"server={connHost};Uid={connUser};Pwd={connPass};Database={connDb};SSL Mode=None";
                }

                x.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
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