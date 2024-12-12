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

namespace CityRoots.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            builder.Services.AddScoped<IScheduleService, ScheduleService>();
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
        private static async Task SendAllPredictData(ApplicationDbContext _context)
        {
            var predictData = new List<AiPredict>
    {
        new AiPredict {
            ArabicName = "جرب التفاح",
            EnglishName = "Scab Apple",
            Diagnosis = "بقع بنیة على الثمار، بقع صفراء على الأوراق",
            Recommendation = "مبیدات فطریة، تقلیم الأغصان المصابة"
        },
        new AiPredict {
            ArabicName = "العفن الأسود في التفاح",
            EnglishName = "Rot Black Apple",
            Diagnosis = "بقع سوداء على الثمار، تساقط الأوراق",
            Recommendation = "مبیدات فطریة، إزالة الأجزاء المصابة"
        },
        new AiPredict {
            ArabicName = "صدأ التفاح الناتج عن الأرز",
            EnglishName = "Rust Cedar Apple",
            Diagnosis = "بقع برتقالیة على الأوراق، تشوھات في الثمار",
            Recommendation = "مبیدات فطریة، زراعة أصناف مقاومة"
        },
        new AiPredict {
            ArabicName = "البیاض الدقیقي على الكرز",
            EnglishName = "Mildew Powdery Cherry",
            Diagnosis = "ظھور طبقة بیضاء دقیقیة على الأوراق والبراعم والثمار",
            Recommendation = "استخدام مبیدات فطریة مناسبة، تقلیم الأغصان المصابة، زراعة أصناف مقاومة"
        },
        new AiPredict {
            ArabicName = "بقعة الورقة الرمادیة على الذرة",
            EnglishName = "Spot Leaf Gray Corn",
            Diagnosis = "ظھور بقع رمادیة على أوراق الذرة، تطور البقع إلى خطوط رمادیة داكنة، موت الأنسجة المصابة",
            Recommendation = "زراعة أصناف مقاومة، تدویر المحاصیل، استخدام مبیدات فطریة"
        },
        new AiPredict {
            ArabicName = "صدأ الذرة الشائع",
            EnglishName = "Rust Common Corn",
            Diagnosis = "ظھور بقع برتقالیة أو بنیة على سطح الأوراق، تطور البقع إلى بثرات تنتشر منھا الأبواغ الفطریة، تقلیل محصول الذرة",
            Recommendation = "زراعة أصناف مقاومة، استخدام مبیدات فطریة، تدویر المحاصیل"
        },
        new AiPredict {
            ArabicName = "بقعة الورقة الشمالیة على الذرة",
            EnglishName = "Blight Leaf Northern Corn",
            Diagnosis = "ظھور بقع بیضاویة الشكل على أوراق الذرة، تطور البقع إلى خطوط طویلة داكنة اللون، موت الأنسجة المصابة",
            Recommendation = "زراعة أصناف مقاومة، تدویر المحاصیل، استخدام مبیدات فطریة"
        },
        new AiPredict {
            ArabicName = "العفن الأسود على العنب",
            EnglishName = "Rot Black Grape",
            Diagnosis = "ظھور بقع بنیة داكنة على الأوراق والعنقود، تطور البقع إلى اللون الأسود، موت الأنسجة المصابة",
            Recommendation = "تقلیم الأغصان المصابة، استخدام مبیدات فطریة، زراعة أصناف مقاومة"
        },
        new AiPredict {
            ArabicName = "الجدري الأسود على العنب",
            EnglishName = "Measles Black Grape",
            Diagnosis = "ظھور بقع سوداء صغیرة على الأوراق، تطور البقع إلى قرحات غائرة، ضعف نمو الكرمة",
            Recommendation = "تقلیم الأغصان المصابة، استخدام مبیدات فطریة، زراعة أصناف مقاومة"
        },
        new AiPredict {
            ArabicName = "مرض اصفرار الشجر الحمضي",
            EnglishName = "Huanglongbing Orange",
            Diagnosis = "اصفرار الأوراق، تقزم الشجرة، ثمار صغیرة وقلیلة العصیر",
            Recommendation = "إزالة الأشجار المصابة، مكافحة الحشرات الناقلة للمرض، زراعة أصناف مقاومة"
        },
        new AiPredict {
            ArabicName = "فیروس تجعد الأوراق الصفراء للطماطم",
            EnglishName = "Virus Curl Leaf Yellow Tomato",
            Diagnosis = "اصفرار وتجعد وانحناء أوراق لأسفل",
            Recommendation = "مكافحة الحشرات (ذبابة بیضاء)، أصناف مقاومة"
        },
         new AiPredict {
            ArabicName = "البقعة البكتیریة على الدراق",
            EnglishName = "Spot Bacterial Peach",
            Diagnosis = "ظھور بقع صغیرة دائریة على الأوراق والثمار، تطور البقع إلى قرحات، تساقط الأوراق والثمار",
            Recommendation = "تقلیم الأغصان المصابة، استخدام مبیدات بكتیریة، زراعة أصناف مقاومة"
        },
        new AiPredict {
            ArabicName = "بقعة الورقة على العنب",
            EnglishName = "Blight Leaf Grape",
            Diagnosis = "ظھور بقع بنیة على الأوراق، تطور البقع إلى مناطق میتة، تساقط الأوراق",
            Recommendation = "تقلیم الأغصان المصابة، استخدام مبیدات فطریة، زراعة أصناف مقاومة"
        },
        new AiPredict {
            ArabicName = "بقعة بكتیریة على الفلفل",
            EnglishName = "Spot Bacterial Pepper",
            Diagnosis = "بقع صغیرة داكنة مائیة على الأوراق والثمار",
            Recommendation = "مبیدات نحاسیة، تناوب المحاصیل، أصناف مقاومة"
        },
        new AiPredict {
            ArabicName = "البیاض المبكر على البطاطس",
            EnglishName = "Blight Early Potato",
            Diagnosis = "بقع كبیرة بنیة داكنة على الأوراق مع حلقات متحدة المركز",
            Recommendation = "مبیدات فطریة، تناوب المحاصیل، أصناف مقاومة"
        },
        new AiPredict {
            ArabicName = "البیاض المتأخر على البطاطس",
            EnglishName = "Blight Late Potato",
            Diagnosis = "آفات مائیة على الأوراق والساق، نمو فطري أبیض على الجانب السفلي من الأوراق",
            Recommendation = "مبیدات فطریة، تناوب المحاصیل، أصناف مقاومة"
        },
        new AiPredict {
            ArabicName = "البیاض الدقیقي على القرع",
            EnglishName = "Mildew Powdery Squash",
            Diagnosis = "نمو أبیض بودري على الأوراق والثمار",
            Recommendation = "مبیدات فطریة، أصناف مقاومة، تھویة جیدة"
        },
        new AiPredict {
            ArabicName = "حروق أوراق الفراولة",
            EnglishName = "Scorch Leaf Strawberry",
            Diagnosis = "حواف أوراق بنیة میتة",
            Recommendation = "مبیدات فطریة، ري مناسب، تھویة جیدة"
        },
        new AiPredict {
            ArabicName = "بقعة بكتیریة على الطماطم",
            EnglishName = "Spot Bacterial Tomato",
            Diagnosis = "بقع صغیرة داكنة مائیة على الأوراق والثمار",
            Recommendation = "مبیدات نحاسیة، تناوب المحاصیل، أصناف مقاومة"
        },
        new AiPredict {
            ArabicName = "البیاض المبكر على الطماطم",
            EnglishName = "Blight Early Tomato",
            Diagnosis = "بقع كبیرة بنیة داكنة على الأوراق مع حلقات متحدة المركز",
            Recommendation = "مبیدات فطریة، تناوب المحاصیل، أصناف مقاومة"
        },
        new AiPredict {
            ArabicName = "البیاض المتأخر على الطماطم",
            EnglishName = "Blight Late Tomato",
            Diagnosis = "آفات مائیة على الأوراق والساق، نمو فطري أبیض على الجانب السفلي من الأوراق",
            Recommendation = "مبیدات فطریة، تناوب المحاصیل، أصناف مقاومة"
        },
        new AiPredict {
            ArabicName = "العفن الورقي على الطماطم",
            EnglishName = "Mold Leaf Tomato",
            Diagnosis = "بقع صفراء بنیة على السطح العلوي للأوراق، عفن رمادي على السطح السفلي للأوراق",
            Recommendation = "مبیدات فطریة، تھویة جیدة، تجنب الري العلوي"
        },
        new AiPredict {
            ArabicName = "بقعة سیبتوریا على الطماطم",
            EnglishName = "Spot Leaf Septoria Tomato",
            Diagnosis = "بقع صغیرة بنیة داكنة مع مراكز بیضاء على الأوراق",
            Recommendation = "مبیدات فطریة، تناوب المحاصیل، أصناف مقاومة"
        },
        new AiPredict {
            ArabicName = "سوس العنكبوت ذو البقعتین",
            EnglishName = "Mites Spider Tomato",
            Diagnosis = "حشرات صغیرة صفراء خضراء تمتص العصارة من الأوراق، مما یسبب بقعًا صفراء وتلونًا",
            Recommendation = "صابون حشري، حشرات مفترسة، تجنب استخدام المبیدات"
        },
        new AiPredict {
            ArabicName = "بقعة الھدف على الطماطم",
            EnglishName = "Spot Target Tomato",
            Diagnosis = "بقع كبیرة ذات حلقات متحدة المركز على الأوراق",
            Recommendation = "مبیدات فطریة، تناوب المحاصیل، أصناف مقاومة"
        },
        new AiPredict {
            ArabicName = "فیروس موزاییك الطماطم",
            EnglishName = "Virus Mosaic Tomato",
            Diagnosis = "أوراق مرقطة وصفراء ومشوھة، نمو متقزم",
            Recommendation = "بذور خالیة من الأمراض، نظافة، مكافحة الحشرات"
        },
        new AiPredict {
            ArabicName = "فیروس تجعد الأوراق الصفراء للطماطم",
            EnglishName = "Virus Curl Leaf Yellow Tomato",
            Diagnosis = "اصفرار وتجعد وانحناء أوراق لأسفل",
            Recommendation = "مكافحة الحشرات (ذبابة بیضاء)، أصناف مقاومة"
        }
    };

            var existingEntries = _context.AiPredicts
                .Select(p => p.EnglishName)
                .ToHashSet();

            var newEntries = predictData
                .Where(p => !existingEntries.Contains(p.EnglishName))
                .ToList();

            if (newEntries.Any())
            {
                _context.AiPredicts.AddRange(newEntries);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Successfully added {newEntries.Count} new entries.");
            }
            else
            {
                Console.WriteLine("No new entries to add.");
            }
        }
    }
}
