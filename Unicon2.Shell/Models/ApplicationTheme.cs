using System.Windows.Media;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Shell.Models
{
    public class ApplicationTheme : ViewModelBase
    {
        private string _name;

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get { return this._name; }
            set { SetProperty(ref this._name, value); }
        }

        private Brush colorBrush;

        /// <summary>
        /// The color brush
        /// </summary>
        public Brush ColorBrush
        {
            get { return colorBrush; }
            set { SetProperty(ref this.colorBrush, value); }
        }

        private Brush borderColorBrush;

        /// <summary>
        /// The border color brush
        /// </summary>
        public Brush BorderColorBrush
        {
            get { return borderColorBrush; }
            set { SetProperty(ref this.borderColorBrush, value); }
        }
    }
}