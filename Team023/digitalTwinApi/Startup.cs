using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using digitalTwinApi.Hubs;
using digitalTwinApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace digitalTwinApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None
            };
            
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                            .WithOrigins("http://localhost:3000")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });

            services.AddDbContext<DigitalTwinContext>(options =>
                //options.UseNpgsql("Server=localhost;Port=5432;Database=digitaltwin;User Id=postgres;Password=admin"));
                options.UseNpgsql("Server=digitaltwindb.postgres.database.azure.com;Database=digitaltwin;Port=5432;User Id=djamadmin@digitaltwindb;Password=Welkom2019!"));
            
            services.AddSignalR()
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerSettings.ContractResolver = jsonSerializerSettings.ContractResolver;
                    options.PayloadSerializerSettings.ReferenceLoopHandling = jsonSerializerSettings.ReferenceLoopHandling;
                    options.PayloadSerializerSettings.PreserveReferencesHandling = jsonSerializerSettings.PreserveReferencesHandling;
                });
            
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = jsonSerializerSettings.ContractResolver;
                    options.SerializerSettings.ReferenceLoopHandling = jsonSerializerSettings.ReferenceLoopHandling;
                    options.SerializerSettings.PreserveReferencesHandling = jsonSerializerSettings.PreserveReferencesHandling;
                })
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
  
            app.UseCors(Policies.CORS);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("AllowAll");
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            
            app.UseSignalR(routes =>
            {
                routes.MapHub<SensorDataHub>("/sensors");
            });
        }
    }
}
