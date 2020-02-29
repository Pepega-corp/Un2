using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Progress;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Values;
using Unicon2.Model.Connection;
using Unicon2.Model.DefaultDevice;
using Unicon2.Model.FragmentSettings;
using Unicon2.Model.Progress;
using Unicon2.Model.Values;
using Unicon2.Model.Values.Range;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Model.Module
{
    public class UniconModelModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register<IConnectionState, DeviceConnectionState>();

            container.Register<IBoolValue, BoolValue>();
            container.Register<INumericValue, NumericValue>();
            container.Register<IChosenFromListValue, ChosenFromListValue>();
            container.Register<IRange, DefaultRange>();
            container.Register<IBitGroupValue, BitGroupValue>();
            container.Register<IDeviceSharedResources, DeviceSharedResources>();
            container.Register<IErrorValue, ErrorValue>();
            container.Register<IStringValue, StringValue>();
            container.Register<ITimeValue, TimeValue>();
            container.Register<IFragmentSettings, DefaultFragmentSettings>();
            container.Register<IBitMaskValue, BitMaskValue>();

            container.Register<IQuickAccessMemoryApplyingContext, QuickAccessMemoryApplyingContext>();
            container.Register<IFragmentSetting, QuickMemoryAccessSetting>(ApplicationGlobalNames.QUICK_ACCESS_MEMORY_CONFIGURATION_SETTING);
            
            container.Register<IQueryResult<ushort[]>, DefaultQueryResult<ushort[]>>();
            container.Register<IQueryResult<bool[]>, DefaultQueryResult<bool[]>>();
            container.Register<IQueryResult<bool>, DefaultQueryResult<bool>>();
            container.Register<IQueryResult<ushort>, DefaultQueryResult<ushort>>();
            container.Register<IQueryResult, DefaultQueryResult>();

            container.Register<IQueryResultFactory, QueryResultFactory>();

            container.Register<ITaskProgressReport, TaskProgressReport>();

            container.Register<IDeviceCreator, DefaultDeviceCreator>();
            container.Register(typeof(IDevice), typeof(DefaultDevice.DefaultDevice));

            ISerializerService serializer = container.Resolve<ISerializerService>();
            serializer.AddKnownTypeForSerialization(typeof(BoolValue));
            serializer.AddKnownTypeForSerialization(typeof(NumericValue));
            serializer.AddKnownTypeForSerialization(typeof(ChosenFromListValue));
            serializer.AddKnownTypeForSerialization(typeof(BitGroupValue));
            serializer.AddKnownTypeForSerialization(typeof(ErrorValue));
            serializer.AddKnownTypeForSerialization(typeof(StringValue));
            serializer.AddKnownTypeForSerialization(typeof(BitMaskValue));
            serializer.AddKnownTypeForSerialization(typeof(TimeValue));

            serializer.AddKnownTypeForSerialization(typeof(DeviceSharedResources));
            
            serializer.AddKnownTypeForSerialization(typeof(QuickMemoryAccessSetting));
            serializer.AddKnownTypeForSerialization(typeof(DefaultFragmentSettings));
            serializer.AddNamespaceAttribute("deviceSharedResourcesNS", "DeviceSharedResourcesNS");
            serializer.AddNamespaceAttribute("quickMemoryAccessSetting", "QuickMemoryAccessSettingNS");
            serializer.AddNamespaceAttribute("defaultFragmentSettings", "defaultFragmentSettingsNS");
            serializer.AddKnownTypeForSerialization(typeof(DefaultRange));
            serializer.AddKnownTypeForSerialization(typeof(DefaultDevice.DefaultDevice));
            serializer.AddKnownTypeForSerialization(typeof(DeviceConnectionState));
            serializer.AddNamespaceAttribute("defaultRange", "DefaultRangeNS");
            serializer.AddNamespaceAttribute("defaultDevice", "DefaultDeviceNS");

            serializer.AddNamespaceAttribute("array", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");

            serializer.AddNamespaceAttribute("values", "ValuesNS");
        }
    }
}
