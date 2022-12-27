using AgentApp;
using AgentApp.Repository;
using AgentApp.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<IAgentRepository, AgentService>();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();