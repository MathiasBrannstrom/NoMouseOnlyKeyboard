using System.Windows.Input;

namespace NoMouseOnlyKeyboard.Interfaces
{
    public interface IKeyMapping
    {
        Action GetActionFromKey(Key key);
        bool HasKeyMappingForKey(Key key);

        IEnumerable<Key> GetAllMappedKeys();

        IEnumerable<Key> GetAllKeysForAction(Action action);
    }
}