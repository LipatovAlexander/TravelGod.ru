using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TravelGod.ru.DAL;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Infrastructure.Validation;
using TravelGod.ru.Infrastructure.Validation.Interfaces;
using TravelGod.ru.Services.Middlewares;
using TravelGod.ru.ViewModels;

namespace TravelGod.ru
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages()
                    .AddMvcOptions(options =>
                    {
                        options.ModelValidatorProviders.Add(new ValidationProvider(services.BuildServiceProvider()));
                    })
                    .AddRazorPagesOptions(options =>
                    {
                        options.Conventions.AddPageRoute("/Trips/Concrete", "Trips/{id}");
                    });

            // Register ModelValidator<TModel> adapter class
            services.AddSingleton(typeof(ModelValidator<>), typeof(ModelValidator<>));
            // Auto-register all validator implementations
            services.AddScoped(typeof(IValidator<SignInModel>), typeof(SignInValidator));
            services.AddScoped(typeof(IValidator<SignUpModel>), typeof(SignUpValidator));
            services.AddScoped(typeof(IValidator<FormFile>), typeof(ImageValidator));

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

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Errors/{0}");

            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<AuthenticationMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }
    }
}