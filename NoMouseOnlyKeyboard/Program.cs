using NoMouseOnlyKeyboard;
using NoMouseOnlyKeyboard.Interfaces;
using NoMouseOnlyKeyboard.Services;
using NoMouseOnlyKeyboard.Services.ActionHandling;
using UIImplementationWindowsWPF;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<IKeyMapping, DefaultKeyMapping>();
        services.AddSingleton<IKeyListenerService,  KeyListenerUsingLoopService>();
        services.AddSingleton<IUIService, UIService>();
        // Mouse action handler services
        services.AddSingleton<HandlerServices>();
    })
    .Build();



host.Run();

