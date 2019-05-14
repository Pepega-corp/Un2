using System;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.GeneralFactories;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Services.GeneralFactories
{
    public class GeneralViewModelFactory<T> : IGeneralViewModelFactory<T> where T : IViewModel
    {
        private readonly ITypesContainer _container;


        public GeneralViewModelFactory(ITypesContainer container)
        {
            this._container = container;
        }


        #region Implementation of IGeneralViewModelFactory<out T>

        public T CreateEditorViewModelByStrongName(IStronglyNamed modelStronglyNamed)
        {
            T viewModel = this._container.Resolve<T>(modelStronglyNamed.StrongName + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
            viewModel.Model = modelStronglyNamed;
            return viewModel;
        }

        public T CreateViewModelByModelType(object model)
        {
            T viewModel = this._container.Resolve<T>();
            viewModel.Model = model;
            return viewModel;
        }

        public T CreateViewModelWithModelByModelType(Type modelType)
        {
            T viewModel = this._container.Resolve<T>();
            object model = this._container.Resolve(modelType);
            viewModel.Model = model;
            return viewModel;
        }

        public T CreateViewModelByType()
        {
            T viewModel = this._container.Resolve<T>();
            return viewModel;
        }

        #endregion
    }
}
