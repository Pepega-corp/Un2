using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using org.mariuszgromada.math.mxparser;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Infrastructure.Services;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Visitors;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Formatting.Visitors
{
    public class FormatterFormatBackVisitor : IFormatterVisitor<Task<ushort[]>>
    {
        private readonly IFormattedValue _formattedValue;
        private readonly FormattingContext _formattingContext;

        public FormatterFormatBackVisitor(IFormattedValue formattedValue, FormattingContext formattingContext)
        {
            _formattedValue = formattedValue;
            _formattingContext = formattingContext;
        }

        public async Task<ushort[]> VisitBoolFormatter(IUshortsFormatter boolFormatter)
        {
            if (_formattedValue is IBoolValue)
            {
                return ((IBoolValue) _formattedValue).BoolValueProperty ? new[] {(ushort) 1} : new[] {(ushort) 0};
            }

            throw new ArgumentException();
        }

        public async Task<ushort[]> VisitAsciiStringFormatter(IUshortsFormatter boolFormatter)
        {
            throw new NotImplementedException();
        }

        public async Task<ushort[]> VisitTimeFormatter(IUshortsFormatter boolFormatter)
        {
            throw new NotImplementedException();
        }

        public async Task<ushort[]> VisitDirectUshortFormatter(IUshortsFormatter boolFormatter)
        {
            if (_formattedValue is INumericValue)
            {
                if ((_formattedValue as INumericValue).NumValue % 1 != 0) throw new ArgumentException();
                return new[] {(ushort) (_formattedValue as INumericValue).NumValue};
            }

            throw new ArgumentException();
        }

        public async Task<ushort[]> VisitFormulaFormatter(IUshortsFormatter formatter)
        {
            try
            {
                IFormulaFormatter formulaFormatter = formatter as IFormulaFormatter;

                (_formattedValue as INumericValue).NumValue = Math.Round((_formattedValue as INumericValue).NumValue,
                    formulaFormatter.NumberOfSimbolsAfterComma);
                string numstr = (_formattedValue as INumericValue).NumValue.ToString().Replace(',', '.');
                Expression expression = new Expression("solve(" + "(" + formulaFormatter.FormulaString + ")" + "-" +
                                                       numstr +
                                                       ",x,0," + ushort.MaxValue + ")");

                if (formulaFormatter.UshortFormattableResources != null)
                {
                    //  int index = 1;
                    //   foreach (IUshortFormattable formattableUshortResource in formulaFormatter.UshortFormattableResources)
                    //    {
                    // if (formattableUshortResource is IDeviceValueContaining)
                    //{
                    //    IFormattedValue value =
                    //       formattableUshortResource.UshortsFormatter.Format(
                    //           (formattableUshortResource as IDeviceValueContaining).DeviceUshortsValue);
                    //   if (value is INumericValue)
                    //   {
                    //       double num = (value as INumericValue).NumValue;
                    //       expression.addArguments(new Argument("x" + index++, num));
                    //  }
                    //}
                    //   }
                }

                double x = expression.calculate();
                if (double.IsNaN(x)) throw new ArgumentException();

                return new[] {(ushort) x};

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ushort[]> VisitString1251Formatter(IUshortsFormatter boolFormatter)
        {
            throw new NotImplementedException();
        }

        public async Task<ushort[]> VisitUshortToIntegerFormatter(IUshortsFormatter boolFormatter)
        {
            throw new NotImplementedException();
        }

        public async Task<ushort[]> VisitDictionaryMatchFormatter(IUshortsFormatter formatter)
        {
            IChosenFromListValue chosenFromListValue = _formattedValue as IChosenFromListValue;
            IDictionaryMatchingFormatter dictionaryMatchingFormatter = formatter as IDictionaryMatchingFormatter;

            ushort[] resultedUshorts = new ushort[1];
            if (dictionaryMatchingFormatter.IsKeysAreNumbersOfBits)
            {
                BitArray bitArray = new BitArray(16);
                ushort numberKey = dictionaryMatchingFormatter.StringDictionary
                    .First((pair => pair.Value == chosenFromListValue.SelectedItem)).Key;
                bitArray[numberKey] = true;
                resultedUshorts[0] = (ushort) bitArray.GetIntFromBitArray();
            }
            else
            {
                resultedUshorts[0] = dictionaryMatchingFormatter.StringDictionary
                    .First((pair => pair.Value == chosenFromListValue.SelectedItem))
                    .Key;
            }

            return resultedUshorts;
        }

        public async Task<ushort[]> VisitBitMaskFormatter(IUshortsFormatter boolFormatter)
        {
            throw new NotImplementedException();
        }

        public async Task<ushort[]> VisitMatrixFormatter(IUshortsFormatter formatter)
        {
            throw new NotImplementedException();
        }

        public async Task<ushort[]> VisitCodeFormatter(IUshortsFormatter formatter)
        {
            var service = StaticContainer.Container.Resolve<ICodeFormatterService>();
            var codeFormatter = formatter as ICodeFormatter;
            var fun = service.GetFormatBackUshortsFunc(codeFormatter.CodeExpression, _formattingContext.DeviceContext,
                _formattingContext.IsLocal,_formattingContext.ValueOwner);
            var res = await fun.Item.Invoke(_formattedValue);
            return res;
        }
    }
}