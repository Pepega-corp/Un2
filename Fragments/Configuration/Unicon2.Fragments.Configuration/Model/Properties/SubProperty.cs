using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Configuration.Model.Properties
{
    [JsonObject(MemberSerialization.OptIn)]

    public class SubProperty : DefaultProperty, ISubProperty
    {
        private IComplexProperty _complexProperty;

        public SubProperty()
        {
            this.BitNumbersInWord = new List<int>();
        }


        public void SetParent(IComplexProperty complexProperty)
        {
            this._complexProperty = complexProperty;
        }

        private ushort[] GetUshortValueFromParentsValue(ushort[] parentUshorts)
        {
            bool[] bools = new bool[this.BitNumbersInWord.Count];
            BitArray bitArray = new BitArray(new int[] { parentUshorts[0] });
            int index = 0;
            foreach (int bitNum in this.BitNumbersInWord)
            {
                bools[index] = bitArray[bitNum];
                index++;
            }
            return new[] { (ushort)(new BitArray(bools).GetIntFromBitArray()) };
        }

        [JsonProperty]
        public List<int> BitNumbersInWord { get; set; }

        public Action LocalValueChanged { get; set; }
        
        protected override IConfigurationItem OnCloning()
        {
            SubProperty subProperty = new SubProperty
            {
                UshortsFormatter = this.UshortsFormatter,
                Address = this.Address,
                NumberOfPoints = this.NumberOfPoints,
                MeasureUnit = this.MeasureUnit,
                IsMeasureUnitEnabled = this.IsMeasureUnitEnabled,
                Range = this.Range.Clone() as IRange,
                IsRangeEnabled = this.IsRangeEnabled
            };
            this.BitNumbersInWord.ForEach((i => subProperty.BitNumbersInWord.Add(i)));
            return subProperty;
        }
        public override T Accept<T>(IConfigurationItemVisitor<T> visitor)
        {
            return visitor.VisitSubProperty(this);
        }
    }
}
