using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
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

        public SubProperty()
        {
            BitNumbersInWord = new List<int>();
        }


        private ushort[] GetUshortValueFromParentsValue(ushort[] parentUshorts)
        {
            bool[] bools = new bool[BitNumbersInWord.Count];
            BitArray bitArray = new BitArray(new int[] { parentUshorts[0] });
            int index = 0;
            foreach (int bitNum in BitNumbersInWord)
            {
                bools[index] = bitArray[bitNum];
                index++;
            }
            return new[] { (ushort)(new BitArray(bools).GetIntFromBitArray()) };
        }

        [JsonProperty]
        public List<int> BitNumbersInWord { get; set; }

        
        protected override IConfigurationItem OnCloning()
        {
            SubProperty subProperty = new SubProperty
            {
                UshortsFormatter = UshortsFormatter,
                Address = Address,
                NumberOfPoints = NumberOfPoints,
                MeasureUnit = MeasureUnit,
                IsMeasureUnitEnabled = IsMeasureUnitEnabled,
                Range = Range.Clone() as IRange,
                IsRangeEnabled = IsRangeEnabled
            };
            BitNumbersInWord.ForEach((i => subProperty.BitNumbersInWord.Add(i)));
            return subProperty;
        }
        public override T Accept<T>(IConfigurationItemVisitor<T> visitor)
        {
            return visitor.VisitSubProperty(this);
        }
    }
}
