using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using Newtonsoft.Json;
using NModbus4.Serial;
using Unicon2.Connections.ModBusRtuConnection.Interfaces;
using Unicon2.Connections.ModBusRtuConnection.Interfaces.Factories;
using Unicon2.Connections.ModBusRtuConnection.Keys;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Connections.ModBusRtuConnection.Services
{

    [JsonObject(MemberSerialization.OptIn)]
    public class ComConnectionManager : IComConnectionManager
    {
        private readonly IComPortConfigurationFactory _comPortConfigurationFactory;
        private readonly ISerializerService _serializerService;

        public ComConnectionManager()
        {
            _comPortConfigurationFactory = StaticContainer.Container.Resolve<IComPortConfigurationFactory>();
            _serializerService = StaticContainer.Container.Resolve<ISerializerService>();
            ComPortConfigurationsDictionary = new Dictionary<string, IComPortConfiguration>();
            if (File.Exists(StringKeys.COMPORT_CONFIGURATION_SETTINGS + ".json"))
            {
                try
                {
                    ComPortConfigurationsDictionary =
                        _serializerService.DeserializeFromFile<Dictionary<string, IComPortConfiguration>>(
                            StringKeys.COMPORT_CONFIGURATION_SETTINGS + ".json");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new SerializationException();
                }
            }
        }


        public List<string> GetSerialPortNames()
        {
            List<string> portNames = SerialPort.GetPortNames().ToList();
            foreach (string portName in portNames)
            {
                if (!ComPortConfigurationsDictionary.ContainsKey(portName))
                {
                    ComPortConfigurationsDictionary.Add(portName,
                        _comPortConfigurationFactory.CreateComPortConfiguration());
                }
            }

            return portNames;
        }

        [JsonProperty] public Dictionary<string, IComPortConfiguration> ComPortConfigurationsDictionary { get; set; }

        public IComPortConfiguration GetComPortConfiguration(string portName)
        {
            if (portName == null) return null;
            if (ComPortConfigurationsDictionary.ContainsKey(portName))
                return ComPortConfigurationsDictionary[portName];
            return null;
        }

        public void SetComPortConfigurationByName(IComPortConfiguration comPortConfiguration, string portName)
        {
            if (!ComPortConfigurationsDictionary.ContainsKey(portName))
            {
                ComPortConfigurationsDictionary.Add(portName,
                    _comPortConfigurationFactory.CreateComPortConfiguration());
            }

            ComPortConfigurationsDictionary[portName] = comPortConfiguration;
        }

        public SerialPortAdapter GetSerialPortAdapter(string portName)
        {
            SerialPort serialPort = new SerialPort(portName);
            SerialPortAdapter serialPortAdapter = new SerialPortAdapter(serialPort);
            if (!ComPortConfigurationsDictionary.ContainsKey(portName)) return null;
            IComPortConfiguration comPortConfiguration = ComPortConfigurationsDictionary[portName];
            serialPort.BaudRate = comPortConfiguration.BaudRate;
            serialPort.DataBits = comPortConfiguration.DataBits;
            serialPort.StopBits = comPortConfiguration.StopBits;
            serialPort.Parity = comPortConfiguration.Parity;
            serialPort.ReadTimeout = comPortConfiguration.WaitAnswer;
            serialPort.WriteTimeout = comPortConfiguration.WaitAnswer;
            try
            {
                serialPort.Open();
            }
            catch (Exception portExc)
            {
                Debug.Write(portExc.Message);
                throw portExc;
            }

            try
            {
                _serializerService.SerializeInFile(ComPortConfigurationsDictionary, StringKeys.COMPORT_CONFIGURATION_SETTINGS + ".json");
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }

            return serialPortAdapter;
        }

        public SerialPortAdapter GetSerialPortAdapter(string portName, IComPortConfiguration comPortConfiguration)
        {
            SerialPort serialPort = new SerialPort(portName);
            SerialPortAdapter serialPortAdapter = new SerialPortAdapter(serialPort);
            if (!ComPortConfigurationsDictionary.ContainsKey(portName)) return null;
            comPortConfiguration = ComPortConfigurationsDictionary[portName];
            serialPort.BaudRate = comPortConfiguration.BaudRate;
            serialPort.DataBits = comPortConfiguration.DataBits;
            serialPort.StopBits = comPortConfiguration.StopBits;
            serialPort.Parity = comPortConfiguration.Parity;
            serialPort.ReadTimeout = comPortConfiguration.WaitAnswer;
            serialPort.WriteTimeout = comPortConfiguration.WaitAnswer;
            serialPort.Open();

            return serialPortAdapter;
        }
    }
}