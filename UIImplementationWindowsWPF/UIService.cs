using HintNavigation;
using NoMouseOnlyKeyboard.Interfaces;
using System.Windows;
using Point = NoMouseOnlyKeyboard.Interfaces.Point;

namespace UIImplementationWindowsWPF
{
    public class UIService : IUIService
    {
        private GridNavigationViewModel _gridNavigationViewModel;

        public UIService() 
        {
            _gridNavigationViewModel = new GridNavigationViewModel();
            CreateWindows();
        }

        private void CreateWindows()
        {
            var currentScreen = Screen.PrimaryScreen;
            var screens = Screen.AllScreens;

            _gridNavigationViewModel.UpdateAvailableRegions(currentScreen.Bounds, screens.Except(new[] {currentScreen}).Select(s => s.Bounds));

            foreach (var region in _gridNavigationViewModel.RegionViewModels)
            {

                Thread t = new Thread(() => CreateWindow(region));
                t.SetApartmentState(ApartmentState.STA);
                t.Start();

            }
            
        }

        private void CreateWindow(RegionViewModel vm)
        {


            var view = new GridNavigationView
            {
                DataContext = vm
            };

            view.Top = vm.Top;
            view.Left = vm.Left;
            view.Width = vm.Width;
            view.Height = vm.Height;

            view.ShowDialog();
        }

        public Task<Point> ShowGridNavigationLabels()
        {
            return _gridNavigationViewModel.GetSelectedPosition();
        }
    }
}
