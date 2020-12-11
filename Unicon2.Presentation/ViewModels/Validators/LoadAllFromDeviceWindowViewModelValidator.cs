using System.IO;
using FluentValidation;
using Unicon2.Presentation.ViewModels.Windows;

namespace Unicon2.Presentation.ViewModels.Validators
{
    public class LoadAllFromDeviceWindowViewModelValidator : AbstractValidator<LoadAllFromDeviceWindowViewModel>
    {
        public LoadAllFromDeviceWindowViewModelValidator()
        {
            RuleFor(model => model.PathToFolderToSave)
                .Must(s => File.GetAttributes(s).HasFlag(FileAttributes.Directory)).WithMessage("dir");
        }
    }
}