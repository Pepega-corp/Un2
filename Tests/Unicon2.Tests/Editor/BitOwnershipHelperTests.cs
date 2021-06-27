using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Unicon2.Formatting.Editor.ViewModels.Helpers;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Properties;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Services;
using Unicon2.Model.Values.Range;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Values;
using Unicon2.Tests.Utils;
using Unicon2.Unity.Common;
using Unicon2.Unity.Interfaces;
using Unity;

namespace Unicon2.Tests.Editor
{
    [TestFixture]
    public class BitOwnershipHelperTests
    {
        private ITypesContainer _typesContainer;

        public BitOwnershipHelperTests()
        {
            _typesContainer =
                new TypesContainer(Program.GetApp().Container.Resolve(typeof(IUnityContainer)) as IUnityContainer);
        }

        [Test]
        public async Task BitOwnershipHelperCheckNoSameAddresses()
        {
            List<IConfigurationItemViewModel> list = new List<IConfigurationItemViewModel>();

            
            var property = EditorHelpers.AddPropertyViewModel(list, 1, _typesContainer);
            
            var res = BitOwnershipHelper.CreateBitViewModelsWithOwnership(
                property, list);
            Assert.True(res.All(model =>model.IsBitEditEnabled ));
        }

        [Test]
        public async Task BitOwnershipHelperCheckOneWithSameAddress()
        {
            List<IConfigurationItemViewModel> list = new List<IConfigurationItemViewModel>();


            var property = EditorHelpers.AddPropertyViewModel(list, 1, _typesContainer);
            EditorHelpers.AddPropertyViewModel(list, 1, _typesContainer);

            var res = BitOwnershipHelper.CreateBitViewModelsWithOwnership(
                property, list);
            Assert.True(res.All(model => !model.IsBitEditEnabled));
        }

        [Test]
        public async Task BitOwnershipHelperCheckOneBitWithSameAddress()
        {
            List<IConfigurationItemViewModel> list = new List<IConfigurationItemViewModel>();


            var property = EditorHelpers.AddPropertyViewModel(list, 1, _typesContainer);
            var secondProp=EditorHelpers.AddPropertyViewModel(list, 1, _typesContainer);
            secondProp.IsFromBits = true;
            property.IsFromBits = true;
            secondProp.BitNumbersInWord[15].IsChecked = true;
            var res = BitOwnershipHelper.CreateBitViewModelsWithOwnership(
                property, list);
            Assert.False(res[15].IsBitEditEnabled);
            Assert.True(res.Where(model => model != res[15]).All(model => model.IsBitEditEnabled));
        }
    }
}
