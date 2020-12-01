using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StratisQAPI.Data;
using StratisQAPI.Entities;
using StratisQAPI.Helpers;
using StratisQAPI.Services;

namespace StratisQAPI
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
            services.AddIdentity<ApplicationUser, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredLength = 5;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequireUppercase = false;
            }).
            AddEntityFrameworkStores<StratisQDbContextUsers>().
            AddDefaultTokenProviders();
            services.AddSwaggerGen(settings =>
            {
                settings.SwaggerDoc("v1", new OpenApiInfo { Title = "StratisQ API", Version = "v1" });
                settings.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });

            services.AddAuthentication().
            AddJwtBearer(cfg => {
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    //ValidIssuer = "http://qaapi.stratisq.co.za:81",
                    //ValidAudience = "http://qa.stratisq.co.za",
                    ValidIssuer = "https://localhost:44391",
                    ValidAudience = "http://localhost:4200",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysupersedkjhulfgyuerfw344cret"))
                };

            });

            services.AddDbContext<StratisQDbContext>
            (options => options.UseSqlServer(Configuration.GetConnectionString("StratisQDbContext")));

            services.AddDbContext<StratisQDbContextUsers>
            (options => options.UseSqlServer(Configuration.GetConnectionString("StratisQDbUsersContext")));

            services.AddTransient<Seed>();
            services.AddSingleton<IHostedService, MyServiceA>();

            services.AddCors(cfg => {

                cfg.AddPolicy(name: "CorsPolicy",
                    builder => builder
                    .WithOrigins("http://qa.stratisq.co.za", "http://localhost:4200")
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader().Build()
                    );
            });


            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Seed seed)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            seed.SaveTenant();
            seed.SeedUser();
            seed.SeedRoles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "StratisQ API V1");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
