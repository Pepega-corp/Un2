using System;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels
{
    public interface IEditorConfigurationItemViewModel : IConfigurationItemViewModel, ICloneable, INameable
    {
        T Accept<T>(IConfigurationItemViewModelVisitor<T> visitor);
    }
}