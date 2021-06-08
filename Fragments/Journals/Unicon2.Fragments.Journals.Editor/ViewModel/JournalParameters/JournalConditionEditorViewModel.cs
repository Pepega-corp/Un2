using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Journals.Editor.Interfaces.JournalParameters;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Journals.Editor.ViewModel.JournalParameters
{
    public class JournalConditionEditorViewModel : ViewModelBase, IJournalConditionEditorViewModel
    {
        private List<IJournalParameter> _availableJournalParameters;
        private string _selectedJournalParameter;
        private List<string> _conditionsList;
        private string _selectedCondition;
        private ushort _ushortValueToCompare;
        private List<string> _conditionResultList;
        private string _selectedConditionResult;
        private IJournalCondition _journalCondition;
        private readonly IFormatterEditorFactory _formatterEditorFactory;
        private bool _isInEditMode;


        public JournalConditionEditorViewModel(IJournalCondition journalCondition, IFormatterEditorFactory formatterEditorFactory)
        {
            this._journalCondition = journalCondition;
            this._formatterEditorFactory = formatterEditorFactory;
            this.ShowFormatterParameters = new RelayCommand(this.OnShowFormatterParameters);
        }

        private void OnShowFormatterParameters()
        {
            this._formatterEditorFactory.EditFormatterByUser(this,new List<IConfigurationItemViewModel>());
            this.RaisePropertyChanged(nameof(this.UshortFormatterString));
        }


        public void SetAvailablePatameters(List<IJournalParameter> availableJournalParameters)
        {
            this._availableJournalParameters = availableJournalParameters;
        }

        public List<string> AvailableJournalParameters
        {
            get { return this._availableJournalParameters.Select((parameter => parameter.Name)).ToList(); }
        }

        public string SelectedJournalParameter
        {
            get { return this._selectedJournalParameter; }
            set
            {
                this._selectedJournalParameter = value;
                this.RaisePropertyChanged();
            }
        }

        public List<string> ConditionsList
        {
            get { return this._conditionsList; }
            set
            {
                this._conditionsList = value;
                this.RaisePropertyChanged();
            }
        }

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

        public List<string> ConditionResultList
        {
            get { return this._conditionResultList; }
            set
            {
                this._conditionResultList = value;
                this.RaisePropertyChanged();

            }
        }

        public string SelectedConditionResult
        {
            get { return this._selectedConditionResult; }
            set
            {
                this._selectedConditionResult = value;
                this.RaisePropertyChanged();

            }
        }

        public string UshortFormatterString => FormatterParametersViewModel.Name;

        public ICommand ShowFormatterParameters { get; }

        public string StrongName => nameof(JournalConditionEditorViewModel);

        public object Model
        {
            get
            {
                _journalCondition.UshortsFormatter= StaticContainer.Container.Resolve<ISaveFormatterService>()
                    .CreateUshortsParametersFormatter(FormatterParametersViewModel);
                return this._journalCondition;

            }
            set
            {
                this._journalCondition = value as IJournalCondition;
                this.SelectedJournalParameter = this._journalCondition.BaseJournalParameter.Name;
                this.SelectedCondition = this._journalCondition.ConditionsEnum.ToString();
                FormatterParametersViewModel=StaticContainer.Container.Resolve<IFormatterViewModelFactory>()
                    .CreateFormatterViewModel(_journalCondition.UshortsFormatter);
                this.UshortValueToCompare = this._journalCondition.UshortValueToCompare;
                this.RaisePropertyChanged(nameof(this.UshortFormatterString));
            }
        }

        public bool IsInEditMode
        {
            get { return this._isInEditMode; }
            set
            {
                this._isInEditMode = value;
                this.RaisePropertyChanged();
            }
        }

        public void StartEditElement()
        {
            this.IsInEditMode = true;
        }

        public void StopEditElement()
        {
            this.Save();
            this.IsInEditMode = false;
        }



        private void Save()
        {
            this._journalCondition.BaseJournalParameter =
                this._availableJournalParameters.First((parameter => parameter.Name == this.SelectedJournalParameter));
            ConditionsEnum conditionsEnum;
            Enum.TryParse(this.SelectedCondition, out conditionsEnum);
            this._journalCondition.ConditionsEnum = conditionsEnum;
            this._journalCondition.UshortsFormatter = this._journalCondition.UshortsFormatter;
            this._journalCondition.UshortValueToCompare = this.UshortValueToCompare;
        }

        public string Name { get; set; }
        public IFormatterParametersViewModel FormatterParametersViewModel { get; set; }
    }
}
