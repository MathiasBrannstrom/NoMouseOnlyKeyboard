using System.Windows.Input;

namespace NoMouseOnlyKeyboard.Services.Interfaces
{
    internal interface IKeyMapping
    {
        Action GetActionFromKey(Key key);
        bool HasKeyMappingForKey(Key key);

        IEnumerable<Key> GetAllMappedKeys();
    }
}