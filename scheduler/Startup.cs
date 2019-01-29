using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using scheduler.DataStore;
using scheduler.Models;
using scheduler.Services;
using Swashbuckle.AspNetCore.Swagger;
using WebAPIApplication;

namespace scheduler
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
            .AddJsonOptions(o =>
            {
                if (o.SerializerSettings.ContractResolver != null)
                {
                    var castedResolver = o.SerializerSettings.ContractResolver
                     as DefaultContractResolver;
                    castedResolver.NamingStrategy = null;
                }
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "TimeTable API",
                    Version = "v1",
                    Description = "A simple scheduler API",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Voltis Agolli",
                        Email = "v.agolli@live.com",
                        Url = string.Empty
                    }
                });
            });

            string domain = $"https://{Configuration["Auth0:Domain"]}/";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0:ApiIdentifier"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = "http://localhost:3000/roles"
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:courses", policy => policy.Requirements.Add(new HasScopeRequirement("read:courses", domain)));
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            services.AddDbContext<ScheduleContext>(o =>
                o.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ISchdulesRepository, SchedulesRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, ScheduleContext scheduleContext)
        {
            loggerFactory.AddNLog();

            app.UseSwagger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            scheduleContext.EnsureSeedDataForContext();

            app.UseStatusCodePages();

            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<Schedule, ScheduleViewModel>().ReverseMap();
                config.CreateMap<Schedule, Schedule>();
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TimeTable API V1");
            });

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
