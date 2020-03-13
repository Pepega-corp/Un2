using System;
using System.Collections.Generic;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Interfaces.DependentProperty;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.DependentProperty;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.DependentProperty
{
    public class ConditionViewModel : PropertyEditorViewModel, IConditionViewModel, IUshortFormattableEditorViewModel
    {

        private IDependancyCondition _dependancyCondition;
        private readonly ISharedResourcesGlobalViewModel _sharedResourcesGlobalViewModel;
        private readonly IFormatterEditorFactory _formatterEditorFactory;
        private ushort _ushortValueToCompare;
        private string _selectedConditionResult;
        private string _selectedCondition;

        public ConditionViewModel(IDependancyCondition dependancyCondition,
            ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel,
            IFormatterEditorFactory formatterEditorFactory, ITypesContainer container, IRangeViewModel rangeViewModel,
            ILocalizerService localizerService) : base(container, rangeViewModel, localizerService)
        {
            _dependancyCondition = dependancyCondition;
            _sharedResourcesGlobalViewModel = sharedResourcesGlobalViewModel;
            _formatterEditorFactory = formatterEditorFactory;
            SelectPropertyFromResourceCommand = new RelayCommand(OnSelectPropertyFromResourceExecute);
            ConditionResultList = new List<string>(Enum.GetNames(typeof(ConditionResultEnum)));
            ConditionsList = new List<string>(Enum.GetNames(typeof(ConditionsEnum)));
            ShowFormatterParameters = new RelayCommand(OnShowFormatterParameters);
        }


        private void OnShowFormatterParameters()
        {
            _formatterEditorFactory.EditFormatterByUser(this);
            RaisePropertyChanged(nameof(UshortFormatterString));
        }

        private void OnSelectPropertyFromResourceExecute()
        {
           // this._dependancyCondition.LocalAndDeviceValuesContaining = this._sharedResourcesViewModelFactory.OpenSharedResourcesForSelecting(typeof(ILocalAndDeviceValuesContaining)) as ILocalAndDeviceValuesContaining;
            RaisePropertyChanged(nameof(ReferencedResorcePropertyName));
        }


        public ICommand SelectPropertyFromResourceCommand { get; }
        public string ReferencedResorcePropertyName
        {
            get { return ""; /* return this._dependancyCondition?.LocalAndDeviceValuesContaining?.Name;*/ }
        }

        public List<string> ConditionsList { get; set; }

        public string SelectedCondition
        {
            get { return _selectedCondition; }
            set
            {
                _selectedCondition = value;
                RaisePropertyChanged();
            }
        }

        public ushort UshortValueToCompare
        {
            get { return _ushortValueToCompare; }
            set
            {
                _ushortValueToCompare = value;
                RaisePropertyChanged();
            }
        }

        public List<string> ConditionResultList { get; set; }

        public string SelectedConditionResult
        {
            get { return _selectedConditionResult; }
            set
            {
                _selectedConditionResult = value;
                RaisePropertyChanged();
            }
        }
          public string UshortFormatterString =>"";

        //  public string UshortFormatterString => this._dependancyCondition?.UshortsFormatter?.StrongName;
        public ICommand ShowFormatterParameters { get; }

        public string StrongName => ConfigurationKeys.DEPENDANCY_CONDITION +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public object Model
        {
            get { return GetModel(); }
            set { SetModel(value as IDependancyCondition); }
        }

        private void SetModel(IDependancyCondition dependancyCondition)
        {
            _dependancyCondition = dependancyCondition;
            SelectedCondition = _dependancyCondition.ConditionsEnum.ToString();
            SelectedConditionResult = _dependancyCondition.ConditionResult.ToString();
            UshortValueToCompare = _dependancyCondition.UshortValueToCompare;
            RaisePropertyChanged(nameof(UshortFormatterString));
        }


        private IDependancyCondition GetModel()
        {
            _dependancyCondition.UshortValueToCompare = UshortValueToCompare;
            ConditionsEnum cond;
            Enum.TryParse(SelectedCondition, out cond);
            _dependancyCondition.ConditionsEnum = cond;
            ConditionResultEnum condRes;
            Enum.TryParse(SelectedConditionResult, out condRes);
            _dependancyCondition.ConditionResult = condRes;
            return _dependancyCondition;
        }
    }
}