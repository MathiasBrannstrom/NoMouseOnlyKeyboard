
namespace NoMouseOnlyKeyboard.Interfaces
{
    public interface IUIService
    {
        Task<Tuple<int, int>> ShowGridNavigationLabels();
    }
}
