using System.Windows.Media;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Shell.Models
{
    public class AccentColor : ViewModelBase
    {
        private string name;

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private Brush colorBrush;

        /// <summary>
        /// The color brush
        /// </summary>
        public Brush ColorBrush
        {
            get { return colorBrush; }
            set { SetProperty(ref colorBrush, value); }
        }
    }
}