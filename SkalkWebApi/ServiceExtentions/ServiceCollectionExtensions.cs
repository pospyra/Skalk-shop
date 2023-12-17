using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Skalk.BLL.Interfaces;
using Skalk.BLL.IServices;
using Skalk.BLL.MappingProfile;
using Skalk.BLL.Services;
using Skalk.DAL.Enums;
using Skalk.DocumentGeneration.Interfaces;
using Skalk.DocumentGeneration.Service;
using SkalkWebApi.WebApi.Helpers;
using System.Text;

namespace SkalkWebApi.ServiceExtentions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("SkalkConnectionDB");
            services.AddDbContext<SkalkContext>(options => options.UseNpgsql(connectionString));

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1251");

            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICurrencyService, CurrencyService>();

            services.AddTransient<IShoppingCartService, ShoppingCartService>();
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IDocumentGenerationService, DocumentGenerationService>();

            services.AddScoped<IClaimAccessor, HttpContextClaimsAccessor>();
        }
        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<UserProfile>();
                cfg.AddProfile<ShoppingCartProfile>();
                cfg.AddProfile<OrderProfile>();
            });
        }

        public static void ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    string secretKey = configuration["Token:SecretJWTKey"];
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateActor = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ManagerOnly", policy =>
                    policy.RequireRole(Roles.Manager.ToString()));

                options.AddPolicy("UserOnly", policy =>
                    policy.RequireRole(Roles.User.ToString()));
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Skalk",
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    Description = @"Enter 'Bearer' [space] and then your token in the text input below. <br/><b>Example:</b> 'Bearer 12345abcdef'",
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                      }
                });
            });
        }

    }
}
