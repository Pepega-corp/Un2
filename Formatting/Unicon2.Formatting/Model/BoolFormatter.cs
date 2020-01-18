using System;
using System.Runtime.Serialization;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Model
{
    [DataContract(Name = nameof(BoolFormatter), Namespace = "BoolFormatterNS", IsReference = true)]

    public class BoolFormatter : UshortsFormatterBase
    {
        private ITypesContainer _container;

        public BoolFormatter(ITypesContainer container)
        {
            this._container = container;
        }


        protected override IFormattedValue OnFormatting(ushort[] ushorts)
        {
            IBoolValue val = this._container.Resolve<IBoolValue>();
            if (ushorts[0] == 0)
            {
                val.BoolValueProperty = false;
            }
            else
            {
                val.BoolValueProperty = true;
            }
            return val;
        }

        public override ushort[] FormatBack(IFormattedValue formattedValue)
        {
            if (formattedValue is IBoolValue)
            {
                return ((IBoolValue) formattedValue).BoolValueProperty ? new[] { (ushort)1 } : new[] { (ushort)0 };
            }
            throw new ArgumentException();
        }

        public override object Clone()
        {
            return new BoolFormatter(this._container);
        }

        public override void InitializeFromContainer(ITypesContainer container)
        {
            this._container = container;
            base.InitializeFromContainer(container);
        }

        public override string StrongName => StringKeys.BOOL_FORMATTER;
    }
}
