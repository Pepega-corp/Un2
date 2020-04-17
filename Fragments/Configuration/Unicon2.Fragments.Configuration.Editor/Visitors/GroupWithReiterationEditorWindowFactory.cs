using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.View;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Infrastructure.Common;

namespace Unicon2.Fragments.Configuration.Editor.Visitors
{
    public class GroupWithReiterationEditorWindowFactory
    {
        public static void StartEditingGroupWithReiteration(
            IConfigurationGroupEditorViewModel parent)
        {
            //    if ((parent.Model as IItemsGroup).GroupInfo == null)
            //  {
            //      (parent.Model as IItemsGroup).GroupInfo = configurationItemFactory.ResolveGroupWithReiterationInfo();
            //  }
            var window = new GroupWithReiterationInfoEditorView();
            window.DataContext = new GroupWithReiterationEditorViewModel(parent, () => window.Close());
            window.ShowDialog();
        }
    }
}