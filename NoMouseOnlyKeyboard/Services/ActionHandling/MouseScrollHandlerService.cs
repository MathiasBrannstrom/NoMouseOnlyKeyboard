using NoMouseOnlyKeyboard.Interfaces;

namespace NoMouseOnlyKeyboard.Services.ActionHandling
{
    internal class MouseScrollHandlerService
    {
        private IKeyListenerService _keyListenerService;

        public MouseScrollHandlerService(IKeyListenerService keyListenerService)
        {
            _keyListenerService = keyListenerService;
        }


    }
}