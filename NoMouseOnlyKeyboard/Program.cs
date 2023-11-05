using NoMouseOnlyKeyboard;
using NoMouseOnlyKeyboard.Services;
using NoMouseOnlyKeyboard.Services.ActionHandling;
using NoMouseOnlyKeyboard.Services.Interfaces;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<IKeyMapping, DefaultKeyMapping>();
        services.AddSingleton<IKeyListenerService,  KeyListenerUsingLoopService>();
        
        // Mouse action handler services
        services.AddSingleton<HandlerServices>();
    })
    .Build();



host.Run();

