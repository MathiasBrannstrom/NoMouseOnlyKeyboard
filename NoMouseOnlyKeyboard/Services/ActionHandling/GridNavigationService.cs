using NoMouseOnlyKeyboard.Interfaces;
using Action = NoMouseOnlyKeyboard.Interfaces.Action;

namespace NoMouseOnlyKeyboard.Services.ActionHandling
{
    internal class GridNavigationService
    {
        private readonly IKeyListenerService _keyListenerService;
        private readonly IUIService _uiService;

        public GridNavigationService(IKeyListenerService keyListenerService, IUIService uiService)
        {
            _keyListenerService = keyListenerService;
            _uiService = uiService;
            _keyListenerService.IsActionKeyHeld[Action.ShowGridNavigationLabels].ValueChanged += HasGridNavigationActionKeyHeldChanged;
        }

        private void HasGridNavigationActionKeyHeldChanged()
        {
            if (!_keyListenerService.IsActionKeyHeld[Action.ShowGridNavigationLabels].Value)
                return;

            Thread t = new Thread(ShowGridNavigationLabels);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void ShowGridNavigationLabels()
        {
            var task = _uiService.ShowGridNavigationLabels();
            task.ContinueWith(t =>
            {
                if (t.IsCanceled || t.IsFaulted)
                    return;

                var result = t.Result;
                //Move mouse to position here.
                Console.WriteLine($"X: {result.Item1}, Y: {result.Item2}");
            });
        }
    }
}
