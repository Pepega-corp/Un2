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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Unicon2.Fragments.Configuration.ViewModel;

namespace Unicon2.Fragments.Configuration.Views
{
    /// <summary>
    /// Логика взаимодействия для ExportSelectionWindow.xaml
    /// </summary>
    public partial class ExportSelectionWindow 
    {
        public ExportSelectionWindow(ExportSelectionViewModel exportSelectionViewModel)
        {
            DataContext = exportSelectionViewModel;
            InitializeComponent();
        }
    }
}
