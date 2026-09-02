using AffiliationProgram.Configs;
using AffiliationProgram.Data;
using AffiliationProgram.ErrorHandler;
using AffiliationProgram.Services;
using Dapper;
using MySqlConnector;
using StackExchange.Redis;

namespace AffiliationProgram;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        DefaultTypeMap.MatchNamesWithUnderscores = true;

        builder.Services
            .AddOptions<AffiliateOptions>()
            .Bind(builder.Configuration.GetSection(AffiliateOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var connectionString = builder.Configuration.GetConnectionString("MariaDb")
                               ?? throw new InvalidOperationException("Connection string 'MariaDb' is not configured.");

        builder.Services.AddMySqlDataSource(connectionString);

        builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(builder.Configuration["Redis:ConnectionString"]!));

        builder.Services.AddScoped<IAffiliateRepository, AffiliateRepository>();
        builder.Services.AddSingleton<IClickTrackingStore, RedisClickTrackingStore>();
        builder.Services.AddScoped<IAffiliateTrackingService, AffiliateTrackingService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IRegistrationService, RegistrationService>();

        builder.Services.AddExceptionHandler<ExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }
}