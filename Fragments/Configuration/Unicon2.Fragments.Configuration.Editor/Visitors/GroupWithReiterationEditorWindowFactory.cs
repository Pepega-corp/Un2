using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.View;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;

namespace Unicon2.Fragments.Configuration.Editor.Factories
{
   public class GroupWithReiterationEditorWindowFactory
    {
        public static void StartEditingGroupWithReiteration(
            IConfigurationGroupEditorViewModel parent, IConfigurationItemFactory configurationItemFactory)
        {
         //    if ((parent.Model as IItemsGroup).GroupInfo == null)
          //  {
          //      (parent.Model as IItemsGroup).GroupInfo = configurationItemFactory.ResolveGroupWithReiterationInfo();
          //  }
            var window = new GroupWithReiterationInfoEditorView();
            window.DataContext = new GroupWithReiterationEditorViewModel(parent,()=>window.Close(),configurationItemFactory.ResolveReiterationSubGroupInfo);
            window.ShowDialog();
        }
    }
}
