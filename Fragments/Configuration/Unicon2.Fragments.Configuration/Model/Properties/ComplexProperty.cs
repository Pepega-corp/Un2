using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Configuration.Model.Properties
{
    [DataContract(Namespace = "ComplexPropertyNS", Name = nameof(ComplexProperty), IsReference = true)]
    public class ComplexProperty : DefaultProperty, IComplexProperty
    {

        private bool[] _baseBools;
        public ComplexProperty(Func<IRange> range) : base(range)
        {
            this.SubProperties = new List<ISubProperty>();
        }

        [DataMember]
        public List<ISubProperty> SubProperties { get; set; }

        [DataMember]
        public bool IsGroupedProperty { get; set; }


        public override string StrongName => ConfigurationKeys.COMPLEX_PROPERTY;



        public override void InitializeLocalValue(IConfigurationItem localConfigurationItem)
        {
            foreach (ISubProperty subProperty in this.SubProperties)
            {
                subProperty.LocalUshortsValue = subProperty.DeviceUshortsValue;
                subProperty.ConfigurationItemChangedAction?.Invoke();
            }
            base.InitializeLocalValue(localConfigurationItem);
        }
        public override void InitializeValue(IConfigurationItem localConfigurationItem)
        {
            foreach (ISubProperty subProperty in this.SubProperties)
            {
                subProperty.LocalUshortsValue = new ushort[] { 0 };
                subProperty.InitEditableValueAction?.Invoke();
            }
            base.InitializeValue(localConfigurationItem);
        }


        public override async Task<bool> Write()
        {
            bool isToWrite = false;
            if (this.LocalUshortsValue == null) return false;
            foreach (ISubProperty subProperty in this.SubProperties)
            {
                if (!subProperty.IsValuesEqual)
                {

                    isToWrite = true;
                }
            }
            if (isToWrite)
            {
                this.LocalUshortsValue = this.GetParentLocalUshortsFromChildren(this.SubProperties);
                if (this._dataProvider is IQuickMemoryAccessDataProviderStub)
                {
                    List<int> bitNumbers = new List<int>();
                    this.SubProperties.ForEach((property => bitNumbers.AddRange(property.BitNumbersInWord)));
                    await (this._dataProvider as IQuickMemoryAccessDataProviderStub).WriteMultipleRegistersByBitNumbersAsync(
                        this.Address, this.LocalUshortsValue, "WritingComplexProperty", bitNumbers);
                }
                else
                {
                   return await base.Write();
                }
            }
            return isToWrite;
        }



        private ushort[] GetParentLocalUshortsFromChildren(List<ISubProperty> subProperties)
        {
            foreach (ISubProperty subProperty in subProperties)
            {
                BitArray subPropertyBitArray = new BitArray(new int[] { subProperty.LocalUshortsValue[0] });
                foreach (int bitNum in subProperty.BitNumbersInWord)
                {
                    this._baseBools[bitNum] = subPropertyBitArray[subProperty.BitNumbersInWord.IndexOf(bitNum)];
                }
            }
            return new[] { (ushort)(new BitArray(this._baseBools).GetIntFromBitArray()) };
        }



        public override async Task Load()
        {
            await base.Load();
            this._baseBools = this.DeviceUshortsValue?.GetBoolArrayFromUshortArray();
        }


        protected override IConfigurationItem OnCloning()
        {
            ComplexProperty complexProperty = new ComplexProperty(this._rangeGetFunc);
            this.SubProperties.ForEach((property =>
            {
                ISubProperty subProperty = property.Clone() as ISubProperty;
                subProperty.SetParent(complexProperty);
                complexProperty.SubProperties.Add(subProperty);

            }));
            complexProperty.UshortsFormatter = this.UshortsFormatter;
            complexProperty.Address = this.Address;
            complexProperty.NumberOfPoints = this.NumberOfPoints;
            complexProperty.MeasureUnit = this.MeasureUnit;
            complexProperty.IsMeasureUnitEnabled = this.IsMeasureUnitEnabled;
            complexProperty.Range = this.Range.Clone() as IRange;
            complexProperty.IsRangeEnabled = this.IsRangeEnabled;
            return complexProperty;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext sc)
        {
            foreach (ISubProperty subProperty in this.SubProperties)
            {
                subProperty.SetParent(this);
                subProperty.LocalValueChanged += () => this.ChangeLocalUshortValue(subProperty.BitNumbersInWord, subProperty.LocalUshortsValue);
            }
            this._baseBools = new bool[16];
        }

        private void ChangeLocalUshortValue(List<int> subPropertyBitNumbersInWord, ushort[] subPropertyLocalUshortsValue)
        {
            if (this.LocalUshortsValue == null) return;
            if (subPropertyLocalUshortsValue == null) return;
            BitArray bitArrayParent = new BitArray(new int[] { this.LocalUshortsValue[0] });
            BitArray bitArraySub = new BitArray(new int[] { subPropertyLocalUshortsValue[0] });

            foreach (int bitNum in subPropertyBitNumbersInWord)
            {
                bitArrayParent[bitNum] = bitArraySub[subPropertyBitNumbersInWord.IndexOf(bitNum)];
            }
            this.LocalUshortsValue = new[] { (ushort)bitArrayParent.GetIntFromBitArray() };
        }
    }
}
