using System;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.ViewModel.Elements
{
    public abstract class MeasuringElementViewModelBase : ViewModelBase, IMeasuringElementViewModel
    {
        private IFormattedValueViewModel _formattedValueViewModel;
        private string _groupName;
		private Lazy<Guid> _idLazy=new Lazy<Guid>(Guid.NewGuid);

		
		public abstract string StrongName { get; }

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


        public Guid Id => _idLazy.Value;
    }
}
