using MassTransit;
using zzz;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumer<FileReceivedConsumer, FileReceivedConsumerDefinition>();
            busConfigurator.AddConsumer<CustomerAuditConsumer, CustomerAuditConsumerDefinition>();

            busConfigurator.UsingAzureServiceBus((context, cfg) =>
            {
                cfg.Host(hostContext.Configuration["ServiceBus:BusInstance"]);
                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddHostedService<PublishEventHostedService>();
    });

await builder.Build().RunAsync();
