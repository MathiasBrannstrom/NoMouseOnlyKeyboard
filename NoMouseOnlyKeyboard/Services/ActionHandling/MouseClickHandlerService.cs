using NoMouseOnlyKeyboard.WindowsAPI;
using System.Runtime.InteropServices;
using NoMouseOnlyKeyboard.Interfaces;
using static NoMouseOnlyKeyboard.WindowsAPI.User32;
using Action = NoMouseOnlyKeyboard.Interfaces.Action;

namespace NoMouseOnlyKeyboard.Services.ActionHandling
{
    internal class MouseClickHandlerService
    {
        private readonly IKeyListenerService _keysBeingHeld;

        // Move to NativeMethods?
        const int INPUT_MOUSE = 0;
        const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        const uint MOUSEEVENTF_LEFTUP = 0x0004;
        const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const uint MOUSEEVENTF_RIGHTUP = 0x0010;

        public MouseClickHandlerService(IKeyListenerService keysBeingHeld)
        {
            _keysBeingHeld = keysBeingHeld;
            _keysBeingHeld.IsActionKeyHeld[Action.MouseLeftButton].ValueChanged += HandleLeftMouseButtonStateChanged;
            _keysBeingHeld.IsActionKeyHeld[Action.MouseRightButton].ValueChanged += HandleRightMouseButtonStateChanged;
        }

        private void HandleRightMouseButtonStateChanged()
        {
            var newValue = _keysBeingHeld.IsActionKeyHeld[Action.MouseRightButton].Value;
            var state = newValue ? MOUSEEVENTF_RIGHTDOWN : MOUSEEVENTF_RIGHTUP;

            INPUT[] inputs = new INPUT[2];

            GetCursorPos(out POINT point);

            inputs[0] = new INPUT();
            inputs[0].type = INPUT_MOUSE;
            inputs[0].mi.dx = point.X;
            inputs[0].mi.dy = point.Y;
            inputs[0].mi.dwFlags = state;

            // Send the input events
            SendInput(2, inputs, Marshal.SizeOf(inputs[0]));
        }

        private void HandleLeftMouseButtonStateChanged()
        {
            var newValue = _keysBeingHeld.IsActionKeyHeld[Action.MouseLeftButton].Value;
            var state = newValue ? MOUSEEVENTF_LEFTDOWN : MOUSEEVENTF_LEFTUP;

            INPUT[] inputs = new INPUT[2];

            GetCursorPos(out POINT point);

            inputs[0] = new INPUT();
            inputs[0].type = INPUT_MOUSE;
            inputs[0].mi.dx = point.X;
            inputs[0].mi.dy = point.Y;
            inputs[0].mi.dwFlags = state;

            // Send the input events
            SendInput(2, inputs, Marshal.SizeOf(inputs[0]));
        }
    }
}
