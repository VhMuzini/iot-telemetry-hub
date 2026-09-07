using IoTTelemetryHub.Application.Common.Interfaces;
using IoTTelemetryHub.Infrastructure.Notifications;
using IoTTelemetryHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoTTelemetryHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Postgres")));

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<INotificationChannel, EmailNotificationChannel>();
        services.AddHttpClient<WebhookNotificationChannel>(client =>
        {
            var webhookUrl = configuration["Notifications:WebhookUrl"];
            if (!string.IsNullOrWhiteSpace(webhookUrl))
                client.BaseAddress = new Uri(webhookUrl);
        });
        services.AddScoped<INotificationChannel>(sp => sp.GetRequiredService<WebhookNotificationChannel>());

        services.AddScoped<INotificationDispatcher, NotificationDispatcher>();

        return services;
    }
}
