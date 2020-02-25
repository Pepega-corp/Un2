using System;
using System.Runtime.Serialization;
using System.Text;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Model
{

    [DataContract(Namespace = "StringFormatter1251NS", IsReference = true)]
    public class StringFormatter1251 : UshortsFormatterBase
    {
        private Func<IStringValue> _stringValueGettingFunc;
        public override string StrongName => nameof(StringFormatter1251);

        protected override IFormattedValue OnFormatting(ushort[] ushorts)
        {
            IStringValue formattedValue = this._stringValueGettingFunc();
            byte[] bytes = new byte[ushorts.Length * 2];
            Buffer.BlockCopy(ushorts, 0, bytes, 0, ushorts.Length * 2);
            string formattedString = Encoding.Default.GetString(bytes);
            formattedValue.StrValue = formattedString;
            return formattedValue;
        }

        public override ushort[] FormatBack(IFormattedValue formattedValue)
        {
            throw new NotImplementedException();
        }

        public override object Clone()
        {
            return new StringFormatter1251();
        }


        public override void InitializeFromContainer(ITypesContainer container)
        {
            this._stringValueGettingFunc = container.Resolve(typeof(Func<IStringValue>)) as Func<IStringValue>;
            base.InitializeFromContainer(container);
        }
    }
}