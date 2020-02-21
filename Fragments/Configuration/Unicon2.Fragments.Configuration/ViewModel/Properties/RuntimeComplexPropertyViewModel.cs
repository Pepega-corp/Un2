using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.ViewModel.Properties
{
    public class RuntimeComplexPropertyViewModel : RuntimePropertyViewModel, IComplexPropertyViewModel
    {
        private readonly IRuntimeConfigurationItemViewModelFactory _runtimeConfigurationItemViewModelFactory;
        private bool _isGroupedProperty;

        public RuntimeComplexPropertyViewModel(ITypesContainer container, IValueViewModelFactory valueViewModelFactory,
            IRuntimeConfigurationItemViewModelFactory runtimeConfigurationItemViewModelFactory) : base(container)
        {
            this._runtimeConfigurationItemViewModelFactory = runtimeConfigurationItemViewModelFactory;
        }


        public override string StrongName => ConfigurationKeys.RUNTIME + ConfigurationKeys.COMPLEX_PROPERTY +
                                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;



        protected override string GetTypeName()
        {
            return ConfigurationKeys.COMPLEX_PROPERTY;
        }

        protected override void SetModel(object model)
        {
            base.SetModel(model);

            IComplexProperty complexProperty = this._model as IComplexProperty;
            foreach (ISubProperty subProperty in complexProperty.SubProperties)
            {
                Infrastructure.ViewModel.IRuntimeConfigurationItemViewModel subPropertyViewModel = this._runtimeConfigurationItemViewModelFactory
                    .CreateRuntimeConfigurationItemViewModel(subProperty);
                this.ChildStructItemViewModels.Add(subPropertyViewModel);
                this.IsCheckable = true;
            }
            this.IsGroupedProperty = (this._model as IComplexProperty).IsGroupedProperty;

        }

        public bool IsGroupedProperty
        {
            get { return this._isGroupedProperty; }
            set
            {
                this._isGroupedProperty = value;
                this.RaisePropertyChanged();
            }
        }
        public override T Accept<T>(IConfigurationItemVisitor<T> visitor)
        {
            return visitor.VisitComplexProperty(this);
        }
    }
}
