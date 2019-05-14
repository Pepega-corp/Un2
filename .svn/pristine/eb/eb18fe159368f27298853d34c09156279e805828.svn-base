using System;
using System.Runtime.Serialization;
using System.Text;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Model
{
    [DataContract(Namespace = "AsciiStringFormatterNS", IsReference = true)]
    public class AsciiStringFormatter : UshortsFormatterBase
    {
        private Func<IStringValue> _stringValueGettingFunc;
        public override string StrongName => nameof(AsciiStringFormatter);

        protected override IFormattedValue OnFormatting(ushort[] ushorts)
        {
            if (this._stringValueGettingFunc == null)
            {
                this.InitializeFromContainer(StaticContainer.Container);
            }
            IStringValue formattedValue = this._stringValueGettingFunc();
            byte[] bytes = new byte[ushorts.Length * 2];
            Buffer.BlockCopy(ushorts, 0, bytes, 0, ushorts.Length * 2);
            string formattedString = Encoding.ASCII.GetString(bytes);

            formattedValue.UshortsValue = ushorts;
            formattedValue.StrValue = formattedString;
            return formattedValue;
        }

        public override ushort[] FormatBack(IFormattedValue formattedValue)
        {
            throw new NotImplementedException();
        }

        public override object Clone()
        {
            return new AsciiStringFormatter();
        }


        #region Implementation of IInitializableFromContainer


        public override void InitializeFromContainer(ITypesContainer container)
        {
            this._stringValueGettingFunc = container.Resolve(typeof(Func<IStringValue>)) as Func<IStringValue>;
            base.InitializeFromContainer(container);
        }

        #endregion
    }
}
