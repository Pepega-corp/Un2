using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel.Elements
{
    public abstract class MeasuringElementEditorViewModelBase : ViewModelBase, IMeasuringElementEditorViewModel
    {
        private string _header;

        public abstract string StrongName { get; }

        public string Header
        {
            get { return this._header; }

            set
            {
                this._header = value;
                this.RaisePropertyChanged();
            }
        }

        public abstract string NameForUiKey { get; }
    }
}
