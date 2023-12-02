using NoMouseOnlyKeyboard.Interfaces;
using Action = NoMouseOnlyKeyboard.Interfaces.Action;
using static NoMouseOnlyKeyboard.WindowsAPI.User32;

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

            ShowGridNavigationLabels();
        }

        private void ShowGridNavigationLabels()
        {
            var task = _uiService.ShowGridNavigationLabels();
            task.ContinueWith(t =>
            {
                if (t.IsCanceled || t.IsFaulted)
                    return;

                MoveMouseToPoint(t.Result);
            });
        }


        private void MoveMouseToPoint(Point p)
        {
            var x = (int)Math.Round(p.X);
            var y = (int)Math.Round(p.Y);

            Console.WriteLine($"Move mouse to {x}, {y}");
            SetCursorPos(x, y);
        }
    }
}
