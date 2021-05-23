using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unicon2.Formatting.Editor.ViewModels;
using Unicon2.Formatting.Editor.ViewModels.FormatterParameters;
using Unicon2.Formatting.Editor.ViewModels.InnerMembers;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Visitors;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Formatting.Editor.Factories
{
    public class FormatterViewModelFactory : IFormatterViewModelFactory, IFormatterVisitor<IUshortsFormatterViewModel>
    {

        public FormatterViewModelFactory()
        {
        }


        public IUshortsFormatterViewModel VisitBoolFormatter(IUshortsFormatter formatter)
        {
            return new BoolFormatterViewModel();
        }

        public IUshortsFormatterViewModel VisitAsciiStringFormatter(IUshortsFormatter formatter)
        {
            return new AsciiStringFormatterViewModel();
        }

        public IUshortsFormatterViewModel VisitTimeFormatter(IUshortsFormatter formatter)
        {
            var viewModel = new DefaultTimeFormatterViewModel();
            IDefaultTimeFormatter defaultTimeFormatter = formatter as IDefaultTimeFormatter;
            viewModel.MillisecondsDecimalsPlaces = defaultTimeFormatter.MillisecondsDecimalsPlaces.ToString();
            viewModel.NumberOfPointsInUse = defaultTimeFormatter.NumberOfPointsInUse.ToString();
            viewModel.YearPointNumber = defaultTimeFormatter.YearPointNumber.ToString();
            viewModel.MonthPointNumber = defaultTimeFormatter.MonthPointNumber.ToString();
            viewModel.DayInMonthPointNumber = defaultTimeFormatter.DayInMonthPointNumber.ToString();
            viewModel.HoursPointNumber = defaultTimeFormatter.HoursPointNumber.ToString();
            viewModel.MinutesPointNumber = defaultTimeFormatter.MinutesPointNumber.ToString();
            viewModel.SecondsPointNumber = defaultTimeFormatter.SecondsPointNumber.ToString();
            viewModel.MillisecondsPointNumber = defaultTimeFormatter.MillisecondsPointNumber.ToString();
            return viewModel;
        }

        public IUshortsFormatterViewModel VisitDirectUshortFormatter(IUshortsFormatter formatter)
        {
            return new DirectFormatterViewModel();
        }

        public IUshortsFormatterViewModel VisitFormulaFormatter(IUshortsFormatter formatter)
        {
            IUshortsFormatterViewModel formatterViewModel =
                StaticContainer.Container.Resolve<IUshortsFormatterViewModel>(
                    StringKeys.FORMULA_FORMATTER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            var formulaFormatter = formatter as IFormulaFormatter;
            (formatterViewModel as IFormulaFormatterViewModel).NumberOfSimbolsAfterComma =
                formulaFormatter.NumberOfSimbolsAfterComma;
            (formatterViewModel as IFormulaFormatterViewModel).FormulaString =
                formulaFormatter.FormulaString;
            (formatterViewModel as IFormulaFormatterViewModel).ArgumentViewModels.Clear();
            (formatterViewModel as IFormulaFormatterViewModel).ArgumentViewModels.AddCollection(formulaFormatter
                .UshortFormattableResources.Select(s => new ArgumentViewModel()
                {
                    ResourceNameString = s,
                    ArgumentName = s,
                    TestValue = 1,
                }).ToList());
            return formatterViewModel;
        }

        public IUshortsFormatterViewModel VisitString1251Formatter(IUshortsFormatter formatter)
        {
            return new StringFormatter1251ViewModel();
        }

        public IUshortsFormatterViewModel VisitUshortToIntegerFormatter(IUshortsFormatter formatter)
        {
            throw new System.NotImplementedException();
        }

        public IUshortsFormatterViewModel VisitDictionaryMatchFormatter(IUshortsFormatter formatter)
        {
            var vm = new DictionaryMatchingFormatterViewModel();
            if (!(formatter is IDictionaryMatchingFormatter dictionaryMatchingFormatter)) return null;
            foreach (KeyValuePair<ushort, string> kvp in dictionaryMatchingFormatter.StringDictionary)
            {
                vm.KeyValuesDictionary.Add(new BindableKeyValuePair<ushort, string>(kvp.Key, kvp.Value));
            }

            vm.IsKeysAreNumbersOfBits = dictionaryMatchingFormatter.IsKeysAreNumbersOfBits;
            vm.DefaultMessage = dictionaryMatchingFormatter.DefaultMessage;
            vm.UseDefaultMessage = dictionaryMatchingFormatter.UseDefaultMessage;
            return vm;

        }

        public IUshortsFormatterViewModel VisitBitMaskFormatter(IUshortsFormatter formatter)
        {
            var vm = new DefaultBitMaskFormatterViewModel();
            vm.BitSignatures =
                new ObservableCollection<StringWrapper>(
                    (formatter as IBitMaskFormatter).BitSignatures.Select(s => new StringWrapper(s)));
            return vm;
        }

        public IUshortsFormatterViewModel VisitMatrixFormatter(IUshortsFormatter formatter)
        {
            throw new System.NotImplementedException();
        }

        IFormatterParametersViewModel IFormatterViewModelFactory.CreateFormatterViewModel(
            IUshortsFormatter ushortsFormatter)
        {
            if (ushortsFormatter == null) return null;
            var formatterViewModel = ushortsFormatter.Accept(this);
            var formatterParametersViewModel = new FormatterParametersViewModel()
            {
                Name = ushortsFormatter.Name,
                IsFromSharedResources = ushortsFormatter.Name != null,
                RelatedUshortsFormatterViewModel = formatterViewModel
            };
            return formatterParametersViewModel;

        }
    }
}