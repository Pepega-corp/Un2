using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Formatting.Editor.ViewModels.Validators;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Formatting.Infrastructure.ViewModel.InnerMembers;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Factories;
using Unicon2.Infrastructure.Services;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Editor.ViewModels
{

    public class FormulaFormatterViewModel : UshortsFormatterViewModelBase, IFormulaFormatterViewModel, IInitializableFromContainer
    {
        private readonly ILocalizerService _localizerService;
        private readonly ITypesContainer _container;
        private readonly Func<IArgumentViewModel> _argumentViewModelGettingFunc;
        private readonly ISharedResourcesViewModelFactory _sharedResourcesViewModelFactory;
        private IFormulaFormatter _formulaFormatter;
        private string _formulaString;
        private string _testResult;
        private double _testValueOfX;
        private ushort _numberOfSimbolsAfterComma;
        private string _formulaTooltipString;

        public FormulaFormatterViewModel(ILocalizerService localizerService, ITypesContainer container,
            Func<IArgumentViewModel> argumentViewModelGettingFunc, ISharedResourcesViewModelFactory sharedResourcesViewModelFactory)
        {
            this._localizerService = localizerService;
            this._container = container;
            this._argumentViewModelGettingFunc = argumentViewModelGettingFunc;
            this._sharedResourcesViewModelFactory = sharedResourcesViewModelFactory;
            this.ArgumentViewModels = new ObservableCollection<IArgumentViewModel>();
            this._formulaFormatter = this._container.Resolve<IUshortsFormatter>(StringKeys.FORMULA_FORMATTER) as IFormulaFormatter;

            if (this._formulaFormatter == null)
                throw new ArgumentException();

            this.CheckCommand = new RelayCommand(this.OnCheckCommandExecute);
            this.DeleteArgumentCommand = new RelayCommand<IArgumentViewModel>(this.OnDeleteArgumentExecute);
            this.AddArgumentCommand = new RelayCommand(this.OnAddArgumentExecute);
            this._formulaFormatter.NumberOfSimbolsAfterComma = 3;

            this.InitializeFormulaTooltip();
        }

        private void OnAddArgumentExecute()
        {
            this.SaveChanges();
            IUshortFormattable resource =
                this._sharedResourcesViewModelFactory.OpenSharedResourcesForSelecting(typeof(IUshortFormattable)) as IUshortFormattable;
            if (resource != null)
            {
                this._formulaFormatter.UshortFormattables.Add(resource);
            }
            this.InitFromFormatter(this._formulaFormatter);
        }

        private void OnDeleteArgumentExecute(IArgumentViewModel argumentViewModel)
        {
            this.ArgumentViewModels.Remove(argumentViewModel);
            this._formulaFormatter.UshortFormattables.Remove(argumentViewModel.Model as IUshortFormattable);
        }

        private void OnCheckCommandExecute()
        {
            this.FireErrorsChanged(nameof(this.TestValueOfX));
            this.FireErrorsChanged(nameof(this.FormulaString));
            if (this.HasErrors) return;
            this._formulaFormatter.FormulaString = this.FormulaString;
            this._formulaFormatter.NumberOfSimbolsAfterComma = this.NumberOfSimbolsAfterComma;
            if (this.ArgumentViewModels.Count > 0)
            {
                this.TestResult = this._localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.DYNAMIC_VALUES_CHECKING_IMPOSSIBLE);
                return;
            }
            try
            {
                this.TestResult = this._formulaFormatter.Format(new[] { (ushort)this.TestValueOfX }).ToString();
            }
            catch
            {
                this.TestResult = this._localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.ERROR);
            }
        }


        protected override void OnValidate()
        {
            FluentValidation.Results.ValidationResult result = new FormulaFormatterViewModelValidator(this._localizerService).Validate(this);
            this.SetValidationErrors(result);
        }

        public override IUshortsFormatter GetFormatter()
        {
            this.SaveChanges();
            return this._formulaFormatter;
        }

        private void SaveChanges()
        {
            this._formulaFormatter.FormulaString = this.FormulaString;
            this._formulaFormatter.NumberOfSimbolsAfterComma = this.NumberOfSimbolsAfterComma;
        }

        public override void InitFromFormatter(IUshortsFormatter ushortsFormatter)
        {
            if (ushortsFormatter == null)
            {
                this._formulaFormatter = this._container.Resolve<IUshortsFormatter>(StringKeys.FORMULA_FORMATTER) as IFormulaFormatter;
                this.ArgumentViewModels.Clear();
                this.FormulaString = string.Empty;
                this.NumberOfSimbolsAfterComma = 4;
            }
            if (!(ushortsFormatter is IFormulaFormatter)) return;

            this._formulaFormatter = ushortsFormatter as IFormulaFormatter;


            this.ArgumentViewModels.Clear();
            int index = 1;
            if (this._formulaFormatter.UshortFormattables != null)
            {
                foreach (IUshortFormattable resource in this._formulaFormatter.UshortFormattables)
                {

                    IArgumentViewModel argumentViewModel = this._argumentViewModelGettingFunc();
                    argumentViewModel.ArgumentName = "x" + index++;
                    argumentViewModel.Model = resource;
                    this.ArgumentViewModels.Add(argumentViewModel);
                }
            }
            this.NumberOfSimbolsAfterComma = this._formulaFormatter.NumberOfSimbolsAfterComma;
            this.FormulaString = this._formulaFormatter.FormulaString;
        }

        public string FormulaToolTipString
        {
            get { return _formulaTooltipString; }
            set
            {
                this._formulaTooltipString = value;
                this.RaisePropertyChanged();
            }
        }

        public string FormulaString
        {
            get { return this._formulaString; }
            set
            {
                this._formulaString = value;
                this.RaisePropertyChanged();
            }
        }

        public double TestValueOfX
        {
            get { return this._testValueOfX; }
            set
            {
                this._testValueOfX = value;
                this.RaisePropertyChanged();
            }
        }

        public string TestResult
        {
            get { return this._testResult; }
            set
            {
                this._testResult = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand CheckCommand { get; set; }

        public ObservableCollection<IArgumentViewModel> ArgumentViewModels { get; }

        public ICommand DeleteArgumentCommand { get; }

        public ICommand AddArgumentCommand { get; }

        public ushort NumberOfSimbolsAfterComma
        {
            get { return this._numberOfSimbolsAfterComma; }
            set
            {
                this._numberOfSimbolsAfterComma = value;
                this.RaisePropertyChanged();
            }
        }


        public override string StrongName => this._formulaFormatter?.StrongName;

        public override object Model
        {
            get { return this._formulaFormatter; }
            set
            {
                if (value is IFormulaFormatter)
                    this.InitFromFormatter(value as IUshortsFormatter);
            }
        }

        public override object Clone()
        {
            FormulaFormatterViewModel cloneFormulaFormatterViewModel = new FormulaFormatterViewModel(this._localizerService, this._container, this._argumentViewModelGettingFunc, this._sharedResourcesViewModelFactory);
            this.SaveChanges();
            cloneFormulaFormatterViewModel.InitFromFormatter(this._formulaFormatter.Clone() as IUshortsFormatter);
            return cloneFormulaFormatterViewModel;
        }

        public bool IsValid
        {
            get
            {
                this.FireErrorsChanged(nameof(this.FormulaString));
                if (this.HasErrors)
                {
                    if (this.GetErrors(nameof(this.FormulaString)) == null) return true;
                    return false;
                }
                return true;
            }
        }

        private void InitializeFormulaTooltip()
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("Some text");
            sb.AppendLine("Some text");
            sb.AppendLine("Some text");
            sb.AppendLine("Some text");
            sb.AppendLine("Some text");
            sb.AppendLine("Some text");
            sb.AppendLine("Some text");

            _formulaTooltipString = sb.ToString();

        }

        public bool IsInEditMode { get; set; }

        public void StartEditElement()
        {
            this.IsInEditMode = true;
        }

        public void StopEditElement()
        {
            this.SaveChanges();
            this.IsInEditMode = false;
        }


        public bool IsInitialized { get; private set; }

        public void InitializeFromContainer(ITypesContainer container)
        {
            this.IsInitialized = true;
            this._formulaFormatter?.InitializeFromContainer(container);
        }
    }
}