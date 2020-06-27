using System.Collections.Generic;
using Newtonsoft.Json;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
namespace Unicon2.Model.Connection
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DeviceConnectionState : IConnectionState
    {
        public DeviceConnectionState()
        {
            ExpectedValues = new List<string>();
        }
 
        [JsonProperty] public string RelatedResourceString { get; set; }
        [JsonProperty] public List<string> ExpectedValues { get; set; }
        [JsonProperty] public IComPortConfiguration DefaultComPortConfiguration { get; set; }

        public object Clone()
        {
            IConnectionState connectionState = new DeviceConnectionState();
            if (ExpectedValues != null)
            {
                connectionState.ExpectedValues.AddRange(ExpectedValues);

            }
            connectionState.RelatedResourceString = this.RelatedResourceString;
            connectionState.DefaultComPortConfiguration = DefaultComPortConfiguration?.Clone() as IComPortConfiguration;
            return connectionState;
        }
    }
}