using System;
using System.Collections.Generic;
using System.Linq;
using Unicon2.Unity.Commands;
using Unicon2.Web.Presentation.ViewModels;

namespace Unicon2.Web.Presentation.Services
{
    public interface IDeviceDefinitionSyncService
    {
        List<DefinitionInfoViewModel> DownloadDefinitionsInfo();
        void UploadDefinition(string id, string definitionData);
    }

    public class LocalDeviceDefinitionSyncService : IDeviceDefinitionSyncService
    {
        private List<(string id, string name, DateTime versionDateTime, string definition)> _dataBase=new List<(string id, string name, DateTime versionDateTime, string definition)>();


        public List<DefinitionInfoViewModel> DownloadDefinitionsInfo()
        {
            return _dataBase.Select(tuple => new DefinitionInfoViewModel(tuple.id, tuple.name, tuple.versionDateTime,
		            new RelayCommand(() => { }, () => false),
		            new RelayCommand(() => { }, () => false),
		            new RelayCommand(() => { }, () => false)))
                .ToList();
        }

        public void UploadDefinition(string id, string definitionData)
        {
            var r = _dataBase.FindIndex(tuple => tuple.id == id);
            var old = _dataBase[r];
            _dataBase[r] = (old.id,old.name,old.versionDateTime,old.definition);
        }
    }
}