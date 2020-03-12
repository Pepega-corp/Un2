using System.IO.Ports;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Connections.ModBusRtuConnection.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ComPortConfiguration : IComPortConfiguration
    {
        [JsonProperty] public int BaudRate { get; set; }
        [JsonProperty] public int DataBits { get; set; }
        [JsonProperty] public StopBits StopBits { get; set; }
        [JsonProperty] public Parity Parity { get; set; }
        [JsonProperty] public int WaitAnswer { get; set; }
        [JsonProperty] public int WaitByte { get; set; }
        [JsonProperty] public int OnTransmission { get; set; }
        [JsonProperty] public int OffTramsmission { get; set; }

        public object Clone()
        {
            return new ComPortConfiguration()
            {
                BaudRate = BaudRate,
                DataBits = DataBits,
                OffTramsmission = OffTramsmission,
                OnTransmission = OnTransmission,
                Parity = Parity,
                StopBits = StopBits,
                WaitAnswer = WaitAnswer,
                WaitByte = WaitByte
            };
        }
    }
}