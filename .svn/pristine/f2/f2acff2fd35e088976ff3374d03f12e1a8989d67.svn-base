using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Infrastructure.Model;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Measuring.Model
{
    [DataContract(Namespace = "MeasuringGroupNS")]
    public class MeasuringGroup : IMeasuringGroup
    {
        private Func<IRange> _rangesGettingFunc;

        public MeasuringGroup(Func<IRange> rangesGettingFunc)
        {
            this._rangesGettingFunc = rangesGettingFunc;
            this.MeasuringElements = new List<IMeasuringElement>();
        }

        #region Implementation of IDataProviderContaining

        public void SetDataProvider(IDataProvider dataProvider)
        {
            foreach (IMeasuringElement measuringElement in this.MeasuringElements)
            {
                if (measuringElement is IDataProviderContaining)
                {
                    (measuringElement as IDataProviderContaining).SetDataProvider(dataProvider);
                }
            }
        }


        public List<IRange> GetAddressesRanges()
        {
            List<IRange> ranges = new List<IRange>();
            foreach (IMeasuringElement measuringElement in this.MeasuringElements)
            {
                if (measuringElement is IAddressableItem)
                {
                    IRange range = this._rangesGettingFunc();
                    range.RangeFrom = (measuringElement as IAddressableItem).Address;
                    range.RangeTo = range.RangeFrom + (measuringElement as IAddressableItem).NumberOfPoints;
                    ranges.Add(range);
                }
            }
            return ranges;
        }

        public async Task Load()
        {
            if (this.MeasuringElements.Any((element => !(element is ILoadable))))
            {
                await Task.Delay(10);
                return;
            }
            for (int i = 0; i < this.MeasuringElements.Count; i++)
            {
                if (this.MeasuringElements[i] is ILoadable)
                {
                    await (this.MeasuringElements[i] as ILoadable).Load();
                }
            }

        }

        #endregion

        #region Implementation of INameable
        [DataMember]
        public string Name { get; set; }

        #endregion

        #region Implementation of IMeasuringGroup
        [DataMember]
        public List<IMeasuringElement> MeasuringElements { get; set; }

        #endregion

        #region Implementation of IInitializableFromContainer

        public bool IsInitialized { get; private set; }

        public void InitializeFromContainer(ITypesContainer container)
        {
            if (this.IsInitialized) return;
            foreach (IMeasuringElement measuringElement in this.MeasuringElements)
            {
                (measuringElement as IInitializableFromContainer)?.InitializeFromContainer(container);
            }
            this._rangesGettingFunc = container.Resolve<Func<IRange>>();
            this.IsInitialized = true;
        }

        #endregion
    }
}
