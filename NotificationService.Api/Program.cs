using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NotificationService.Application.Interfaces;
using NotificationService.Infrastructure.Consumers;
using NotificationService.Infrastructure.Persistence;
using NotificationService.Infrastructure.Repositories;
using Npgsql;
using MediatR;
using NotificationService.Application.Features.Handler;
using NotificationService.Infrastructure.EmailStrategy;
using Org.BouncyCastle.Asn1.X509.Qualified;
using NotificationService.Infrastructure.Services;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Notification Service API",
        Version = "v1"
    });

});
builder.Services.AddDbContext<NotificationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<NotificationConsumer>();
    x.AddConsumer<SendMailForgotPasswordConsumer>();
    x.AddEntityFrameworkOutbox<NotificationContext>(cfg =>
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
        cfg.ReceiveEndpoint("register-sendmail-queue", e =>
        {
            e.ConfigureConsumer<NotificationConsumer>(context);
        });
        cfg.ReceiveEndpoint("send-token-queue", e =>
        {
            e.ConfigureConsumer<SendMailForgotPasswordConsumer>(context);
        });
    });
});
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(Program).Assembly,
        typeof(NotificationSendMailHandler).Assembly
    );
});

builder.Services.AddScoped<RegisterSendMail>();
builder.Services.AddScoped<ForgotPasswordSendMail>();

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<INotificationStrategy, RegisterSendMail>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<INotificationStrategySelector, NotificationStrategySelector>();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notification Service API V1");
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
