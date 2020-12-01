using System;
using System.Windows.Input;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Web.Presentation.ViewModels
{
    public class DefinitionInfoViewModel : ViewModelBase
    {
        public DefinitionInfoViewModel(string id, string name, DateTime? versionDateTime, ICommand uploadCommand, ICommand updateCommand, ICommand deleteCommand)
        {
            Id = id;
            Name = name;
            VersionDateTime = versionDateTime;
            UploadCommand = uploadCommand;
            UpdateCommand = updateCommand;
            DeleteCommand = deleteCommand;
        }

        public string Id { get; }

        public string Name { get; }

        public DateTime? VersionDateTime { get; }

        public ICommand UploadCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
    }




}