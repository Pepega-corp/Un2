using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Model
{
    [DataContract(IsReference = true, Namespace = "DefaultBitMaskFormatterNS")]
    public class DefaultBitMaskFormatter : UshortsFormatterBase, IBitMaskFormatter
    {
        private Func<IBitMaskValue> _bitMaskValueGettingFunc;

        public DefaultBitMaskFormatter(Func<IBitMaskValue> bitMaskValueGettingFunc)
        {
            this._bitMaskValueGettingFunc = bitMaskValueGettingFunc;
        }

        #region Overrides of UshortsFormatterBase

        public override ushort[] FormatBack(IFormattedValue formattedValue)
        {
            throw new NotImplementedException();
        }

        protected override IFormattedValue OnFormatting(ushort[] ushorts)
        {
            IBitMaskValue bitMaskValue = this._bitMaskValueGettingFunc();
            foreach (ushort uUshort in ushorts)
            {
                List<bool> bools = new List<bool>();
                BitArray bitArray = new BitArray(new[] { (int)uUshort });
                foreach (bool o in bitArray)
                {
                    bools.Add(o);
                }
                bitMaskValue.BitArray.Add(bools.Take(16).ToList());
            }
            bitMaskValue.BitSignatures.AddRange(this.BitSignatures);
            return bitMaskValue;
        }

        public override string StrongName => StringKeys.DEFAULT_BIT_MASK_FORMATTER;
        public override object Clone()
        {
            DefaultBitMaskFormatter clone = new DefaultBitMaskFormatter(this._bitMaskValueGettingFunc);
            clone.BitSignatures = new List<string>(this.BitSignatures);
            return clone;
        }

        #endregion

        #region Implementation of IBitMaskFormatter
        [DataMember]
        public string BitSignaturesInOneLine { get; set; }

        public List<string> BitSignatures { get; set; }

        #endregion


        #region Overrides of UshortsFormatterBase

        public override void InitializeFromContainer(ITypesContainer container)
        {
            this._bitMaskValueGettingFunc = container.Resolve<Func<IBitMaskValue>>();
            base.InitializeFromContainer(container);
        }

        #endregion

        [OnDeserialized]
        private void OnDeserialized(StreamingContext sc)
        {
            if (this.BitSignaturesInOneLine == null) return;
            string[] bitSignatures = this.BitSignaturesInOneLine.Split(',');
            this.BitSignatures = new List<string>();
            foreach (string bitSignature in bitSignatures)
            {
                string preparedString = bitSignature.Replace("&lt", "<").Replace("&gt", ">");
                if (preparedString.Contains("\""))
                {
                    preparedString = preparedString.Remove(preparedString.LastIndexOf("\""));
                    preparedString = preparedString.Remove(0, preparedString.IndexOf("\""));


                    this.BitSignatures.Add(preparedString.Replace("\"", ""));
                }
            }
        }


        [OnSerializing]
        private void OnSerializing(StreamingContext sc)
        {

            StringBuilder sb = new StringBuilder();
            this.BitSignatures.ForEach((s =>
            {
                sb.Append("\"");
                string preparedString = s.Replace("<", "&lt").Replace(">", "&gt");
                sb.Append(preparedString);
                sb.Append("\"");
                if (this.BitSignatures.IndexOf(s) != this.BitSignatures.Count - 1)
                {
                    sb.Append(",");
                }
            }));
            this.BitSignaturesInOneLine = sb.ToString();
        }

    }





}
