

using NoMouseOnlyKeyboard.Interfaces;

namespace NoMouseOnlyKeyboard.Services.ActionHandling
{
    internal class HandlerServices
    {
        public HandlerServices(IKeyListenerService keyListenerService, IUIService uiService)
        {
            new MouseHandlerServices(keyListenerService);
            new GridNavigationService(keyListenerService, uiService);

        }
    }
}
