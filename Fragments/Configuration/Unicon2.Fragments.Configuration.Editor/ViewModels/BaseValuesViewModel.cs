using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using Unicon2.Fragments.Configuration.Editor.Interfaces;
using Unicon2.Infrastructure.Functional;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels
{
    public class BaseValuesViewModel : IBaseValuesViewModel
    {
        public BaseValuesViewModel()
        {
            BaseValuesViewModels = new ObservableCollection<IBaseValueViewModel>();
        }
        public ObservableCollection<IBaseValueViewModel> BaseValuesViewModels { get; }
    }

    public class BaseValueViewModel :ViewModelBase, IBaseValueViewModel
    {
        private string _name;
        private bool _isBaseValuesMemoryFilled;

        public BaseValueViewModel()
        {
            
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value; 
                RaisePropertyChanged();
            }
        }

        public Result<Dictionary<ushort, ushort>> BaseValuesMemory { get; set; }

        public bool IsBaseValuesMemoryFilled
        {
            get => _isBaseValuesMemoryFilled;
            set
            {
                _isBaseValuesMemoryFilled = value; 
                RaisePropertyChanged();
            }
        }
    }
}