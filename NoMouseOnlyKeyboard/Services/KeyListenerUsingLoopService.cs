using Utilities;
using System.Windows.Input;
using NoMouseOnlyKeyboard.Interfaces;
using Action = NoMouseOnlyKeyboard.Interfaces.Action;

namespace NoMouseOnlyKeyboard.Services
{
    internal class KeyListenerUsingLoopService : IKeyListenerService
    {
        private readonly IKeyMapping _keyMapping;
        private bool _isDisposed;

        public Dictionary<Action, ValueHolder<bool>> IsActionKeyHeld { get; } = new Dictionary<Action, ValueHolder<bool>>();

        public KeyListenerUsingLoopService(IKeyMapping keyMapping)
        {
            _keyMapping = keyMapping;

            foreach (Action action in Enum.GetValues(typeof(Action)))
            {
                IsActionKeyHeld[action] = new ValueHolder<bool>(false);
            }

            StartLoop();
        }

        private void StartLoop()
        {
            Thread t = new Thread(KeyboardCheckingLoop);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void KeyboardCheckingLoop()
        {
            //Probably should be done with cancellation token, but think this actually is perfectly fine too.
            while (!_isDisposed)
            {
                foreach (Action action in Enum.GetValues(typeof(Action)))
                {
                    var isAnyKeyForActionHeld = false;

                    foreach(var key in _keyMapping.GetAllKeysForAction(action))
                    {
                        isAnyKeyForActionHeld |= Keyboard.IsKeyDown(key);
                    }

                    if (IsActionKeyHeld[action].Value != isAnyKeyForActionHeld)
                    {
                        Console.WriteLine(action.ToString() + (isAnyKeyForActionHeld? " pressed." : " released."));
                    }

                    IsActionKeyHeld[action].Value = isAnyKeyForActionHeld;

                    
                }

                Thread.Sleep(10);
            }
        }

        public void Dispose()
        {
            _isDisposed = true;
        }
    }
}
