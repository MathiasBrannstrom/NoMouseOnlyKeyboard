using NoMouseOnlyKeyboard.Interfaces;

namespace NoMouseOnlyKeyboard.Services.ActionHandling
{
    internal class MouseHandlerServices
    {
        internal MouseHandlerServices(IKeyListenerService keyListenerService) 
        {
            new MouseMoveHandlerService(keyListenerService);
            new MouseClickHandlerService(keyListenerService);
            new MouseScrollHandlerService(keyListenerService);
        }
    }
}
