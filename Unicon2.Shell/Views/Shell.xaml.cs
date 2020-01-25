using System;
using MahApps.Metro.Controls;
using System.Windows;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.Events;

namespace Unicon2.Shell.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Shell : MetroWindow
    {
        public Shell()
        {
            this.InitializeComponent();
        }
          

        private void Shell_Initialized(object sender, System.EventArgs e)
        {
            foreach (var item in fileContextMenu.Items)
            {
                ((FrameworkElement)item).DataContext = DataContext;
            }

            foreach (var item in deviceContextMenu.Items)
            {
                ((FrameworkElement)item).DataContext = DataContext;
            }

            oscOpenButton.DataContext = DataContext;
            dynamicContentButton.DataContext = DataContext;
        }

    }
}
