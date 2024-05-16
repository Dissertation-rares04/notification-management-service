using NotificationManagementService.Business.Implementation;
using NotificationManagementService.Business.Interface;
using NotificationManagementService.Core.AppSettings;
using NotificationManagementService.WorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.Configure<RabbitMQSettings>(hostContext.Configuration.GetSection("RabbitMQ"));

        services.Configure<MailSettings>(hostContext.Configuration.GetSection("MailSettings"));
        services.AddTransient<IMailService, MailService>();

        services.Configure<Auth0Settings>(hostContext.Configuration.GetSection("Auth0"));
        services.AddSingleton<IAuth0HttpClient, Auth0HttpClient>();

        services.AddSingleton<IMessageHandler, NotificationTasksMessageHandler>();

        services.AddHostedService<RabbitMQConsumerWorker>();
    })
    .Build();

await host.RunAsync();
