using NoMouseOnlyKeyboard.Interfaces;
using NoMouseOnlyKeyboard.WindowsAPI;
using static NoMouseOnlyKeyboard.WindowsAPI.User32;
using Action = NoMouseOnlyKeyboard.Interfaces.Action;

namespace NoMouseOnlyKeyboard.Services.ActionHandling
{
    internal class MouseMoveHandlerService
    {
        private IKeyListenerService _keysBeingHeld;
        private CancellationTokenSource _cancellationTokenSource;

        // Define the base speed and speed modifiers
        const double BaseSpeed = 8;
        const double SpeedUpModifier = 3.0;
        const double SpeedDownModifier = 0.3;


        public MouseMoveHandlerService(IKeyListenerService keysBeingHeld)
        {

            _keysBeingHeld = keysBeingHeld;
            _keysBeingHeld.IsActionKeyHeld[Action.MouseMoveUp].ValueChanged += HandleMouseMoveKeyHeldChanged;
            _keysBeingHeld.IsActionKeyHeld[Action.MouseMoveDown].ValueChanged += HandleMouseMoveKeyHeldChanged;
            _keysBeingHeld.IsActionKeyHeld[Action.MouseMoveLeft].ValueChanged += HandleMouseMoveKeyHeldChanged;
            _keysBeingHeld.IsActionKeyHeld[Action.MouseMoveRight].ValueChanged += HandleMouseMoveKeyHeldChanged;
            _keysBeingHeld.IsActionKeyHeld[Action.MouseScrollUp].ValueChanged += HandleMouseMoveKeyHeldChanged;
            _keysBeingHeld.IsActionKeyHeld[Action.MouseScrollDown].ValueChanged += HandleMouseMoveKeyHeldChanged;


        }

        private bool IsMouseBeingMoved()
        {
            return _keysBeingHeld.IsActionKeyHeld[Action.MouseMoveUp].Value ||
                   _keysBeingHeld.IsActionKeyHeld[Action.MouseMoveDown].Value ||
                   _keysBeingHeld.IsActionKeyHeld[Action.MouseMoveLeft].Value ||
                   _keysBeingHeld.IsActionKeyHeld[Action.MouseMoveRight].Value ||
                   _keysBeingHeld.IsActionKeyHeld[Action.MouseScrollUp].Value ||
                   _keysBeingHeld.IsActionKeyHeld[Action.MouseScrollDown].Value;


        }

        private void HandleMouseMoveKeyHeldChanged()
        {
            var isMouseBeingMoved = IsMouseBeingMoved();

            if (isMouseBeingMoved && _cancellationTokenSource == null)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = _cancellationTokenSource.Token;
                Task.Run(() => MouseMoveLoop(cancellationToken), cancellationToken);

                return;
            }

            if (!isMouseBeingMoved && _cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
        }

        private void MouseMoveLoop(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;


                double speedModifier = 1; // Default speed modifier

                if (_keysBeingHeld.IsActionKeyHeld[Action.MouseSpeedUp].Value)
                {
                    speedModifier *= SpeedUpModifier;
                }
                if (_keysBeingHeld.IsActionKeyHeld[Action.MouseSpeedDown].Value)
                {
                    speedModifier *= SpeedDownModifier;
                }
                var mouseMoveSpeed = BaseSpeed * speedModifier;

                if (_keysBeingHeld.IsActionKeyHeld[Action.MouseMoveUp].Value)
                {
                    MoveMouse(0, -mouseMoveSpeed);
                }
                if (_keysBeingHeld.IsActionKeyHeld[Action.MouseMoveLeft].Value)
                {
                    MoveMouse(-mouseMoveSpeed, 0);
                }
                if (_keysBeingHeld.IsActionKeyHeld[Action.MouseMoveDown].Value)
                {
                    MoveMouse(0, mouseMoveSpeed);
                }
                if (_keysBeingHeld.IsActionKeyHeld[Action.MouseMoveRight].Value)
                {
                    MoveMouse(mouseMoveSpeed, 0);
                }

                int mouseScrollSpeed = (int)(15*speedModifier);

                if (_keysBeingHeld.IsActionKeyHeld[Action.MouseScrollUp].Value)
                {
                    ScrollMouse(mouseScrollSpeed);
                }
                if (_keysBeingHeld.IsActionKeyHeld[Action.MouseScrollDown].Value)
                {
                    ScrollMouse(-mouseScrollSpeed);
                }

                Thread.Sleep(10);
            }
        }

        private void ScrollMouse(int scrollAmount)
        {
            mouse_event(0x0800, 0, 0, (uint)scrollAmount, 0);
        }

        private void MoveMouse(double deltaX, double deltaY)
        {
            GetCursorPos(out POINT currentPosition);

            int newX = (int)Math.Round(currentPosition.X + deltaX);
            int newY = (int)Math.Round(currentPosition.Y + deltaY);

            SetCursorPos(newX, newY);
        }


    }
}
