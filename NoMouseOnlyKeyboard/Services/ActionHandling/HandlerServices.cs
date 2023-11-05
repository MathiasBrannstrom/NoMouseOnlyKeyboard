
using NoMouseOnlyKeyboard.Services.Interfaces;

namespace NoMouseOnlyKeyboard.Services.ActionHandling
{
    internal class HandlerServices
    {
        public HandlerServices(IKeyListenerService keyListenerService)
        {
            new MouseHandlerServices(keyListenerService);
        }
    }
}
