using System;
using System.Runtime.Serialization;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Model
{
    [DataContract(Namespace = "DirectUshortFormatterNS", IsReference = true)]
    public class DirectUshortFormatter : UshortsFormatterBase
    {
        private Func<INumericValue> _numericValueGettingFunc;
        public override string StrongName => nameof(DirectUshortFormatter);
        protected override IFormattedValue OnFormatting(ushort[] ushorts)
        {
            INumericValue numericValue = this._numericValueGettingFunc();
            numericValue.NumValue = ushorts[0];
            return numericValue;
        }

        public override ushort[] FormatBack(IFormattedValue formattedValue)
        {
            if (formattedValue is INumericValue)
            {
                if ((formattedValue as INumericValue).NumValue % 1 != 0) throw new ArgumentException();
                return new ushort[] { (ushort)(formattedValue as INumericValue).NumValue };
            }
            throw new ArgumentException();
        }

        public override object Clone()
        {
            return new DirectUshortFormatter();
        }

        public override void InitializeFromContainer(ITypesContainer container)
        {
            this._numericValueGettingFunc = container.Resolve(typeof(Func<INumericValue>)) as Func<INumericValue>;
            base.InitializeFromContainer(container);
        }
    }
}
