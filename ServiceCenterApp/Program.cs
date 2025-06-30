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

namespace ServiceCenterApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
