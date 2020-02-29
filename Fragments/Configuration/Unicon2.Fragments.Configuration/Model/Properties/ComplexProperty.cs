using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Configuration.Model.Properties
{
    [DataContract(Namespace = "ComplexPropertyNS", Name = nameof(ComplexProperty), IsReference = true)]
    public class ComplexProperty : DefaultProperty, IComplexProperty
    {

        private bool[] _baseBools;
        public ComplexProperty()
        {
            SubProperties = new List<ISubProperty>();
        }

        [DataMember]
        public List<ISubProperty> SubProperties { get; set; }

        [DataMember]
        public bool IsGroupedProperty { get; set; }


        //public override async Task<bool> Write()
        //{
        //    bool isToWrite = false;
        //    if (this.LocalUshortsValue == null) return false;
        //    foreach (ISubProperty subProperty in SubProperties)
        //    {
        //        if (!subProperty.IsValuesEqual)
        //        {

        //            isToWrite = true;
        //        }
        //    }
        //    if (isToWrite)
        //    {
        //        this.LocalUshortsValue = GetParentLocalUshortsFromChildren(SubProperties);
        //        if (_dataProvider is IQuickMemoryAccessDataProviderStub)
        //        {
        //            List<int> bitNumbers = new List<int>();
        //            SubProperties.ForEach((property => bitNumbers.AddRange(property.BitNumbersInWord)));
        //            await (_dataProvider as IQuickMemoryAccessDataProviderStub).WriteMultipleRegistersByBitNumbersAsync(
        //                Address, this.LocalUshortsValue, "WritingComplexProperty", bitNumbers);
        //        }
        //        else
        //        {
        //           return await base.Write();
        //        }
        //    }
        //    return isToWrite;
        //}

        // IQueryResult queryResult = this._queryResultFactory.CreateDefaultQueryResult();
        //if (this.IsMemoryValuesSetsContainsAddress(startAddress))
        //{
        //    bool[] bitArrayToWrite = dataToWrite.GetBoolArrayFromUshortArray();
        //    bool[] bitArrayExisting =
        //        (new[] { this.GetValueFromMemoryValuesSets(startAddress) }).GetBoolArrayFromUshortArray();
        //    for (int i = 0; i < bitArrayToWrite.Length; i++)
        //    {
        //        if (bitNumbers.Contains(i))
        //        {
        //            bitArrayExisting[i] = bitArrayToWrite[i];
        //        }
        //    }
        //    this.SetValueFromMemoryValuesSets(startAddress, bitArrayExisting.BoolArrayToUshort());
        //}

        //queryResult.IsSuccessful = true;
        //return queryResult;

        //private ushort[] GetParentLocalUshortsFromChildren(List<ISubProperty> subProperties)
        //{
        //    foreach (ISubProperty subProperty in subProperties)
        //    {
        //        BitArray subPropertyBitArray = new BitArray(new int[] { subProperty.LocalUshortsValue[0] });
        //        foreach (int bitNum in subProperty.BitNumbersInWord)
        //        {
        //            _baseBools[bitNum] = subPropertyBitArray[subProperty.BitNumbersInWord.IndexOf(bitNum)];
        //        }
        //    }
        //    return new[] { (ushort)(new BitArray(_baseBools).GetIntFromBitArray()) };
        //}


        protected override IConfigurationItem OnCloning()
        {
            ComplexProperty complexProperty = new ComplexProperty();
            SubProperties.ForEach((property =>
            {
                ISubProperty subProperty = property.Clone() as ISubProperty;
                subProperty.SetParent(complexProperty);
                complexProperty.SubProperties.Add(subProperty);

            }));
            complexProperty.UshortsFormatter = UshortsFormatter;
            complexProperty.Address = Address;
            complexProperty.NumberOfPoints = NumberOfPoints;
            complexProperty.MeasureUnit = MeasureUnit;
            complexProperty.IsMeasureUnitEnabled = IsMeasureUnitEnabled;
            complexProperty.Range = Range.Clone() as IRange;
            complexProperty.IsRangeEnabled = IsRangeEnabled;
            return complexProperty;
        }

        public override T Accept<T>(IConfigurationItemVisitor<T> visitor)
        {
            return visitor.VisitComplexProperty(this);
        }

    }
}
