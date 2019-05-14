using System;
using System.Collections.Generic;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Interfaces.DependentProperty;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.DependentProperty;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Infrastructure.Interfaces.Factories;
using Unicon2.Infrastructure.Interfaces.Values;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.DependentProperty
{
    public class ConditionViewModel : DisposableBindableBase, IConditionViewModel
    {

        private IDependancyCondition _dependancyCondition;
        private readonly ISharedResourcesViewModelFactory _sharedResourcesViewModelFactory;
        private readonly IFormatterEditorFactory _formatterEditorFactory;
        private ushort _ushortValueToCompare;
        private string _selectedConditionResult;
        private string _selectedCondition;

        public ConditionViewModel(IDependancyCondition dependancyCondition, ISharedResourcesViewModelFactory sharedResourcesViewModelFactory, IFormatterEditorFactory formatterEditorFactory)
        {
            this._dependancyCondition = dependancyCondition;
            this._sharedResourcesViewModelFactory = sharedResourcesViewModelFactory;
            this._formatterEditorFactory = formatterEditorFactory;
            this.SelectPropertyFromResourceCommand = new RelayCommand(this.OnSelectPropertyFromResourceExecute);
            this.ConditionResultList = new List<string>(Enum.GetNames(typeof(ConditionResultEnum)));
            this.ConditionsList = new List<string>(Enum.GetNames(typeof(ConditionsEnum)));
            this.ShowFormatterParameters = new RelayCommand(this.OnShowFormatterParameters);
        }

        private void OnShowFormatterParameters()
        {
            this._formatterEditorFactory.EditFormatterByUser(this._dependancyCondition);
            this.RaisePropertyChanged(nameof(this.UshortFormatterString));
        }

        private void OnSelectPropertyFromResourceExecute()
        {
            this._dependancyCondition.LocalAndDeviceValuesContaining = this._sharedResourcesViewModelFactory.OpenSharedResourcesForSelecting(typeof(ILocalAndDeviceValuesContaining)) as ILocalAndDeviceValuesContaining;
            this.RaisePropertyChanged(nameof(this.ReferencedResorcePropertyName));
        }


        #region Implementation of IConditionViewModel

        public ICommand SelectPropertyFromResourceCommand { get; }
        public string ReferencedResorcePropertyName
        {
            get { return this._dependancyCondition?.LocalAndDeviceValuesContaining?.Name; }
        }

        public List<string> ConditionsList { get; set; }

        public string SelectedCondition
        {
            get { return this._selectedCondition; }
            set
            {
                this._selectedCondition = value;
                this.RaisePropertyChanged();
            }
        }

        public ushort UshortValueToCompare
        {
            get { return this._ushortValueToCompare; }
            set
            {
                this._ushortValueToCompare = value;
                this.RaisePropertyChanged();
            }
        }

        public List<string> ConditionResultList { get; set; }

        public string SelectedConditionResult
        {
            get { return this._selectedConditionResult; }
            set
            {
                this._selectedConditionResult = value;
                this.RaisePropertyChanged();
            }
        }

        public string UshortFormatterString => this._dependancyCondition?.UshortsFormatter?.StrongName;
        public ICommand ShowFormatterParameters { get; }

        #endregion

        #region Implementation of IStronglyNamed

        public string StrongName => ConfigurationKeys.DEPENDANCY_CONDITION +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        #endregion

        #region Implementation of IViewModel

        public object Model
        {
            get { return this.GetModel(); }
            set { this.SetModel(value as IDependancyCondition); }
        }

        private void SetModel(IDependancyCondition dependancyCondition)
        {
            this._dependancyCondition = dependancyCondition;
            this.SelectedCondition = this._dependancyCondition.ConditionsEnum.ToString();
            this.SelectedConditionResult = this._dependancyCondition.ConditionResult.ToString();
            this.UshortValueToCompare = this._dependancyCondition.UshortValueToCompare;
            this.RaisePropertyChanged(nameof(this.UshortFormatterString));
        }


        private IDependancyCondition GetModel()
        {
            this._dependancyCondition.UshortValueToCompare = this.UshortValueToCompare;
            ConditionsEnum cond;
            Enum.TryParse(this.SelectedCondition, out cond);
            this._dependancyCondition.ConditionsEnum = cond;
            ConditionResultEnum condRes;
            Enum.TryParse(this.SelectedConditionResult, out condRes);
            this._dependancyCondition.ConditionResult = condRes;
            return this._dependancyCondition;
        }


        #endregion
    }
}