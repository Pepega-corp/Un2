using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Formatting.Editor.ViewModels.Validators;
using Unicon2.Formatting.Editor.Visitors;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Formatting.Infrastructure.ViewModel.InnerMembers;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.Formatting;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Editor.ViewModels
{
    public class FormulaFormatterViewModel : UshortsFormatterViewModelBase, IFormulaFormatterViewModel
    {
        private readonly ILocalizerService _localizerService;
        private readonly ITypesContainer _container;
        private readonly Func<IArgumentViewModel> _argumentViewModelGettingFunc;
        private readonly ISharedResourcesGlobalViewModel _sharedResourcesGlobalViewModel;
        private readonly IFormattingService _formattingService;
        private IFormulaFormatter _formulaFormatter;
        private string _formulaString;
        private string _testResult;
        private double _testValueOfX;
        private ushort _numberOfSimbolsAfterComma;
        private string _formulaTooltipString;

        public FormulaFormatterViewModel(ILocalizerService localizerService, ITypesContainer container,
            Func<IArgumentViewModel> argumentViewModelGettingFunc,
            ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel, IFormattingService formattingService)
        {
            _localizerService = localizerService;
            _container = container;
            _argumentViewModelGettingFunc = argumentViewModelGettingFunc;
            _sharedResourcesGlobalViewModel = sharedResourcesGlobalViewModel;
            _formattingService = formattingService;
            ArgumentViewModels = new ObservableCollection<IArgumentViewModel>();
            _formulaFormatter =
                _container.Resolve<IUshortsFormatter>(StringKeys.FORMULA_FORMATTER) as IFormulaFormatter;

            if (_formulaFormatter == null)
                throw new ArgumentException();

            CheckCommand = new RelayCommand(OnCheckCommandExecute);
            DeleteArgumentCommand = new RelayCommand<IArgumentViewModel>(OnDeleteArgumentExecute);
            AddArgumentCommand = new RelayCommand(OnAddArgumentExecute);
            _formulaFormatter.NumberOfSimbolsAfterComma = 3;

            InitializeFormulaTooltip();
        }

        public override T Accept<T>(IFormatterViewModelVisitor<T> visitor)
        {
            return visitor.VisitFormulaFormatter(this);
        }

        private void OnAddArgumentExecute()
        {
            SaveChanges();
            IUshortFormattable resource =
                _sharedResourcesGlobalViewModel.OpenSharedResourcesForSelecting<IUshortFormattable>();
            if (resource != null)
            {
                _formulaFormatter.UshortFormattables.Add(resource);
            }

            //  this.InitFromFormatter(this._formulaFormatter);
        }

        private void OnDeleteArgumentExecute(IArgumentViewModel argumentViewModel)
        {
            ArgumentViewModels.Remove(argumentViewModel);
            _formulaFormatter.UshortFormattables.Remove(argumentViewModel.Model as IUshortFormattable);
        }

        private void OnCheckCommandExecute()
        {
            FireErrorsChanged(nameof(TestValueOfX));
            FireErrorsChanged(nameof(FormulaString));
            if (HasErrors) return;
            _formulaFormatter.FormulaString = FormulaString;
            _formulaFormatter.NumberOfSimbolsAfterComma = NumberOfSimbolsAfterComma;
            if (ArgumentViewModels.Count > 0)
            {
                TestResult =
                    _localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages
                        .DYNAMIC_VALUES_CHECKING_IMPOSSIBLE);
                return;
            }

            try
            {
                TestResult = _formattingService.FormatValue(_formulaFormatter, new[] {(ushort) TestValueOfX})
                    .ToString();
            }
            catch
            {
                TestResult =
                    _localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.ERROR);
            }
        }


        protected override void OnValidate()
        {
            FluentValidation.Results.ValidationResult result =
                new FormulaFormatterViewModelValidator(_localizerService).Validate(this);
            SetValidationErrors(result);
        }

        //public override IUshortsFormatter GetFormatter()
        //{
        //    this.SaveChanges();
        //    return this._formulaFormatter;
        //}

        private void SaveChanges()
        {
            _formulaFormatter.FormulaString = FormulaString;
            _formulaFormatter.NumberOfSimbolsAfterComma = NumberOfSimbolsAfterComma;
        }

        //public override void InitFromFormatter(IUshortsFormatter ushortsFormatter)
        //{
        //    if (ushortsFormatter == null)
        //    {
        //        this._formulaFormatter =
        //            this._container.Resolve<IUshortsFormatter>(StringKeys.FORMULA_FORMATTER) as IFormulaFormatter;
        //        this.ArgumentViewModels.Clear();
        //        this.FormulaString = string.Empty;
        //        this.NumberOfSimbolsAfterComma = 4;
        //    }

        //    if (!(ushortsFormatter is IFormulaFormatter)) return;

        //    this._formulaFormatter = ushortsFormatter as IFormulaFormatter;


        //    this.ArgumentViewModels.Clear();
        //    int index = 1;
        //    if (this._formulaFormatter.UshortFormattables != null)
        //    {
        //        foreach (IUshortFormattable resource in this._formulaFormatter.UshortFormattables)
        //        {

        //            IArgumentViewModel argumentViewModel = this._argumentViewModelGettingFunc();
        //            argumentViewModel.ArgumentName = "x" + index++;
        //            argumentViewModel.Model = resource;
        //            this.ArgumentViewModels.Add(argumentViewModel);
        //        }
        //    }

        //    this.NumberOfSimbolsAfterComma = this._formulaFormatter.NumberOfSimbolsAfterComma;
        //    this.FormulaString = this._formulaFormatter.FormulaString;
        //}

        public string FormulaToolTipString
        {
            get { return _formulaTooltipString; }
            set
            {
                _formulaTooltipString = value;
                RaisePropertyChanged();
            }
        }

        public string FormulaString
        {
            get { return _formulaString; }
            set
            {
                _formulaString = value;
                RaisePropertyChanged();
            }
        }

        public double TestValueOfX
        {
            get { return _testValueOfX; }
            set
            {
                _testValueOfX = value;
                RaisePropertyChanged();
            }
        }

        public string TestResult
        {
            get { return _testResult; }
            set
            {
                _testResult = value;
                RaisePropertyChanged();
            }
        }

        public ICommand CheckCommand { get; set; }

        public ObservableCollection<IArgumentViewModel> ArgumentViewModels { get; }

        public ICommand DeleteArgumentCommand { get; }

        public ICommand AddArgumentCommand { get; }

        public ushort NumberOfSimbolsAfterComma
        {
            get { return _numberOfSimbolsAfterComma; }
            set
            {
                _numberOfSimbolsAfterComma = value;
                RaisePropertyChanged();
            }
        }


        public override string StrongName => StringKeys.FORMULA_FORMATTER;

        public override object Clone()
        {
            FormulaFormatterViewModel cloneFormulaFormatterViewModel =
                new FormulaFormatterViewModel(_localizerService, _container,
                    _argumentViewModelGettingFunc, _sharedResourcesGlobalViewModel, _formattingService);
            SaveChanges();
            // cloneFormulaFormatterViewModel.InitFromFormatter(this._formulaFormatter.Clone() as IUshortsFormatter);
            return cloneFormulaFormatterViewModel;
        }

        public bool IsValid
        {
            get
            {
                FireErrorsChanged(nameof(FormulaString));
                if (HasErrors)
                {
                    if (GetErrors(nameof(FormulaString)) == null) return true;
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
            IsInEditMode = true;
        }

        public void StopEditElement()
        {
            SaveChanges();
            IsInEditMode = false;
        }
    }
}