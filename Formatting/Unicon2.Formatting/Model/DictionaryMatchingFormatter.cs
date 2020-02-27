using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Model
{
    [DataContract(Namespace = "DictionaryMatchingFormatterNS", IsReference = true)]
    public class DictionaryMatchingFormatter : UshortsFormatterBase, IDictionaryMatchingFormatter
    {
        private Func<IChosenFromListValue> _chosenFromListValueGettingFunc;
        private ITypesContainer _container;

        public DictionaryMatchingFormatter(ITypesContainer container)
        {
            this._container = container;
            this.StringDictionary = new Dictionary<ushort, string>();
        }

        public override string StrongName => nameof(DictionaryMatchingFormatter);

        protected override IFormattedValue OnFormatting(ushort[] ushorts)
        {
            IChosenFromListValue chosenFromListValue = this._chosenFromListValueGettingFunc();
            chosenFromListValue.InitList(this.StringDictionary.Values);


            if (!this.StringDictionary.Any((pair => pair.Key == ushorts[0]))) throw new ArgumentException();
            if (this.IsKeysAreNumbersOfBits)
            {
                BitArray bitArray = new BitArray(new int[] { ushorts[0] });
                bool isValueExists = false;
                for (ushort i = 0; i < bitArray.Length; i++)
                {
                    if ((bitArray[i]) && (this.StringDictionary.ContainsKey(i)))
                    {
                        chosenFromListValue.SelectedItem = this.StringDictionary[i];
                        isValueExists = true;
                    }
                }
                if ((!isValueExists) && (this.StringDictionary.ContainsKey(0)))
                {
                    chosenFromListValue.SelectedItem = this.StringDictionary[0];
                }
            }
            else
            {
                chosenFromListValue.SelectedItem = this.StringDictionary[ushorts[0]];
            }
            return chosenFromListValue;
        }

        public override ushort[] FormatBack(IFormattedValue formattedValue)
        {
            IChosenFromListValue chosenFromListValue = formattedValue as IChosenFromListValue;
            ushort[] resultedUshorts = new ushort[1];
            if (this.IsKeysAreNumbersOfBits)
            {
                BitArray bitArray = new BitArray(16);
                ushort numberKey = this.StringDictionary.First((pair => pair.Value == chosenFromListValue.SelectedItem)).Key;
                bitArray[numberKey] = true;
                resultedUshorts[0] = (ushort)bitArray.GetIntFromBitArray();
            }
            else
            {
                resultedUshorts[0] = this.StringDictionary.First((pair => pair.Value == chosenFromListValue.SelectedItem))
                    .Key;
            }
            return resultedUshorts;
        }

        public override object Clone()
        {
            DictionaryMatchingFormatter cloneDictionaryMatchingFormatter = new DictionaryMatchingFormatter(this._container);
            cloneDictionaryMatchingFormatter.StringDictionary = new Dictionary<ushort, string>(this.StringDictionary);
            cloneDictionaryMatchingFormatter.InitializeFromContainer(this._container);
            cloneDictionaryMatchingFormatter.IsKeysAreNumbersOfBits = this.IsKeysAreNumbersOfBits;
            return cloneDictionaryMatchingFormatter;
        }

        [DataMember]
        public Dictionary<ushort, string> StringDictionary { get; set; }
        [DataMember]
        public bool IsKeysAreNumbersOfBits { get; set; }

        public override void InitializeFromContainer(ITypesContainer container)
        {
            this._container = container;
            this._chosenFromListValueGettingFunc =
                this._container.Resolve(typeof(Func<IChosenFromListValue>)) as Func<IChosenFromListValue>;
            base.InitializeFromContainer(container);
        }
    }
}