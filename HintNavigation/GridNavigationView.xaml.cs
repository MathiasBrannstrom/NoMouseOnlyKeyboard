using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace HintNavigation
{
    /// <summary>
    /// Interaction logic for GridNavigationView.xaml
    /// </summary>
    public partial class GridNavigationView
    {
        private RegionViewModel _viewModel;
        private Dispatcher _dispatcher;

        public GridNavigationView()
        {
            InitializeComponent();
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        
        private void GridNavigationView_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Escape)
            {
                _viewModel.Visible.Value = false;
                return;
            }
        }

        //protected override void OnDeactivated(EventArgs e)
        //{
        //    //_viewModel.Visible.Value = false;
        //    base.OnDeactivated(e);
        //}



        private void GridNavigationView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _viewModel = ((RegionViewModel)DataContext);
        }
       
        private void MatchStringControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Activate();
            MatchStringControl.Focus();
            MatchStringControl.Text = "";
            Keyboard.Focus(MatchStringControl);
        }

        private void MatchStringControl_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //if(Keyboard.FocusedElement != MatchStringControl)
            //{
            //    //_viewModel.Visible.Value = false;
            //}
            //Console.WriteLine($"Currently focused element: {Keyboard.FocusedElement}");
        }

        private void MatchStringControl_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            // Don't allow losing focus while overlay is visible.
            if(_viewModel.Visible.Value)
                e.Handled = true;
        }
    }
}
