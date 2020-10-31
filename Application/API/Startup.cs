using System;
using Infrastructure.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Domain.Application;
using Domain.Model.Identity;
using Domain.Model.Verification;
using Domain.Service;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Converters;
using Twilio;

namespace API
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
                });

            services.AddCors();
            services.AddDbContext<FoodDeliveryDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("FoodDeliveryConnectionString")));

            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

            ConfigureAuthService(services);
            ConfigureVerificationService(services);
            ConfigureEmailSenderService(services);

            services.AddScoped<IAdminService, AdminService>();

            services.AddScoped<IMainService, MainService>();
            services.AddScoped<IImageService, ImageService>();

            services.AddDistributedMemoryCache();
            services.AddScoped<ICartService, CartService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            AddRoles(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowOrigin");
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });

           
        }

        private void AddRoles(IServiceCollection services)
        {
            var roleManager = services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var role in (UserRole[]) Enum.GetValues(typeof(UserRole)))
            {
                if (!roleManager.RoleExistsAsync(role.ToString()).Result)
                {
                    var identityRole = new IdentityRole(role.ToString());
                    _ = roleManager.CreateAsync(identityRole).Result;
                }
            }
        }

        private void ConfigureAuthService(IServiceCollection services)
        {
            services.AddIdentityCore<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<FoodDeliveryDbContext>();

            var jwt = new JwtSettings();
            Configuration.GetSection("JwtSettings").Bind(jwt);

            var tokenValidationParameter = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwt.Key)),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            services.AddSingleton(jwt);
            services.AddSingleton(tokenValidationParameter);

            services.AddScoped<IVerificationStateService, VerificationStateService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x => { x.TokenValidationParameters = tokenValidationParameter; });
            services.AddAuthorization();
        }

        private void ConfigureVerificationService(IServiceCollection services)
        {
            var accountSid = Configuration["Twilio:AccountSID"];
            var authToken = Configuration["Twilio:AuthToken"];
            TwilioClient.Init(accountSid, authToken);

            services.Configure<TwilioVerifySettings>(Configuration.GetSection("Twilio"));
            services.AddScoped<IVerificationService, VerificationService>();
        }

        private void ConfigureEmailSenderService(IServiceCollection services)
        {
            services.Configure<TwilioSendGridSettings>(Configuration.GetSection("SendGrid"));
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
