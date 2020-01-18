using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.ViewModel.Elements
{
    public abstract class MeasuringElementViewModelBase : ViewModelBase, IMeasuringElementViewModel
    {
        protected IMeasuringElement _measuringElement;
        private IFormattedValueViewModel _formattedValueViewModel;
        private string _groupName;

        public abstract string StrongName { get; }

        public object Model
        {
            get { return this.GetModel(); }
            set { this.SetModel(value); }
        }



        protected virtual void SetModel(object model)
        {
            this._measuringElement = model as IMeasuringElement;
            this.Header = this._measuringElement.Name;
        }


        protected virtual IMeasuringElement GetModel()
        {
            return this._measuringElement;
        }

        public string Header { get; private set; }

        public string GroupName
        {
            get { return this._groupName; }
            set
            {
                this._groupName = value;
                this.RaisePropertyChanged();
            }
        }

        public IFormattedValueViewModel FormattedValueViewModel
        {
            get { return this._formattedValueViewModel; }
            set
            {
                this._formattedValueViewModel = value;
                this.RaisePropertyChanged();
            }
        }
    }
}
