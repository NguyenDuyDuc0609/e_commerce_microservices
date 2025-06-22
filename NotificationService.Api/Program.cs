using MassTransit;
using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Interfaces;
using NotificationService.Infrastructure.Consumers;
using NotificationService.Infrastructure.Persistence;
using NotificationService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<NotificationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<NotificationConsumer>();
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
    });
});

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IEmailSender, EmailSender>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
