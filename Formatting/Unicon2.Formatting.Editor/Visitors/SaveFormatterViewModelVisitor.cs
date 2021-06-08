using System;
using System.Collections.Generic;
using System.Linq;
using Unicon2.Formatting.Editor.ViewModels;
using Unicon2.Formatting.Model;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Formatting.Editor.Visitors
{
    public class SaveFormatterViewModelVisitor : IFormatterViewModelVisitor<IUshortsFormatter>
    {

        public IUshortsFormatter VisitBoolFormatter(BoolFormatterViewModel formatterViewModel)
        {
            return new BoolFormatter();
        }

        public IUshortsFormatter VisitAsciiStringFormatter(AsciiStringFormatterViewModel formatterViewModel)
        {
            return new AsciiStringFormatter();
        }

        public IUshortsFormatter VisitTimeFormatter(DefaultTimeFormatterViewModel formatterViewModel)
        {
            var formatter =
                new DefaultTimeFormatter
                {
                    NumberOfPointsInUse = int.Parse(formatterViewModel.NumberOfPointsInUse),
                    MillisecondsDecimalsPlaces = int.Parse(formatterViewModel.MillisecondsDecimalsPlaces),
                    YearPointNumber = int.Parse(formatterViewModel.YearPointNumber),
                    MonthPointNumber = int.Parse(formatterViewModel.MonthPointNumber),
                    DayInMonthPointNumber = int.Parse(formatterViewModel.DayInMonthPointNumber),
                    HoursPointNumber = int.Parse(formatterViewModel.HoursPointNumber),
                    MinutesPointNumber = int.Parse(formatterViewModel.MinutesPointNumber),
                    SecondsPointNumber = int.Parse(formatterViewModel.SecondsPointNumber),
                    MillisecondsPointNumber = int.Parse(formatterViewModel.MillisecondsPointNumber)
                };
            return formatter;
        }

        public IUshortsFormatter VisitDirectUshortFormatter(DirectFormatterViewModel formatterViewModel)
        {
            return new DirectUshortFormatter();
        }

        public IUshortsFormatter VisitFormulaFormatter(FormulaFormatterViewModel formatterViewModel)
        {
            return new FormulaFormatter()
            {
                FormulaString = formatterViewModel.FormulaString,
                NumberOfSimbolsAfterComma = formatterViewModel.NumberOfSimbolsAfterComma,
                UshortFormattableResources = formatterViewModel.ArgumentViewModels.Select(model => model.ResourceNameString).ToList()
            };
        }

        public IUshortsFormatter VisitString1251Formatter(StringFormatter1251ViewModel formatterViewModel)
        {
            return new StringFormatter1251();
        }

        public IUshortsFormatter VisitUshortToIntegerFormatter(UshortToIntegerFormatterViewModel formatterViewModel)
        {
            return new UshortToIntegerFormatter();
        }

        public IUshortsFormatter VisitDictionaryMatchFormatter(DictionaryMatchingFormatterViewModel formatterViewModel)
        {
            var formatter = new DictionaryMatchingFormatter();
            formatter.StringDictionary = new Dictionary<ushort, string>();
            foreach (BindableKeyValuePair<ushort, string> bkvp in formatterViewModel.KeyValuesDictionary)
            {
                formatter.StringDictionary.Add(bkvp.Key, bkvp.Value);
            }
            formatter.IsKeysAreNumbersOfBits = formatterViewModel.IsKeysAreNumbersOfBits;
            formatter.UseDefaultMessage = formatterViewModel.UseDefaultMessage;
            formatter.DefaultMessage = formatterViewModel.DefaultMessage;
            return formatter;
        }

        public IUshortsFormatter VisitBitMaskFormatter(DefaultBitMaskFormatterViewModel formatterViewModel)
        {
            var formatter = new DefaultBitMaskFormatter();
            formatter.BitSignatures = formatterViewModel.BitSignatures.Select(wrapper => wrapper.StringValue).ToList();
            return formatter;
        }

        public IUshortsFormatter VisitCodeFormatter(CodeFormatterViewModel formatterViewModel)
        {
            var formatter = new CodeFormatter
            {
                FormatCodeString = formatterViewModel.FormatCodeString,
                FormatBackCodeString = formatterViewModel.FormatBackCodeString
            };
            return formatter;
        }
    }
}
