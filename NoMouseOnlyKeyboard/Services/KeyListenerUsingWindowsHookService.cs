using System.Runtime.InteropServices;
using System.Diagnostics;
using static NoMouseOnlyKeyboard.WindowsAPI.User32;
using NoMouseOnlyKeyboard.Services.Interfaces;
using Utilities;
using System.Windows.Input;

namespace NoMouseOnlyKeyboard.Services
{
    internal class KeyListenerUsingWindowsHookService : IDisposable, IKeyListenerService
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private nint _hookId = 0;
        private LowLevelKeyboardProc _keyboardProc;
        private readonly IKeyMapping _keyMapping;
        public Dictionary<Action, ValueHolder<bool>> IsActionKeyHeld { get; } = new Dictionary<Action, ValueHolder<bool>>();

        public KeyListenerUsingWindowsHookService(IKeyMapping keyMapping)
        {
            _keyMapping = keyMapping;

            foreach (Action action in Enum.GetValues(typeof(Action)))
            {
                IsActionKeyHeld[action] = new ValueHolder<bool>(false);
            }


            _keyboardProc = KeyboardEvent;
            _hookId = StartListeningToKeyboard(_keyboardProc);
        }

        private nint StartListeningToKeyboard(LowLevelKeyboardProc proc)
        {
            using (ProcessModule module = Process.GetCurrentProcess().MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(module.ModuleName), 0);
            }
        }

        private nint KeyboardEvent(int nCode, nint wParam, nint lParam)
        {
            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (wParam == WM_KEYDOWN)
                {
                    HandleKeyChanged((Key)vkCode, ButtonEvent.Pressed);
                    Console.WriteLine(vkCode);
                }
                else if (wParam == WM_KEYUP)
                {
                    HandleKeyChanged((Key)vkCode, ButtonEvent.Released);
                }
            }
            return CallNextHookEx(_hookId, nCode, wParam, lParam);
        }
        private void HandleKeyChanged(Key key, ButtonEvent buttonEvent)
        {
            if (!_keyMapping.HasKeyMappingForKey(key))
                return;

            IsActionKeyHeld[_keyMapping.GetActionFromKey(key)].Value = buttonEvent == ButtonEvent.Pressed;
        }

        public void Dispose()
        {
            UnhookWindowsHookEx(_hookId);
        }
    }
}
