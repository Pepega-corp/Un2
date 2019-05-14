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
            get { return this.name; }
            set { SetProperty(ref this.name, value); }
        }

        private Brush colorBrush;

        /// <summary>
        /// The color brush
        /// </summary>
        public Brush ColorBrush
        {
            get { return this.colorBrush; }
            set { SetProperty(ref this.colorBrush, value); }
        }
    }
}