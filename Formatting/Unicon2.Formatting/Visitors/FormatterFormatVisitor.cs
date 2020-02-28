using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.mariuszgromada.math.mxparser;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Model;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Visitors;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Visitors
{
    public class FormatterFormatVisitor : IFormatterVisitor<IFormattedValue>
    {
        private readonly ushort[] _ushortsPayload;
        private readonly ITypesContainer _typesContainer;
        private readonly ConcurrentDictionary<string, ConcurrentBag<IterationDefinition>> _iterationDefinitionsCache;
        
        public FormatterFormatVisitor(ushort[] ushortsPayload, ITypesContainer typesContainer,
            ConcurrentDictionary<string, ConcurrentBag<IterationDefinition>> iterationDefinitionsCache)
        {
            _ushortsPayload = ushortsPayload;
            _typesContainer = typesContainer;
            _iterationDefinitionsCache = iterationDefinitionsCache;
        }


        public IFormattedValue VisitBoolFormatter(IUshortsFormatter boolFormatter)
        {
            IBoolValue boolValue = _typesContainer.Resolve<IBoolValue>();
            if (_ushortsPayload[0] == 0)
            {
                boolValue.BoolValueProperty = false;
            }
            else
            {
                boolValue.BoolValueProperty = true;
            }
            return boolValue;
        }

        public IFormattedValue VisitAsciiStringFormatter(IUshortsFormatter formatter)
        {
            IStringValue stringValue = _typesContainer.Resolve<IStringValue>();  
            byte[] bytes = new byte[_ushortsPayload.Length * 2];
            Buffer.BlockCopy(_ushortsPayload, 0, bytes, 0, _ushortsPayload.Length * 2);
            string formattedString = Encoding.ASCII.GetString(bytes);
            stringValue.StrValue = formattedString;
            return stringValue;
        }

        public IFormattedValue VisitTimeFormatter(IUshortsFormatter formatter)
        {
            ITimeValue value = _typesContainer.Resolve<ITimeValue>();
            IDefaultTimeFormatter timeFormatter = formatter as IDefaultTimeFormatter;
            value.NumberOfPointsInUse = timeFormatter.NumberOfPointsInUse;
            value.MillisecondsDecimalsPlaces = timeFormatter.MillisecondsDecimalsPlaces;
            for (int i = 0; i < timeFormatter.NumberOfPointsInUse; i++)
            {
                if (timeFormatter.YearPointNumber == i)
                {
                    value.YearValue = _ushortsPayload[i];
                }
                else if (timeFormatter.MonthPointNumber == i)
                {
                    value.MonthValue = _ushortsPayload[i];
                }
                else if (timeFormatter.DayInMonthPointNumber == i)
                {
                    value.DayInMonthValue = _ushortsPayload[i];
                }
                else if (timeFormatter.HoursPointNumber == i)
                {
                    value.HoursValue = _ushortsPayload[i];
                }
                else if (timeFormatter.MinutesPointNumber == i)
                {
                    value.MinutesValue = _ushortsPayload[i];
                }
                else if (timeFormatter.SecondsPointNumber == i)
                {
                    value.SecondsValue = _ushortsPayload[i];
                }
                else if (timeFormatter.MillisecondsPointNumber == i)
                {
                    value.MillisecondsValue = _ushortsPayload[i];
                }
            }
            return value;
        }

        public IFormattedValue VisitDirectUshortFormatter(IUshortsFormatter formatter)
        {
            INumericValue numericValue = _typesContainer.Resolve<INumericValue>();
            numericValue.NumValue = _ushortsPayload[0];
            return numericValue;
        }

        public IFormattedValue VisitFormulaFormatter(IUshortsFormatter formatter)
        {
            INumericValue formattedValue = _typesContainer.Resolve<INumericValue>();
            IterationDefinition iterationDefinition = new IterationDefinition();
            IFormulaFormatter formulaFormatter = formatter as IFormulaFormatter;

            iterationDefinition.FormulaString = formulaFormatter.FormulaString;

            iterationDefinition.ArgumentNames = new List<string>();
            iterationDefinition.ArgumentValues = new List<double>();
            iterationDefinition.ArgumentNames.Add("x");
            iterationDefinition.ArgumentValues.Add(_ushortsPayload[0]);
            if (formulaFormatter.UshortFormattables != null)
            {
                int index = 1;
                foreach (IUshortFormattable formattableUshortResource in formulaFormatter.UshortFormattables)
                {
                   // if (formattableUshortResource is IDeviceValueContaining)
                   // {
                   //     IFormattedValue value = formattableUshortResource.UshortsFormatter
                    //        .Format((formattableUshortResource as IDeviceValueContaining).DeviceUshortsValue);
                    //    if (value is INumericValue)
                     //   {
                     //       double num = (value as INumericValue).NumValue;
                    //        iterationDefinition.ArgumentNames.Add("x" + index++);
                     //       iterationDefinition.ArgumentValues.Add(num);
                    //    }
                  //  }
                }
            }

            formattedValue.NumValue = MemoizeCalculateResult(iterationDefinition, formulaFormatter.FormulaString,
                formulaFormatter.NumberOfSimbolsAfterComma);
            return formattedValue;
        }



        private double MemoizeCalculateResult(IterationDefinition iterationDefinition, string formula, int numberOfSimbolsAfterComma)
        {
            if (_iterationDefinitionsCache.ContainsKey(formula))
            {
                IterationDefinition iteration = _iterationDefinitionsCache[formula]
                    .FirstOrDefault(definition => definition.CheckEquality(iterationDefinition));
                if (iteration != null)
                {
                    return iteration.Result;
                }
            }

            Expression expression = new Expression(formula);
            List<Argument> arguments = new List<Argument>();
            for (int i = 0; i < iterationDefinition.ArgumentValues.Count; i++)
            {
                arguments.Add(new Argument(iterationDefinition.ArgumentNames[i],
                    iterationDefinition.ArgumentValues[i]));
            }

            expression.addArguments(arguments.ToArray());
            double result = expression.calculate();
            if (Double.IsNaN(result)) throw new ArgumentException();

            result = Math.Round(result, numberOfSimbolsAfterComma);

            iterationDefinition.Result = result;
            if (_iterationDefinitionsCache.ContainsKey(formula))
            {
                _iterationDefinitionsCache[formula].Add(iterationDefinition);
            }
            else
            {
                _iterationDefinitionsCache.TryAdd(formula, new ConcurrentBag<IterationDefinition>() {iterationDefinition});
            }

            return result;
        }



        public IFormattedValue VisitString1251Formatter(IUshortsFormatter formatter)
        {
            IStringValue formattedValue = _typesContainer.Resolve<IStringValue>();
            byte[] bytes = new byte[_ushortsPayload.Length * 2];
            Buffer.BlockCopy(_ushortsPayload, 0, bytes, 0, _ushortsPayload.Length * 2);
            string formattedString = Encoding.Default.GetString(bytes);
            formattedValue.StrValue = formattedString;
            return formattedValue;
        }

        public IFormattedValue VisitUshortToIntegerFormatter(IUshortsFormatter formatter)
        {
            if (_ushortsPayload.Length != 2) throw new ArgumentException("Number of words must be equal 2");
            INumericValue numValue = _typesContainer.Resolve<INumericValue>();
            numValue.NumValue = _ushortsPayload.GetIntFromTwoUshorts();
            return numValue;
        }

        public IFormattedValue VisitDictionaryMatchFormatter(IUshortsFormatter formatter)
        {
            IChosenFromListValue chosenFromListValue = _typesContainer.Resolve<IChosenFromListValue>();
            IDictionaryMatchingFormatter dictionaryMatchingFormatter = formatter as IDictionaryMatchingFormatter;

            chosenFromListValue.InitList(dictionaryMatchingFormatter.StringDictionary.Values);

            if (!dictionaryMatchingFormatter.StringDictionary.Any((pair => pair.Key == _ushortsPayload[0]))) throw new ArgumentException();
            if (dictionaryMatchingFormatter.IsKeysAreNumbersOfBits)
            {
                BitArray bitArray = new BitArray(new int[] { _ushortsPayload[0] });
                bool isValueExists = false;
                for (ushort i = 0; i < bitArray.Length; i++)
                {
                    if ((bitArray[i]) && (dictionaryMatchingFormatter.StringDictionary.ContainsKey(i)))
                    {
                        chosenFromListValue.SelectedItem = dictionaryMatchingFormatter.StringDictionary[i];
                        isValueExists = true;
                    }
                }
                if ((!isValueExists) && (dictionaryMatchingFormatter.StringDictionary.ContainsKey(0)))
                {
                    chosenFromListValue.SelectedItem = dictionaryMatchingFormatter.StringDictionary[0];
                }
            }
            else
            {
                chosenFromListValue.SelectedItem = dictionaryMatchingFormatter.StringDictionary[_ushortsPayload[0]];
            }
            return chosenFromListValue;
        }

        public IFormattedValue VisitBitMaskFormatter(IUshortsFormatter formatter)
        {
            IBitMaskFormatter bitMaskFormatter = formatter as IBitMaskFormatter;
            IBitMaskValue bitMaskValue = _typesContainer.Resolve<IBitMaskValue>();
            foreach (ushort uUshort in _ushortsPayload)
            {
                List<bool> bools = new List<bool>();
                BitArray bitArray = new BitArray(new[] {(int) uUshort});
                foreach (bool o in bitArray)
                {
                    bools.Add(o);
                }

                bitMaskValue.BitArray.Add(bools.Take(16).Reverse().ToList());
            }

            bitMaskValue.BitSignatures.AddRange(bitMaskFormatter.BitSignatures);
            return bitMaskValue;
        }
    }
}