using System.IO.Ports;
using System.Runtime.Serialization;
using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Connections.ModBusRtuConnection.Model
{
    [DataContract(Namespace = "ComPortConfigurationNS")]
    public class ComPortConfiguration : IComPortConfiguration
    {
        [DataMember] public int BaudRate { get; set; }
        [DataMember] public int DataBits { get; set; }
        [DataMember] public StopBits StopBits { get; set; }
        [DataMember] public Parity Parity { get; set; }
        [DataMember] public int WaitAnswer { get; set; }
        [DataMember] public int WaitByte { get; set; }
        [DataMember] public int OnTransmission { get; set; }
        [DataMember] public int OffTramsmission { get; set; }

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