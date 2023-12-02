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

namespace HintNavigation
{
    /// <summary>
    /// Interaction logic for GridNavigationView.xaml
    /// </summary>
    public partial class GridNavigationView
    {
        private GridNavigationViewModel _viewModel;

        public GridNavigationView()
        {
            InitializeComponent();
        }

        


        private void GridNavigationView_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Escape)
            {
                _viewModel.Visible = false;
                return;
            }

            foreach(var item in _viewModel.GridNavigationItems)
                if(e.Key.ToString()== item.Key)
                {
                    _viewModel.SelectGridNavigationItem(item);
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
    }
}
