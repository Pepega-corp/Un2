using System;
using System.Windows.Input;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Web.Presentation.ViewModels
{
    public class DefinitionInfoViewModel : ViewModelBase
    {
        public DefinitionInfoViewModel(string id, string name, DateTime versionDateTime)
        {
            Id = id;
            Name = name;
            VersionDateTime = versionDateTime;
        }

        public string Id { get; }

        public string Name { get; }

        public DateTime VersionDateTime { get; }
    }


    public class LocalDefinitionInfoViewModel : DefinitionInfoViewModel
    {
        private bool _isSynchronized;

        public LocalDefinitionInfoViewModel(string id, string name, DateTime version, ICommand uploadDefinition, bool isSynchronized) : base(
            id, name, version)
        {
            UploadDefinition = uploadDefinition;
            IsSynchronized = isSynchronized;
        }

        public bool IsSynchronized
        {
            get => _isSynchronized;
            set
            {
                _isSynchronized = value; 
                RaisePropertyChanged();
            }
        }

        public ICommand UploadDefinition { get; }
    }

    public class ServerDefinitionInfoViewModel : DefinitionInfoViewModel
    {
        public ServerDefinitionInfoViewModel(string id, string name, DateTime version, ICommand downloadDefinition) :
            base(id,
                name, version)
        {
            DownloadDefinition = downloadDefinition;
        }

        public ICommand DownloadDefinition { get; }
    }
}