using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel.Elements
{
    public abstract class MeasuringElementEditorViewModelBase : ViewModelBase, IMeasuringElementEditorViewModel
    {
        protected IMeasuringElement _measuringElement;
        private string _header;

        #region Implementation of IStronglyNamed

        public abstract string StrongName { get; }

        #endregion

        #region Implementation of IViewModel

        public object Model
        {
            get { return this.GetModel(); }
            set { this.SetModel(value); }
        }

        protected virtual void SetModel(object value)
        {
            this._measuringElement = value as IMeasuringElement;
            this._header = this._measuringElement.Name;
            this.RaisePropertyChanged(nameof(this.Header));
        }

        protected virtual IMeasuringElement GetModel()
        {
            this._measuringElement.Name = this.Header;
            return this._measuringElement;
        }


        #endregion

        #region Implementation of IMeasuringElementViewModel

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

        #endregion
    }
}
