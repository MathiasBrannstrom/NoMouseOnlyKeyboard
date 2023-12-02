using HintNavigation;
using NoMouseOnlyKeyboard.Interfaces;

namespace UIImplementationWindowsWPF
{
    public class UIService : IUIService
    {
        private GridNavigationViewModel _gridNavigationViewModel;

        public UIService() 
        {
            _gridNavigationViewModel = new GridNavigationViewModel();

            Thread t = new Thread(CreateWindow);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void CreateWindow()
        {
            var view = new GridNavigationView
            {
                DataContext = _gridNavigationViewModel
            };

            view.ShowDialog();
        }

        public Task<Point> ShowGridNavigationLabels()
        {
            return _gridNavigationViewModel.GetSelectedPosition();
        }
    }
}
