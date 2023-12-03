using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HintNavigation
{
    /// <summary>
    /// Interaction logic for GridNavigationView.xaml
    /// </summary>
    public partial class GridNavigationView
    {
        private GridNavigationViewModel _viewModel;
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
                _viewModel.Visible = false;
                return;
            }
        }




        protected override void OnDeactivated(EventArgs e)
        {
            _viewModel.Visible = false;
            base.OnDeactivated(e);
        }



        private void GridNavigationView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _viewModel = ((GridNavigationViewModel)DataContext);
        }
       
        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var canvas = (Canvas)sender;
            _viewModel.UpdateSizeOfGrid(canvas.ActualWidth, canvas.ActualHeight);
        }

        private void MatchStringControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            MatchStringControl.Focus();
            MatchStringControl.Text = "";
            Keyboard.Focus(MatchStringControl);
        }

        private void MatchStringControl_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(Keyboard.FocusedElement != MatchStringControl)
            {
                _viewModel.Visible = false;
            }
            //Console.WriteLine($"Currently focused element: {Keyboard.FocusedElement}");
        }

        private void MatchStringControl_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            // Don't allow losing focus while overlay is visible.
            if(_viewModel.Visible)
                e.Handled = true;
        }
    }
}
