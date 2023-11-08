using NoMouseOnlyKeyboard.Interfaces;
using System.Windows.Input;
using Action = NoMouseOnlyKeyboard.Interfaces.Action;

namespace NoMouseOnlyKeyboard.Services
{
    internal class DefaultKeyMapping : IKeyMapping
    {
        private readonly Dictionary<Key, Action> _keyActionMappings = new Dictionary<Key, Action>
        {
            { Key.F13, Action.MouseMoveUp },
            { Key.F14, Action.MouseMoveLeft },
            { Key.F15, Action.MouseMoveDown },
            { Key.F16, Action.MouseMoveRight },
            { Key.F17, Action.MouseLeftButton },
            { Key.F18, Action.MouseRightButton },
            { Key.F24, Action.MouseMiddleButton },
            { Key.F20, Action.MouseDoubleClick },
            { Key.F21, Action.MouseScrollUp },
            { Key.F22, Action.MouseScrollDown },
            { Key.F23, Action.ShowUINavigationLabels },
            { Key.F19, Action.ShowGridNavigationLabels },
            { Key.LeftShift, Action.MouseSpeedUp },
            { Key.RightShift, Action.MouseSpeedUp },
            { Key.LeftCtrl, Action.MouseSpeedDown },
            { Key.RightCtrl, Action.MouseSpeedDown }
        };
        private Dictionary<Action, IEnumerable<Key>> _reverseMapping;

        public DefaultKeyMapping()
        {
            _reverseMapping = ReverseKeyMapping(_keyActionMappings);
        }

        internal Dictionary<Action, IEnumerable<Key>> ReverseKeyMapping(Dictionary<Key, Action> keyMapping)
        {
            var result = new Dictionary<Action, List<Key>>();

            foreach (var kvp in keyMapping)
            {
                if (!result.ContainsKey(kvp.Value))
                {
                    var list = new List<Key>() { kvp.Key };
                    result.Add(kvp.Value, list);
                }
                else
                {
                    result[kvp.Value].Add(kvp.Key);
                }
            }

            return result.ToDictionary(pair => pair.Key, pair => pair.Value.AsEnumerable());
        }

        public bool HasKeyMappingForKey(Key key)
        {
            return _keyActionMappings.ContainsKey(key);
        }

        public Action GetActionFromKey(Key key)
        {
            if (_keyActionMappings.TryGetValue(key, out var action))
            {
                return action;
            }

            throw new ArgumentOutOfRangeException(nameof(key), key, null);
        }

        public IEnumerable<Key> GetAllMappedKeys()
        {
            return _keyActionMappings.Keys;
        }

        public IEnumerable<Key> GetAllKeysForAction(Action action)
        {
            // No check for missing mappings here.
            return _reverseMapping[action];
        }
    }
}