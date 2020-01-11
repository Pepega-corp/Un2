using Unicon2.Fragments.Measuring.Infrastructure.Factories;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Measuring.Factories
{
    public class MeasuringElementFactory : IMeasuringElementFactory
    {
        private readonly ITypesContainer _container;

        public MeasuringElementFactory(ITypesContainer container)
        {
            this._container = container;
        }

        public IMeasuringElement CreateAnalogMeasuringElement()
        {
            return this._container.Resolve<IMeasuringElement>(MeasuringKeys.ANALOG_MEASURING_ELEMENT);
        }

        public IMeasuringElement CreateDiscretMeasuringElement()
        {
            return this._container.Resolve<IMeasuringElement>(MeasuringKeys.DISCRET_MEASURING_ELEMENT);
        }

        public IMeasuringElement CreateControlSignal()
        {
            return this._container.Resolve<IMeasuringElement>(MeasuringKeys.CONTROL_SIGNAL);
        }
    }
}
