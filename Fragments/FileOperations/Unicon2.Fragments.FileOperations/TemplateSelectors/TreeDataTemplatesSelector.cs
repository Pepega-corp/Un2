using System.Windows;
using System.Windows.Controls;
using Unicon2.Fragments.FileOperations.Infrastructure.ViewModel.BrowserElements;

namespace Unicon2.Fragments.FileOperations.TemplateSelectors
{
  public  class TreeDataTemplatesSelector:DataTemplateSelector
    {
        public DataTemplate DirectoryTreeDataTemplate { get; set; }
        public DataTemplate FileTreeDataTemplate { get; set; }


        #region Overrides of DataTemplateSelector

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IDeviceFileViewModel)
            {
                return FileTreeDataTemplate;
            }
            if (item is IDeviceDirectoryViewModel)
            {
                return DirectoryTreeDataTemplate;
            }
            return base.SelectTemplate(item, container);
        }

        #endregion
    }
}
