using AnimeciBackend.Data;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System;

namespace AnimeciBackend
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Anime), typeof(Bolum), typeof(Video), typeof(UAnimeListe));

            services.AddDbContext<AnimeciDbContext>(options =>
                //options.UseNpgsql(Configuration.GetConnectionString("PostgreSQL")));
                options.UseNpgsql(Environment.GetEnvironmentVariable("PGSQL_CSTR")));

            services.AddIdentity<ApplicationUser, IdentityRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequiredLength = 6;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<AnimeciDbContext>()
                .AddDefaultTokenProviders();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            // RSAKeyUtils.GenerateKeyAndSave("idsrv4animeci.json");
            RsaSecurityKey rsa = new RsaSecurityKey(RSAKeyUtils.GetKeyParameters("idsrv4animeci.json")); 

            services.AddIdentityServer()
                // .AddTemporarySigningCredential()
                .AddSigningCredential(rsa)
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<ApplicationUser>();

            services.AddSingleton<ParseFrom>();
            services.AddSingleton<FCheckup>();

            //services.AddQuartz();

            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters().AddJsonOptions(options =>
                {
                    // options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCors("CorsPolicy");

            app.UseStaticFiles();

            app.UseIdentity();
            app.UseIdentityServer();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = "http://animeci.cdilbaz.com/",
                // Authority = "http://localhost:5000/",
                AllowedScopes = { "api1" },

                RequireHttpsMetadata = false
            });

            app.UseMvc();
        }
    }
}
