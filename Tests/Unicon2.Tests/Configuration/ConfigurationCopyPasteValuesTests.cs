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
        public async Task CheckStructureSimilar()
        {
            var config = Program.RefreshProject();
            IConfigurationTreeWalker treeWalker = StaticContainer.Container.Resolve<IConfigurationTreeWalker>();
            Assert.True(treeWalker.IsStructureSimilar(config.configurationViewModel.RootConfigurationItemViewModels[0],
                config.configurationViewModel.RootConfigurationItemViewModels[0]));
        }

        [Test]
        public async Task CheckStructureNotSimilar()
        {
            var config = Program.RefreshProject();
            IConfigurationTreeWalker treeWalker = StaticContainer.Container.Resolve<IConfigurationTreeWalker>();
            Assert.False(treeWalker.IsStructureSimilar(config.configurationViewModel.RootConfigurationItemViewModels[1],
                config.configurationViewModel.RootConfigurationItemViewModels[0]));
        }

        [Test]
        public async Task CopyValues()
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

            ((IChosenFromListValueViewModel) ((IRuntimeSubPropertyViewModel) config.configurationViewModel
                    .RootConfigurationItemViewModels[0].ChildStructItemViewModels[2]
                    .ChildStructItemViewModels[1].ChildStructItemViewModels[0]
                    .ChildStructItemViewModels[0])
                .LocalValue).SelectedItem = "Срабатывание";


            await treeWalker.CopyValuesToItem(config.configurationViewModel
                    .RootConfigurationItemViewModels[0].ChildStructItemViewModels[2]
                    .ChildStructItemViewModels[1].ChildStructItemViewModels[0],
                config.configurationViewModel
                    .RootConfigurationItemViewModels[0].ChildStructItemViewModels[2]
                    .ChildStructItemViewModels[1].ChildStructItemViewModels[1]);

            Assert.True(((IChosenFromListValueViewModel) ((IRuntimeSubPropertyViewModel) config.configurationViewModel
                    .RootConfigurationItemViewModels[0].ChildStructItemViewModels[2]
                    .ChildStructItemViewModels[1].ChildStructItemViewModels[1]
                    .ChildStructItemViewModels[0])
                .LocalValue).SelectedItem == "Срабатывание");
        }
    }
}