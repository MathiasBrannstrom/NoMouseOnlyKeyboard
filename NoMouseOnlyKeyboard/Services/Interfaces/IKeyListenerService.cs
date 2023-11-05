using System.Windows.Input;
using Utilities;

namespace NoMouseOnlyKeyboard.Services.Interfaces
{
    public delegate void KeyEvent(Key key, ButtonEvent buttonEvent);

    internal interface IKeyListenerService
    {
        Dictionary<Action, ValueHolder<bool>> IsActionKeyHeld { get; }
        void Dispose();
    }
}