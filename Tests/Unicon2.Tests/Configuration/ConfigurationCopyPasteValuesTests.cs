using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Unicon2.Fragments.Configuration.Infrastructure.Services;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Tests.Configuration
{
    [TestFixture]
    public class ConfigurationCopyPasteValuesTests
    {
        [Test]
        public void CheckStructureSimilar()
        {
            var config = Program.RefreshProject();
            IConfigurationTreeWalker treeWalker = StaticContainer.Container.Resolve<IConfigurationTreeWalker>();
            Assert.True(treeWalker.IsStructureSimilar(config.configurationViewModel.RootConfigurationItemViewModels[0],
                config.configurationViewModel.RootConfigurationItemViewModels[0]));
        }

        [Test]
        public void CheckStructureNotSimilar()
        {
            var config = Program.RefreshProject();
            IConfigurationTreeWalker treeWalker = StaticContainer.Container.Resolve<IConfigurationTreeWalker>();
            Assert.False(treeWalker.IsStructureSimilar(config.configurationViewModel.RootConfigurationItemViewModels[1],
                config.configurationViewModel.RootConfigurationItemViewModels[0]));
        }

        [Test]
        public void CopyValues()
        {
            var config = Program.RefreshProject();
            IConfigurationTreeWalker treeWalker = StaticContainer.Container.Resolve<IConfigurationTreeWalker>();
            Assert.True(treeWalker.IsStructureSimilar(
                config.configurationViewModel
                    .RootConfigurationItemViewModels[0].ChildStructItemViewModels[2]
                    .ChildStructItemViewModels[1].ChildStructItemViewModels[0],
                config.configurationViewModel
                    .RootConfigurationItemViewModels[0].ChildStructItemViewModels[2]
                    .ChildStructItemViewModels[1].ChildStructItemViewModels[1]));

            ((IChosenFromListValueViewModel) ((IRuntimePropertyViewModel) config.configurationViewModel
                    .RootConfigurationItemViewModels[0].ChildStructItemViewModels[2]
                    .ChildStructItemViewModels[1].ChildStructItemViewModels[0]
                    .ChildStructItemViewModels[0])
                .LocalValue).SelectedItem = "Срабатывание";


            treeWalker.CopyValuesToItem(config.configurationViewModel
                    .RootConfigurationItemViewModels[0].ChildStructItemViewModels[2]
                    .ChildStructItemViewModels[1].ChildStructItemViewModels[0],
                config.configurationViewModel
                    .RootConfigurationItemViewModels[0].ChildStructItemViewModels[2]
                    .ChildStructItemViewModels[1].ChildStructItemViewModels[1]);

            Assert.True(((IChosenFromListValueViewModel) ((IRuntimePropertyViewModel) config.configurationViewModel
                    .RootConfigurationItemViewModels[0].ChildStructItemViewModels[2]
                    .ChildStructItemViewModels[1].ChildStructItemViewModels[1]
                    .ChildStructItemViewModels[0])
                .LocalValue).SelectedItem == "Срабатывание");
        }
    }
}