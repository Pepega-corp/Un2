using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Unicon2.Fragments.Configuration.Editor.Helpers;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Properties;
using Unicon2.Fragments.Configuration.Editor.Visitors;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Tests.Utils;

namespace Unicon2.Tests.Configuration
{
    [TestFixture]
    public class ComplexPropertiesMigratorTests
    {

        public ComplexPropertiesMigratorTests()
        {
            Program.GetApp();
        }

        [Test]
        public void CheckMigrator()
        {
            var rootItems=new List<IConfigurationItemViewModel>();


            var complexProperty = ConfigurationItemEditorViewModelFactory.Create().VisitComplexProperty(null) as ComplexPropertyEditorViewModel;

            complexProperty.AddSubProperty();
            complexProperty.AddSubProperty();
            complexProperty.AddSubProperty();
            complexProperty.SubPropertyEditorViewModels[0].BitNumbersInWord[0].ChangeValueByOwnerCommand.Execute(complexProperty.SubPropertyEditorViewModels[0]);
            complexProperty.SubPropertyEditorViewModels[1].BitNumbersInWord[10].ChangeValueByOwnerCommand.Execute(complexProperty.SubPropertyEditorViewModels[1]);
            complexProperty.SubPropertyEditorViewModels[2].BitNumbersInWord[12].ChangeValueByOwnerCommand.Execute(complexProperty.SubPropertyEditorViewModels[2]);




            var complexProperty2 = ConfigurationItemEditorViewModelFactory.Create().VisitComplexProperty(null) as ComplexPropertyEditorViewModel;

            complexProperty2.AddSubProperty();
            complexProperty2.SubPropertyEditorViewModels[0].BitNumbersInWord[0].ChangeValueByOwnerCommand.Execute(complexProperty2.SubPropertyEditorViewModels[0]);
            complexProperty2.IsGroupedProperty = true;

            var rootGroup = new ConfigurationGroupEditorViewModel()
            {
                ChildStructItemViewModels = new ObservableCollection<IConfigurationItemViewModel>()
                    {complexProperty, complexProperty2}

            };

            complexProperty.Parent = rootGroup;
            complexProperty2.Parent = rootGroup;

            rootItems.Add(rootGroup);


           var resPrepared= ComplexPropertiesMigrator.GetAllComplexPropertiesInConfiguration(rootItems);
           Assert.AreEqual(2, resPrepared.Count);

           var res = ComplexPropertiesMigrator.MigrateComplexProperties(resPrepared);

           Assert.True(res.IsSuccess);

            

        }
    }
}