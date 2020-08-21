using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.FileOperations.Infrastructure.FileOperations;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.FileOperations.FileOperations
{
    public class DirectoryReader : IDeviceContextConsumer
    {
        private const string DIRECTORY_CMD = "GETDIR";
        private readonly ICommandSender _commandSender;
        private readonly ICommandStateReader _commandStateReader;
        private readonly IDataReader _dataReader;
        private DeviceContext _deviceContext;
        private IDataProvider _dataProvider;

        public string Directory { get; private set; }
        
        public DeviceContext DeviceContext
        {
            get => _deviceContext;
            set
            {
                _deviceContext = value;
                _dataProvider = _deviceContext.DataProviderContainer.DataProvider;
                _commandSender.SetDataProvider(_dataProvider);
                _commandStateReader.SetDataProvider(_dataProvider);
            }
        }

        public DirectoryReader(ICommandSender commandSender, ICommandStateReader commandStateReader, IDataReader dataReader)
        {
            _commandSender = commandSender;
            _commandStateReader = commandStateReader;
            _dataReader = dataReader;
        }

        public async void ReadDirectory()
        {
            await _commandSender.SetCommand(DIRECTORY_CMD);
            
            _commandStateReader.ReadCommandState();

            var dataWordsLen = _commandStateReader.DataLength % 2 == 0
                ? _commandStateReader.DataLength / 2
                : _commandStateReader.DataLength / 2 + 1;

            Directory = (await _dataReader.ReadData((ushort)dataWordsLen)).Replace("/", "");
        }
    }
}