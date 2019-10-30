using System;
using System.Windows;

namespace Unicon2.Infrastructure.Interfaces.Factories
{
    public interface ISharedResourcesEditorFactory
    {
        void OpenResourceForEdit(INameable resource, object _owner);
    }
}