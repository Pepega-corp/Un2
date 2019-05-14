using System;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Infrastructure.GeneralFactories
{
    public interface IGeneralViewModelFactory<out T> where T : IViewModel
    {
        T CreateEditorViewModelByStrongName(IStronglyNamed modelStronglyNamed);
        T CreateViewModelByModelType(object model);
        T CreateViewModelWithModelByModelType(Type modelType);
        T CreateViewModelByType();

    }
}