using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SagaCoordinator.Application.Handlers;
using SagaCoordinator.Application.Interfaces;
using SagaCoordinator.Application.Saga;
using SagaCoordinator.Domain.Constracts.SagaStates;
using SagaCoordinator.Infrastructure.Consumers;
using SagaCoordinator.Infrastructure.Persistence;
using SagaCoordinator.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
var redisSettings = builder.Configuration.GetSection("Redis");
string redisHost = redisSettings["Host"];
int redisPort = int.Parse(redisSettings["Port"]);
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = $"{redisHost}:{redisPort}";
    options.InstanceName = "Saga_";
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Saga Coordinator Service API",
        Version = "v1"
    });
});
builder.Services.AddDbContext<SagaContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<SagaStatusConsumer>();

    x.AddSagaStateMachine<RegisterSaga, RegisterSagaState>()
    .EntityFrameworkRepository(r =>
    {
       r.ExistingDbContext<SagaContext>();
       r.UsePostgres();
    });

    x.AddEntityFrameworkOutbox<SagaContext>(cfg =>
    {
        cfg.QueryDelay = TimeSpan.FromSeconds(30);
        cfg.DuplicateDetectionWindow = TimeSpan.FromMinutes(10);
        cfg.DisableInboxCleanupService();
        cfg.UsePostgres();
        cfg.UseBusOutbox();
    });

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(RegisterSagaHanlder).Assembly);
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ISagaRepository, SagaRepository>();
builder.Services.AddScoped<ISagaRedis, SagaRedis>();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Saga Coordinator Service API V1");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
