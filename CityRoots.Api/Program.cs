using CityRoots.Core.Helpers;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using CityRoots.Core.Services;
using CityRoots.EF;
using CityRoots.EF.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CityRoots.Core.Interfaces.Services;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Dashboard;
using CityRoots.Core.CustomValidation;
using Microsoft.OpenApi.Models;
using CityRoots.Core.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Serilog;

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

            builder.Services.AddHangfireServer();

            // Configure Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.AllowedUserNameCharacters = null;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredUniqueChars = 1;
            })
            .AddUserValidator<CustomUserValidator<ApplicationUser>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

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

                // Enable SignalR to use JWT Authentication
                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/ChatHub"))) // Ensure it's for SignalR
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            // Register Services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IMailingService, MailingService>();
            builder.Services.AddScoped<ICommunicationService, CommunicationService>();
            builder.Services.AddScoped<IFarmService, FarmService>();
            builder.Services.AddScoped<IFarmerService, FarmerService>();
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped<ILandParcelService, LandParcelService>();
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
            builder.Services.AddScoped<HarvestNotificationLogService>();
            builder.Services.AddScoped<ScheduleNotificationLogService>();
            builder.Services.AddScoped<RecommendationService, RecommendationService>();
            builder.Services.AddScoped<IInvestmentRequestService, InvestmentRequestService>();
            builder.Services.AddScoped<IRateService, RateService>();
            builder.Services.AddScoped<InteractionsService, InteractionsService>();
            builder.Services.AddScoped<IChatService, ChatService>();

            builder.Services.AddScoped<IScheduleService, ScheduleService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<ICycleNotificationService, CycleNotificationService>();
            builder.Services.AddScoped<IScheduleNotificationService, ScheduleNotificationService>();
            builder.Services.AddScoped<IFavouriteFarmersService, FavouriteFarmersService>();
            builder.Services.AddScoped<PayPalService>();

            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CityRoots API", Version = "v1" });

                // Add JWT Authentication to Swagger
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, new string[] { }}
    });
            });

            // SignalR Configuration
            builder.Services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
                options.MaximumReceiveMessageSize = 64 * 1024; // 64 KB
            });

            builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>(); // Custom provider for SignalR

            // CORS for SignalR
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    builder => builder
                        .WithOrigins("https://yourfrontenddomain.com", "http://localhost:3000") // Allow frontend URL
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            builder.Services.AddAutoMapper(typeof(Program));
            var logPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Logs", "log-.txt");

            Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Logs", "log-.txt"),
                  rollingInterval: RollingInterval.Day,
                  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}") // Include time in log entries
    .CreateLogger();


            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            // Seed roles
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var Context = services.GetRequiredService<ApplicationDbContext>();
                await SeedRolesAsync(roleManager);
            }

            // Middleware Configuration
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHangfireDashboard("/Hangfire", new DashboardOptions
            {
                Authorization = new[] { new AllowAllAuthorizationFilter() }
            });

            RecurringJob.AddOrUpdate<NotificationBackGroundService>(
                  "ProcessCycleNotifications",
                  Jop => Jop.ProcessNotificationsAsync(),
                  "*/5 * * * *"
              );
            app.UseStaticFiles();
            app.UseCors("AllowSpecificOrigins");
            app.UseAuthentication();
            app.UseAuthorization();

            // Register SignalR Hubs
            app.MapHub<ChatHub>("/ChatHub");

            app.MapControllers();
            app.Run();
        }

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
            public bool Authorize(DashboardContext context) => true;
        }
    }
}

