using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ServiceCenterAppDalEF.DbCreating;
using ServiceCenterAppBLL.Mapping;
using FluentValidation;
using ServiceCenterAppBLL.Filters;
using ServiceCenterAppBLL.Interfaces;
using ServiceCenterAppBLL.Services;
using ServiceCenterAppDalEF.Repositories.Interfaces;
using ServiceCenterAppDalEF.Repositories;
using ServiceCenterAppDalEF.UnitOfWork;
using ServiceCenterAppDalEF.Interfaces;
using ServiceCenterAppBLL.Validations.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ServiceCenterApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Введіть токен у форматі: Bearer {your JWT token}"
                });
                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<RepairDbContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            builder.Services.AddControllers();
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<ClientCreateDtoValidator>();

            // Додавання кешування
            builder.Services.AddMemoryCache();

            // Реєстрація фільтрів
            builder.Services.AddScoped<GlobalExceptionFilter>();
            builder.Services.AddScoped<ValidationFilter>();
            builder.Services.AddScoped<LoggingFilter>();
            builder.Services.AddScoped<AuthorizationFilter>();
            builder.Services.AddScoped<CacheFilter>();

            // Реєстрація репозиторіїв
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IClientRepository, ClientRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IRepairTypeRepository, RepairTypeRepository>();
            builder.Services.AddScoped<IAdditionalServiceRepository, AdditionalServiceRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

            // Реєстрація Unit of Work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Реєстрація сервісів
            builder.Services.AddScoped<IClientService, ClientService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IRepairTypeService, RepairTypeService>();
            builder.Services.AddScoped<IAdditionalServiceService, AdditionalServiceService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();

            // Додаємо Identity
            builder.Services.AddIdentity< ServiceCenterAppDalEF.Entities.ApplicationUser, IdentityRole >()
                .AddEntityFrameworkStores<RepairDbContext>()
                .AddDefaultTokenProviders();

            // Налаштування JWT
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
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
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
                };
            });

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            });

            var app = builder.Build();

            // Додаємо генерацію тестових даних
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<RepairDbContext>();
                ServiceCenterAppDalEF.DbCreating.DataGeneration.DataGenerator.GenerateAll(context);
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
