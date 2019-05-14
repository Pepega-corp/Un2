using System;
using System.Runtime.Serialization;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Model
{
    [DataContract(IsReference = true, Namespace = "UshortToIntegerFormatterNS")]
    public class UshortToIntegerFormatter : UshortsFormatterBase
    {
        private Func<INumericValue> _numValueGettingFunc;

        public UshortToIntegerFormatter(Func<INumericValue> numValueGettingFunc)
        {
            this._numValueGettingFunc = numValueGettingFunc;
        }

        #region Overrides of UshortsFormatterBase

        public override ushort[] FormatBack(IFormattedValue formattedValue)
        {
            throw new System.NotImplementedException();
        }

        protected override IFormattedValue OnFormatting(ushort[] ushorts)
        {
            if (ushorts.Length != 2) throw new ArgumentException("Number of words must be equal 2");
            INumericValue numValue = this._numValueGettingFunc();
            numValue.NumValue = ushorts.GetIntFromTwoUshorts();
            return numValue;
        }

        public override string StrongName => nameof(UshortToIntegerFormatter);
        public override object Clone()
        {
            throw new System.NotImplementedException();
        }

        #endregion


        #region Overrides of UshortsFormatterBase

        public override void InitializeFromContainer(ITypesContainer container)
        {
            this._numValueGettingFunc = container.Resolve<Func<INumericValue>>();
            base.InitializeFromContainer(container);
        }

        #endregion
    }
}