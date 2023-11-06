using System.Windows.Input;
using Utilities;

namespace NoMouseOnlyKeyboard.Interfaces
{
    public delegate void KeyEvent(Key key, ButtonEvent buttonEvent);

    public interface IKeyListenerService
    {
        Dictionary<Action, ValueHolder<bool>> IsActionKeyHeld { get; }
        void Dispose();
    }
}