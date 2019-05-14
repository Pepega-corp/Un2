using Unicon2.Fragments.FileOperations.Editor.ViewModel;
using Unicon2.Fragments.FileOperations.Infrastructure.Keys;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.FileOperations.Editor.Module
{
    public class FileOperationsEditorModule : IUnityModule
    {
        public void Initialize(ITypesContainer containerRegistry)
        {
            containerRegistry.Register(typeof(IFragmentEditorViewModel), typeof(FileBrowserEditorViewModel),
                FileOperationsKeys.FILE_BROWSER + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
            containerRegistry.Resolve<IXamlResourcesService>().AddResourceAsGlobal("Resources/FileOperationsTemplates.xaml", this.GetType().Assembly);
        }
    }
}
