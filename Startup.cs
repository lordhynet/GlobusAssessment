using FluentValidation;
using FluentValidation.AspNetCore;
using GlobusAssessment.Application.DTOs;
using GlobusAssessment.Application.Services.Implementations;
using GlobusAssessment.Application.Services.Interfaces;
using GlobusAssessment.Application.Settings;
using GlobusAssessment.Domain.Models;
using GlobusAssessment.Persistence;
using GlobusAssessment.Persistence.Repositories;
using GLobusAssessment.Api.Filters;
using GLobusAssessment.Api.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;


namespace GLobusAssessment.Api
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

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                })
                .AddFluentValidation();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GlobusAssessment.API", Version = "v1" });
                c.ParameterFilter<StateParameterFilter>();
                c.ParameterFilter<LocalGovernmentParameterFilter>();

            }).AddSwaggerGenNewtonsoftSupport();

            services.AddDbContextPool<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<Customer, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddTransient<IValidator<AddCustomerDto>, AddCustomerModelValidator>();
            services.AddTransient<IValidator<ConfirmPhoneNumberDto>, ConfirmPhoneNumberModelValidator>();

            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IUserRepository, UserRepository>();
            services.Configure<GoldSettings>(Configuration.GetSection(GoldSettings.ConfigSection));
            services.AddScoped<IGoldService, GoldService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOtpService, OtpService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GlobusAssessment.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //Seed the database with some users and the state with lga
            DatabaseSeeder.EnsurePopulated(app).Wait();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
