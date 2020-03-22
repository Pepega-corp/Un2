using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Measuring.Infrastructure.Factories
{
    public interface IMeasuringElementFactory
    {
	    IAnalogMeasuringElement CreateAnalogMeasuringElement();
        IDiscretMeasuringElement CreateDiscretMeasuringElement();
        IControlSignal CreateControlSignal();

    }
}