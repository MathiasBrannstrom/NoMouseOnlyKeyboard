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
        private IEnumerable<Key> _relevantKeys;

        public Dictionary<Action, ValueHolder<bool>> IsActionKeyHeld { get; } = new Dictionary<Action, ValueHolder<bool>>();

        public KeyListenerUsingLoopService(IKeyMapping keyMapping)
        {
            _keyMapping = keyMapping;

            foreach (Action action in Enum.GetValues(typeof(Action)))
            {
                IsActionKeyHeld[action] = new ValueHolder<bool>(false);
            }

            _relevantKeys = _keyMapping.GetAllMappedKeys();

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
                foreach (var key in _relevantKeys)
                {
                    IsActionKeyHeld[_keyMapping.GetActionFromKey(key)].Value = Keyboard.IsKeyDown(key);
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
