﻿using System;
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
    /// Interaction logic for OverlayView.xaml
    /// </summary>
    public partial class OverlayView
    {
        public OverlayView()
        {
            InitializeComponent();
        }

        private void OverlayView_OnLoaded(object sender, RoutedEventArgs e)
        {
            //var m = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
            //var scaleX = m.M11;
            //var scaleY = m.M22;

            // scale the items for non-96 DPIs
            //layoutGrid.LayoutTransform = new ScaleTransform(1 / scaleX, 1 / scaleY);

            // resize the window for non-96 DPIs
            //var vm = DataContext as OverlayViewModel;
            //Left = vm.Bounds.Left / scaleX;
            //Top = vm.Bounds.Top / scaleY;
            //Width = vm.Bounds.Width / scaleX;
            //Height = vm.Bounds.Height / scaleY;
        }

        private void OverlayView_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
