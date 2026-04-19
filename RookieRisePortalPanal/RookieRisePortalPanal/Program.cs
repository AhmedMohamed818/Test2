using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RookieRisePortalPanal.Data.Context;
using RookieRisePortalPanal.Data.Entities;
using RookieRisePortalPanal.Repositories.CompaniesRepository;
using RookieRisePortalPanal.Repositories.Exceptions;
using RookieRisePortalPanal.Repositories.TokenRepository;
using RookieRisePortalPanal.Repositories.UsersRepository;
using RookieRisePortalPanal.Services.AccountService;
using RookieRisePortalPanal.Services.AppConfigration;
using RookieRisePortalPanal.Services.AppConfigration.RookieRisePortalPanal.Services.AppConfigration;
using RookieRisePortalPanal.Services.CompanyService;
using RookieRisePortalPanal.Services.CurrentUserService;
using RookieRisePortalPanal.Services.EmailService;
using RookieRisePortalPanal.Services.JwtService;
using RookieRisePortalPanal.Services.MappingProfile;
using System.Globalization;
using System.Text;

namespace RookieRise.SignIn.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // =========================
            // Controllers
            // =========================
            builder.Services.AddControllers();

            // =========================
            // Swagger + JWT
            // =========================
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "RookieRise API",
                    Version = "v1"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // =========================
            // Localization
            // =========================
            builder.Services.AddLocalization();

            // =========================
            // DB
            // =========================
            builder.Services.AddDbContext<RookieRiseDbContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            // =========================
            // Settings (Options Pattern) 
            // =========================
            builder.Services.Configure<JwtSettings>(
                builder.Configuration.GetSection("JwtOptions"));

            builder.Services.Configure<SmtpSettings>(
                builder.Configuration.GetSection("SmtpSettings"));

            var jwtSettings = builder.Configuration
                .GetSection("JwtOptions")
                .Get<JwtSettings>();

            // =========================
            // Authentication 
            // =========================
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });

            // =========================
            // Identity
            // =========================
            builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<RookieRiseDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromHours(24);
            });

            // =========================
            // Core Services
            // =========================
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

            // =========================
            // Repositories
            // =========================
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
            builder.Services.AddScoped<IUserTokenRepository, UserTokenRepository>();

            // =========================
            // Services
            // =========================
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ICompanyService, CompanyService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
           // builder.Services.AddAutoMapper(typeof(CompanyProfile));
            builder.Services.AddAutoMapper(M => M.AddProfile(new CompanyProfile()));

            // =========================
            // CORS
            // =========================
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            var app = builder.Build();

            // =========================
            // Localization Middleware
            // =========================
            var supportedCultures = new[] { "en-US", "ar-EG" };

            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture("en-US")
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);

            // =========================
            // Global Exception Handler
            // =========================
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.Clear();
                    context.Response.ContentType = "application/json";

                    var feature = context.Features.Get<IExceptionHandlerFeature>();

                    if (feature != null)
                    {
                        var error = feature.Error;
                        var culture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

                        string message = culture == "ar"
                            ? error switch
                            {
                                UnAuthorizedException => "البريد الإلكتروني أو كلمة المرور غير صحيحة",
                                ValidationException => "بيانات غير صحيحة",
                                DuplicatedEmailBadRequestException => "هذا البريد مستخدم بالفعل",
                                UserNotFoundException => "المستخدم غير موجود",
                                _ => "خطأ في السيرفر"
                            }
                            : error switch
                            {
                                UnAuthorizedException => "Invalid email or password",
                                ValidationException => "Validation error",
                                DuplicatedEmailBadRequestException => "Email already exists",
                                UserNotFoundException => "User not found",
                                _ => "Server error"
                            };

                        context.Response.StatusCode = error switch
                        {
                            UnAuthorizedException => 401,
                            ValidationException => 400,
                            DuplicatedEmailBadRequestException => 400,
                            UserNotFoundException => 404,
                            _ => 500
                        };

                        await context.Response.WriteAsJsonAsync(new { message });
                    }
                });
            });

            // =========================
            // Swagger
            // =========================
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAngular");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}