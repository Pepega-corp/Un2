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
            _container = container;
        }

        public IAnalogMeasuringElement CreateAnalogMeasuringElement()
        {
            return _container.Resolve<IAnalogMeasuringElement>();
        }

        public IDiscretMeasuringElement CreateDiscretMeasuringElement()
        {
            return _container.Resolve<IDiscretMeasuringElement>();
        }

        public IControlSignal CreateControlSignal()
        {
            return _container.Resolve<IControlSignal>();
        }

        public IDateTimeMeasuringElement CreateDateTimeMeasuringElement()
        {
            return _container.Resolve<IDateTimeMeasuringElement>();
        }
    }
}
