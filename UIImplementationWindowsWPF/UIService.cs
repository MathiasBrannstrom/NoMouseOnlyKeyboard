using NoMouseOnlyKeyboard.Interfaces;

namespace UIImplementationWindowsWPF
{
    public class UIService : IUIService
    {
        public Task<Tuple<int, int>> ShowGridNavigationLabels()
        {
            var view = new OverlayView
            {
                //DataContext = vm
            };
            //vm.CloseOverlay = () => view.Close();
            view.ShowDialog();

            return Task.FromResult(new Tuple<int, int>(13, 37));
        }
    }
}
