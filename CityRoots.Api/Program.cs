using CityRoots.Core.Helpers;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using CityRoots.Core.Services;
using CityRoots.EF;
using CityRoots.EF.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc.Routing;
using CityRoots.Api.Helpers;
using CityRoots.Core.Interfaces.Services;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Dashboard;

namespace CityRoots.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Register Hangfire services
            builder.Services.AddHangfire(configuration =>
                configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                             .UseSimpleAssemblyNameTypeSerializer()
                             .UseRecommendedSerializerSettings()
                             .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                             {
                                 QueuePollInterval = TimeSpan.FromSeconds(15),
                                 SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                                 CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                                 UseRecommendedIsolationLevel = true,
                                 DisableGlobalLocks = true
                             }));

            // Add the Hangfire server
            builder.Services.AddHangfireServer();

            // Add services to the container.
            // Configure the password constraints
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredUniqueChars = 1;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"]
                };
            });

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IMailingService, MailingService>();
            builder.Services.AddScoped<ICommunicationService, CommunicationService>();
            builder.Services.AddScoped<IFarmService,FarmService>();
            builder.Services.AddScoped<IFarmerService,FarmerService>();
            builder.Services.AddScoped<IImageService,ImageService>();
            builder.Services.AddScoped<ILandParcelService,LandParcelService>();
            builder.Services.AddScoped<ICropService, CropService>();
            builder.Services.AddScoped<IHarvestService, HarvestService>();  
            builder.Services.AddHttpClient<IWeatherService, WeatherService>();
            builder.Services.AddScoped<ICycleService, CycleService>();
            builder.Services.AddScoped<IOpenInvestmentCycleService, OpenInvestmentCycleService>();
            builder.Services.AddScoped<ICycleUpdateService, CycleUpdateService>();
            builder.Services.AddScoped<AiPredictionService, AiPredictionService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IPurchaseRequestService, PurchaseRequestService>();
            builder.Services.AddScoped<IHarvestNotificationService, HarvestNotificationService>();
            builder.Services.AddScoped<CycleNotificationLogService, CycleNotificationLogService>();
            builder.Services.AddScoped< HarvestNotificationLogService>();
            builder.Services.AddScoped<ScheduleNotificationLogService>();
            builder.Services.AddScoped<RecommendationService,RecommendationService>();



            builder.Services.AddScoped<IScheduleService, ScheduleService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<ICycleNotificationService, CycleNotificationService>();
            builder.Services.AddScoped<IScheduleNotificationService, ScheduleNotificationService>();
            builder.Services.AddScoped<IFavouriteFarmersService, FavouriteFarmersService>();
            builder.Services.AddSingleton<PayPalService>();
           
            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.AddControllers();
     

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Register IActionContextAccessor and IUrlHelper
            builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            builder.Services.AddTransient<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            var app = builder.Build();

            // Seed roles
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var Context = services.GetRequiredService<ApplicationDbContext>();
                await SeedRolesAsync(roleManager);
                //await SendAllPredictData(Context);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHangfireDashboard("/Hangfire", new DashboardOptions
            {
                Authorization = new[] { new AllowAllAuthorizationFilter() }
            });

            // Schedule the job to run every 5 minutes
            RecurringJob.AddOrUpdate<NotificationBackGroundService>(
                "ProcessCycleNotifications",
                Jop => Jop.ProcessNotificationsAsync(),
                Cron.MinuteInterval(5)  // Cron expression: Every 5 minutes
            );

            app.UseHttpsRedirection();
            app.UseAuthentication(); // Ensure authentication middleware is added
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        // Method to seed roles
        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { "Investor", "Farmer", "Merchant" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        public class AllowAllAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                return true; // Allow all access. Replace with your logic for restricted access.
            }
        }
    }
}
